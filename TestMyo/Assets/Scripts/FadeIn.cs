using UnityEngine;
using System.Collections;

public class FadeIn : MonoBehaviour {
	public Texture black;
	float fadeLevel = 1;

	// Use this for initialization
	void Start () {
	}
	
	// OnGUI is called once per frame
	void OnGUI () {
		GUI.depth = 0;
		GUI.color = new Color (1, 1, 1, fadeLevel);
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), black, ScaleMode.StretchToFill, true);
		if (fadeLevel > 0) fadeLevel -= 0.5f * Time.deltaTime;
	}
}
