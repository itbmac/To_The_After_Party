using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {

	// Public settings
	public int TotalTime = 60;
	public bool BlobsEnabled = true;
	public bool BlobsUseGestures = true;
	public GameObject Reticle;
	
	public float BlobChargeTime = 0.4f;
	public float BlobCooldown = 0.2f;
	
	public enum GameState {NotStarted, WaitForCalibrate, WaitForStart, Running, Over}
	public GameState CurrentState { get; private set; }
	
	private float startTime;
	
	public static GameManager Instance;
	
	public Action GameStartCallbacks;
	
	public AudioClip FiveSecondsLeft;
	public AudioClip FifteenSecondsLeft;
	public AudioClip ThirtySecondsLeft;
	public AudioClip RedWins;
	public AudioClip BlueWins;
	public AudioClip PaintingBegin;
	
	void Awake() {
		CurrentState = GameState.NotStarted;
		Instance = this;
	}
	
	void Start() {
		if (!ThalmicHub.instance.hubInitialized)
			Debug.Log ("detected hub not initialized");
		else
			Debug.Log("hub is good");
		
		HUD.Instance.message = 0;
	}
	
	private void StartGame() {
		CurrentState = GameState.Running;
		HUD.Instance.message = 0;
		
		startTime = Time.time;
		if (GameStartCallbacks != null)
			GameStartCallbacks();
			
		audio.PlayOneShot(PaintingBegin, 1);
	
	}
	
	private int lastTimeRemaining;
	
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
		
		if (CurrentState == GameState.NotStarted) {
			if (Input.GetKeyDown(KeyCode.Space)) {
				HUD.Instance.message = 1;
				CurrentState = GameState.WaitForCalibrate;
			}
		} else if (CurrentState == GameState.WaitForCalibrate) {
			bool allSynced = true;
			foreach (Transform t in ThalmicHub.instance.transform) {
				allSynced = allSynced && t.GetComponent<ThalmicMyo>().armSynced;
			}
			if (allSynced || Input.GetKeyDown(KeyCode.Space)) {
				CurrentState = GameState.WaitForStart;
				HUD.Instance.message = 2;
			}
		} else if (CurrentState == GameState.WaitForStart) {
		
			if (Input.GetKeyDown(KeyCode.Space)) {
				StartGame ();
			}
			
		} if (CurrentState == GameState.Running) {
			if (timeRemaining <= 30 && lastTimeRemaining > 30) {
				// 30 seconds left!
				audio.PlayOneShot(ThirtySecondsLeft);
				
				// TODO: sound, maybe HUD message?
			} else if (timeRemaining <= 15 && lastTimeRemaining > 15) {
				// 15 seconds left!
				audio.PlayOneShot(FifteenSecondsLeft);
				
				// TODO: sound, maybe HUD message?
			} else if (timeRemaining <= 5 && lastTimeRemaining > 5) {
				// 5 seconds left!
				audio.PlayOneShot(FiveSecondsLeft);
				
				// TODO: sound, maybe HUD message?
			} else if (timeRemaining == 0) {
				// TODO: game over
				
				CurrentState = GameState.Over;
				if (Painting.Instance.RedScore > Painting.Instance.BlueScore) {
					// red wins
					audio.PlayOneShot(RedWins);
					
					// TODO: HUD and sound
					
					HUD.Instance.message = 3;
				} else {
					// blue wins (break ties to blue)
					audio.PlayOneShot(BlueWins);
					
					// TODO: HUD and sound
					HUD.Instance.message = 4;
				}
				
				// TODO: HUD - press space to restart
			}
		}
		
		lastTimeRemaining = timeRemaining;
	}
}
