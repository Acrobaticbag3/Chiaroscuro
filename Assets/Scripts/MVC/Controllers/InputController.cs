using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private FleetManager fleetManager;

    [Header("Settings")]
    [SerializeField] private float fleetZoomThreshold = 10f;

    //Selection box
    private Vector2 dragStart;
    private bool dragging;

    // selection state
    private readonly List<ShipModel> tempSelection = new();
    private FleetGroup selectedFleet;

    void Update()
    {
        HandleLeftMouse();
        HandleRightMouse();
    }

    private void HandleLeftMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragStart = Input.mousePosition;
            dragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
            bool fleetMode = mainCamera.orthographicSize >= fleetZoomThreshold;

            if (fleetMode)
            {
                // fleet ring selection
                Vector2 world = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                DeselectAllShips();
                selectedFleet = fleetManager.TryPickFleet(world);
                if (selectedFleet != null)
                {
                    selectedFleet.IsSelected = true;
                    selectedFleet.UpdateGeometry();
                }
                else
                {
                    // Click empty space --> clear fleet selection
                    if (selectedFleet != null)
                    {
                        selectedFleet.IsSelected = false;
                        selectedFleet.UpdateGeometry();
                    }
                    selectedFleet = null;
                }
            }
        }
        else
        {
            // Box select ships
            Rect r = GetScreenRect(dragStart, Input.mousePosition);
            bool addToSelection = Input.GetKey(KeyCode.LeftShift);

            if (!addToSelection) DeselectAllShips();
            selectedFleet = null;

            foreach (var ship in ShipManager.AllShips)
            {
                Vector2 screenPos = mainCamera.WorldToScreenPoint(ship.Position);
                if (r.Contains(screenPos))
                {
                    ship.IsSelected = true;
                }
            }
        }
    }

    private void HandleRightMouse()
    {
        if (!Input.GetMouseButtonDown(1)) return;

        Vector2 world = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        bool fleetMode = mainCamera.orthographicSize >= fleetZoomThreshold;

        if (fleetMode && selectedFleet != null)
        {
            selectedFleet.ExecuteFleetOrder(world);
        }
        else
        {
            foreach (var ship in ShipManager.AllShips)
                if (ship.IsSelected)
                {
                    // Delegate to own behaviour
                    if (ship.Behavior != null) ship.Behavior.ExecuteOrder(ship, world);
                    else ship.TargetPosition = world;
                }
        }
    }

    private Rect GetScreenRect(Vector2 start, Vector2 end)
    {
        Vector2 bl = new(Mathf.Min(start.x, end.x), Mathf.Min(start.y, end.y));
        Vector2 size = new(Mathf.Abs(start.x - end.x), Mathf.Abs(start.y - end.y));
        return new Rect(bl, size);
    }

    private void DeselectAllShips()
    {
        foreach (var ship in ShipManager.AllShips) ship.IsSelected = false;
    }

    // === === === === === === === === === === === === === === === === === \\
    // === === === move to view layer once prototyping is done === === === \\
    // === === === === === === === === === === === === === === === === === \\

    // Draw selection
    // IMGUI for quick prototyping, replace later, probably
    private void OnGUI()
    {
        if (!dragging) return;

        Rect r = GetScreenRect(dragStart, Input.mousePosition);
        if (r.width < 2f || r.height < 2f) return;

        // Flip
        r.y = Screen.height - r.y - r.height;

        // Draw box, move to view layer once prototyping done
        Color c = new Color(0.2f, 0.8f, 1f, 0.15f);
        Color b = new Color(0.2f, 0.8f, 1f, 0.8f);

        DrawRect(r, c);
        DrawRectOutline(r, 2f, b);
    }

    // Quick IMGUI helpers
    private static Texture2D _tex;
    private static Texture2D WhiteTex => _tex ??= new Texture2D(1, 1);

    private static void DrawRect(Rect r, Color col)
    {
        var prev = GUI.color;
        GUI.color = col;
        GUI.DrawTexture(r, WhiteTex);
        GUI.color = prev;
    }

    private static void DrawRectOutline(Rect r, float thickness, Color col)
    {
        DrawRect(new Rect(r.xMin, r.yMin, r.width, thickness), col);
        DrawRect(new Rect(r.xMin, r.yMax - thickness, r.width, thickness), col);
        DrawRect(new Rect(r.xMin, r.yMin, thickness, r.height), col);
        DrawRect(new Rect(r.xMax - thickness, r.yMin, thickness, r.height), col);
    }
}
