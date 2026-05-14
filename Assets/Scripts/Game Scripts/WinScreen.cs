using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    [Tooltip("Must match the scene name in Build Settings exactly")]
    [SerializeField] string mainMenuScene = "Main Menu";
    void OnEnable()
    {
        Cursor.visible   = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale   = 1f;   
    }


}