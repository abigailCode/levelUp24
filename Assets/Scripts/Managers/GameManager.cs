using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }
    public int followersCount = 0;
    public bool isActive = true;
    int _status = 0;

    enum Status {
        Motionless = 0,
        Patrolling = 20,
        Alert = 50,
        Crazy = 70
    }

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else Destroy(gameObject);
    }

    public void PauseGame() {
        isActive = false;
    }

    public void ResumeGame() {
        isActive = true;
    }

    public void GameOver() {
        PauseGame();
        Debug.Log("Game Over");
    }

    /**
     * By default the enemies are motionless.
     * When the status of the world reaches 20%, the enemies start patrolling.
     * When the status of the world reaches 50%, the enemies start being in alert.
     * When the status of the world reaches 70%, the enemies start going crazy.
     */
    public void UpdateStatus(int value) {
        _status += value;
        if (_status >= ((int)Status.Patrolling) && _status < ((int)Status.Alert)) {
            // Start patrolling
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies) {
                EnemyController enemyController = enemy.GetComponent<EnemyController>();
                enemyController.StartGroupPatrol(enemyController.GetGroupMembers());
            }
        } else if (_status >= ((int)Status.Alert) && _status < ((int)Status.Crazy)) {
            // Start alert
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies) {
                EnemyController enemyController = enemy.GetComponent<EnemyController>();
                enemyController.StartGroupAlert(enemyController.GetGroupMembers());
            }
        } else if (_status >= ((int)Status.Crazy)) {
            // Start going crazy
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies) {
                EnemyController enemyController = enemy.GetComponent<EnemyController>();
                enemyController.StartGroupCrazy(enemyController.GetGroupMembers());
            }
        } else StartCoroutine(DestroyWorld());
    }

    IEnumerator DestroyWorld() {
        int count = 0;
        while (count < 10) {
            // Lower the saturation of the world
            yield return new WaitForSeconds(1f);
            count++;
        }
        GameOver();
    }
}
