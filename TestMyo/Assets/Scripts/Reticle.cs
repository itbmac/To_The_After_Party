using UnityEngine;
using System.Collections;

public class Reticle : MonoBehaviour {

	private GameObject arm;
	private GameObject painting;
	
	public void Initialize(GameObject arm, GameObject painting, Color32 color) {
		this.arm = arm;
		this.painting = painting;
		
		
		transform.GetChild(0).GetComponent<Renderer>().material.color = color;
	}
	
	// Update is called once per frame
	void Update () {
		if (arm == null || painting == null) {
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
