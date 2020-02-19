using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCamera : MonoBehaviour
{
	private Vector3 targetPosition;
    public float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = transform.position;
    }


    public void moveToTarget(Vector3 pos) {
        targetPosition = pos;
    }

    // Update is called once per frame
    void Update()
    {
        // RaycastHit hit;
        // if (Physics.Raycast(transform.position, transform.forward, out hit)) {
        //     Debug.Log(hit);
        //     Debug.Log(hit.transform);
        //     Debug.Log(hit.transform.parent);
        //     if (hit.transform.parent.tag == "MoveTarget") {
        //         targetPosition = hit.transform.position;
        //     }
        // }

        Vector3 nextToTarget = targetPosition - new Vector3(0, -1, -2);
        transform.position = Vector3.Lerp(transform.position, nextToTarget, speed * Time.deltaTime);
        transform.LookAt(targetPosition);
    }
}
