using UnityEngine;
using System.Collections;


public class DebugForward : MonoBehaviour {
	void OnDrawGizmos() {
		Gizmos.color = Color.blue;
		Gizmos.DrawRay(transform.position, transform.forward);
	}
}
