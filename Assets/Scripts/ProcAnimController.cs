using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcAnimController : MonoBehaviour
{
    // Start is called before the first frame update
    public LayerMask isGround;
    public float stepThreshold = 1.3f;
    public Vector3 downwardCastOffset = new Vector3(0f, 0f, 0.75f);
    public Transform targetIK;
    public Vector3 targetOffset = new Vector3(0f, 0.1f, 0f);
    public bool hasInitialHit = false;

    Vector3  currTarget;
    // Vector3 castPoint;
    public Transform castPoint;
    void Start()
    {
    //    castPoint= transform.position + downwardCastOffset;
    }

    // Update is called once per frame
    RaycastHit hit;
    void FixedUpdate()
    {
        if (!hasInitialHit) {
            updateHit();
            hasInitialHit = true;
        }
        float distanceToHit = Vector3.Distance(transform.position, hit.point);
        float testDistance = Vector3.Distance(castPoint.position, currTarget);
        Debug.Log(testDistance);
        if (testDistance > stepThreshold) {
            updateHit();
            Debug.Log(testDistance);
        }
        
        updateTargetIKTransform();
        
    }
    

    void updateHit() {
        // castPoint = transform.position + downwardCastOffset;
        Vector3 castStart = castPoint.position;
        if (Physics.Raycast(castStart, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, isGround))
        {
            currTarget = hit.point;
            Debug.DrawRay(castStart, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
           updateTargetIKTransform();
        }
        else
        {
            Debug.DrawRay(castStart, transform.TransformDirection(Vector3.down) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
    }
    void updateTargetIKTransform() {
        targetIK.position = currTarget;
        Vector3 orient = Vector3.Cross(-hit.normal, targetIK.forward);
        targetIK.up = -Vector3.up;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        
        Gizmos.DrawSphere(hit.point, 0.1f);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(castPoint.position, 0.1f);
    }
}
