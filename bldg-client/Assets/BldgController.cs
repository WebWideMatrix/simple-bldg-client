using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BldgController : MonoBehaviour
{

	public GameObject bldg;

    public GameObject clickedObject; 

    public ChaseCamera camera;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Started");
        for (int i = 0; i < 10; i++) {
        	GameObject b = Instantiate(bldg);
            BldgObject s = b.AddComponent<BldgObject>();
            Debug.Log(s);
            s.bldgController = this;
        	b.transform.position = new Vector3(
        		Random.Range(-10F, 10F),
        		0,
        		Random.Range(-10F, 10F)
    		);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void handleClick(Vector3 position) {
        Debug.Log("click");
        camera.moveToTarget(position);
    }

    public void handleRightClick(Vector3 position) {
        Debug.Log("right click");
    }
}
