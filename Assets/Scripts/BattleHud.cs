using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    public Text nameText;
    public Text levelText;
    public Text armorText; // Новое поле для отображения брони
    public Text ammoText;  // Новое поле для отображения боезапаса
    public Slider hpSlider;
    public Slider overheatSlider; //  слайдер для отображения перегрева

    // Установка HUD на основе данных юнита
    public void SetHud(Unit unit)
    {
        nameText.text = unit.unitName;
        levelText.text = "Lv " + unit.unitLevel;
        armorText.text = "Armor: " + unit.armor;
        ammoText.text = "Ammo: " + unit.currentAmmo + "/" + unit.maxAmmo;

        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;

        overheatSlider.maxValue = unit.maxOverheat;
        overheatSlider.value = unit.overheat;
    }

    // Обновление здоровья
    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }

    // Обновление боезапаса
    public void SetAmmo(int currentAmmo, int maxAmmo)
    {
        ammoText.text = "Ammo: " + currentAmmo + "/" + maxAmmo;
    }

    // Обновление перегрева
    public void SetOverheat(float overheat, float maxOverheat)
    {
        overheatSlider.value = overheat;
    }

    // Обновление брони (если нужно динамически менять броню)
    public void SetArmor(int armor)
    {
        armorText.text = "Armor: " + armor;
    }
}
