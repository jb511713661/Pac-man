using UnityEngine;

public class PacStudentAutoMove : MonoBehaviour
{
    [Header("Path (clockwise)")]
    public Transform[] points;    // 4 corners: TL -> TR -> BR -> BL

    [Header("Movement")]
    public float speed = 3f;      // units per second 

    [Header("Animation")]
    public Animator animator;
    public string walkRightState = "Walk_Right";
    public string walkLeftState = "Walk_Left";
    public string walkUpState = "Walk_Up";
    public string walkDownState = "Walk_Down";

    [Header("Audio (optional)")]
    public AudioSource moveLoop;  

    int idx;              
    float t;              
    Vector3 a, b;         
    float segLen;         
    Vector3 dir;          

    void Start()
    {
        if (points == null || points.Length < 2)
        {
            Debug.LogError("PacStudentAutoMove: please assign 4 path points (clockwise).");
            enabled = false; return;
        }

       
        idx = 1;
        a = points[0].position;
        b = points[1].position;
        segLen = Mathf.Max(0.0001f, Vector3.Distance(a, b));
        dir = (b - a).normalized;

       
        transform.position = a;

        if (moveLoop != null && moveLoop.clip != null)
        {
            moveLoop.loop = true;
            moveLoop.Play();
        }
    }

    void Update()
    {
        
        t += (speed * Time.deltaTime) / segLen;

        if (t >= 1f)
        {
            
            transform.position = b;

            
            var prev = b;
            idx = (idx + 1) % points.Length;  // 1->2->3->0->1...
            a = prev;
            b = points[idx].position;
            segLen = Mathf.Max(0.0001f, Vector3.Distance(a, b));
            dir = (b - a).normalized;
            t = 0f;
        }
        else
        {
            
            transform.position = Vector3.Lerp(a, b, t);
        }

        
        PlayWalkByDir(dir);
    }

    void PlayWalkByDir(Vector3 d)
    {
        if (animator == null) return;

        if (Mathf.Abs(d.x) > Mathf.Abs(d.y))
        {
            if (d.x >= 0f) animator.Play(walkRightState);
            else animator.Play(walkLeftState);
        }
        else
        {
            if (d.y >= 0f) animator.Play(walkUpState);
            else animator.Play(walkDownState);
        }
    }

#if UNITY_EDITOR
    
    void OnDrawGizmosSelected()
    {
        if (points == null || points.Length < 2) return;
        Gizmos.color = Color.yellow;
        for (int i = 0; i < points.Length; i++)
        {
            var p0 = points[i];
            var p1 = points[(i + 1) % points.Length];
            if (p0 && p1)
            {
                Gizmos.DrawSphere(p0.position, 0.07f);
                Gizmos.DrawLine(p0.position, p1.position);
            }
        }
    }
#endif
}
