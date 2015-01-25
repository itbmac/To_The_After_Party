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
	
	public readonly Color OurRed = new Color32(220, 30, 30, 255);
	public readonly Color OurBlue = new Color32(50, 100, 220, 255);
	
	public int DefaultBrushRadius = 2;
	
	public static Painting Instance;
	public GameObject PaintBallPrefab;
	public Material redMaterial;
	public Material blueMaterial;
	
	void Awake() {
		Instance = this;
	}
	
  	void Start() {  	
		tex = renderer.material.mainTexture as Texture2D;

		LeftBrush = new Paintbrush(LeftArm, OurBlue);
		RightBrush = new Paintbrush(RightArm, OurRed);

		Reset();
		
		white = tex.width * tex.height;
		totalPixels = white;
    }
    
    int red, blue, white, totalPixels;
	void DrawBrush(Texture2D tex, Vector2 pixelUV, Color color, int brushRadius) {
		float br2 = brushRadius*brushRadius;
		for(int i = -brushRadius; i < brushRadius; i++) {
			for(int j = -brushRadius; j < brushRadius; j++) {
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
    }
    
    private Vector2? GetUVHit(Ray r) {
		RaycastHit hit;
		if (!collider.Raycast(r, out hit, float.PositiveInfinity))
			return null;
			
		Renderer renderer = hit.collider.renderer;
		MeshCollider meshCollider = hit.collider as MeshCollider;
		if (renderer == null || renderer.sharedMaterial == null || renderer.sharedMaterial.mainTexture == null || meshCollider == null)
			return null;
		
		pixelUV = hit.textureCoord;
		pixelUV.x *= tex.width;
		pixelUV.y *= tex.height;
		return pixelUV;
    }
    
    void Paint(Paintbrush brush) {
    	if (brush.currentState != Paintbrush.State.Paint)
    		return;
		
		Vector2? possibleHit = GetUVHit(brush.GetRay());
		if (possibleHit == null)
			return;
		
		Vector2 pixelUV = (Vector2)possibleHit;
		
		if (brush.ConnectToLastPixel) {
			Vector2 diff = pixelUV - brush.LastPixel;
			float dist = diff.magnitude;
			Vector2 diffNormalized = diff.normalized;
			for (float interpolate = 0; interpolate < dist; interpolate += 1) {
				DrawBrush(tex, brush.LastPixel + diffNormalized * interpolate, brush.MyColor, DefaultBrushRadius);
			}
		}
		DrawBrush(tex, pixelUV, brush.MyColor, DefaultBrushRadius);
		
		brush.ConnectToLastPixel = true;
		brush.LastPixel = pixelUV;
    }
  
    void Update() {
    	if (GameManager.Instance.CurrentState != GameManager.GameState.Running)
    		return;
    		
		Paint(LeftBrush);
		Paint(RightBrush);
		
		LeftBrush.Update();
		RightBrush.Update();
		
		if (Input.GetKeyDown(KeyCode.Q)) {
			Reset();
		}
		
		HUD.Instance.redScore = Mathf.Sqrt(red / (float)totalPixels);
		HUD.Instance.blueScore = Mathf.Sqrt(blue / (float)totalPixels);
		
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
  	
  	public void PaintBallHit(Vector3 pos, Vector3 paintingHitNormal, Color32 color, float power) {
		Vector2? maybeHit = GetUVHit(new Ray(pos, -paintingHitNormal));
		
		if (maybeHit == null) {
			Debug.Log ("missed");
			return;
		}
			
		Vector2 pixelUV = (Vector2)maybeHit;
  	
		DrawBrush(tex, pixelUV, color, (int)(30*power));
  		
		tex.Apply();
  	}
  
}
