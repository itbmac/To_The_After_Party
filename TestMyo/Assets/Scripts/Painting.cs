using UnityEngine;
using System.Collections;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class Painting : MonoBehaviour {

	public Camera mainCamera;
	private Texture2D tex;
	private Vector2 pixelUV;
	
	public GameObject MyArm;
	public GameObject MyOtherArm;
	
	public int BrushRadius = 2;
	
	private Vector2 lastPixel;
	private bool connectToLastPixel = false;
	private ThalmicMyo thalmicMyo;
	private Pose _lastPose = Pose.Unknown;
	private bool handForward = true;

  	void Start() {
		tex = renderer.material.mainTexture as Texture2D;
		thalmicMyo = MyArm.GetComponent<JointOrientation>().myo.GetComponent<ThalmicMyo> ();

		Reset();
    }
    
	void DrawBrush(Texture2D tex, Vector2 pixelUV) {
		for(int i = -BrushRadius; i < BrushRadius; i++) {
			for(int j = -BrushRadius; j < BrushRadius; j++) {
				if (Mathf.Abs(i) + Mathf.Abs (j) < BrushRadius)
					tex.SetPixel((int) pixelUV.x+i,(int) pixelUV.y+j, Color.black);
			}
		}
    }
  
    void Update() {

		ThalmicMyo thalmicMyo = MyArm.GetComponent<JointOrientation>().myo.GetComponent<ThalmicMyo> ();
		if (thalmicMyo.pose != _lastPose) {
			_lastPose = thalmicMyo.pose;
			if (thalmicMyo.pose == Thalmic.Myo.Pose.Fist) {
				handForward = !handForward;
				thalmicMyo.Vibrate(VibrationType.Short);
			}	
		}

		Ray r  = new Ray(
			MyArm.transform.position,
			MyArm.transform.forward
		);
		
		if (Input.GetKeyDown(KeyCode.Q)) {
			Reset();
		}
		
		if (!handForward) {
			connectToLastPixel = false;
			return;
		}
		
		RaycastHit hit;
		// TODO: should probably filter for this painting object -Greg
		if (!Physics.Raycast(r, out hit))
			return;
		
		Renderer renderer = hit.collider.renderer;
		MeshCollider meshCollider = hit.collider as MeshCollider;
		if (renderer == null || renderer.sharedMaterial == null || renderer.sharedMaterial.mainTexture == null || meshCollider == null)
			return;
		
		pixelUV = hit.textureCoord;
		pixelUV.x *= tex.width;
		pixelUV.y *= tex.height;
		
		if (connectToLastPixel) {
			Vector2 diff = pixelUV - lastPixel;
			float dist = diff.magnitude;
			Vector2 diffNormalized = diff.normalized;
			for (float interpolate = 0; interpolate < dist; interpolate += 1) {
				DrawBrush(tex, lastPixel + diffNormalized * interpolate);
			}
		}
		DrawBrush(tex, pixelUV);
		
		connectToLastPixel = true;
		lastPixel = pixelUV;
		tex.Apply();
	}
	
	void Reset() {
		connectToLastPixel = false;
		for(int i = 0; i < tex.width; i++) {
			for(int j = 0; j < tex.height; j++) {
				tex.SetPixel(i,j, Color.white);
			}
		}
		tex.Apply();
	}

	void OnApplicationQuit() {
		Reset ();
  	}
  
}
