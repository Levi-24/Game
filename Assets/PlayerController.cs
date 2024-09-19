using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public Tilemap tilemap;
    float moveTime = .2f;
    public Vector3Int currentCell;
    public bool isMoving = false;

    private void Start()
    {
        currentCell = tilemap.WorldToCell(transform.position);
        transform.position = tilemap.GetCellCenterWorld(currentCell);
    }

    private void Update()
    {
        if (!isMoving)
        {
            Vector3Int direction = Vector3Int.zero;
            if (Input.GetKey(KeyCode.W))
            direction = new Vector3Int(0, 1, 0);  // Fel (W)
            else if (Input.GetKey(KeyCode.S))
            direction = new Vector3Int(0, -1, 0); // Le (S)
            else if (Input.GetKey(KeyCode.A))
            direction = new Vector3Int(-1, 0, 0); // Bal (A)
            else if (Input.GetKey(KeyCode.D))
            direction = new Vector3Int(1, 0, 0);  // Jobb (D)
            if (direction != Vector3Int.zero)
            {
                Vector3Int targetCell = currentCell + direction;

                if (IsValidTile(targetCell))
                {
                    StartCoroutine(MoveToCell(targetCell));
                }
            }
        }
    }

    private IEnumerator MoveToCell(Vector3Int targetCellPosition)       // Mozgás a jelenlegi cellától a cél celláig
    {
        isMoving = true;

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = tilemap.GetCellCenterWorld(targetCellPosition);

        float elapsedTime = 0;

        while(elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / moveTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        currentCell = targetCellPosition;
        isMoving = false;
    }

    private bool IsValidTile(Vector3Int targetCell)     // Ha a cél tile neve = TestTile, akkor lehet arra moydulni
    {
        TileBase tile = tilemap.GetTile(targetCell);
        if (tile.name == "TestTile") return true;
        else return tile != null;
    }
}
