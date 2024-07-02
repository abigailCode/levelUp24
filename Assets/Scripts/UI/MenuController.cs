using UnityEngine;

public class MenuController : MonoBehaviour {

    public void StartGame() => SCManager.instance.LoadScene("GameScene");

    public void GoToSettings() => SCManager.instance.LoadScene("GeneralSettingsScene");

    public void GoToRanking() => SCManager.instance.LoadScene("RankingScene");

    public void GoToMenu() => SCManager.instance.LoadScene("MenuScene");

    public void ReloadScene(string scene) => SCManager.instance.LoadScene(scene);

    public void ExitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE_WIN
        Application.Quit();
#endif
    }
}