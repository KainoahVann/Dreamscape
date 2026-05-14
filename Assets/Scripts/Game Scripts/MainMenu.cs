using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Scene Names (must match Build Settings exactly)")]
    public string fpsGameScene      = "FPSLevel";
    public string survivalGameScene = "SurvivalLevel";

    public void StartFPS()
    {
        SceneManager.LoadScene(fpsGameScene);
    }

    public void StartSurvival()
    {
        SceneManager.LoadScene(survivalGameScene);
    }

    public void MainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}