using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
        SetupBattle();
    }

    void SetupBattle()
    {
        GameObject playerGameObject = Instantiate(playerPrefab, playerSpawn);
        playerUnit = playerGameObject.GetComponent<Unit>();

        GameObject enemyGameObject = Instantiate(enemyPrefab, enemySpawn);
        enemyUnit = enemyGameObject.GetComponent<Unit>();

        dialogText.text = "Itt egy macska!";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);
    }
}
