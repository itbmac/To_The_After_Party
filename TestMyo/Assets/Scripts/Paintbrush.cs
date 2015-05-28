using UnityEngine;
using System.Collections;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;


public class Paintbrush {

	private ThalmicMyo thalmicMyo;
	private bool forward;
	private Pose _lastPose = Pose.Unknown;
	public Vector2 LastPixel;
	public bool ConnectToLastPixel;
	
	public Color32 MyColor;
	public readonly GameObject arm;
	
	public enum State {Paint, Charge}
	public State currentState = State.Paint;
	
	JointOrientation jointOrientation;

	public Paintbrush(GameObject arm, Color color, Painting painting) {
		this.MyColor = color;
		this.arm = arm;
		
		jointOrientation = arm.GetComponent<JointOrientation>();
		thalmicMyo = jointOrientation.myo.GetComponent<ThalmicMyo> ();
	
		GameObject reticle = (GameObject)GameObject.Instantiate(GameManager.Instance.Reticle);
		reticle.GetComponent<Reticle>().Initialize(arm, painting.gameObject, color);
	}
	
	public bool IsRunning() {
		return jointOrientation.IsRunning && (thalmicMyo.pose != Pose.Fist || LoadSettings.GetInt("fist_to_withdraw") != 1);
	}
	
	public Ray GetRay() {
		return new Ray(
			arm.transform.position,
			arm.transform.forward
		);
	}
	
	public void Reset() {
		ConnectToLastPixel = false;
	}
	
	private float chargeStart;
	private GameObject paintBall;
	public GameObject PaintBallPrefab;
	
	void Release() {
		thalmicMyo.Vibrate(VibrationType.Short);
		currentState = State.Paint;		
		paintBall.SendMessage("Release");
		lastRelease = Time.time;
	}
	
	void Charge() {
		thalmicMyo.Vibrate(VibrationType.Short);
		currentState = State.Charge;
		chargeStart = Time.time;
		
		paintBall = (GameObject)MonoBehaviour.Instantiate(
			Painting.Instance.PaintBallPrefab, 
			arm.transform.position + arm.transform.forward * .5f, 
			arm.transform.rotation
			);
		paintBall.SendMessage("Initialize", this);
	}
	
	void CheckColorSelectorHit() {
		RaycastHit hit;
		Physics.Raycast(GetRay(), out hit, Mathf.Infinity, LayerMask.GetMask("ColorSelector"));
		if (hit.collider != null) {
			var newColor = hit.collider.GetComponent<ColorSelector>().MyColor;
			MyColor = newColor;
		}
	}
	
	float lastRelease;
	private Quaternion lastRotation;
	bool chargeSwingStarted;
	bool downSinceLastThrow;
	public void Update() {
		if (GameManager.OnePlayerMode)
			CheckColorSelectorHit();
	
		if (!GameManager.Instance.BlobsEnabled || jointOrientation.AutoPilot)
			return;
			
		if (!GameManager.Instance.BlobsUseGestures) {
			bool handIsUp = 140 < arm.transform.rotation.eulerAngles.x && arm.transform.rotation.eulerAngles.x < 300;
			downSinceLastThrow = downSinceLastThrow || !handIsUp;
			
			if (currentState == State.Paint && handIsUp && Time.time - lastRelease > GameManager.Instance.BlobCooldown) {
				Charge();
				
			} else if (currentState == State.Charge && !handIsUp) {
				
				float rotationSpeed = Quaternion.Angle(lastRotation, arm.transform.rotation) / Time.deltaTime;
				if (rotationSpeed > 200)
					Release();
				
//				if (!chargeSwingStarted && rotationSpeed > 200)
//					chargeSwingStarted = true;
//				else if (chargeSwingStarted && rotationSpeed < 50) {
//					Release();
//					chargeSwingStarted = false;
//					downSinceLastThrow = false;
//				}
			}
			
			lastRotation = arm.transform.rotation;
			return;
		}
	
		if (currentState == State.Charge && Time.time - chargeStart > GameManager.Instance.BlobChargeTime) {
			Release();
		}
	
		if (thalmicMyo.pose != _lastPose) {
			_lastPose = thalmicMyo.pose;
			
			if (thalmicMyo.pose != Thalmic.Myo.Pose.Fist && currentState == State.Paint) {
				Charge();
			} else if (thalmicMyo.pose != Pose.Fist && currentState == State.Charge) {
				Release();
			}
		}
	
	}
}
