using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public int TotalTime = 60;
	public bool GameRunning { get; private set; }
	private float startTime;
	
	public static GameManager Instance;
	
	void Awake() {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		GameRunning = true;
	}
	
	// Update is called once per frame
	void Update () {
		int timeRemaining = (int)Mathf.Max (0, TotalTime - (Time.time - startTime));
		HUD.Instance.timer = timeRemaining;
		
		if (timeRemaining == 0) {
			// TODO: game over
			
			GameRunning = false;
		}
	
	}
}
