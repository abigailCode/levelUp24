using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SCManager : MonoBehaviour {
    public static SCManager instance;

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else Destroy(gameObject);

    }

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}