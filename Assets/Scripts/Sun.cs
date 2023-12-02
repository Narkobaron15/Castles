using System;
using UnityEngine;

public class Sun : MonoBehaviour
{
    private Vector3 _initialPosition;
    private Vector3 _midPosition;
    private Vector3 _finalPosition;
    
    private void Awake()
    {
        _initialPosition = new Vector3(-20, 20, 0);
        _midPosition = new Vector3(30, 30, 0);
        _finalPosition = new Vector3(80, 20, 0);
    }
    
    private void Update()
    {
        // 6h: (-20, 20) -> 14h: (30, 30) -> 20h: (80, 20) -> to start position
        var timeOfDay = TimeManager.TimeOfDay;

        gameObject.transform.position = timeOfDay switch
        {
            >= 6 and <= 14 => Vector3.Lerp(_midPosition, 
                _initialPosition, 
                (14 - timeOfDay) / 8),
            > 14 and <= 20 => Vector3.Lerp(_finalPosition, 
                    _midPosition, 
                    (20 - timeOfDay) / 6),
            _ => _initialPosition,
        };
    }
}
