using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraModel : MonoBehaviour {
    // Internal
    [SerializeField] private Vector2 position = Vector2.zero;
    [SerializeField] private float rotation = 0f;
    [SerializeField] private float zoom = 2f;

    // Tweakable
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float maxZoom = 20f;
    [SerializeField] private float minZoom = 2f;

    // Controlled access
    public Vector2 Position {
        get => position;
        set => position = value;
    }

    public float Rotation {
        get => rotation;
        set => rotation = value;
    }

    public float Zoom {
        get => zoom;
        set => zoom = Mathf.Clamp(value, minZoom, maxZoom);
    }

    // Read only --> Potential later use, maybe remove??
    public float MoveSpeed => moveSpeed;
    public float ZoomSpeed => zoomSpeed;
    public float MaxZoom => maxZoom;
    public float MinZoom => minZoom;
}
