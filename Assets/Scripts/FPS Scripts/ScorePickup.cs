using UnityEngine;

public class ScorePickup : MonoBehaviour
{
    public int scoreAmount = 1;

    [Header("Sound")]
    public AudioClip pickupSound;
    [Range(0f, 1f)] public float pickupVolume = 1f;

    private void OnTriggerEnter(Collider other)
    {
        PlayerScore playerScore = other.GetComponent<PlayerScore>();

        if (playerScore != null)
        {
            playerScore.AddScore(scoreAmount);

            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position, pickupVolume);
            }

            Destroy(gameObject);
        }
    }
}