using UnityEngine;
using System.Collections;

public class DelayedFollow : MonoBehaviour {

	public GameObject player;
	public float DistanceToActivate = 4.0f;
	public float DistToPlayer;

	// Use this for initialization
	void Start () {
		this.GetComponent<FollowPlayer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 dist = player.GetComponent<Rigidbody2D>().transform.position - this.GetComponent<Rigidbody2D>().transform.position;
		DistToPlayer = dist.magnitude;
		if (DistToPlayer <= DistanceToActivate) {
			this.GetComponent<FollowPlayer>().enabled = true;
		}
	}
}
