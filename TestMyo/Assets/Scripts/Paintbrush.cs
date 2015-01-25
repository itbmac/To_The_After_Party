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
	public const float ChargeTime = 2.0f;
	private GameObject paintBall;
	public GameObject PaintBallPrefab;
	
	void Release() {
		currentState = State.Paint;		
		paintBall.SendMessage("Release");
	}
	
	public void Update() {
		
		
	
		if (!GameManager.Instance.BlobsEnabled)
			return;
	
		if (currentState == State.Charge && Time.time - chargeStart > ChargeTime) {
			Release();
		}
	
		if (thalmicMyo.pose != _lastPose) {
			_lastPose = thalmicMyo.pose;
			
			if (thalmicMyo.pose != Thalmic.Myo.Pose.Fist && currentState == State.Paint) {
				thalmicMyo.Vibrate(VibrationType.Short);
				currentState = State.Charge;
				chargeStart = Time.time;
				
				paintBall = (GameObject)MonoBehaviour.Instantiate(
					Painting.Instance.PaintBallPrefab, 
					arm.transform.position + arm.transform.forward * .5f, 
					arm.transform.rotation
				);
				paintBall.SendMessage("Initialize", this);
			} else if (thalmicMyo.pose != Pose.Fist && currentState == State.Charge) {
				Release();
			}
		}
	
	}
}
