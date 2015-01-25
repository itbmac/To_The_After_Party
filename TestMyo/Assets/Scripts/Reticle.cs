using UnityEngine;
using System.Collections;

public class Reticle : MonoBehaviour {

	private GameObject arm;
	private GameObject painting;
	
	public void Initialize(GameObject arm, GameObject painting, Color32 color) {
		this.arm = arm;
		this.painting = painting;
		
		renderer.material.color = color;
		
		Debug.Log ("initialized");
	}
	
	// Update is called once per frame
	void Update () {
		if (arm == null || painting == null) {
			return;
		}
		
		Ray r = new Ray(arm.transform.position, arm.transform.forward);
		RaycastHit hit;
		if (!painting.collider.Raycast(r, out hit, float.PositiveInfinity)) {
			return;
		}
		transform.position = hit.point;
	}
}
