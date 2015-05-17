using UnityEngine;
using System.Collections;

public class PowerControllerEnemy : MonoBehaviour {

	public float DistToPlayerToFlipSwitch;

	public string playerTag, trinketsTag;
	private GameObject player, trinkets;
	public float distToPlayer, distToTrinkets;

	private PolyNavAgent polyNavAgent;
	private MoveBetween moveBetween;

	public bool RequiresCompanion = true;

	// Use this for initialization
	void Start () {
		polyNavAgent = GetComponent<PolyNavAgent>();
		moveBetween = GetComponent<MoveBetween>();

		player = GameObject.FindGameObjectWithTag(playerTag);

		if (RequiresCompanion)
			trinkets = GameObject.FindGameObjectWithTag(trinketsTag);

		FlipSwitch(false);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 distBetweenPlayer = this.GetComponent<Rigidbody2D>().transform.position - player.GetComponent<Rigidbody2D>().transform.position;
		distToPlayer = distBetweenPlayer.magnitude;

		if (RequiresCompanion) {
			Vector3 distBetweenTrinkets = this.GetComponent<Rigidbody2D>().transform.position - trinkets.GetComponent<Rigidbody2D>().transform.position;
			distToTrinkets = distBetweenTrinkets.magnitude;
		}

		if ((distToPlayer <= DistToPlayerToFlipSwitch) || (RequiresCompanion && (distToTrinkets <= DistToPlayerToFlipSwitch))) {
			FlipSwitch(true);
		}
		else {
			FlipSwitch(false);
		}
	}

	void FlipSwitch(bool turnOn) {
		polyNavAgent.enabled = turnOn;

		if (moveBetween) {
			moveBetween.enabled = turnOn;
		}
	}
}
