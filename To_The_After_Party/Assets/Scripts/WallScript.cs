using UnityEngine;
using System.Collections;

public class WallScript : MonoBehaviour {
	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "bulletPlayer")
			Destroy(other.gameObject);
	}
}
