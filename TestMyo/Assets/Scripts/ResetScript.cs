using UnityEngine;
using System.Collections;

public class ResetScript : MonoBehaviour {

	private GameManager gameManagerScript;
	public GameObject onePlayerHand;
	public Transform gameManager;
	float spaceDown;
	float tDown;

	void Start () {
		gameManagerScript = gameManager.GetComponent<GameManager>();
	}

	
	void Update () {

		//hold space down to reset game
		if (Input.GetKeyDown(KeyCode.Space)) {
			spaceDown = Time.time;
			if(gameManagerScript.CurrentState == GameManager.GameState.Over) {
				Application.LoadLevel(Application.loadedLevel);
			}	
		}
		if (gameManagerScript.CurrentState == GameManager.GameState.Running &&
			Input.GetKey(KeyCode.Space) && Time.time - spaceDown > 1) {
			Application.LoadLevel(Application.loadedLevel);
		}

		//hold T down to switch game modes and reset game
		if (Input.GetKeyDown(KeyCode.T)) {
			tDown = Time.time;
			if(gameManagerScript.CurrentState == GameManager.GameState.Over) {
				GameManager.OnePlayerMode = !GameManager.OnePlayerMode;
				Application.LoadLevel(Application.loadedLevel);			
			}	
		}
		if (Input.GetKey(KeyCode.T) && Time.time - tDown > 1) {
			GameManager.OnePlayerMode = !GameManager.OnePlayerMode;
			Application.LoadLevel(Application.loadedLevel);
		}
		
	}
}
