using UnityEngine;

public class SyncController : MonoBehaviour {

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            GetComponent<PlayerController>().enabled = true;
            GetComponent<Renderer>().material.color = Color.red;
        }
    }
}