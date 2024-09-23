using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;          // The name of the unit.
    public int unitLevel;            // The level of the unit.
    public int damage;               // The amount of damage the unit can deal.
    public int currentHealth;        // The current health points of the unit.
    public int maxHealth;            // The maximum health points of the unit.

    // Method to take damage. Returns true if the unit is dead.
    public bool TakeDamage(int damage)
    {
        currentHealth -= damage; // Reduce current health by the damage taken.

        // Check if the unit's health has dropped to zero or below.
        if (currentHealth <= 0)
            return true; // The unit is dead.
        else
            return false; // The unit is still alive.
    }

    // Method to heal the unit.
    public void Heal(int heal)
    {
        currentHealth += heal; // Increase current health by the heal amount.

        // Ensure current health does not exceed maximum health.
        if (currentHealth > maxHealth)
            currentHealth = maxHealth; // Set current health to max if it exceeds.
    }
}
