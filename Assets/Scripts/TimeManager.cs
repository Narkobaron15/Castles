using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [Range(0, 24), InspectorName("Time of Day")]
    private static float _timeOfDay;
    
    public static float TimeOfDay => _timeOfDay;
    
    private void Awake()
    {
        _timeOfDay = 8f;
    }
    
    private void Update()
    {
        if (!Application.isPlaying) return;
        
        _timeOfDay += Time.deltaTime; //  / 60f
        _timeOfDay %= 24;
    }
}