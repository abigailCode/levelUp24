using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    [Header("|----------------Next Level Scene Name----------------|")]
    public string nextLevel;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with " + other.name);
        if (other.CompareTag("Player"))
        {
            SCManager.Instance.LoadScene(nextLevel);
            GameManager.Instance.StopAllCoroutines();
            GameManager.Instance.ResumeGame();
        }
    }
}
