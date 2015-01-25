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
	
	public GameObject LeftArm;
	public GameObject RightArm;
	private Paintbrush LeftBrush;
	private Paintbrush RightBrush;
	
	private readonly Color OurRed = new Color32(220, 30, 30, 255);
	private readonly Color OurBlue = new Color32(50, 100, 220, 255);
	
	public int BrushRadius = 2;
	
  	void Start() {
		tex = renderer.material.mainTexture as Texture2D;

		LeftBrush = new Paintbrush(LeftArm, OurBlue);
		RightBrush = new Paintbrush(RightArm, OurRed);

		Reset();
		
		white = tex.width * tex.height;
		totalPixels = white;
    }
    
    void CheckInvariant() {
    	if (red + blue + white != totalPixels || red < 0 || blue < 0 || white < 0)
    		Debug.LogError("Invalid pixel counts " + red + " " + blue + " " + white + " " + totalPixels);
    }
    
    int red, blue, white, totalPixels;
	void DrawBrush(Texture2D tex, Vector2 pixelUV, Color color) {
		float br2 = BrushRadius*BrushRadius;
		for(int i = -BrushRadius; i < BrushRadius; i++) {
			for(int j = -BrushRadius; j < BrushRadius; j++) {
				Vector2 v = new Vector2(i, j);
				int px = (int) pixelUV.x+i;
				int py = (int) pixelUV.y+j;
				
				if (0 <= px && px < tex.width && 0 <= py && py <= tex.height && v.sqrMagnitude < br2) {
					Color32 old = tex.GetPixel(px, py);
					tex.SetPixel(px, py, color);
					
					if (old == OurRed)
						red -= 1;
					else if (old == OurBlue)
						blue -= 1;
					else {
						if (old != Color.white)
							Debug.LogError("didnt kill white, killed " + old);
						white -= 1;
					}
						
					if (color == OurRed)
						red += 1;
					else if (color == OurBlue)
						blue += 1;
					else
						Debug.LogError("Tried to draw unrecognized color");
				}
				
			}
		}
		CheckInvariant();
    }
    
    void Paint(Paintbrush brush) {
		RaycastHit hit;
		Ray r = brush.GetRay();
		Debug.DrawRay(r.origin, r.direction);
		if (!collider.Raycast(brush.GetRay(), out hit, float.PositiveInfinity))
			return;
		
		Renderer renderer = hit.collider.renderer;
		MeshCollider meshCollider = hit.collider as MeshCollider;
		if (renderer == null || renderer.sharedMaterial == null || renderer.sharedMaterial.mainTexture == null || meshCollider == null)
			return;
		
		pixelUV = hit.textureCoord;
		pixelUV.x *= tex.width;
		pixelUV.y *= tex.height;
		
		if (brush.ConnectToLastPixel) {
			Vector2 diff = pixelUV - brush.LastPixel;
			float dist = diff.magnitude;
			Vector2 diffNormalized = diff.normalized;
			for (float interpolate = 0; interpolate < dist; interpolate += 1) {
				DrawBrush(tex, brush.LastPixel + diffNormalized * interpolate, brush.MyColor);
			}
		}
		DrawBrush(tex, pixelUV, brush.MyColor);
		
		brush.ConnectToLastPixel = true;
		brush.LastPixel = pixelUV;
    }
  
    void Update() {
//		if (thalmicMyo.pose != _lastPose) {
//			_lastPose = thalmicMyo.pose;
//			if (thalmicMyo.pose == Thalmic.Myo.Pose.Fist) {
//				handForward = !handForward;
//				thalmicMyo.Vibrate(VibrationType.Short);
//			}	
//		}
//		
		Paint(LeftBrush);
		Paint(RightBrush);
		
		if (Input.GetKeyDown(KeyCode.Q)) {
			Reset();
		}
		
//		if (!handForward) {
//			connectToLastPixel = false;
//			return;
//		}
		
//		Color[] pixels = tex.GetPixels();
//		int mred = 0, mblue = 0, mwhite = 0;
//		foreach (Color c in pixels) {
//			if (c == OurRed)
//				mred += 1;
//			else if (c == OurBlue)
//				mblue += 1;
//			else
//				mwhite += 1;
//		}
		
		float redPercent = (int)(100 * red / (float)totalPixels);
		float bluePercent = (int)(100 * blue / (float)totalPixels);
		float whitePercent = (int)(100 * white / (float)totalPixels);

//		Debug.Log (red + " " + blue + " " + white);
//		Debug.Log (red + " " + mred);
		Debug.Log (bluePercent + " " + redPercent + " " + whitePercent);
		
		
		tex.Apply();
	}
	
	void Reset() {
		LeftBrush.Reset();
		RightBrush.Reset();
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
