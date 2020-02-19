using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BldgObject : MonoBehaviour, IPointerDownHandler
{

    public BldgController bldgController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void OnMouseDown() {
		Debug.Log ("Mouse down on bldg object!");
		bldgController.handleClick (transform.position);
	}

	public void OnPointerDown (PointerEventData eventData) {
		Debug.Log("OnPointerClick - bldg object");
         if (eventData.button == PointerEventData.InputButton.Right) {
             Debug.Log ("Right Mouse Button Clicked on bldg object");
			 bldgController.handleRightClick (transform.position);
         }
     }

	public void OnPointerUp(PointerEventData pointerEventData)
    {
        Debug.Log(name + "No longer being clicked");
    }
}
