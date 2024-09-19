using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public Tilemap tilemap;
    public Tilemap obstaclesTilemap; // Új Tilemap az akadályokhoz
    public float moveTime = 0.2f;
    private Vector3Int currentCell;
    private bool isMoving = false;

    private void Start()
    {
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

        currentCell = tilemap.WorldToCell(transform.position);
        transform.position = tilemap.GetCellCenterWorld(currentCell);
    }

    private void Update()
    {
        if (!isMoving && tilemap != null)
        {
            Vector3Int direction = GetInputDirection();

            if (direction != Vector3Int.zero)
            {
                Vector3Int targetCell = currentCell + direction;

                if (IsValidTile(targetCell))
                {
                    StartCoroutine(MoveToCell(targetCell));
                }
                else
                {
                    Debug.Log("Invalid tile at position: " + targetCell);
                }
            }
        }
    }

    private Vector3Int GetInputDirection()
    {
        if (Input.GetKey(KeyCode.W)) return new Vector3Int(0, 1, 0);  // Fel
        if (Input.GetKey(KeyCode.S)) return new Vector3Int(0, -1, 0); // Le
        if (Input.GetKey(KeyCode.A)) return new Vector3Int(-1, 0, 0); // Bal
        if (Input.GetKey(KeyCode.D)) return new Vector3Int(1, 0, 0);  // Jobb
        return Vector3Int.zero;
    }

    private IEnumerator MoveToCell(Vector3Int targetCellPosition)
    {
        isMoving = true;

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = tilemap.GetCellCenterWorld(targetCellPosition);

        float elapsedTime = 0f;
        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        currentCell = targetCellPosition;
        isMoving = false;

        Debug.Log("Moved to cell: " + targetCellPosition);
    }

    private bool IsValidTile(Vector3Int targetCell)
    {
       
        TileBase obstacleTile = obstaclesTilemap.GetTile(targetCell);
        if (obstacleTile != null)
        {
            Debug.Log("Obstacle at position: " + targetCell);
            return false; 
        }

        TileBase tile = tilemap.GetTile(targetCell);
        if (tile == null)
        {
            return false;
        }

       
        return true;
    }
}
