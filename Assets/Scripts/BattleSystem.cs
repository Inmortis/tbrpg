using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Unit playerUnit;
    Unit enemyUnit;

    public BattleHud playerHud;
    public BattleHud enemyHud;

    public Text dialogueText;

    public BattleState state;



    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();

        dialogueText.text = $"A {enemyUnit.unitName} ({enemyUnit.unitType}) ambushed you!";

        playerHud.SetHud(playerUnit);
        enemyHud.SetHud(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        if (playerUnit.currentAmmo <= 0)
        {
            dialogueText.text = "You are out of ammo!";
            yield return new WaitForSeconds(1f);
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
            yield break;
        }

        if (playerUnit.overheat >= playerUnit.maxOverheat)
        {
            dialogueText.text = "Your weapon is overheated!";
            yield return new WaitForSeconds(1f);
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
            yield break;
        }

        // Проверка попадания
        bool hit = CalculateHitChance(playerUnit.unitLevel, enemyUnit.unitLevel);
        if (!hit)
        {
            dialogueText.text = $"You missed {enemyUnit.unitName}!";
            playerUnit.Shoot(); // Все равно расходуем боезапас и перегреваемся
            yield return new WaitForSeconds(1f);
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
            yield break;
        }

        dialogueText.text = $"You attack {enemyUnit.unitName}!";

        // Рассчитываем урон с учетом брони
        int damageDealt = CalculateDamage(playerUnit.damage, enemyUnit.armor);

        // Наносим урон
        bool isDead = enemyUnit.TakeDamage(damageDealt);

        // Уменьшаем броню врага
        enemyUnit.ReduceArmor(damageDealt);

        // Обновляем HUD врага
        enemyHud.SetHP(enemyUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        dialogueText.text = $"{enemyUnit.unitName} attacks!";

        yield return new WaitForSeconds(1f);

        // Проверка попадания
        bool hit = CalculateHitChance(enemyUnit.unitLevel, playerUnit.unitLevel);
        if (!hit)
        {
            dialogueText.text = $"{enemyUnit.unitName} missed!";
            yield return new WaitForSeconds(1f);
            state = BattleState.PLAYERTURN;
            PlayerTurn();
            yield break;
        }

        // Рассчитываем урон с учетом брони
        int damageDealt = CalculateDamage(enemyUnit.damage, playerUnit.armor);

        // Наносим урон
        bool isDead = playerUnit.TakeDamage(damageDealt);

        // Уменьшаем броню игрока
        playerUnit.ReduceArmor(damageDealt);

        // Обновляем HUD игрока
        playerHud.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "You won the battle!";
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You were defeated!";
        }
    }

    void PlayerTurn()
    {
        dialogueText.text = "Choose an action";
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());
    }

    public void OnReloadButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        dialogueText.text = "Reloading...";
        playerUnit.Reload();
        playerHud.SetHud(playerUnit); // Обновляем HUD, чтобы показать полные патроны
        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    public void OnCoolDownButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        dialogueText.text = "Cooling down...";
        playerUnit.CoolDown(20f); // Снижает перегрев на 20
        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    bool CalculateHitChance(int attackerLevel, int defenderLevel)
    {
        int levelDifference = defenderLevel - attackerLevel;
        float baseChance = 0.8f; // Базовый шанс попасть — 80%
        float modifier = Mathf.Clamp(1f - 0.1f * levelDifference, 0.5f, 1f); // Модификатор от уровня
        return Random.value <= baseChance * modifier;
    }

    int CalculateDamage(int baseDamage, int armor)
    {
        int reducedArmor = Mathf.Clamp(armor, 0, baseDamage / 2); // Броня снижает урон на максимум 50%
        return Mathf.Max(baseDamage - reducedArmor, 1); // Минимальный урон — 1
    }


}