using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }
    public int totalBalls = 0;
    public float status = 0;
    public GameObject HPBar;
    public bool isActive = false;
    int _playerCount = 0;
    int _enemyCount = 0;
    Coroutine _saturationCoroutine;

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
        HPBar = GameObject.Find("HPBar");
        if (HPBar == null) return;
        HPBar.GetComponent<Image>().fillAmount = 0;
        UpdateCounts();
    }

    public void UpdateCounts() {
        _playerCount = GameObject.FindGameObjectsWithTag("Player").Length -1;
        _enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        totalBalls = _playerCount + _enemyCount;
        UpdateStatus();
    }

    public void GameOver() {
        PauseGame();
        Debug.Log("Game Over");
    }

    public void UpdateStatus() {
        float oldStatus = status;
        totalBalls = _playerCount + _enemyCount;
        status = (float)_playerCount / totalBalls * 100;

        if (_saturationCoroutine != null) StopCoroutine(_saturationCoroutine);
        if (status != oldStatus) StartCoroutine(UpdateStatusBar(oldStatus));
        if (status >= 75) {
            StartCoroutine(DestroyWorld());
        } else if (status >= 60) {
            _saturationCoroutine = StartCoroutine(ChangeSaturationCoroutine(2f, -40f));
            StartGroupCrazy();
        } else if (status >= 50) {
            _saturationCoroutine = StartCoroutine(ChangeSaturationCoroutine(2f, -20f));
            StartGroupAlert();
        } else if (status >= 30) {
            _saturationCoroutine= StartCoroutine(ChangeSaturationCoroutine(2f, 0f));
            StartGroupPatrol();
        }
    }

    public void ChangePlayerCount() {
        _playerCount = GameObject.FindGameObjectsWithTag("Player").Length - 1;
        _enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
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
        int count = 20;
        StartCoroutine(ChangeSaturationCoroutine(count));
        while (count > 0) {
            yield return new WaitForSeconds(1f);
            count--;
        }
        GameOver();
    }

    public IEnumerator ChangeSaturationCoroutine(float duration, float endSaturation = -100f) {
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

    IEnumerator UpdateStatusBar(float oldStatus) {
        Debug.Log(oldStatus + " " + status);
        if (oldStatus > status) {
            for (float i = oldStatus; i >= status; i--) {
                HPBar.GetComponent<Image>().fillAmount = i / 100;
                yield return new WaitForSeconds(0.1f);
            }
        } else
            for (float i = oldStatus; i <= status; i++) {
                HPBar.GetComponent<Image>().fillAmount = i / 100;
                yield return new WaitForSeconds(0.1f);
            }
    }
}
