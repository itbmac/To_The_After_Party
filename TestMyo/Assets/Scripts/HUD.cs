using UnityEngine;
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
	public Texture2D[] messages;

	//size parameters for HUD:
	public Vector2 barPos;
	public Vector2 barSize;

	public float redScore;
	public float blueScore;

	public Vector2 timerHUDSize;
	public Vector2 timerSize;
	public int timer;
	
	public Vector2 messageSize;
	public int message;

	//get screen vars:
	private float sw;
	private float sh;
	
	public static HUD Instance;
	
	void Awake() {
		Instance = this;
	}
	void Start () { 
		sw = Screen.width; sh = Screen.height;
	}
	
	Color TransformColorBar(Color c) {
		// TODO
		return c;
	}
	
	Color TransformColorSide(Color c) {
		// TODO
		return c;
	}

	//Draw the HUD:
	void OnGUI() {

		//set the skin texture.
		GUI.skin = textSkin;

		//update screen width and height if it changes
		sw = Screen.width; sh = Screen.height;

		//if gameMode is multiplayer competitive
		if(!GameManager.OnePlayerMode) {
			
			//draw back sides
			GUI.color = TransformColorSide(Painting.Instance.OurRed);
			GUI.DrawTexture(new Rect((int)(sw*.5f-sw*barSize.x-sw*barPos.x), (int)(sh*barPos.y-sh*barSize.y*.5f), 
			                         (int)(sw*barSize.x), (int)(sh*barSize.y)), redBack, ScaleMode.StretchToFill);
			GUI.color = TransformColorSide(Painting.Instance.OurBlue);
			GUI.DrawTexture(new Rect((int)(sw*.5f+sw*barPos.x), (int)(sh*barPos.y-sh*barSize.y*.5f), (int)(sw*barSize.x), 
			                         (int)(sh*barSize.y)), blueBack, ScaleMode.StretchToFill);

			//draw bars
			GUI.color = TransformColorBar(Painting.Instance.OurRed);
			GUI.DrawTexture(new Rect((int)(sw*.5f-sw*barSize.x*(redScore)-sw*barPos.x), (int)(sh*barPos.y-sh*barSize.y*.5f), 
			                         (int)(sw*barSize.x*redScore), (int)(sh*barSize.y)), redBar, ScaleMode.StretchToFill);
			GUI.color = TransformColorBar(Painting.Instance.OurBlue);
			GUI.DrawTexture(new Rect((int)(sw*.5f+sw*barPos.x), (int)(sh*barPos.y-sh*barSize.y*.5f), (int)(sw*barSize.x*blueScore), 
			                         (int)(sh*barSize.y)), blueBar, ScaleMode.StretchToFill);
			GUI.color = Color.white;
			
			//draw the bar percentages
			GUI.Label(new Rect( (int)(sw*.5f-sw*barSize.x-sw*barPos.x) , (int)(sh*barPos.y-sh*timerSize.y*.5f) , (int)(sw*barSize.x), 
			                   (int)(sh*timerHUDSize.y)),((int)(redScore*100f)).ToString()+"%");
			GUI.Label(new Rect( (int)(sw*.5f+sw*barPos.x) , (int)(sh*barPos.y-sh*timerSize.y*.5f) , (int)(sw*barSize.x), 
			                   (int)(sh*timerHUDSize.y)),((int)(blueScore*100f)).ToString()+"%");
		}
		
		//draw the hud timer
		GUI.DrawTexture(new Rect((int)(sw*.5f-sw*timerHUDSize.x*.5f), (int)(sh*barPos.y-sh*timerHUDSize.y*.5f), (int)(sw*timerHUDSize.x), 
		                         (int)(sh*timerHUDSize.y)), timerHUD, ScaleMode.StretchToFill);

		//draw the time
		GUI.Label(new Rect( (int)(sw*.5f-sw*timerSize.x*.5f) , (int)(sh*barPos.y-sh*timerSize.y*.5f) , (int)(sw*timerHUDSize.x), 
		          (int)(sh*timerHUDSize.y)),timer.ToString());

		//draw any additional messages.
		GUI.DrawTexture(new Rect((int)(sw*.5f-sw*messageSize.x*.5f), (int)(sh*.5f-sh*messageSize.y*.5f), (int)(sw*messageSize.x), 
		                         (int)(sh*messageSize.y)), messages[message], ScaleMode.StretchToFill);
	}

}
