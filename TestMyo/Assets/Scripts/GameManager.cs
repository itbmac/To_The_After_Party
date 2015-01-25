using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {

	// Public settings
	public int TotalTime = 60;
	public bool BlobsEnabled = true;
	
	public bool GameRunning { get; private set; }
	private float startTime;
	
	public static GameManager Instance;
	
	public Action GameStart;
	
	void Awake() {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		GameRunning = false;
	}
	
	public void StartGame() {
		GameRunning = true;
		startTime = Time.time;
		if (GameStart != null)
			GameStart();
	}
	
	// Update is called once per frame
	void Update () {
		int timeRemaining = GameRunning ? (int)Mathf.Max (0, TotalTime - (Time.time - startTime)) : TotalTime;
		HUD.Instance.timer = timeRemaining;
		
		if (timeRemaining == 0) {
			// TODO: game over
			
			GameRunning = false;
		}
	
	}
}
