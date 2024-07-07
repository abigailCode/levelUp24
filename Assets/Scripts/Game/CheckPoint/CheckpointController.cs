using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    [Header("|----------------Next Level Scene Name----------------|")]
    public string nextLevel;

    void Start() {
        if (!AudioManager.Instance.IsPlayingCountDown()) AudioManager.Instance.PlaySFX("portal");
    }

    private void OnTriggerEnter(Collider other) {
        if (!AudioManager.Instance.IsPlayingCountDown()) AudioManager.Instance.PlaySFX("portal");
        SCManager.Instance.LoadScene(nextLevel);
        GameManager.Instance.NextLevel();
    }
}
