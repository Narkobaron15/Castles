using System.Linq;
using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [SerializeField, InspectorName("Directional Light")] 
    private Light directionalLight;
    
    [SerializeField, InspectorName("Lighting Preset")] 
    private LightingPreset preset;

    [SerializeField, Range(0, 24), InspectorName("Time of Day")]
    private float timeOfDay;

    private Camera _mainCamera;

    private void Start()
    {
        timeOfDay = 8f;
        _mainCamera = Camera.main;
    }

    private void OnValidate()
    {
        if (directionalLight != null) return;
        
        if (RenderSettings.sun != null)
        {
            directionalLight = RenderSettings.sun;
            return;
        }

        var lights = FindObjectsOfType<Light>();
        try
        {
            directionalLight = lights.First(l => 
                l.type == LightType.Directional);
        }
        catch
        {
            Debug.LogError("No directional light available in the scene.");
        }
    }

    private void UpdateLightning(float timePercent)
    {
        RenderSettings.ambientLight = preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = preset.FogColor.Evaluate(timePercent);
        _mainCamera.backgroundColor = preset.CameraBackgroundColor.Evaluate(timePercent);
        
        if (directionalLight is null) return;
        
        directionalLight.color = preset.DirectionalColor.Evaluate(timePercent);
        directionalLight.transform.localRotation =
            Quaternion.Euler(140f, timePercent * 360f, 0);
    }

    private void Update()
    {
        if (preset is null) return;
        
        if (Application.isPlaying)
        {
            timeOfDay += Time.deltaTime / 60f;
            timeOfDay %= 24;
        }
        
        UpdateLightning(timeOfDay / 24f);
    }
}
