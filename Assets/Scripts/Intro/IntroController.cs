using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroController : MonoBehaviour {
    [SerializeField] GameObject[] texts;
    CameraBehaviour _cameraController;
    bool _direction = false;
    [SerializeField] GameObject playerSection2;
    [SerializeField] GameObject section3;


    public GameObject nextButton;
    public GameObject prevButton;
    public GameObject startButton;
    int index = 0;

    void Start() {
        // AudioManager.Instance.PlayMusic("intro");
        texts[index].SetActive(true);
        prevButton.SetActive(false);

        _cameraController = Camera.main.GetComponent<CameraBehaviour>();
    }

    public void ShowNext() {
        _direction = true;
        texts[index].SetActive(false);
        if (index == 3) index = 0;
        texts[++index].SetActive(true);

        if (index > 0) prevButton.SetActive(true);

        Toggle(index);
    }

    public void ShowPrev() {
        _direction = false;
        texts[index].SetActive(false);
        if (index == 0) index = 3;
        texts[--index].SetActive(true);

        Toggle(index);
    }

    void Toggle(int index) {
        ToggleButtons(index);
        TogglePlayerSection2Animator(index);
        ToggleSection3Animator(index);
        ToggleSaturation(index);
    }

    public void ToggleButtons(int index) {

        if (index == 0) {
            prevButton.SetActive(false);
        } else {
            prevButton.SetActive(true);
        }

        if (index == 3) {
            nextButton.SetActive(false);
            startButton.SetActive(true);
        } else {
            nextButton.SetActive(true);
            startButton.SetActive(false);
        }

        _cameraController.Rotate(_direction);
    }

    public void TogglePlayerSection2Animator(int i) {
        playerSection2.SetActive(i == 1);
    }

    public void ToggleSection3Animator(int i) {
        Debug.Log("i: " + i);
        section3.SetActive(i == 2);
        section3.transform.Find("Player").GetComponent<Animator>().Play("playerIntroSection3");
    }

    public void ToggleSaturation(int index) {
        StopCoroutine(GameManager.Instance.ChangeSaturationCoroutine(1));
        if (index == 3) StartCoroutine(GameManager.Instance.ChangeSaturationCoroutine(5));
        else StartCoroutine(GameManager.Instance.ChangeSaturationCoroutine(5, 5));
    }
}
