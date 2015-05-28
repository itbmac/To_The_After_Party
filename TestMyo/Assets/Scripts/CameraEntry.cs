using UnityEngine;
using System.Collections;

public class CameraEntry : MonoBehaviour {

	public float MOVEMENT_SPEED = 1.0f;

	public Transform startCube, endCube;
	public Transform myCamera;
	public Transform hands;

	private bool completed = false;
	
	public static CameraEntry Instance;

	// Use this for initialization
	void Start () {
		hands.localPosition = new Vector3 (0.0f, -1.0f, 0.345f);
		hands.localEulerAngles = new Vector3 (60.0f, 0.0f, 0.0f);
		myCamera = Camera.main.transform;
		myCamera.position = startCube.position;
		myCamera.eulerAngles = startCube.eulerAngles;
		Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		if(!completed) {
			myCamera.position = new Vector3 (Mathf.Lerp (myCamera.position.x, endCube.position.x, MOVEMENT_SPEED * Time.deltaTime),
			                               Mathf.Lerp (myCamera.position.y, endCube.position.y, MOVEMENT_SPEED * Time.deltaTime),
			                               Mathf.Lerp (myCamera.position.z, endCube.position.z, MOVEMENT_SPEED * Time.deltaTime));
			myCamera.eulerAngles = new Vector3 (Mathf.Lerp (myCamera.eulerAngles.x, endCube.eulerAngles.x, MOVEMENT_SPEED * Time.deltaTime),
			                                  Mathf.Lerp (myCamera.eulerAngles.y, endCube.eulerAngles.y, MOVEMENT_SPEED * Time.deltaTime),
			                                  Mathf.Lerp (myCamera.eulerAngles.z, endCube.eulerAngles.z, MOVEMENT_SPEED * Time.deltaTime));
          	
          	if (Input.GetKeyDown(KeyCode.Space) /* || GameManager.Instance.GetDoubleTapFlag() */) {
          		myCamera.position = endCube.position;
				myCamera.eulerAngles = endCube.eulerAngles;
          	}
		}
		if((myCamera.eulerAngles - endCube.eulerAngles).magnitude < 0.05) {
			myCamera.eulerAngles = endCube.eulerAngles;
			completed = true;
		}
		hands.localPosition = new Vector3 (0.0f,
		                                   Mathf.Lerp (hands.localPosition.y, -0.229f, MOVEMENT_SPEED * Time.deltaTime),
		                                   0.3f);
		hands.localEulerAngles = new Vector3 (Mathf.Lerp (myCamera.localEulerAngles.x, -10.0f, MOVEMENT_SPEED * Time.deltaTime),
		                                      0.0f,
		                                      0.0f);
	}

	public bool Completed() {
		return completed;
	}
}
