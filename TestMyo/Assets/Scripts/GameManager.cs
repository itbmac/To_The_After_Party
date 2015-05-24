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
	
	public static bool OnePlayerMode;

	void Awake() {
		CurrentState = GameState.NotStarted;
		Instance = this;
	}
	
	void Start() {
		if (!ThalmicHub.instance.hubInitialized)
			Debug.LogWarning("Hub not initialized");
		
		HUD.Instance.message = 0;
		Cursor.visible = false;
		
		int duration = LoadSettings.GetInt("duration");
		if (duration != -1)
			TotalTime = duration;

		/*
		int onePlayerModeSetting = LoadSettings.GetInt ("one_player_mode");
		if (onePlayerModeSetting == 1) {
			OnePlayerMode = true;
		} else {
			OnePlayerMode = false;
		}
		*/
	}
	
	private void StartGame() {
		CurrentState = GameState.Running;
		HUD.Instance.message = 0;
		
		startTime = Time.time;
		if (GameStartCallbacks != null)
			GameStartCallbacks();
			
		GetComponent<AudioSource>().PlayOneShot(PaintingBegin, 1);
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
			if (Input.GetKeyDown(KeyCode.Space) || CameraEntry.Instance.Completed()) {
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
				GetComponent<AudioSource>().PlayOneShot(ThirtySecondsLeft, 1);
			} else if (timeRemaining <= 15 && lastTimeRemaining > 15) {
				// 15 seconds left!
				GetComponent<AudioSource>().PlayOneShot(FifteenSecondsLeft, 1);
			} else if (timeRemaining <= 5 && lastTimeRemaining > 5) {
				// 5 seconds left!
				GetComponent<AudioSource>().PlayOneShot(FiveSecondsLeft, 1);
			} else if (timeRemaining == 0) {
				CurrentState = GameState.Over;
				if (Painting.Instance.RedScore > Painting.Instance.BlueScore) {
					// red wins
					GetComponent<AudioSource>().PlayOneShot(RedWins, 1);
					HUD.Instance.message = 3;
				} else {
					// blue wins (break ties to blue)
					GetComponent<AudioSource>().PlayOneShot(BlueWins, 1);
					HUD.Instance.message = 4;
				}
				
				var shouldSaveScreenshot = LoadSettings.GetInt("save_screenshot_on_game_over");
				if (shouldSaveScreenshot == 1) {
					// don't just convert to bool, because needs to be not -1
					// which indicates setting not found
					Screenshot.Instance.SavePainting();
				}
			}
		}
		
		lastTimeRemaining = timeRemaining;
	}
}
