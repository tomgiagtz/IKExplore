using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmProcAnimController : MonoBehaviour
{
    // Start is called before the first frame update    
    public Transform targetIK;
    public Transform body;
    public ArmProcAnimController oppositeArm;

    public float stepRadius = 0.3f;
    public float stepLength = 1f;
    float stepHeight = 0.2f; 
    public float speed = 1;
    
    
    float lerp;

    Vector3 currentTarget, newTarget;
    public Vector3 footOffset = Vector3.zero;

    Vector3 oldPosition, currentPosition, newPosition;
    Vector3 oldNormal, currentNormal, newNormal;

  
    public Transform targetPoint;
    void Start()
    {
    //    castPoint= transform.position + downwardCastOffset;
        currentPosition = newPosition = oldPosition = targetIK.position;
        currentNormal = newNormal = oldNormal = targetIK.up;
        lerp = 1f;
    }

    // Update is called once per frame
    
    void Update()
    {
        targetIK.position = currentPosition;

        // if (shouldUpdateTarget) {
        //     updateCurrentTarget();
        // }
        float targetToGroundDistance = Vector3.Distance(newPosition, targetPoint.position);
        float thisToTargetDistance = Vector3.Distance(targetIK.position, transform.position);
        // if (VERBOSE) Debug.Log(targetToGroundDistance);
        // if (VERBOSE) Debug.Log(thisToTargetDistance);

        if (targetToGroundDistance > stepRadius && lerp >= 1f && !oppositeArm.IsMoving()) {
            lerp = 0;
            int direction = body.InverseTransformPoint(targetPoint.position).z > body.InverseTransformPoint(newPosition).z ? 1 : -1;
            newPosition = targetPoint.position + (body.forward * stepLength * direction) + footOffset;
            newNormal = targetPoint.up;
            // targetIK.position = targetPoint.position;
            // updateCurrentTarget();
            // updateTargetPath();
        }

        if (lerp < 1) {
             Vector3 tempPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
            tempPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            currentPosition = tempPosition;
            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);
            lerp += Time.deltaTime * speed;
        }   else {
            oldPosition = newPosition;
            oldNormal = newNormal;
        }

        // if (VERBOSE) Debug.DrawLine(targetIK.position, targetPoint.position, Color.green, Time.deltaTime * 2);
        
    }

    public bool IsMoving() {
        return lerp < 1;
    }
    // float stepSpeed = 1f;


    


    // void moveIKTransform() {
    //     targetIK.position = targetPoint.position;
    //     targetIK.up = -targetPoint.TransformDirection(Vector3.up);
    // }

    // void updateCurrentTarget() {
    //     currentTarget = new Vector3(targetPoint.position.x, targetPoint.position.y, targetPoint.position.z);
        
    //     targetIK.position = currentTarget;
    //      if (VERBOSE) {
            
    //         // Debug.DrawLine(targetIK.position, currentTarget, Color.blue, 10f);
    //     }
    // }


    // Vector3[] currentPath = new Vector3[3];
    // void updateTargetPath() {
    //     updateCurrentTarget();
    //     //first element is current position
    //     currentPath[0] = targetIK.position;
    //     //second is half between target and curr, but with added y
    //     currentPath[1] = (targetIK.position + currentTarget) * 0.5f + Vector3.up * stepHeight;
    //     //third is target
    //     currentPath[2] = currentTarget;

       
    //     // currentTarget = new Vector3(targetPoint.position.x, targetPoint.position.y, targetPoint.position.z);
    // }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        
        Gizmos.DrawSphere(currentTarget, 0.2f);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(targetPoint.position, 0.2f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(newPosition, 0.22f);

        // Gizmos.color = Color.green;
        // Gizmos.DrawSphere(castPoint.position, 0.1f);
    }
}
