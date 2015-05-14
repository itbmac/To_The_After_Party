using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveCube : MonoBehaviour {

	public GameObject myArm;
	
	public IList<Vector3> pastAccels;
	public IList<Vector3> pastVelocities;
	
	private bool baseAccelSet = false;
	private Vector3 baseAccel;
	
	Vector3 velocity;
	Vector3 startPos;

	// Use this for initialization
	void Start () {
		pastAccels = new List<Vector3>();
		pastVelocities = new List<Vector3>();
		startPos = transform.position;
	}
	
	bool frozenUntilCalmDown = false;
	bool freezeOnSignChange = false;
	float referenceSign = 1;
	float freezeStart;
	public static bool HandForward = false;
	
	float lastChanged;
	
	// Update is called once per frame
	void Update () {
//		ThalmicMyo thalmicMyo = myArm.GetComponent<JointOrientation>().myo.GetComponent<ThalmicMyo> ();
//		Vector3 accel = myArm.transform.rotation * (thalmicMyo.accelerometer);
		Vector3 accel = myArm.GetComponent<JointOrientation>().Accel;
		Debug.DrawRay(transform.position, accel, Color.blue);
		
		pastAccels.Add(accel);
		if (pastAccels.Count > 10) {
			pastAccels.RemoveAt(0);
			
			Vector3 sum = pastAccels[0];
			float sumMag = 0;
			for (int i = 1; i < pastAccels.Count; i += 1) {
				Vector3 diff = pastAccels[i] - pastAccels[i - 1];
				sumMag += diff.magnitude;
				sum += pastAccels[i];
			}
			sumMag /= pastAccels.Count - 1;
			sum /= pastAccels.Count;
			
			if (sumMag < .0075) {
				baseAccel = sum;
				baseAccelSet = true;
//				Debug.Log ("calibrated");
			}
		}
		
		Vector3 processedAccel = accel - baseAccel;
		if (baseAccelSet) {
//			Debug.Log (processedAccel.z);
			
//			Vector3 reducedAccel = processedAccel;
			float accelPivot = 0.04f; // anything below this magnitude doesn't count as motion
			if (processedAccel.magnitude >= accelPivot) {
//				reducedAccel = processedAccel.normalized * (processedAccel.magnitude - accelPivot);
//				velocity = velocity * 1 + reducedAccel;	
			}
			
			if (processedAccel.magnitude < .1) {
				velocity *= .2f;
//				Debug.Log ("damp");
			}
//			
//			if (velocity.magnitude < 0.001f)
//				velocity = Vector3.zero;
//			Debug.Log ("accel = " + processedAccel + " velocity =" + velocity + " " + velocity.magnitude);
//			
////			Vector3 toStart = startPos - transform.position;
////			velocity = .9f*velocity + .1f * (toStart.normalized);
//			velocity.z = 0;
//			
//			transform.position += velocity * .5f;
//			Vector3 newPos = transform.position;
//			newPos.x = Mathf.Clamp(newPos.x, -5, 5);
//			newPos.y = Mathf.Clamp(newPos.y, -5, 5);
//			transform.position = newPos;

			velocity += processedAccel;
			velocity.x = 0;
			velocity.y = 0;
//			if (velocity.magnitude > 1.2f)
//				velocity = velocity.normalized * 1.2f;
			
			pastVelocities.Add (velocity);
			if (pastVelocities.Count > 10)
				pastVelocities.RemoveAt(0);
			Vector3 avgVelocity = Vector3.zero;
			foreach (Vector3 v in pastVelocities) {
				avgVelocity += v;
			}
			avgVelocity /= 10;
			
//			if (avgVelocity.magnitude < 0.2f)
//				avgVelocity = Vector3.zero;
//			else {
//				avgVelocity = avgVelocity.normalized * (avgVelocity.magnitude - 0.2f);
//			}
			
//			Debug.Log (avgVelocity);
			
			Vector3 freezeProcessedVelocity = avgVelocity;
			if (frozenUntilCalmDown) {
			
				if (/*avgVelocity.magnitude < 0.1f &&*/ (Time.time - freezeStart > 0.25f)) {
					frozenUntilCalmDown = false;
					velocity = Vector3.zero;
//					Debug.Log ("thaw");
				} else
					freezeProcessedVelocity = Vector3.zero;
			} else if (freezeOnSignChange) {
				if (Mathf.Sign (avgVelocity.z) != referenceSign) {
//					Debug.Log ("Frozen!");
					frozenUntilCalmDown = true;
					freezeOnSignChange = false;
					freezeProcessedVelocity = Vector3.zero;
					freezeStart = Time.time;
				}
			} else if (Mathf.Abs(avgVelocity.z) > .75f) {
//				Debug.Log ("freezeTrigger");
				freezeOnSignChange = true;
				referenceSign = Mathf.Sign(avgVelocity.z);
			}
			
			
			
//			Vector3 reducedVelocity = new Vector3(0, 0, Mathf.Sign(avgVelocity.z) Mathf.Sqrt(Mathf.Abs(avgVelocity.z)));
		
		
//			if (freezeProcessedVelocity.magnitude < 0.2f) {
//				Vector3 homePos;
//				if (transform.position.z >= 0)
//					homePos = startPos + new Vector3(0, 0, 5);
//				else
//					homePos = startPos - new Vector3(0, 0, 5);
//			
//				freezeProcessedVelocity = (homePos - transform.position);
//				if (freezeProcessedVelocity.magnitude < 0.001) {
//					transform.position = homePos;
//					return;
//				}
//					
//				else {
////					if (freezeProcessedVelocity.magnitude > 1)
////						freezeProcessedVelocity.Normalize();
//					freezeProcessedVelocity *= .2f;
//				}
//			}
//			else {
//				freezeProcessedVelocity = freezeProcessedVelocity.normalized * (freezeProcessedVelocity.magnitude - 0.2f);
//			}
			
			if (freezeProcessedVelocity.z > 0)
				gameObject.GetComponent<Renderer>().material.color = Color.green;
			else if (freezeProcessedVelocity.z == 0)
				gameObject.GetComponent<Renderer>().material.color = Color.white;
			else if (freezeProcessedVelocity.z < 0)
				gameObject.GetComponent<Renderer>().material.color = Color.red;	

//			Vector3 newPos = transform.position + freezeProcessedVelocity;
//			transform.position = newPos;
//			Debug.Log (freezeProcessedVelocity);
		
			if (Time.time - lastChanged > 0.75) {
				bool old = HandForward;
				
				if (HandForward && freezeProcessedVelocity.z < -.75)
					HandForward = false;
				else if (!HandForward && freezeProcessedVelocity.z > .75)
					HandForward = true;
					
				if (old != HandForward) {
					lastChanged = Time.time;
					Debug.Log (HandForward);
				}
			}
			
			transform.position = startPos + new Vector3(0, 0, freezeProcessedVelocity.z);
		}
		
		
	}
}
