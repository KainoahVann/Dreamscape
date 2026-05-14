using UnityEngine;
using UnityEngine.SceneManagement;

public class HouseObject : MonoBehaviour, IInteractable
{
    [Header("Scene")]
    [Tooltip("Must match the scene name exactly as it appears in Build Settings")]
    [SerializeField] string interiorSceneName;

    [Header("Prompt")]
    [Header("Audio")]
    [SerializeField] AudioClip doorSound;


    public void Interact(GameObject player)
    {
        if (string.IsNullOrEmpty(interiorSceneName))
        {
            Debug.LogWarning("[House] interiorSceneName is not set.");
            return;
        }

        if (doorSound != null)
            AudioSource.PlayClipAtPoint(doorSound, transform.position);

        SceneManager.LoadScene(interiorSceneName);
    }
}