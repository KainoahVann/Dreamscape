using UnityEngine;

public class AudioZone : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource musicSource;

    [Header("Inside Zone Settings")]
    public float insidePitch = 1.2f;

    [Header("Outside Zone Settings")]
    public float outsidePitch = 1.0f;

    [Header("Transition")]
    public float transitionSpeed = 2f;

    private float targetPitch;
    private Transform _player;
    private BoxCollider _zone;
    private bool _wasInside = false;

    void Start()
    {
        targetPitch = outsidePitch;
        _zone = GetComponent<BoxCollider>();

        GameObject found = GameObject.FindGameObjectWithTag("Player");
        if (found != null)
            _player = found.transform;
        else
            Debug.LogError("AudioZone: No GameObject tagged 'Player' found.");

        if (_zone == null)
            Debug.LogError("AudioZone: No BoxCollider found on this GameObject.");

        if (musicSource == null)
            Debug.LogError("AudioZone: No AudioSource assigned.");
    }

    void Update()
    {
        if (_player == null || _zone == null || musicSource == null) return;

        bool inside = _zone.bounds.Contains(_player.position);

        if (inside != _wasInside)
        {
            _wasInside = inside;
            Debug.Log(inside ? "AudioZone: Player entered zone." : "AudioZone: Player exited zone.");
        }

        targetPitch = inside ? insidePitch : outsidePitch;
        musicSource.pitch = Mathf.Lerp(musicSource.pitch, targetPitch, Time.deltaTime * transitionSpeed);
    }
}