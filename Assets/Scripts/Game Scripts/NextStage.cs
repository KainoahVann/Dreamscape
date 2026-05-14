using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStage : MonoBehaviour
{
    [Tooltip("Exact name of the scene to load (must be added to Build Settings)")]
    public string targetScene;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            LoadScene();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Player"))
            LoadScene();
    }

    void LoadScene()
    {
        if (string.IsNullOrEmpty(targetScene))
        {
            Debug.LogWarning("NextStage: No target scene name set in Inspector.");
            return;
        }

        SceneManager.LoadScene(targetScene);
    }
}