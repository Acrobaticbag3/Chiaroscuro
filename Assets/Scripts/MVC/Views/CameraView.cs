using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Reads CameraModel every frame and applies to camera transform
    May handle smoothing, shake or other visual effect (later stage).
*/
public class CameraView : MonoBehaviour {
    [SerializeField] private CameraModel cameraModel;
    [SerializeField] private Transform cameraTransform;
    private Camera cam;

    private void Awake() => cam = cameraTransform.GetComponent<Camera>();

    private void LateUpdate() {
        // Does not break MVC as we're only reading from model.
        Vector3 pos = cameraModel.Position;
        float zoom = cameraModel.Zoom;
        float rot = cameraModel.Rotation;

        // y --> zoom, x/z --> model pos
        cameraTransform.position = Vector3.Lerp(
            cameraTransform.position,
            new Vector3(pos.x, pos.y, -10f),
            Time.deltaTime * 5f
        );

        cameraTransform.rotation = Quaternion.Lerp(
            cameraTransform.rotation,
            Quaternion.Euler(0f, 0f, cameraModel.Rotation),
            Time.deltaTime * 5f
        );

        cam.orthographicSize = Mathf.Lerp(
            cam.orthographicSize,
            cameraModel.Zoom,
            Time.deltaTime
        );
    }
}
