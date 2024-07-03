using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroController : MonoBehaviour
{
    [SerializeField] GameObject[] texts;
    CameraBehaviour _cameraController;
    bool _direction = true;

  
    public GameObject nextButton;
    public GameObject prevButton;
    int index = 0;

    void Start()
    {
       // AudioManager.instance.PlayMusic("intro");
        texts[index].SetActive(true);
        prevButton.SetActive(false);

        _cameraController = Camera.main.GetComponent<CameraBehaviour>();
    }

    public void ShowNext()
    {
        texts[index].SetActive(false);
        if (index == 3) index = 0;
        texts[++index].SetActive(true);

        if(index>0) prevButton.SetActive(true);

        ToggleButtons(index);

    }

    public void ShowPrev()
    {
        texts[index].SetActive(false);
        if (index == 0) index = 3;
        texts[--index].SetActive(true);

        ToggleButtons(index);

    }

    public void ToggleButtons(int index)
    {

        if (index == 0)
        {
            prevButton.SetActive(false);
        }
        else
        {
            prevButton.SetActive(true);
        }

        if (index == 3)
        {
            nextButton.SetActive(false);
        }
        else
        {
            nextButton.SetActive(true);
        }

        _cameraController.Rotate(_direction);
        _direction = !_direction;
    }



    public void SkipIntro()
    {
        SCManager.instance.LoadScene("Level1");
    }
}
