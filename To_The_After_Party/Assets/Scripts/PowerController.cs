using UnityEngine;
using System.Collections;

public class PowerController : MonoBehaviour {

	public float DistToPlayerToLookAtSwitch = 60.0f;
	public float DistToPlayerToFlipSwitch = 45.0f;
	public int NumFramesUntilRefresh = 25;

	public string playerTag, trinketsTag;
	private GameObject player, trinkets;
	public float distToPlayer, distToTrinkets;

	private int CurFrameNumber = 0;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag(playerTag);
		trinkets = GameObject.FindGameObjectWithTag(trinketsTag);

		//FlipSwitch(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (CurFrameNumber == NumFramesUntilRefresh) {
			CurFrameNumber = 0;

			Vector3 distBetweenPlayer = this.GetComponent<Rigidbody2D>().transform.position - player.GetComponent<Rigidbody2D>().transform.position;
			Vector3 distBetweenTrinkets = this.GetComponent<Rigidbody2D>().transform.position - trinkets.GetComponent<Rigidbody2D>().transform.position;
			distToPlayer = distBetweenPlayer.magnitude;
			distToTrinkets = distBetweenTrinkets.magnitude;

			if ((distToPlayer <= DistToPlayerToLookAtSwitch) || (distToTrinkets <= DistToPlayerToLookAtSwitch)) {
				for(int i=0; i<transform.childCount; i++)
				{
					GameObject obj = transform.GetChild(i).gameObject;
					if (obj.tag == "wall") {
						Vector3 distTo = obj.transform.position - player.GetComponent<Rigidbody2D>().transform.position;
						if (distTo.magnitude <= DistToPlayerToFlipSwitch) {
							obj.SetActive(true);
						}
						else {
							//obj.SetActive(false);
						}
					}
				}
			}
			else {
				//FlipSwitch(false);
			}
		}
		else {
			CurFrameNumber++;
		}
	}

	void FlipSwitch(bool turnOn) {
		for(int i=0; i<transform.childCount; i++)
		{
			if (transform.GetChild(i).gameObject.tag == "wall") {
				transform.GetChild(i).gameObject.SetActiveRecursively(turnOn);
			}
		}
	}
}
