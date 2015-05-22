using UnityEngine;
using System.Collections;

public class Reticle : MyMonoBehaviour {

	private GameObject arm;
	private GameObject painting;
	private JointOrientation jointOrientation;
	
	public void Initialize(GameObject arm, GameObject painting, Color32 color) {
		this.arm = arm;
		this.painting = painting;
		jointOrientation = arm.GetComponent<JointOrientation>();
		
		transform.GetChild(0).GetComponent<Renderer>().material.color = color;
	}
	
	// Update is called once per frame
	void Update () {
		if (arm == null || painting == null) {
			return;
		}
		
		if (!jointOrientation.IsRunning) {
			gameObject.SetActive(false);
			return;
		}
		
		Ray r = new Ray(arm.transform.position, arm.transform.forward);
		RaycastHit hit;
		if (!painting.GetComponent<Collider>().Raycast(r, out hit, float.PositiveInfinity)) {
			transform.GetChild(0).GetComponent<Renderer>().enabled = false;
			return;
		}
		if (!transform.GetChild(0).GetComponent<Renderer>().enabled)
			transform.GetChild(0).GetComponent<Renderer>().enabled = true;
		
		transform.position = hit.point;
	}
}
