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
	
	public readonly Color MyColor;
	public readonly GameObject arm;
	
	public enum State {Paint, Charge}
	public State currentState = State.Paint;
	
	private Painting painting;

	public Paintbrush(GameObject arm, Color color, Painting painting) {
		this.MyColor = color;
		this.arm = arm;
		this.painting = painting;
		
		thalmicMyo = arm.GetComponent<JointOrientation>().myo.GetComponent<ThalmicMyo> ();
	
		GameObject reticle = (GameObject)GameObject.Instantiate(GameManager.Instance.Reticle);
		reticle.GetComponent<Reticle>().Initialize(arm, painting.gameObject, color);
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
	
	float lastRelease;
	private Quaternion lastRotation;
	bool chargeSwingStarted;
	bool downSinceLastThrow;
	public void Update() {
		if (!GameManager.Instance.BlobsEnabled)
			return;
			
		if (!GameManager.Instance.BlobsUseGestures) {
			bool handIsUp = 200 < arm.transform.rotation.eulerAngles.x && arm.transform.rotation.eulerAngles.x < 300;
			downSinceLastThrow = downSinceLastThrow || !handIsUp;
			
			if (currentState == State.Paint && handIsUp && downSinceLastThrow && Time.time - lastRelease > GameManager.Instance.BlobCooldown) {
				Charge();
				
			} else if (currentState == State.Charge && Time.time - chargeStart > .4) {
				float rotationSpeed = Quaternion.Angle(lastRotation, arm.transform.rotation) / Time.deltaTime;
				if (!chargeSwingStarted && rotationSpeed > 200)
					chargeSwingStarted = true;
				else if (chargeSwingStarted && rotationSpeed < 50) {
					Release();
					chargeSwingStarted = false;
					downSinceLastThrow = false;
				}
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
