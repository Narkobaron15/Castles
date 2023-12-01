using UnityEngine;

public class Goal : MonoBehaviour {
    // public static bool GoalMet = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Projectile")) return;
        
        // GoalMet = true;
        var mat = GetComponent<Renderer>().material;
        var c = mat.color;
        c.a = 1;
        mat.color = c;
    }
}