using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public static GameObject Poi;
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
                    ProjectileDestruction(Poi);
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

    private async void ProjectileDestruction(
        [NotNull] GameObject projectile,
        float awaitSeconds = 1.5f)
    {
        if (projectile is null) throw new ArgumentNullException(nameof(projectile));
        await Task.Delay(TimeSpan.FromSeconds(awaitSeconds));
        Destroy(projectile);
    }
}
