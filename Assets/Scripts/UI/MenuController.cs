using UnityEngine;

public class MenuController : MonoBehaviour {
    public MenuController menuController;

    public void PerformAction(string action, string scene = "") {
        if (!GameManager.instance.isActive)
            return;

        switch (action) {
            case "StartGame":
                SCManager.instance.LoadScene("GameScene");
                break;
            case "GoToSettings":
                SCManager.instance.LoadScene("GeneralSettingsScene");
                break;
            case "GoToRanking":
                SCManager.instance.LoadScene("RankingScene");
                break;
            case "GoToMenu":
                SCManager.instance.LoadScene("MenuScene");
                break;
            case "LoadScene":
                SCManager.instance.LoadScene(scene);
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

    public void StartGame() => menuController.PerformAction("StartGame");

    public void GoToSettings() => menuController.PerformAction("GoToSettings");

    public void GoToRanking() => menuController.PerformAction("GoToRanking");

    public void GoToMenu() => menuController.PerformAction("GoToMenu");

    public void LoadScene(string scene) => menuController.PerformAction("LoadScene", scene);

    public void ExitGame() => menuController.PerformAction("ExitGame");
}