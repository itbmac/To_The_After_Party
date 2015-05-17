using UnityEngine;
using System.Collections;

#pragma warning disable

public class CameraEffects : MonoBehaviour {

	public Camera camera;
	private CameraEntry cameraEntryScript;
	private bool enabled = false;

	// waviness
	public float shakeAmount = 0.5f;
	public float maxShakeAmount = 4.0f;
	public float shakeIncreaseFactor = 0.4f;
	public Vector3 timeFactors = new Vector3(5.0f,7.0f,11.0f);
	private Vector3 originalEulerAngles;
	public float slownessFactor = 10.0f;
	public float maxSlownessFactor = 3.0f;
	public float slownessReductionFactor = 0.05f;
	// for the waviness (sine function)
	private float shakeTime = 0.0f;

	// blur
	public float blurIntensity = 0.0f;
	public float maxBlurIntensity = 0.6f;
	public float blurIntensityFactor = 0.15f;
	private MotionBlur blurScript;

	// Use this for initialization
	void Start () {
		cameraEntryScript = camera.GetComponent<CameraEntry>();
		blurScript = camera.GetComponent<MotionBlur>();
		blurScript.blurAmount = 0.0f;
		blurScript.enabled = true;
		originalEulerAngles = camera.transform.localEulerAngles;
		enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(GameManager.Instance.CurrentState != GameManager.GameState.Running) {
			originalEulerAngles = camera.transform.localEulerAngles;
			return;
		}
		
		if (GameManager.Instance.CurrentState != GameManager.GameState.Running)
			return;
		
		// shakiness
		float devX = shakeAmount * Mathf.Sin (timeFactors.x * shakeTime / slownessFactor);
		float devY = shakeAmount * Mathf.Sin (timeFactors.y * shakeTime / slownessFactor);
		float devZ = shakeAmount * Mathf.Sin (timeFactors.z * shakeTime / slownessFactor);
		camera.transform.localEulerAngles = originalEulerAngles + new Vector3(devX,devY,devZ);
		shakeTime += Time.deltaTime;
		shakeAmount += shakeIncreaseFactor * Time.deltaTime;
		if (shakeAmount > maxShakeAmount) {
			shakeAmount = maxShakeAmount;
		}
		slownessFactor -= slownessReductionFactor * Time.deltaTime;
		if (slownessFactor < maxSlownessFactor) {
			slownessFactor = maxSlownessFactor;
		}

		// blurriness
		blurIntensity += blurIntensityFactor * Time.deltaTime;
		blurScript.blurAmount = blurIntensity;
	}
}
