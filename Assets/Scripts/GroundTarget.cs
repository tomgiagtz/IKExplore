using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTarget : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform body;

    public LayerMask isGround;
    public float heightOffset = 0.1f;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 castPoint = new Vector3(transform.position.x, body.position.y, transform.position.z);
        if (Physics.Raycast(castPoint, Vector3.down, out hit, 3f, isGround)) {
            Debug.DrawRay(castPoint, Vector3.down * hit.distance, Color.yellow);
            transform.LookAt(hit.point, -transform.up);
            transform.position = hit.point + Vector3.up * heightOffset;
            // Debug.Log(hit.distance);
        } else {
            Debug.DrawRay(castPoint, Vector3.down * 100f, Color.red);
        }
    }
}
