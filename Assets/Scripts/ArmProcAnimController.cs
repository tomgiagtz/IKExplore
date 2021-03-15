using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArmProcAnimController : MonoBehaviour
{
    // Start is called before the first frame update
    public LayerMask isGround;
    
    public Transform targetIK;

    public bool shouldUpdateTarget = false;
    public float stepRadius = 0.3f;
    public float repulseRadius;
    public bool isGrounded;
    public bool VERBOSE = false;
    
    

  
    public Transform targetPoint;
    Tweener moveTween;
    void Start()
    {
    //    castPoint= transform.position + downwardCastOffset;
        DOTween.Init();
        moveTween = targetIK.DOMove(currentTarget, 0.1f).SetAutoKill(false);  

    }

    // Update is called once per frame
    
    void Update()
    {
        moveIKTransform();
        

        // if (shouldUpdateTarget) {
        //     updateCurrentTarget();
        // }
        float targetToGroundDistance = Vector3.Distance(targetIK.position, targetPoint.position);
        float thisToTargetDistance = Vector3.Distance(targetIK.position, transform.position);
        if (VERBOSE) Debug.Log(targetToGroundDistance);
        // if (VERBOSE) Debug.Log(thisToTargetDistance);

        if (targetToGroundDistance > stepRadius) {
            updateCurrentTarget();
            // updateTargetPath();
        }

        if (VERBOSE) Debug.DrawLine(targetIK.position, targetPoint.position, Color.green, Time.deltaTime * 2);
        
    }
    float stepHeight = 0.1f;
    // float stepSpeed = 1f;


    Vector3 stepTarget;
    Vector3 currentTarget;
    bool currentFinished, stepFinished;


    void moveIKTransform() {
        Vector3 currPosition = targetIK.position;
        moveTween.ChangeEndValue(currentTarget).Restart();
        targetIK.up = -targetPoint.TransformDirection(Vector3.up);
    }

    void updateCurrentTarget() {
        currentTarget = new Vector3(targetPoint.position.x, targetPoint.position.y, targetPoint.position.z);
        // currentTarget.y
        moveTween.ChangeStartValue(targetIK.position).ChangeEndValue(currentTarget);
         if (VERBOSE) {
            
            Debug.DrawLine(targetIK.position, currentTarget, Color.blue, 10f);
        }
    }


    Vector3[] currentPath = new Vector3[3];
    void updateTargetPath() {
        updateCurrentTarget();
        //first element is current position
        currentPath[0] = targetIK.position;
        //second is half between target and curr, but with added y
        currentPath[1] = (targetIK.position + currentTarget) * 0.5f + Vector3.up * stepHeight;
        //third is target
        currentPath[2] = currentTarget;

       
        // currentTarget = new Vector3(targetPoint.position.x, targetPoint.position.y, targetPoint.position.z);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        
        Gizmos.DrawSphere(currentTarget, 0.2f);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(targetPoint.position, 0.3f);

        // Gizmos.color = Color.green;
        // Gizmos.DrawSphere(castPoint.position, 0.1f);
    }
}
