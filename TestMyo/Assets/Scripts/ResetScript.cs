using UnityEngine;
using System.Collections;

public class ResetScript : MonoBehaviour {

	public Transform gameManager;
	private GameManager gameManagerScript;

	// Use this for initialization
	void Start () {
		gameManagerScript = gameManager.GetComponent<GameManager>();
	}
	
	float spaceDown;
	
	// If valid, we reset the entire game.
	void Update () {
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
		
	}
}
