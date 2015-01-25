using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {

	// Public settings
	public int TotalTime = 60;
	public bool BlobsEnabled = true;
	
	public enum GameState {NotStarted, Running, Over}
	public GameState CurrentState { get; private set; }
	
	private float startTime;
	
	public static GameManager Instance;
	
	public Action GameStart;
	
	void Awake() {
		CurrentState = GameState.NotStarted;
		Instance = this;
	}
	
	public void StartGame() {
		CurrentState = GameState.Running;
		
		startTime = Time.time;
		if (GameStart != null)
			GameStart();
	}
	
	// Update is called once per frame
	void Update () {
		int timeRemaining;
		if (CurrentState == GameState.Running)
			timeRemaining = (int)Mathf.Max (0, TotalTime - (Time.time - startTime));
		else if (CurrentState == GameState.Over)
			timeRemaining = 0;
		else
			timeRemaining = TotalTime;
			
		HUD.Instance.timer = timeRemaining;
		
		if (timeRemaining == 0 && CurrentState == GameState.Running) {
			// TODO: game over
			
			CurrentState = GameState.Over;
		}
	
	}
}
