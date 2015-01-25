using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour {

	void Update () {
		if (Input.GetKeyDown(KeyCode.M))
			audio.mute = !audio.mute;
	}
}
