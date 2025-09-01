using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetGroup
{
    private int fleetId;
    private List<ShipModel> members = new();
    private Vector2 center;
    private float radius;
    private bool isSelected;

    [SerializeField] public FleetRingView ringView;

    public int FleedId
    {
        get => fleetId;
        set => fleetId = value;
    }

    public List<ShipModel> Members
    {
        get => members;
        set => members = value;
    }

    public Vector2 Center
    {
        get => center;
        set => center = value;
    }

    public float Radius
    {
        get => radius;
        set => radius = value;
    }

    public bool IsSelected
    {
        get => isSelected;
        set => isSelected = value;
    }

    public void UpdateGeometry()
    {
        if (members.Count == 0)
        {
            center = Vector2.zero;
            radius = 0f;
            return;
        }

        // center = avrg
        Vector2 sum = Vector2.zero;
        foreach (var member in members) sum += member.Position;
        center = sum / members.Count;

        // Radius = max dist --> center + padding
        float rad = 0f;
        foreach (var member in members) rad = Mathf.Max(rad, (member.Position - center).sqrMagnitude);

        radius = Mathf.Sqrt(rad) + 1.2f;
        ringView?.SetCircle(center, radius, isSelected);
    }

    public void ExecuteFleetOrder(Vector2 target)
    {
        foreach (var member in members)
        {
            var behaviour = member.Behavior;
            if (behaviour != null) behaviour.ExecuteOrder(member, target);
            else member.TargetPosition = target;    // Fallback in case of no defined behaviour
        }
    }
}
