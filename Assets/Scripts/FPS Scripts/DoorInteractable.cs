using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    public string nextSceneName;
    public bool requiresKey = true;

    [Header("Sounds")]
    public AudioSource audioSource;
    public AudioClip lockedSound;
    public AudioClip openSound;

    public void Interact(GameObject player)
    {
        PlayerInventory inventory = player.GetComponentInParent<PlayerInventory>();

        if (requiresKey && (inventory == null || !inventory.hasKey))
        {
            if (audioSource != null && lockedSound != null)
            {
                audioSource.PlayOneShot(lockedSound);
            }

            Debug.Log("Door is locked.");
            return;
        }

        StartCoroutine(OpenDoorRoutine());
    }

    IEnumerator OpenDoorRoutine()
    {
        if (audioSource != null && openSound != null)
        {
            audioSource.PlayOneShot(openSound);

            yield return new WaitForSeconds(openSound.length);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }

        SceneManager.LoadScene(nextSceneName);
    }
}