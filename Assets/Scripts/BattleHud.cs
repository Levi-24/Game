using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHud : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI unitName;        // Reference to the UI element displaying the unit's name.
    [SerializeField] TextMeshProUGUI healthPoints;     // Reference to the UI element displaying the unit's health points.
    [SerializeField] TextMeshProUGUI unitLevel;        // Reference to the UI element displaying the unit's level.
    [SerializeField] Slider healthSlider;               // Reference to the health slider UI element.

    // Method to set up the HUD with the unit's information.
    public void SetHUD(Unit unit)
    {
        // Set the unit's name in the HUD.
        unitName.text = unit.unitName;

        // Set the unit's level in the HUD.
        unitLevel.text = "Lvl " + unit.unitLevel;

        // Set the health points display in the HUD.
        healthPoints.text = "Hp: " + unit.currentHealth + "/" + unit.maxHealth;

        // Set the maximum value of the health slider to the unit's max health.
        healthSlider.maxValue = unit.maxHealth;

        // Set the current value of the health slider to the unit's current health.
        healthSlider.value = unit.currentHealth;
    }

    // Method to update the health bar when the unit takes damage or heals.
    public void SetHpBar(Unit unit)
    {
        // Update the health slider value to the unit's current health.
        healthSlider.value = unit.currentHealth;

        // Update the health points display to reflect the current and max health.
        healthPoints.text = "Hp: " + unit.currentHealth + "/" + unit.maxHealth;
    }
}
