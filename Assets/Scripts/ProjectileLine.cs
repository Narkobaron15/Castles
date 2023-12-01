using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    public static ProjectileLine S;
    
    [Header("Set in Inspector")]
    public float minDist = 0.1f;

    private LineRenderer _line;
    private GameObject _poi;
    private List<Vector3> _points;
    
    private void Awake() {
        S = this;
        _line = GetComponent<LineRenderer>();
        _line.enabled = false;
        _points = new List<Vector3>();
    }
    
    private GameObject Poi {
        get => _poi;
        set {
            _poi = value;
            if (_poi is null) return;
            
            _line.enabled = false;
            _points = new List<Vector3>();
            AddPoint();
        }
    }
    
    public void Clear() {
        _poi = null;
        _line.enabled = false;
        _points = new List<Vector3>();
    }
    
    private void AddPoint() {
        var pt = _poi.transform.position;
        if ( _points.Count > 0 && (pt - LastPoint).magnitude < minDist ) return;
        
        if ( _points.Count == 0 ) {
            var launchPosDiff = pt - Slingshot.LaunchPos;
            
            _points.Add( pt + launchPosDiff );
            _points.Add(pt);
            _line.positionCount = 2;
            
            _line.SetPosition(0, _points[0] );
            _line.SetPosition(1, _points[1] );
            
            _line.enabled = true;
        } else {
            _points.Add( pt );
            
            _line.positionCount = _points.Count;
            _line.SetPosition( _points.Count-1, LastPoint );
            _line.enabled = true;
        }
    }

    private Vector3 LastPoint => _points is null ? Vector3.zero : _points[^1];

    private void FixedUpdate () {
        if (Poi is null) {
            if (FollowCam.Poi is not null) {
                if (FollowCam.Poi.CompareTag("Projectile")) {
                    Poi = FollowCam.Poi;
                } else return;
            } else return;
        }
        
        AddPoint();
        if ( FollowCam.Poi is null ) Poi = null;
    }
}