using UnityEngine;
using System.Collections;

public class TimedDeath : MonoBehaviour {

	void Start () {
		Destroy(gameObject, 5);
	}
}
