using UnityEngine;
using System.Collections;

public class ResetScript : MonoBehaviour {

	public Transform gameManager;
	private GameManager gameManagerScript;

	// Use this for initialization
	void Start () {
		gameManagerScript = gameManager.GetComponent<GameManager>();
	}
	
	// If valid, we reset the entire game.
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space) && gameManagerScript.CurrentState == GameManager.GameState.Over) {
			Application.LoadLevel("Test Painting");
		}
	}
}
