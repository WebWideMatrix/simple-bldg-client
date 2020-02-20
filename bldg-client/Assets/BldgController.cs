using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Models;
using Proyecto26;
using UnityEngine.SceneManagement;

public class BldgController : MonoBehaviour
{

    // TODO: change to array

	public GameObject twitBldg;
	public GameObject articleBldg;
	public GameObject conceptBldg;
	public GameObject dailyFeedBldg;
	public GameObject userBldg;


	public GameObject bldg;

    public GameObject clickedObject; 

    public ChaseCamera camera;

    string basePath = "http://localhost:4000/v1/bldgs";
    string currentAddress;
	string currentFlr;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Started");
        if (currentAddress == null) {
			// if (PlayerPrefs.GetString ("currentAddress") != null) {
			// 	currentAddress = PlayerPrefs.GetString ("currentAddress");
			// } else {
			// 	currentAddress = "g-b(17,24)-l0";
			// }
            currentAddress = "g-b(17,24)-l0";
			AddressChanged ();
		}
        // for (int i = 0; i < 10; i++) {
        // 	GameObject b = Instantiate(bldg);
        //     BldgObject s = b.AddComponent<BldgObject>();
        //     Debug.Log(s);
        //     s.bldgController = this;
        // 	b.transform.position = new Vector3(
        // 		Random.Range(-10F, 10F),
        // 		0,
        // 		Random.Range(-10F, 10F)
    	// 	);
        // }
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


	// public void GoToBldg(string address) {
	// 	currentAddress = address;
	// 	if (AddressUtils.isBldg(address)) {
	// 		currentAddress = AddressUtils.generateInsideAddress (address);
	// 	}
	// 	AddressChanged ();
	// }

	// public void GoIn() {
	// 	InputField input = GameObject.FindObjectOfType<InputField> ();
	// 	currentAddress = input.text;
	// 	AddressChanged ();
	// }


	// public void GoOut() {
	// 	InputField input = GameObject.FindObjectOfType<InputField> ();

	// 	currentAddress = AddressUtils.getContainerFlr(input.text);

	// 	AddressChanged ();
	// }


	// public void GoUp() {
	// 	InputField input = GameObject.FindObjectOfType<InputField> ();
	// 	currentAddress = input.text;
	// 	int level = AddressUtils.getFlrLevel(currentAddress);
	// 	currentAddress = AddressUtils.replaceFlrLevel (currentAddress, level + 1);
	// 	AddressChanged ();
	// }


	// public void GoDown() {
	// 	InputField input = GameObject.FindObjectOfType<InputField> ();
	// 	currentAddress = input.text;
	// 	int level = AddressUtils.getFlrLevel(currentAddress);
	// 	if (level > 0) {
	// 		currentAddress = AddressUtils.replaceFlrLevel (currentAddress, level - 1);
	// 	}
	// 	AddressChanged ();
	// }


	public void AddressChanged() {
		Debug.Log ("Address changed to: " + currentAddress);
		// InputField input = GameObject.FindObjectOfType<InputField> ();
		// if (input.text != currentAddress) {
		// 	input.text = currentAddress;
		// }

		PlayerPrefs.SetString ("currentAddress", currentAddress);

		// TODO validate address

		currentFlr = AddressUtils.extractFlr(currentAddress);

		// TODO check whether it changed

		// check whether we need to switch scene
		if (currentAddress.ToLower () == "g" && SceneManager.GetActiveScene().name == "Floor") {
			SceneManager.LoadScene ("Ground");
			return;
		}
		if (currentAddress.ToLower () != "g" && SceneManager.GetActiveScene().name == "Ground") {
			SceneManager.LoadScene ("Floor");
			return;
		}

		// load the new address
		switchAddress (currentAddress);
	}

	void switchAddress(string address) {
		if (address.ToLower() != "g") {
			updateFloorSign ();
		}

		GameObject[] currentFlrBuildings = GameObject.FindGameObjectsWithTag("Building");
		foreach (GameObject bldg in currentFlrBuildings) {
			GameObject.Destroy (bldg);
		}

		// We can add default request headers for all requests
		RestClient.DefaultRequestHeaders["Authorization"] = "Bearer ...";
        Debug.Log("Expecting URL to be: http://localhost:4000/v1/bldgs/look/g-b(17,24)-l0");
        string url = basePath + "/look/" + address;
        Debug.Log("But got URL: " + url);
		RestClient.GetArray<Bldg>(url).Then(res =>
			{
				int count = 0;
				foreach (Bldg b in res) {
					count += 1;
					Debug.Log("processing bldg " + count);
					
					// The area is 16x12, going from (8,6) - (-8,-6)

					Vector3 baseline = new Vector3(-8f, 0F, -6f);	// WHY? if you set the correct Y, some images fail to display
					baseline.x += b.x;
					baseline.z += b.y;
					GameObject prefab = getPrefabByEntityClass(b.entity_type);
					GameObject bldgClone = (GameObject) Instantiate(prefab, baseline, Quaternion.identity);
					bldgClone.tag = "Building";
                    BldgObject bldgObject = bldgClone.AddComponent<BldgObject>();
					bldgObject.initialize(b, this);
					TextMesh[] labels = bldgClone.GetComponentsInChildren<TextMesh>();
					foreach (TextMesh label in labels) {
						// if (label.name == "DayInWeek")
						// 	label.text = extractDatePart(b.web_url, "DayInWeek");	
						// else if (label.name == "Month")
						// 	label.text = extractDatePart(b.web_url, "Month");	
						// else if (label.name == "Date")
						// 	label.text = extractDatePart(b.web_url, "Date");	
						if (label.name == "TwitText")
							label.text = b.summary;
						else if (label.name == "AuthorName")
							label.text = b.name;
						else if (label.name == "ArticleTitle")
							label.text = b.name;					
						else if (label.name == "UserName")
							label.text = b.name;							
						else if (label.name == "SiteName") {
							if (b.summary != null)
								label.text = b.summary;
						}
					}
					Debug.Log("About to call renderAuthorPicture on bldg " + count);
                    // TODO create picture element
					// controller.renderMainPicture();
				}

			});
	}

	GameObject getPrefabByEntityClass(string entity_type) {
		switch (entity_type) {
		case "twitter-social-post":
			return twitBldg;
		case "article-text":
			return articleBldg;
		case "daily-feed":
			return dailyFeedBldg;
		case "user":
			return userBldg;
		default:
			return twitBldg;
		}
	}

	void updateFloorSign() {
		TextMesh flrSign = GameObject.FindGameObjectWithTag ("FloorSign").GetComponent<TextMesh>();
		flrSign.text = currentFlr.ToUpper ();
	}

}
