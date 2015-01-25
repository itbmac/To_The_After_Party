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
	private readonly GameObject arm;

	public Paintbrush(GameObject arm, Color color) {
		this.MyColor = color;
		this.arm = arm;
		
		thalmicMyo = arm.GetComponent<JointOrientation>().myo.GetComponent<ThalmicMyo> ();
	
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
}
