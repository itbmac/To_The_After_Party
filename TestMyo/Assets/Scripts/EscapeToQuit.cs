﻿using UnityEngine;
using System.Collections;

public class EscapeToQuit : MonoBehaviour {

	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
	}
}
