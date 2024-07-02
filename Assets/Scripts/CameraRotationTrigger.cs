using System;
using UnityEngine;

public class CameraRotationTrigger : MonoBehaviour
{
    CameraBehaviour cameraController;
    bool direction = true;

    void Start() {
        cameraController = Camera.main.GetComponent<CameraBehaviour>();
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            PlayerBehaviour playerMovement = other.GetComponent<PlayerBehaviour>();
            cameraController.Rotate(direction);
            direction = !direction;
        }
    }
}
