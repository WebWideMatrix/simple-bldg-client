using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour
{



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("right")) {
        	transform.RotateAround(transform.position, Vector3.up, 40f * Time.deltaTime);
        }
        if (Input.GetKey("left")) {
        	transform.RotateAround(transform.position, Vector3.up, -40f * Time.deltaTime);
        }
        if (Input.GetKey("up")) {
            transform.position += transform.forward * 3.5f * Time.deltaTime;
        }
        if (Input.GetKey("down")) {
            transform.position -= Vector3.forward * 3.5f * Time.deltaTime;
        }
    }
}
