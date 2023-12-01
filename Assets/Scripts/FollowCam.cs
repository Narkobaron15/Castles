using JetBrains.Annotations;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [CanBeNull] public static GameObject Poi;
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;
    
    public float camZ;
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
        camZ = this.transform.position.z;
    }
    
    private void FixedUpdate()
    {
        if (Poi == null) return;
        
        var destination = Poi.transform.position;
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        destination = Vector3.Lerp(transform.position, destination, easing);
        destination.z = camZ;
        
        transform.position = destination;
        _mainCamera.orthographicSize = destination.y + 10;
    }
}
