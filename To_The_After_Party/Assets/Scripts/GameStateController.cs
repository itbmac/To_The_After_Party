using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameStateController : MonoBehaviour {

	public string PlayerOneTag, PlayerTwoTag;
	public GameObject PlayerOneObject, PlayerTwoObject;
	public bool TwoPlayerMode = true;

	public int SpawnPointNumber = 0;
	public List<Vector2> SpawnPointsPlayerOne = new List<Vector2>();
	public List<Vector2> SpawnPointsPlayerTwo = new List<Vector2>();

	public string SwitchLevelString;

	// Use this for initialization
	void Start () {
		SpawnPointNumber = 0;
		PlayerOneObject = GameObject.FindGameObjectWithTag(PlayerOneTag);
		PlayerTwoObject = GameObject.FindGameObjectWithTag(PlayerTwoTag);
	}
	
	// Update is called once per frame
	void Update () {
		if (!PlayerOneObject.activeSelf || (TwoPlayerMode && !PlayerTwoObject.activeSelf)) {
			ResetGame();
		}
	}

	public void ResetGame() {
		if (TwoPlayerMode) {
			PlayerTwoObject.transform.position = new Vector3(SpawnPointsPlayerTwo[SpawnPointNumber].x, SpawnPointsPlayerTwo[SpawnPointNumber].y, 0);
			PlayerTwoObject.GetComponent<PlayerMovement>().enabled = true;
			PlayerTwoObject.SetActive(true);
		}

		PlayerOneObject.transform.position = new Vector3(SpawnPointsPlayerOne[SpawnPointNumber].x, SpawnPointsPlayerOne[SpawnPointNumber].y, 0);
		PlayerOneObject.GetComponent<PlayerMovement>().enabled = true;
		PlayerOneObject.SetActive(true);
	}

	void OnDrawGizmosSelected(){
		for ( int i = 0; i < SpawnPointsPlayerOne.Count; i++)
			Gizmos.DrawSphere(SpawnPointsPlayerOne[i], 0.05f);
		for ( int i = 0; i < SpawnPointsPlayerTwo.Count; i++)
			Gizmos.DrawSphere(SpawnPointsPlayerTwo[i], 0.05f);
	}
}
