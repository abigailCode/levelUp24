using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
    public MenuController menuController;
    public GameObject credits;
    public GameObject settings;

    public void PerformAction(string action, string scene = "") {
        //if (!GameManager.instance.isActive)
        //    return;

        switch (action) {
            case "GoToIntro":
                SCManager.Instance.LoadScene("Intro");
                break;
            case "StartGame":
                SCManager.Instance.LoadScene("Level1");
                break;
            case "ShowSettings":
                // SCManager.instance.LoadScene("GeneralSettingsScene");
                settings.SetActive(true);
                break;
            case "HideSettings":
                settings.SetActive(false);
                break;
            case "ShowCredits":
                credits.SetActive(true);
                break;
            case "HideCredits":
                credits.SetActive(false);
                break;
            case "GoToRanking":
                SCManager.Instance.LoadScene("RankingScene");
                break;
            case "GoToMenu":
                SCManager.Instance.LoadScene("MenuScene");
                break;
            case "LoadScene":
                SCManager.Instance.LoadScene(scene);
                break;
            case "ExitGame":
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE_WIN
                Application.Quit();
#endif
                break;
        }
    }

    public void GoToIntro() => menuController.PerformAction("GoToIntro");

    public void StartGame() => menuController.PerformAction("StartGame");

    public void ShowSettings() => menuController.PerformAction("ShowSettings");

    public void HideSettings() => menuController.PerformAction("HideSettings");

    public void ShowCredits() => menuController.PerformAction("ShowCredits");

    public void HideCredits() => menuController.PerformAction("HideCredits");

    public void GoToRanking() => menuController.PerformAction("GoToRanking");

    public void GoToMenu() => menuController.PerformAction("GoToMenu");

    public void LoadScene(string scene) => menuController.PerformAction("LoadScene", scene);

    public void ExitGame() => menuController.PerformAction("ExitGame");
}