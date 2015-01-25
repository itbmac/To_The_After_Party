using UnityEngine;
using System.Collections;

public class CameraEntry : MonoBehaviour {

	public float MOVEMENT_SPEED = 1.0f;

	public Transform startCube, endCube;
	public Transform camera;
	public Transform hands;

	public bool completed = false;

	// Use this for initialization
	void Start () {
		hands.localPosition = new Vector3 (0.0f, -1.0f, 0.345f);
		hands.localEulerAngles = new Vector3 (60.0f, 0.0f, 0.0f);
		camera.position = startCube.position;
		camera.eulerAngles = startCube.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
		if(!completed) {
			camera.position = new Vector3 (Mathf.Lerp (camera.position.x, endCube.position.x, MOVEMENT_SPEED * Time.deltaTime),
			                               Mathf.Lerp (camera.position.y, endCube.position.y, MOVEMENT_SPEED * Time.deltaTime),
			                               Mathf.Lerp (camera.position.z, endCube.position.z, MOVEMENT_SPEED * Time.deltaTime));
			camera.eulerAngles = new Vector3 (Mathf.Lerp (camera.eulerAngles.x, endCube.eulerAngles.x, MOVEMENT_SPEED * Time.deltaTime),
			                                  Mathf.Lerp (camera.eulerAngles.y, endCube.eulerAngles.y, MOVEMENT_SPEED * Time.deltaTime),
			                                  Mathf.Lerp (camera.eulerAngles.z, endCube.eulerAngles.z, MOVEMENT_SPEED * Time.deltaTime));
		}
		if((camera.eulerAngles - endCube.eulerAngles).magnitude < 0.05) {
			camera.eulerAngles = endCube.eulerAngles;
			completed = true;
		}
		hands.localPosition = new Vector3 (0.0f,
		                                   Mathf.Lerp (hands.localPosition.y, -0.229f, MOVEMENT_SPEED * Time.deltaTime),
		                                   0.3f);
		hands.localEulerAngles = new Vector3 (Mathf.Lerp (camera.localEulerAngles.x, -10.0f, MOVEMENT_SPEED * Time.deltaTime),
		                                      0.0f,
		                                      0.0f);
	}
}
