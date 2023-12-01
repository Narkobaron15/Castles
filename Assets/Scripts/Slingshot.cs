using System;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    private static Slingshot S;
    
    [Header("Set in Inspector")]
    public GameObject prefabProjectile;
    public float velocityMultiplicator = 8f;
    
    [Header("Set in Inspector")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    
    public GameObject projectile;
    
    private Rigidbody _projectileRigidbody;
    public static Vector3 LaunchPos => S == null ? Vector3.zero : S.launchPos;

    public bool aimingMode;

    private Camera _mainCamera;
    private SphereCollider _sphereCollider;

    private void Awake()
    {
        S = this;
        Transform launchPointTrans = transform.Find("LaunchPoint");
        
        _sphereCollider = this.GetComponent<SphereCollider>();
        // Find the launch point transform (halo)
        var launchPointTransform = transform.Find("LaunchPoint");
        launchPoint = launchPointTransform.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTransform.position;

        _mainCamera = Camera.main;
    }

    private void OnMouseDown()
    {
        // The player has pressed the mouse button while over Slingshot
        aimingMode = true;
        // Instantiate a projectile
        projectile = Instantiate(prefabProjectile);
        // Start it at the launch point
        projectile.transform.position = launchPos;
        // Set it to isKinematic for now
        _projectileRigidbody = projectile.GetComponent<Rigidbody>();
        _projectileRigidbody.isKinematic = true;
    }

    private void OnMouseEnter()
    {
        // Show the launch point (its halo)
        launchPoint.SetActive(true);
    }
    
    private void OnMouseExit()
    {
        // Hide the launch point (its halo)
        launchPoint.SetActive(false);
    }

    private void Update()
    {
        if (!aimingMode) return;
        
        // Get the current mouse position in 2D screen coordinates
        // And convert into world points
        var mousePos2D = Input.mousePosition;
        mousePos2D.z = -_mainCamera.transform.position.z;
        var mousePos3D = _mainCamera.ScreenToWorldPoint(mousePos2D);
        
        var mouseDelta = mousePos3D - launchPos;

        var maxMagnitude = _sphereCollider.radius;
        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        var projPos = launchPos + mouseDelta;
        if (projPos.x > launchPos.x + 0.2f)
        {
            aimingMode = false;
            Destroy(projectile);
            return;
        }

        projectile.transform.position = projPos;

        if (!Input.GetMouseButtonUp(0)) return;
        
        aimingMode = false;
        _projectileRigidbody.isKinematic = false;
        _projectileRigidbody.velocity = -mouseDelta * velocityMultiplicator;
        
        FollowCam.Poi = projectile;
        projectile = null;
    }
}
