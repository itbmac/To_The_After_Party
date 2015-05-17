using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class ControlHelpPrompts : MonoBehaviour {

	public GameObject InstructionScreen;
	public List<GameObject> GUITextObjs = new List<GameObject>();

	public float ControlMinValue = 0.05f;
	public float DisplayLengthAfterUse = 1.0f;

	public float UnusedTimeLength_LeftStick = 5.0f;
	public float UnusedTimeLength_RightStick = 15.0f;
	public float UnusedTimeLength_RightTrigger = 20.0f;

	public float UnusedTime_LeftStick = 0f;
	public float UnusedTime_RightStick = 0f;
	public float UnusedTime_RightTrigger = 0f;

	public float UsedTime_LeftStick = 0f;
	public float UsedTime_RightStick = 0f;
	public float UsedTime_RightTrigger = 0f;

	public bool HasBeenUsedIndicator_LeftStick = false;
	public bool HasBeenUsedIndicator_RightStick = false;
	public bool HasBeenUsedIndicator_RightTrigger = false;

	public bool CanFire = true;

	void Start() {
		for (int i = 0; i < GUITextObjs.Count; i++) {
			GUITextObjs[i].SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!InstructionScreen.activeSelf) {
			InControl.InputDevice inputDevice = InputManager.ActiveDevice;

			if (!HasBeenUsedIndicator_LeftStick) {
				float xL = inputDevice.LeftStickX;
				float yL = inputDevice.LeftStickY;
				
				if ((xL >= ControlMinValue) && (yL >= ControlMinValue)) {
					UsedTime_LeftStick = Time.time;
					HasBeenUsedIndicator_LeftStick = true;
				}
				else {
					UnusedTime_LeftStick += Time.deltaTime;
				}
			}

			if (!HasBeenUsedIndicator_RightStick) {
				float xR = inputDevice.RightStickX;
				float yR = inputDevice.RightStickY;
				
				if ((xR >= ControlMinValue) && (yR >= ControlMinValue)) {
					UsedTime_RightStick = Time.time;
					HasBeenUsedIndicator_RightStick = true;
				}
				else {
					UnusedTime_RightStick += Time.deltaTime;
				}
			}

			if (CanFire && !HasBeenUsedIndicator_RightTrigger) {
				bool pulled = inputDevice.RightTrigger;

				if (pulled) {
					UsedTime_RightTrigger = Time.time;
					HasBeenUsedIndicator_RightTrigger = true;
				}
				else {
					UnusedTime_RightTrigger += Time.deltaTime;
				}
			}

			if (!HasBeenUsedIndicator_LeftStick && (UnusedTime_LeftStick >= UnusedTimeLength_LeftStick)) {
				GUITextObjs[0].GetComponent<GUIText>().text = "Use the left thumbstick to Move.";
				GUITextObjs[0].SetActive(true);
			}
			else if (HasBeenUsedIndicator_LeftStick && (Time.time <= DisplayLengthAfterUse + UsedTime_LeftStick)) {
				GUITextObjs[0].SetActive(false);
			}
			else if (!HasBeenUsedIndicator_RightStick && (UnusedTime_RightStick >= UnusedTimeLength_RightStick))  {
				GUITextObjs[0].GetComponent<GUIText>().text = "Use the right thumbstick to aim.";
				GUITextObjs[0].SetActive(true);
			}
			else if (HasBeenUsedIndicator_RightStick && (Time.time <= DisplayLengthAfterUse + UsedTime_RightStick)) {
				GUITextObjs[0].SetActive(false);
			}
			else if (CanFire && !HasBeenUsedIndicator_RightTrigger && (UnusedTime_RightTrigger >= UnusedTimeLength_RightTrigger)) {
				GUITextObjs[0].GetComponent<GUIText>().text = "Use the right trigger to fire.";
				GUITextObjs[0].SetActive(true);
			}
			else if (HasBeenUsedIndicator_RightTrigger && (Time.time <= DisplayLengthAfterUse + UsedTime_RightTrigger)) {
				GUITextObjs[0].SetActive(false);
			}

			if(HasBeenUsedIndicator_LeftStick && HasBeenUsedIndicator_RightStick && (!CanFire || HasBeenUsedIndicator_RightTrigger)) {
				this.enabled = false;
			}
		}
	}
}
