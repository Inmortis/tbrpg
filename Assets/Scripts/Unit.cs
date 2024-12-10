using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // Основные параметры
    public string unitName;         // Имя юнита
    public int unitLevel;           // Уровень юнита
    public int damage;              // Урон
    public int maxHP;               // Максимальное здоровье
    public int currentHP;           // Текущее здоровье
    public int armor;               // Броня юнита
    public UnitType unitType;       // Тип юнита (перечисление)
    public int maxAmmo;             // Максимальное количество патронов
    public int currentAmmo;         // Текущее количество патронов
    public float overheat;          // Текущий уровень перегрева
    public float maxOverheat = 100; // Максимальный уровень перегрева

    // Метод получения урона
    public bool TakeDamage(int dmg)
    {
        int effectiveDamage = Mathf.Max(dmg - armor, 0); // Урон уменьшается на значение брони
        currentHP -= effectiveDamage;

        // Возврат true, если юнит погиб
        return currentHP <= 0;
    }

    // Метод для стрельбы
    public bool Shoot()
    {
        if (currentAmmo > 0 && overheat < maxOverheat)
        {
            currentAmmo--;
            overheat += 10; // Перегрев увеличивается на 10
            return true;    // Успешная атака
        }
        return false;        // Не удалось выстрелить
    }

    // Метод охлаждения
    public void CoolDown(float amount)
    {
        overheat -= amount;
        if (overheat < 0)
            overheat = 0; // Перегрев не может быть меньше 0
    }

    // Метод перезарядки патронов
    public void Reload()
    {
        currentAmmo = maxAmmo; // Обновляем боезапас до максимума
    }

    // Перечисление типов юнитов
    public enum UnitType
    {
        Scout,       // Разведчик
        Soldier,     // Солдат
        Commander,   // Командир
        Mechanic,    // Механик
        Tank         // Танк
    }

    public void ReduceArmor(int damageDealt)
    {
        armor = Mathf.Max(armor - damageDealt / 2, 0); // Уменьшение брони
    }
}