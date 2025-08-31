using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipModel : MonoBehaviour
{
    // Internals
    private Vector2 position;
    private bool isSelected = false;
    public int groupId = -1;
    private Vector2 targetPosition;

    public Vector2 Position
    {
        get => transform.position;  // flatten to Vector2
        set => transform.position = value;
    }

    public bool IsSelected
    {
        get => isSelected;
        set => isSelected = value;
    }

    public int GroupId
    {
        get => groupId;
        set => groupId = value;
    }

    public Vector2 TargetPosition
    {
        get => targetPosition;
        set => targetPosition = value;
    }

    public void Awake()
    {
        ShipManager.RegisterShip(this);
    }

    public void OnDestroy()
    {
        ShipManager.UnregisterShip(this);
    }
}
