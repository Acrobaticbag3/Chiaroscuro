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

            Vector2 mousePos = Input.mousePosition;
            Vector2 world = mainCamera.ScreenToWorldPoint(mousePos);
            float dragDistance = (dragStart - mousePos).magnitude;
            bool fleetMode = mainCamera.orthographicSize >= fleetZoomThreshold;
            bool addToSelection = Input.GetKey(KeyCode.LeftShift);

            // === CASE 1: Fleet Selection ===
            if (fleetMode)
            {
                var oldFleet = selectedFleet;
                selectedFleet = fleetManager.TryPickFleet(world);

                // Deselect old
                if (oldFleet != selectedFleet)
                {
                    oldFleet?.SetSelected(false);
                    if (!addToSelection) DeselectAllShips();
                }

                selectedFleet?.SetSelected(true);

                // In fleet ship selection
                if (selectedFleet != null && !addToSelection)
                {
                    DeselectAllShips();
                    foreach (var ship in selectedFleet.Members)
                        ship.IsSelected = true;
                }
            }
            // === CASE 2: Single Click Selection ===
            else if (dragDistance < 2f)
            {
                if (!addToSelection) DeselectAllShips();
                selectedFleet = null;

                ShipModel clicked = TryPickClosestShip(world, 0.5f);
                if (clicked != null)
                    clicked.IsSelected = true;
            }
            // === CASE 3: Box Selection ===
            else
            {
                if (!addToSelection) DeselectAllShips();
                selectedFleet = null;

                Rect r = GetScreenRect(dragStart, mousePos);
                foreach (var ship in ShipManager.AllShips)
                {
                    Vector2 screenPos = mainCamera.WorldToScreenPoint(ship.Position);
                    if (r.Contains(screenPos))
                        ship.IsSelected = true;
                }
            }
        }
    }

    // Pixed to pick the closest ship
    private ShipModel TryPickClosestShip(Vector2 worldPoint, float radius)
    {
        ShipModel closest = null;
        float closestSqr = radius * radius;

        foreach (var ship in ShipManager.AllShips)
        {
            float sqr = (ship.Position - worldPoint).sqrMagnitude;
            if (sqr <= closestSqr)
            {
                closestSqr = sqr;
                closest = ship;
            }
        }
        return closest;
    }

    private void HandleRightMouse()
    {
        if (!Input.GetMouseButtonDown(1)) return;

        Vector2 world = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        if (selectedFleet != null)
        {
            selectedFleet.ExecuteFleetOrder(world);
        }
        else
        {
            foreach (var ship in ShipManager.AllShips)
            {
                if (ship.IsSelected)
                {
                    if (ship.Behavior != null)
                        ship.Behavior.ExecuteOrder(ship, world);
                    else
                        ship.TargetPosition = world;
                }
            }
        }
    }

    private Rect GetScreenRect(Vector2 start, Vector2 end)
    {
        // Builds a rect in bottom-left origin space (same as WorldToScreenPoint)
        Vector2 bl = new(Mathf.Min(start.x, end.x), Mathf.Min(start.y, end.y));
        Vector2 size = new(Mathf.Abs(start.x - end.x), Mathf.Abs(start.y - end.y));
        return new Rect(bl, size);
    }

    private void DeselectAllShips()
    {
        foreach (var ship in ShipManager.AllShips) ship.IsSelected = false;
    }

    // === === === === === === === === === === === === === === === === === \\
    // === === === === === OnGUI only affects visuals  === === === === === \\
    // === === === === === === === === === === === === === === === === === \\

    private void OnGUI()
    {
        if (!dragging) return;

        Rect r = GetScreenRect(dragStart, Input.mousePosition);
        if (r.width < 2f || r.height < 2f) return;

        // Convert rect into top-left origin space for IMGUI drawing
        r.y = Screen.height - r.y - r.height;

        Color c = new Color(0.2f, 0.8f, 1f, 0.15f);
        Color b = new Color(0.2f, 0.8f, 1f, 0.8f);

        DrawRect(r, c);
        DrawRectOutline(r, 2f, b);
    }

    // === Quick IMGUI helpers ===
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
