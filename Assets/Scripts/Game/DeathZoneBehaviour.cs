using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZoneBehaviour : MonoBehaviour {

    void OnTriggerEnter(Collider other) => Destroy(other.gameObject);

    void OnCollisionEnter(Collision collision) => Destroy(collision.gameObject);
}
