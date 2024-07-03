using UnityEngine;

public class SyncController : MonoBehaviour {

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            PlayerController mobController = GetComponent<PlayerController>();
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            mobController.enabled = true;
            mobController.SetAttributes(playerController.GetSpeed(), playerController.GetJumpForce());
            GetComponent<Renderer>().material.color = Color.red;
            Debug.Log("Contact!");
        }
    }
}