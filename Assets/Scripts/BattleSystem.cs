using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public enum BattleStates
{
    START,
    PLAYERCHOICE,
    ENEMYCHOICE,
    LOST,
    WON
}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject enemyPrefab;

    public Transform playerSpawn;
    public Transform enemySpawn;

    public BattleHud playerHUD;
    public BattleHud enemyHUD;

    Unit playerUnit;
    Unit enemyUnit;

    public TextMeshProUGUI dialogText;

    public BattleStates currentState;

    void Start()
    {
        currentState = BattleStates.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGameObject = Instantiate(playerPrefab, playerSpawn);
        playerUnit = playerGameObject.GetComponent<Unit>();

        GameObject enemyGameObject = Instantiate(enemyPrefab, enemySpawn);
        enemyUnit = enemyGameObject.GetComponent<Unit>();

        dialogText.text = "Itt egy macska!";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        currentState = BattleStates.PLAYERCHOICE;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        dialogText.text = "Válassz egy képességet!";
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        enemyHUD.SetHP(enemyUnit.currentHealth, enemyUnit);

        dialogText.text = "A támadás sikeres!";

        yield return new WaitForSeconds(2f);

        if (isDead) {
            currentState = BattleStates.WON;
            StartCoroutine(EndBattle());
        }
        else
        {
            currentState = BattleStates.ENEMYCHOICE;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        dialogText.text = enemyUnit.unitName + " támad!";
        yield return new WaitForSeconds(1f);
        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
        playerHUD.SetHP(playerUnit.currentHealth, playerUnit);
        yield return new WaitForSeconds(1f);

        if (isDead) {
            currentState = BattleStates.LOST;
            StartCoroutine(EndBattle());
        }
        else
        {
            currentState = BattleStates.PLAYERCHOICE;
            PlayerTurn();
        }
    }

    IEnumerator EndBattle()
    {
        if (currentState == BattleStates.WON)
        {
            dialogText.text = "Nyertél!";
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene("GameMap");
        }
        else if (currentState == BattleStates.LOST)
        {
            dialogText.text = "Vesztettél!";
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene("GameMap");
        }
    }

    public void OnAttackButton()
    {
        if (currentState != BattleStates.PLAYERCHOICE)
            return;

        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton()
    {
        if (currentState != BattleStates.PLAYERCHOICE) { return; }
        else if (playerUnit.currentHealth == playerUnit.maxHealth)
        {
            dialogText.text = "Nincs szükséged gyógyításra!";
            return;
        }

        playerUnit.Heal(5);
        playerHUD.SetHP(playerUnit.currentHealth, playerUnit);

        currentState = BattleStates.ENEMYCHOICE;
        StartCoroutine(EnemyTurn());
    }
} 
