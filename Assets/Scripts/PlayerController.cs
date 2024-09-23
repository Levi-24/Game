using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public Tilemap tilemap;                // Reference to the Tilemap that defines the walkable area.
    public Tilemap obstaclesTilemap;       // Reference to another Tilemap that defines obstacles (e.g., walls).
    public float moveTime = 0.3f;          // Time taken to move from one tile to another.
    private Vector3Int currentCell;        // The player's current tile position.
    private bool isMoving = false;         // Indicates whether the player is currently moving.

    float detectionRange = 1f;             // The detection range for enemies.
    GameObject closestEnemy;                // Reference to the closest enemy.

    private void Start()
    {
        // Check if the Tilemap and obstacles Tilemap are assigned.
        if (tilemap == null)
        {
            Debug.LogError("Tilemap is not assigned in PlayerController.");
            return;
        }

        if (obstaclesTilemap == null)
        {
            Debug.LogError("Obstacles Tilemap is not assigned in PlayerController.");
            return;
        }

        // Initialize the player's position to the center of the Tilemap.
        currentCell = tilemap.WorldToCell(transform.position);
        transform.position = tilemap.GetCellCenterWorld(currentCell);
    }

    private void Update()
    {
        // Proceed only if the player is not moving and the Tilemap exists.
        if (!isMoving && tilemap != null)
        {
            Vector3Int direction = GetInputDirection(); // Get the input direction.

            // If there is input, start moving.
            if (direction != Vector3Int.zero)
            {
                Vector3Int targetCell = currentCell + direction; // Determine the target cell.

                // Check if the target cell is valid.
                if (IsValidTile(targetCell))
                {
                    StartCoroutine(MoveToCell(targetCell)); // Start the movement.
                }
                else
                {
                    Debug.Log("Invalid tile at position: " + targetCell);
                }
                DetectNearbyEnemy(); // Check for nearby enemies.
            }
        }
    }

    private Vector3Int GetInputDirection()
    {
        // Determine input direction based on WASD keys.
        if (Input.GetKey(KeyCode.W)) return new Vector3Int(0, 1, 0);  // Up
        if (Input.GetKey(KeyCode.S)) return new Vector3Int(0, -1, 0); // Down
        if (Input.GetKey(KeyCode.A)) return new Vector3Int(-1, 0, 0); // Left
        if (Input.GetKey(KeyCode.D)) return new Vector3Int(1, 0, 0);  // Right
        return Vector3Int.zero; // No input
    }

    private IEnumerator MoveToCell(Vector3Int targetCellPosition)
    {
        isMoving = true; // Indicate that the player is moving.

        Vector3 startPosition = transform.position; // Starting position.
        Vector3 targetPosition = tilemap.GetCellCenterWorld(targetCellPosition); // Target position.

        float elapsedTime = 0f;
        // Interpolate step by step between the two positions.
        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime; // Increment time.
            yield return null; // Wait for the next frame.
        }

        transform.position = targetPosition; // Set the final position.
        currentCell = targetCellPosition; // Update the current tile position.
        isMoving = false; // Movement is finished.

        Debug.Log("Moved to cell: " + targetCellPosition);
    }

    private bool IsValidTile(Vector3Int targetCell)
    {
        // Check if there is an obstacle on the target tile.
        TileBase obstacleTile = obstaclesTilemap.GetTile(targetCell);
        if (obstacleTile != null)
        {
            Debug.Log("Obstacle at position: " + targetCell);
            return false; // Invalid tile.
        }

        // Check if the target tile exists on the main Tilemap.
        TileBase tile = tilemap.GetTile(targetCell);
        if (tile == null)
        {
            return false; // Invalid tile.
        }

        return true; // Valid tile.
    }

    void DetectNearbyEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // Find all enemies.

        closestEnemy = null; // Reset the closest enemy.

        // Check each enemy to see if any are nearby.
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position); // Calculate distance.

            if (distance <= detectionRange) // If the enemy is within range.
            {
                closestEnemy = enemy; // Set the closest enemy.
            }
        }

        // If there is a nearby enemy, start the battle.
        if (closestEnemy != null)
        {
            TriggerBattleScene(closestEnemy);
        }
    }

    void TriggerBattleScene(GameObject enemy)
    {
        BattleSystem.EnemyToFight = enemy.name; // Set the enemy name for the battle system.

        SceneManager.LoadScene("BattleScene"); // Switch to the battle scene.
    }
}
