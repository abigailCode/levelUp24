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
    Coroutine _destroyWorldCoroutine;
    Coroutine _updateStatusBar;
    int groupStatus = 0;

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
        Debug.Log("Status: " + status);
        if (_saturationCoroutine != null) StopCoroutine(_saturationCoroutine);
        if (status != oldStatus) {
            if (_updateStatusBar != null) StopCoroutine(_updateStatusBar);
            _updateStatusBar = StartCoroutine(UpdateStatusBar(oldStatus));
        }
        if (status >= 75 && _destroyWorldCoroutine == null) {
            _destroyWorldCoroutine = StartCoroutine(DestroyWorld());
        } else if (status >= 60 && groupStatus == 2) {
            _saturationCoroutine = StartCoroutine(ChangeSaturationCoroutine(2f, -40f));
            //StartGroupCrazy();
        } else if (status >= 40 && groupStatus == 1) {
            _saturationCoroutine = StartCoroutine(ChangeSaturationCoroutine(2f, -20f));
            StartGroupAlert();
        } else if (status >= 20 && groupStatus == 0) {
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
        groupStatus = 1;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) {
            Debug.Log("Starting patrol");
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            enemyController.StopAllCoroutines();
            enemyController.StartGroupPatrol();
        }
    }

    void StartGroupAlert() {
        groupStatus = 2;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) {
            Debug.Log("Starting alert");
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            enemyController.StopAllCoroutines();
            enemyController.StartGroupAlert();
        }
    }

    void StartGroupCrazy() {
        groupStatus = 3;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            enemyController.StopAllCoroutines();
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

    public void NextLevel() {
        StopAllCoroutines();
        ResumeGame();
        status = 0;
        groupStatus = 0;
        _saturationCoroutine = null;
        _destroyWorldCoroutine = null;
    }
}
