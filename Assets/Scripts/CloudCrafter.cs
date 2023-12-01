using UnityEngine;

public class CloudCrafter : MonoBehaviour
{
    [Header("Set in inspector")]
    public int numClouds = 40;
    public GameObject cloudPrefab;
    public Vector3 cloudPosMin = new(-50, 5, 10);
    public Vector3 cloudPosMax = new(150, 100, 10);
    public float cloudScaleMin = 1;
    public float cloudScaleMax = 3;
    public float cloudSpeedMult = 0.5f;
    
    private GameObject[] _cloudInstances;
    
    private void Awake()
    {
        // Make an array large enough to hold all the Cloud_ instances
        _cloudInstances = new GameObject[numClouds];
        // Find the CloudAnchor parent GameObject
        var anchor = GameObject.Find("CloudAnchor");
        // Iterate through and make Cloud_s
        GameObject cloud;
        for (var i = 0; i < numClouds; i++)
        {
            // Make an instance of cloudPrefab
            cloud = Instantiate(cloudPrefab);
            // Position cloud
            var cTrans = cloud.transform;
            var pos = Vector3.zero;
            pos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            pos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);
            // Scale cloud
            var scale = Random.value;
            var scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scale);
            // Smaller clouds (with smaller scale values) should be nearer the ground
            pos.y = Mathf.Lerp(cloudPosMin.y, pos.y, scale);
            // Smaller clouds should be further away
            pos.z = 100 - 90 * scale;
            // Apply these transforms to the cloud
            cTrans.position = pos;
            cTrans.localScale = Vector3.one * scaleVal;
            // Make cloud a child of the anchor
            cTrans.SetParent(anchor.transform);
            // Add the cloud to _cloudInstances
            _cloudInstances[i] = cloud;
        }
    }

    private void Update()
    {
        // Iterate over each cloud that was created
        foreach (var cloud in _cloudInstances)
        {
            // Get the cloud scale and position
            var cTrans = cloud.transform;
            var scaleVal = cTrans.localScale.x;
            var pos = cTrans.position;
            // Move larger clouds faster
            pos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;
            // If a cloud has moved too far to the left...
            if (pos.x <= cloudPosMin.x)
            {
                // Move it to the far right
                pos.x = cloudPosMax.x;
            }
            // Apply the new position to the cloud
            cTrans.position = pos;
        }
    }
}
