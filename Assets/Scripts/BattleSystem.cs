using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// Enum to represent different battle states.
public enum BattleStates
{
    START,         // Battle is starting
    PLAYERTURN,    // It's the player's turn
    ENEMYTURN,     // It's the enemy's turn
    LOST,          // The player lost the battle
    WON            // The player won the battle
}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;        // Prefab for the player unit.
    public static string EnemyToFight;                // Name of the enemy to fight.

    [SerializeField] Transform playerSpawn;           // Spawn location for the player.
    [SerializeField] Transform enemySpawn;            // Spawn location for the enemy.

    [SerializeField] BattleHud playerHUD;             // HUD for the player.
    [SerializeField] BattleHud enemyHUD;              // HUD for the enemy.

    [SerializeField] TextMeshProUGUI dialogText;      // Text UI for dialog messages.

    BattleStates currentState;                        // Current state of the battle.
    Unit playerUnit;                                  // Reference to the player's unit.
    Unit enemyUnit;                                   // Reference to the enemy's unit.

    void Start()
    {
        currentState = BattleStates.START;            // Set initial state to START.
        StartCoroutine(SetupBattle());                // Start the battle setup coroutine.
    }

    IEnumerator SetupBattle()
    {
        // Instantiate the player unit.
        GameObject playerGameObject = Instantiate(playerPrefab, playerSpawn);
        playerUnit = playerGameObject.GetComponent<Unit>();

        // Load and instantiate the enemy unit.
        GameObject enemyPrefab = Resources.Load<GameObject>(EnemyToFight);
        GameObject enemyGameObject = Instantiate(enemyPrefab, enemySpawn.position, enemySpawn.rotation);
        enemyUnit = enemyGameObject.GetComponent<Unit>();

        // Display dialog message for the player.
        dialogText.text = "An enemy has appeared!";

        // Set up HUDs for player and enemy.
        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(1f);          // Wait for a second before continuing.

        currentState = BattleStates.PLAYERTURN;       // Set state to PLAYERTURN.
        PlayerTurn();                                  // Start the player's turn.
    }

    void PlayerTurn()
    {
        dialogText.text = "Choose an action!";       // Prompt the player to choose an action.
    }

    // Method called when the attack button is pressed.
    public void OnAttackButton()
    {
        if (currentState != BattleStates.PLAYERTURN) // Check if it's the player's turn.
            return;

        StartCoroutine(PlayerAttack());              // Start the player attack coroutine.
    }

    // Method called when the heal button is pressed.
    public void OnHealButton()
    {
        if (currentState != BattleStates.PLAYERTURN) // Check if it's the player's turn.
            return;

        playerUnit.Heal(5);                           // Heal the player unit by 5 points.
        playerHUD.SetHpBar(playerUnit);               // Update the player's health bar.

        currentState = BattleStates.ENEMYTURN;        // Set state to ENEMYTURN.
        StartCoroutine(EnemyTurn());                   // Start the enemy's turn.
    }

    IEnumerator PlayerAttack()
    {
        // Deal damage to the enemy and check if they are dead.
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        enemyHUD.SetHpBar(enemyUnit);                  // Update the enemy's health bar.

        dialogText.text = "Your attack is effective!"; // Display attack message.

        yield return new WaitForSeconds(1f);           // Wait for a second before continuing.

        // Check if the enemy is dead.
        if (isDead)
        {
            currentState = BattleStates.WON;            // Set state to WON.
            StartCoroutine(EndBattle());                // End the battle.
        }
        else
        {
            currentState = BattleStates.ENEMYTURN;      // Set state to ENEMYTURN.
            StartCoroutine(EnemyTurn());                 // Start the enemy's turn.
        }
    }

    IEnumerator EnemyTurn()
    {
        // Deal damage to the player and check if they are dead.
        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
        playerHUD.SetHpBar(playerUnit);                 // Update the player's health bar.

        dialogText.text = enemyUnit.unitName + " is attacking!"; // Display enemy attack message.

        yield return new WaitForSeconds(1f);            // Wait for a second before continuing.

        // Check if the player is dead.
        if (isDead)
        {
            currentState = BattleStates.LOST;            // Set state to LOST.
            StartCoroutine(EndBattle());                 // End the battle.
        }
        else
        {
            currentState = BattleStates.PLAYERTURN;      // Set state back to PLAYERTURN.
            PlayerTurn();                                 // Start the player's turn again.
        }
    }

    IEnumerator EndBattle()
    {
        // Handle battle ending based on the current state.
        if (currentState == BattleStates.WON)
        {
            dialogText.text = "You won!";                // Display win message.
            yield return new WaitForSeconds(1f);        // Wait before transitioning.
            SceneManager.LoadScene("GameMap");          // Load the game map scene.
        }
        else if (currentState == BattleStates.LOST)
        {
            dialogText.text = "You lost!";               // Display loss message.
            yield return new WaitForSeconds(1f);        // Wait before transitioning.
            SceneManager.LoadScene("GameMap");          // Load the game map scene.
        }
    }
}
