using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    [Header("|----------------Next Level Scene Name----------------|")]
    public string nextLevel;
    private void OnTriggerEnter(Collider other) {
        SCManager.Instance.LoadScene(nextLevel);
        GameManager.Instance.NextLevel();
    }
}
