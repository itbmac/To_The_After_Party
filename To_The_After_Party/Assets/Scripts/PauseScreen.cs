using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class PauseScreen : MonoBehaviour {

	public enum MenuOptionAction {HELP, RESTART_LEVEL, NEXT_LEVEL, PREVIOUS_LEVEL, CREDITS, BACK};
	public enum MenuState {DEFAULT, HELP, CREDITS};

	public GameObject InstructionScreen, ControlHelpPrompt;
	public GameObject Background, TitleText, UnpauseText;
	private string TitleStr, UnpausedStr;
	public List<GameObject> MenuOptionObjs = new List<GameObject>();
	public List<MenuOptionAction> MenuOptionActs = new List<MenuOptionAction>();
	private List<string> MenuOptionStrs = new List<string>();
	public List<string> MenuOptionHelpStrs = new List<string>();
	public List<string> MenuOptionCreditsStrs = new List<string>();
	private MenuState currentMenuState;

	public float UnselectedMenuOptionX = 0.268f;
	public float SelectedMenuOptionX = 0.25f;
	public float NonDefaultMenuOptionX = 0.33333f;

	public string LevelName_Previous, LevelName_Current, LevelName_Next;

	public int SelectedMenuOptionIndex = 0;

	bool paused = false;

	// Use this for initialization
	void Start () {
		for ( int i = 0; i < MenuOptionObjs.Count; i++)
			MenuOptionStrs.Add(MenuOptionObjs[i].guiText.text);

		TitleStr = TitleText.guiText.text;
		UnpausedStr = UnpauseText.guiText.text;

		SwitchActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (!InstructionScreen.activeSelf) {
			InControl.InputDevice inputDevice = InputManager.ActiveDevice;
			
			if(inputDevice.MenuWasPressed)
			{
				paused = togglePause();
			}

			if (paused) {
				if (currentMenuState == MenuState.DEFAULT) {
					for ( int i = 0; i < MenuOptionObjs.Count; i++) {
						MenuOptionObjs[i].guiText.anchor = TextAnchor.UpperLeft;
						if (i == SelectedMenuOptionIndex) {
							MenuOptionObjs[i].transform.position = new Vector3(SelectedMenuOptionX, MenuOptionObjs[i].transform.position.y, 0);
							MenuOptionObjs[i].guiText.text = "> " + MenuOptionStrs[i];
						}
						else {
							MenuOptionObjs[i].transform.position = new Vector3(UnselectedMenuOptionX, MenuOptionObjs[i].transform.position.y, 0);
							MenuOptionObjs[i].guiText.text = MenuOptionStrs[i];
						}
					}

					if (inputDevice.Direction.Down && inputDevice.Direction.HasChanged)
						SelectedMenuOptionIndex = Mathf.Min(MenuOptionObjs.Count - 1, SelectedMenuOptionIndex + 1);
					else if (inputDevice.Direction.Up && inputDevice.Direction.HasChanged)
						SelectedMenuOptionIndex = Mathf.Max(0, SelectedMenuOptionIndex - 1);

					if(inputDevice.Action1) {
						if (MenuOptionActs[SelectedMenuOptionIndex] == MenuOptionAction.HELP) {
							SwitchMenuState(MenuState.HELP);
						}
						else if (MenuOptionActs[SelectedMenuOptionIndex] == MenuOptionAction.RESTART_LEVEL) {
							Time.timeScale = 1f;
							Application.LoadLevel(LevelName_Current);
						}
						else if (MenuOptionActs[SelectedMenuOptionIndex] == MenuOptionAction.NEXT_LEVEL) {
							Time.timeScale = 1f;
							Application.LoadLevel(LevelName_Next);
						}
						else if (MenuOptionActs[SelectedMenuOptionIndex] == MenuOptionAction.PREVIOUS_LEVEL) {
							Time.timeScale = 1f;
							Application.LoadLevel(LevelName_Previous);
						}
						else if (MenuOptionActs[SelectedMenuOptionIndex] == MenuOptionAction.CREDITS) {
							SwitchMenuState(MenuState.CREDITS);
						}
					}
				}
				else if (currentMenuState == MenuState.HELP) {
					for ( int i = 0; i < MenuOptionCreditsStrs.Count; i++) {
						MenuOptionObjs[i].guiText.text = MenuOptionHelpStrs[i];
					}

					if (inputDevice.Action2)
						SwitchMenuState(MenuState.DEFAULT);
				}
				else if (currentMenuState == MenuState.CREDITS) {
					for ( int i = 0; i < MenuOptionCreditsStrs.Count; i++) {
						MenuOptionObjs[i].guiText.text = MenuOptionCreditsStrs[i];
					}

					if (inputDevice.Action2)
						SwitchMenuState(MenuState.DEFAULT);
				}
			}
		}
	}

	bool togglePause()
	{
		if(Time.timeScale == 0f)
		{
			Time.timeScale = 1f;

			SwitchActive(false);

			return(false);
		}
		else
		{
			Time.timeScale = 0f;

			SwitchMenuState(MenuState.DEFAULT);
			SwitchActive(true);
			Background.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height/2, Camera.main.nearClipPlane) );

			return(true);    
		}
	}

	void SwitchMenuState(MenuState newMenuState) {
		currentMenuState = newMenuState;

		if (newMenuState == MenuState.DEFAULT) {
			TitleText.guiText.text = TitleStr;
			UnpauseText.guiText.text = UnpausedStr;
		}
		else if (newMenuState == MenuState.HELP) {
			TitleText.guiText.text = "Help";
			UnpauseText.guiText.text = "Press <B> to Go Back";

			for ( int i = 0; i < MenuOptionObjs.Count; i++) {
				MenuOptionObjs[i].transform.position = new Vector3(NonDefaultMenuOptionX, MenuOptionObjs[i].transform.position.y, 0);
				MenuOptionObjs[i].guiText.anchor = TextAnchor.UpperCenter;

				if (i >= MenuOptionHelpStrs.Count)
					MenuOptionObjs[i].guiText.text = "";
				else 
					MenuOptionObjs[i].guiText.text = MenuOptionHelpStrs[i];
			}
		}
		else if (newMenuState == MenuState.CREDITS) {
			TitleText.guiText.text = "Credits";
			UnpauseText.guiText.text = "Press <B> to Go Back";

			for ( int i = 0; i < MenuOptionObjs.Count; i++) {
				MenuOptionObjs[i].transform.position = new Vector3(NonDefaultMenuOptionX, MenuOptionObjs[i].transform.position.y, 0);
				MenuOptionObjs[i].guiText.anchor = TextAnchor.UpperCenter;

				if (i >= MenuOptionCreditsStrs.Count)
					MenuOptionObjs[i].guiText.text = "";
				else 
					MenuOptionObjs[i].guiText.text = MenuOptionCreditsStrs[i];
			}
		}
	}

	void SwitchActive(bool isActive) {
		ControlHelpPrompt.SetActive(!isActive);
		TitleText.SetActive(isActive);
		UnpauseText.SetActive(isActive);
		Background.GetComponent<SpriteRenderer>().enabled = isActive;

		for ( int i = 0; i < MenuOptionObjs.Count; i++)
			MenuOptionObjs[i].SetActive(isActive);
	}
}
