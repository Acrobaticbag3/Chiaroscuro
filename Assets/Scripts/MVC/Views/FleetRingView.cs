using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetRingView : MonoBehaviour
{
    [SerializeField] private int segments = 64;
    [SerializeField] private float selectedWidth = 0.15f;
    [SerializeField] private float normalWidth = 0.06f;

    private LineRenderer lr;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.loop = true;
        lr.useWorldSpace = true;
        lr.positionCount = segments;
    }

    public void SetCircle(Vector2 center, float radius, bool selected)
    {
        float angleStep = 2f * Mathf.PI / segments;
        for (int i = 0; i < segments; i++)
        {
            float a = i * angleStep;
            Vector3 position = new Vector3(center.x + Mathf.Cos(a) * radius, center.y + Mathf.Sin(a) * radius, 0f);
            lr.SetPosition(i, position);
        }
        lr.widthMultiplier = selected ? selectedWidth : normalWidth;
    }

    public bool IsPointInside(Vector2 worldPoint, Vector2 center, float radius)
    {
        return (worldPoint - center).sqrMagnitude <= radius * radius;
    }
}
