using UnityEngine;
using UnityEngine.Rendering;

public class SkyboxZone : MonoBehaviour
{
    [Header("Skyboxes")]
    public Material outsideSkybox;
    public Material insideSkybox;

    [Header("Ambient Light")]
    public Color outsideAmbientColor = Color.white;
    public Color insideAmbientColor = new Color(0.02f, 0.02f, 0.02f); // near black

    [Header("Fog")]
    public bool useFog = true;
    public Color insideFogColor = new Color(0.01f, 0.01f, 0.01f);
    public float insideFogDensity = 0.05f;
    public Color outsideFogColor = Color.grey;
    public float outsideFogDensity = 0.01f;

    [Header("Transition")]
    public float transitionSpeed = 2f;

    private BoxCollider _zone;
    private Transform _player;
    private bool _wasInside = false;
    private Color _targetAmbient;
    private Color _targetFogColor;
    private float _targetFogDensity;

    void Start()
    {
        _zone = GetComponent<BoxCollider>();

        GameObject found = GameObject.FindGameObjectWithTag("Player");
        if (found != null) _player = found.transform;
        else Debug.LogError("SkyboxZone: No Player tag found.");

        _targetAmbient = outsideAmbientColor;
        _targetFogColor = outsideFogColor;
        _targetFogDensity = outsideFogDensity;

        if (outsideSkybox != null)
            RenderSettings.skybox = outsideSkybox;

        RenderSettings.ambientLight = outsideAmbientColor;
    }

    void Update()
    {
        if (_player == null || _zone == null) return;

        bool inside = _zone.bounds.Contains(_player.position);

        if (inside != _wasInside)
        {
            _wasInside = inside;

            if (inside)
            {
                if (insideSkybox != null)
                    RenderSettings.skybox = insideSkybox;
                _targetAmbient = insideAmbientColor;
                _targetFogColor = insideFogColor;
                _targetFogDensity = insideFogDensity;
            }
            else
            {
                if (outsideSkybox != null)
                    RenderSettings.skybox = outsideSkybox;
                _targetAmbient = outsideAmbientColor;
                _targetFogColor = outsideFogColor;
                _targetFogDensity = outsideFogDensity;
            }

            DynamicGI.UpdateEnvironment();
        }

        RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, _targetAmbient, Time.deltaTime * transitionSpeed);

        if (useFog)
        {
            RenderSettings.fog = true;
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, _targetFogColor, Time.deltaTime * transitionSpeed);
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, _targetFogDensity, Time.deltaTime * transitionSpeed);
        }
    }
}