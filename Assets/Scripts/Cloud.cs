using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [Header("Set in Inspector")] public GameObject cloudSphere;

    public int numSpheresMin = 6;
    public int numSpheresMax = 10;

    public Vector3 sphereOffsetScale = new Vector3(5, 2, 1);
    public Vector2 sphereScaleRangeX = new Vector2(4, 8);
    public Vector2 sphereScaleRangeY = new Vector2(3, 4);
    public Vector2 sphereScaleRangeZ = new Vector2(2, 4);

    public float scaleYMin = 2f;

    private List<GameObject> _spheres;

    private void Start()
    {
        _spheres = new List<GameObject>();
        var num = Random.Range(numSpheresMin, numSpheresMax);
        for (var i = 0; i < num; i++)
        {
            var sp = Instantiate(cloudSphere);
            _spheres.Add(sp);
            var spTrans = sp.transform;
            spTrans.SetParent(this.transform);

            // Choose a random position
            var offset = Random.insideUnitSphere;
            offset.x *= sphereOffsetScale.x;
            offset.y *= sphereOffsetScale.y;
            offset.z *= sphereOffsetScale.z;
            spTrans.localPosition = offset;

            // Choose a random size
            var scale = Vector3.one;
            scale.x = Random.Range(sphereScaleRangeX.x, sphereScaleRangeX.y);
            scale.y = Random.Range(sphereScaleRangeY.x, sphereScaleRangeY.y);
            scale.z = Random.Range(sphereScaleRangeZ.x, sphereScaleRangeZ.y);
            scale.y *= 1 - (Mathf.Abs(offset.x) / sphereOffsetScale.x); // h
            scale.y = Mathf.Max(scale.y, scaleYMin);
            spTrans.localScale = scale;
        }
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space)) Restart();
    // }
    //
    // private void Restart()
    // {
    //     // Delete old spheres
    //     foreach (var sp in _spheres)
    //         Destroy(sp);
    //
    //     Start();
    // }
}
