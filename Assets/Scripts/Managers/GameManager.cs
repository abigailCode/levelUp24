using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }
    public int totalBalls = 4;
    public bool isActive = true;
    private int _playerCount = 1; // Start with 1 player
    private int _enemyCount;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else Destroy(gameObject);

        _enemyCount = totalBalls - _playerCount;
        UpdateStatus();
    }

    public void PauseGame() {
        isActive = false;
    }

    public void ResumeGame() {
        isActive = true;
    }

    public void UpdateCounts() {
        _playerCount = GameObject.FindGameObjectsWithTag("Player").Length;
        _enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        totalBalls = _playerCount + _enemyCount;
        UpdateStatus();
    }

    public void GameOver() {
        PauseGame();
        Debug.Log("Game Over");
    }

    public void UpdateStatus() {
        int totalBalls = _playerCount + _enemyCount;
        float status = (float)_playerCount / totalBalls * 100;

        if (status >= 70) {
            StopCoroutine(ChangeSaturationCoroutine(2f));
            StartCoroutine(DestroyWorld());
        }
        if (status >= 60) {
            StartCoroutine(ChangeSaturationCoroutine(2f, -10f));
            StartGroupCrazy();
        } else if (status >= 40) {
            StartCoroutine(ChangeSaturationCoroutine(2f, -40f));
            StartGroupAlert();
        } else if (status >= 20) {
            StartCoroutine(ChangeSaturationCoroutine(2f, -70f));
            StartGroupPatrol();
        }
    }

    public void ChangePlayerCount(int change) {
        _playerCount += change;
        _enemyCount -= change;
        UpdateStatus();
    }

    void StartGroupPatrol() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            enemyController.StartGroupPatrol();
        }
    }

    void StartGroupAlert() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            enemyController.StartGroupAlert();
        }
    }

    void StartGroupCrazy() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            enemyController.StartGroupCrazy();
        }
    }

    IEnumerator DestroyWorld() {
        int count = 10;
        StartCoroutine(ChangeSaturationCoroutine(count));
        while (count > 0) {
            yield return new WaitForSeconds(1f);
            count--;
        }
        GameOver();
    }

    private IEnumerator ChangeSaturationCoroutine(float duration, float endSaturation = -100f) {
        PostProcessVolume postProcessingVolume = FindObjectOfType<PostProcessVolume>();
        postProcessingVolume.profile.TryGetSettings(out ColorGrading colorAdjustments);
        float startSaturation = colorAdjustments.saturation.value;
        float elapsedTime = 0f;

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float newSaturation = Mathf.Lerp(startSaturation, endSaturation, elapsedTime / duration);
            colorAdjustments.saturation.value = newSaturation;
            yield return null;
        }

        colorAdjustments.saturation.value = endSaturation;
    }
}
