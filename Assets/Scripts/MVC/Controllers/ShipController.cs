using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    private Vector2 selectionStart;
    private bool isSelecting;

    // Update is called once per frame
    void Update()
    {
        HandleSelectionInput();
        HandleOrders();
    }

    private void HandleSelectionInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            selectionStart = Input.mousePosition;
            isSelecting = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isSelecting = false;

            Rect selectionRect = GetScreenRect(selectionStart, Input.mousePosition);
            foreach (var ship in ShipManager.AllShips)
            {
                Vector2 screenPos = mainCamera.WorldToScreenPoint(ship.Position);
                ship.IsSelected = selectionRect.Contains(screenPos);
            }
        }
    }

    private Rect GetScreenRect(Vector2 start, Vector2 end)
    {
        Vector2 bottomLeft = new Vector2(Mathf.Min(start.x, end.x), Mathf.Min(start.y, end.y));
        Vector2 size = new Vector2(Mathf.Abs(start.x - end.x), Mathf.Abs(start.y - end.y));
        return new Rect(bottomLeft, size);
    }

    private void HandleOrders()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            foreach (var ship in ShipManager.AllShips)
            {
                if (ship.IsSelected)
                {
                    ship.TargetPosition = worldPos;
                }
            }
        }
    }
}
