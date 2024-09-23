
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;

    public int damage;
    public int currentHealth;
    public int maxHealth;

    public bool TakeDamage(int damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
            return true;
        else
            return false;
    }

    public void Heal(int heal)
    {
        currentHealth += heal;
    }
}
