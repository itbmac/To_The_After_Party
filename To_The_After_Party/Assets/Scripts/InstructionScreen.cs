using UnityEngine;
using System.Collections;
using InControl;

public class InstructionScreen : MonoBehaviour {

	public GameObject Walls, GameStateControl, Loading, TestLoadCompleteObj;
	public int NumFramesUntilRefresh = 25;

	private int CurFrameNumber = 0;
	private bool firstUpdateComplete;
	private GameStateController gameStateController;

	// Use this for initialization
	void Start () {
		transform.position = Camera.main.transform.position;

		firstUpdateComplete = false;
		gameStateController = GameStateControl.GetComponent<GameStateController>();
		this.GetComponent<SpriteRenderer>().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (!firstUpdateComplete) {
			if (CurFrameNumber == NumFramesUntilRefresh) {
				Walls.SetActiveRecursively(true);
				firstUpdateComplete = true;
			}
			else {
				CurFrameNumber++;
			}
		}
		else {
			if (TestLoadCompleteObj.activeSelf) {
				Loading.guiText.text = "Press <A> to Play";

				InControl.InputDevice inputDevice = InputManager.ActiveDevice;
				if(inputDevice.AnyButton.IsPressed)
				{
					this.gameObject.SetActive(false);
					Loading.SetActive(false);
					gameStateController.ResetGame();
				}
			}
		}
	}
}
