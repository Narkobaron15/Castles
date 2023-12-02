using JetBrains.Annotations;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [CanBeNull] public static GameObject Poi;
    public float easing = 0.05f;
    public Vector2 minXY;
    
    public float camZ;
    private Camera _mainCamera;
    
    private const float DefaultCamSize = 16f;

    private void Awake()
    {
        _mainCamera = Camera.main;
        camZ = this.transform.position.z;
        minXY = new Vector2(14.7f, 5.5f);
    }
    
    private void FixedUpdate()
    {
        Vector3 destination;
        if (Poi is null) destination = new Vector3(minXY.x, minXY.y, 0f);
        else
        {
            destination = Poi.transform.position;
            if (Poi.CompareTag("Projectile"))
            {
                if (Poi.GetComponent<Rigidbody>().IsSleeping())
                {
                    Poi = null;
                    return;
                }
            }
        }

        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        destination = Vector3.Lerp(transform.position, destination, easing);
        destination.z = camZ;
        
        transform.position = destination;
        _mainCamera.orthographicSize = destination.y + DefaultCamSize / 1.53f;
    }
}
