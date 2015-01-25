using UnityEngine;
using System.Collections;

public class Reticle : MonoBehaviour {

	private GameObject arm;
	private GameObject painting;
	
	public void Initialize(GameObject arm, GameObject painting, Color32 color) {
		this.arm = arm;
		this.painting = painting;
		
		
		transform.GetChild(0).renderer.material.color = color;
	}
	
	// Update is called once per frame
	void Update () {
		if (arm == null || painting == null) {
			return;
		}
		
		Ray r = new Ray(arm.transform.position, arm.transform.forward);
		RaycastHit hit;
		if (!painting.collider.Raycast(r, out hit, float.PositiveInfinity)) {
			transform.GetChild(0).renderer.enabled = false;
			return;
		}
		if (!transform.GetChild(0).renderer.enabled)
			transform.GetChild(0).renderer.enabled = true;
		
		transform.position = hit.point;
	}
}
