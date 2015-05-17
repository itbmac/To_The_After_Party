using UnityEngine;
using System.Collections;

public class Free_Roam_AI : MonoBehaviour {

	public float StationarySpeed = 0.02f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (this.GetComponent<Rigidbody2D>().velocity.magnitude <= StationarySpeed * 2) {
			this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		}
	}
}
