using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    CameraController.
    Reads player input (WASD, mouse scroll, Q/E rotation).
    Updates the CameraModel accordingly.
    Does not move camera directly.
*/
public class CameraController : MonoBehaviour {
    // Internal
    [SerializeField] private CameraModel cameraModel;
    [SerializeField] private float rotationSpeed = 50f; // Since input response, does not belong in model, according to MVC

    private void Update() {
        HandleMovement();
        HandleZoom();
        HandleRotation();
    }

    private void HandleMovement() {
        // WASD
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 input = new Vector2(horizontal, vertical);
        if (input != Vector2.zero) {
            Vector2 newPos = cameraModel.Position + input * cameraModel.MoveSpeed * Time.deltaTime;
            cameraModel.Position = newPos;
        }
    }

    private void HandleZoom() {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f) {
            float newZoom = cameraModel.Zoom - scroll * cameraModel.ZoomSpeed * 10f; // Multiply zoom by arbitrary numb till it feel right
            cameraModel.Zoom = newZoom; // Model clamps
        }
    }

    private void HandleRotation() {
        float rotInput = 0f;
        if (Input.GetKey(KeyCode.Q)) rotInput = -1f;
        if (Input.GetKey(KeyCode.E)) rotInput = 1f;

        if (rotInput != 0) {
            float newRot = cameraModel.Rotation + rotInput * rotationSpeed * Time.deltaTime;
            cameraModel.Rotation = newRot;
        }
    }
}
