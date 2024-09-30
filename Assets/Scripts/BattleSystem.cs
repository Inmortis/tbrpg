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

    Unit playerunit;
    Unit enemyunit;

    public BattleHud playerhud;
    public BattleHud enemyhud;

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
        playerunit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyunit = enemyGO.GetComponent<Unit>();

        dialogueText.text = "A " + enemyunit.unitname + " ambushed you";

        playerhud.SetHud(playerunit);
        enemyhud.SetHud(enemyunit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();

    }

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyunit.TakeDamage(playerunit.damage);

        enemyhud.SetHP(enemyunit.currentHP);
        dialogueText.text = "The attack is successful";

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
        dialogueText.text = enemyunit.unitname + " attacks!";

        yield return new WaitForSeconds(1f);

        bool isDead = playerunit.TakeDamage(enemyunit.damage);

        playerhud.SetHP(playerunit.currentHP);

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
            dialogueText.text = "You won the battle";
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You were defeated";
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
}
