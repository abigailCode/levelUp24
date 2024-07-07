using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        AudioManager.Instance.PlayMusic("mainTheme");
    }

    public void PauseGame() {
        StopAllCoroutines();
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
        if (status != oldStatus) {
            if (_updateStatusBar != null) StopCoroutine(_updateStatusBar);
            _updateStatusBar = StartCoroutine(UpdateStatusBar(oldStatus));
        }
        if (status >= 75 && _destroyWorldCoroutine == null) {
            _destroyWorldCoroutine = StartCoroutine(DestroyWorld());
        } else if (status >= 60 && groupStatus == 2) {
            _saturationCoroutine = StartCoroutine(ChangeSaturationCoroutine(2f, -40f));
            StartGroupCrazy();
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
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            enemyController.StopAllCoroutines();
            enemyController.StartGroupPatrol();
        }
    }

    void StartGroupAlert() {
        groupStatus = 2;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            enemy.GetComponent<SyncController>().ChangeBalls(2);
            enemyController.StopAllCoroutines();
            enemyController.StartGroupAlert();
        }
    }

    void StartGroupCrazy() {
        groupStatus = 3;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) {
            enemy.GetComponent<SyncController>().ChangeBalls(3);
            //EnemyController enemyController = enemy.GetComponent<EnemyController>();
            //enemyController.StopAllCoroutines();
            //enemyController.StartGroupCrazy();
        }
    }

    IEnumerator DestroyWorld() {
        int count = 20;
        CameraShake cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        GameObject countdown = GameObject.Find("CountDown");
        TMP_Text countdownText = countdown.GetComponent<TMP_Text>();
        countdown.GetComponent<Animator>().enabled = true;
        StartCoroutine(ChangeSaturationCoroutine(count));
        AudioManager.Instance.PlaySFX("countdown");
        while (count > 0) {
            cameraShake.Shake(0.5f, 0.7f);
            countdownText.text = count.ToString();
            yield return new WaitForSeconds(1f);
            count--;
        }
        AudioManager.Instance.StopSFX();
        countdownText.text = "";
        countdown.GetComponent<Animator>().enabled = false;
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
