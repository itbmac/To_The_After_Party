﻿using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {

	//GUI skin:
	public GUISkin textSkin;

	//load textures
	public Texture2D redBar;
	public Texture2D redBack;
	public Texture2D blueBar;
	public Texture2D blueBack;
	public Texture2D timerHUD;

	//size parameters for HUD:
	public Vector2 barPos;
	public Vector2 barSize;

	public float redScore;
	public float blueScore;

	public Vector2 timerHUDSize;
	public Vector2 timerSize;
	public int timer;

	//get screen vars:
	private float sw;
	private float sh;



	//setup:
	void Start () { 
		sw = Screen.width; sh = Screen.height;
	}

	//Draw the HUD:
	void OnGUI() {

		GUI.skin = textSkin;

		//update screen width and height if it changes
		sw = Screen.width; sh = Screen.height;

		//draw back sides
		GUI.DrawTexture(new Rect((int)(sw*.5f-sw*barSize.x-sw*barPos.x), (int)(sh*barPos.y-sh*barSize.y*.5f), 
		                         (int)(sw*barSize.x), (int)(sh*barSize.y)), redBack, ScaleMode.StretchToFill);
		GUI.DrawTexture(new Rect((int)(sw*.5f+sw*barPos.x), (int)(sh*barPos.y-sh*barSize.y*.5f), (int)(sw*barSize.x), 
		                         (int)(sh*barSize.y)), blueBack, ScaleMode.StretchToFill);

		//draw bars
		GUI.DrawTexture(new Rect((int)(sw*.5f-sw*barSize.x*(redScore)-sw*barPos.x), (int)(sh*barPos.y-sh*barSize.y*.5f), 
		                         (int)(sw*barSize.x*redScore), (int)(sh*barSize.y)), redBar, ScaleMode.StretchToFill);
		GUI.DrawTexture(new Rect((int)(sw*.5f+sw*barPos.x), (int)(sh*barPos.y-sh*barSize.y*.5f), (int)(sw*barSize.x*blueScore), 
		                         (int)(sh*barSize.y)), blueBar, ScaleMode.StretchToFill);


		//draw the hud timer
		GUI.DrawTexture(new Rect((int)(sw*.5f-sw*timerHUDSize.x*.5f), (int)(sh*barPos.y-sh*timerHUDSize.y*.5f), (int)(sw*timerHUDSize.x), 
		                         (int)(sh*timerHUDSize.y)), timerHUD, ScaleMode.StretchToFill);

		//draw the time
		GUI.Label(new Rect( (int)(sw*.5f-sw*timerSize.x*.5f) , (int)(sh*barPos.y-sh*timerSize.y*.5f) , (int)(sw*timerHUDSize.x), 
		          (int)(sh*timerHUDSize.y)),timer.ToString());

	}

}
