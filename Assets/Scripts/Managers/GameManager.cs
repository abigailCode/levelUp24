using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance { get; private set; }
    int _status = 0;

    public bool isActive = true;

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else Destroy(gameObject);
    }

    public void PauseGame() {
        isActive = false;
    }

    public void ResumeGame() {
        isActive = true;
    }
}
