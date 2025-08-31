using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    CameraModel. 
    Stores camera state (position, rotation, zoom).
    Holds tweakable paramaters (MoveSpeed, ZoomSpeed).
    Does not know about input or actual camera transform.
*/
public class CameraModel : MonoBehaviour {
    // Internal
    [SerializeField] private Vector3 position = Vector3.zero;
    [SerializeField] private float rotation = 0f;
    [SerializeField] private float zoom = 2f;

    // Tweakable
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float maxZoom = 20;
    [SerializeField] private float minZoom = 5f;

    // Controlled access
    public Vector3 Position {
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

    // Read only
    public float MoveSpeed => moveSpeed;
    public float ZoomSpeed => zoomSpeed;
    public float MaxZoom => maxZoom;
    public float MinZoom => minZoom;
}
