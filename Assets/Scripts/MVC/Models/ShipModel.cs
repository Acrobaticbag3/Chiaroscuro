using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipModel : MonoBehaviour
{
    // Internals
    [Header("State")]
    [SerializeField] private bool isSelected = false;
    [SerializeField] public int groupId = -1;
    [SerializeField] private Vector2 targetPosition;

    [Header("Role")]
    public MonoBehaviour behaviourComponent;

    public Vector2 Position
    {
        get => transform.position;  // flatten to Vector2
        set => transform.position = new Vector3(value.x, value.y, transform.position.z);
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

    public IShipBehaviour Behavior => behaviourComponent as IShipBehaviour;

    public void Awake()
    {
        ShipManager.RegisterShip(this);
    }

    public void OnDestroy()
    {
        ShipManager.UnregisterShip(this);
    }
}
