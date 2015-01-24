using UnityEngine;
using System.Collections;
using InControl;

public class AdvanceToNextLevel : MonoBehaviour {

	public string levelName;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		InControl.InputDevice inputDevice = InputManager.ActiveDevice;
		
		if(inputDevice.AnyButton.IsPressed)
		{
			Application.LoadLevel(levelName);
		}
	}
}
