using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHud : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI healthPoints;
    public TextMeshProUGUI level;
    public Slider hpSlider;

    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;
        level.text = "Lvl " + unit.unitLevel;
        healthPoints.text = "Hp: " + unit.currentHealth + "/" + unit.maxHealth;
        hpSlider.maxValue = unit.maxHealth;
        hpSlider.value = unit.currentHealth;
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }
}
