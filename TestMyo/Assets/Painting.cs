using UnityEngine;
using System.Collections;

public class Painting : MonoBehaviour {

	public Camera mainCamera;
	private Texture2D tex;
	private Vector2 pixelUV;
	
	public GameObject MyArm;
	public GameObject MyOtherArm;
	
	private const int BRUSH_RADIUS = 2;
	private Vector2 lastPixel;
	private bool connectToLastPixel = false;

  	void Start() {
		tex = renderer.material.mainTexture as Texture2D;

		Reset();
    }
    
	void DrawBrush(Texture2D tex, Vector2 pixelUV) {
		for(int i = -BRUSH_RADIUS; i < BRUSH_RADIUS; i++) {
			for(int j = -BRUSH_RADIUS; j < BRUSH_RADIUS; j++) {
				if (Mathf.Abs(i) + Mathf.Abs (j) < BRUSH_RADIUS)
					tex.SetPixel((int) pixelUV.x+i,(int) pixelUV.y+j, Color.black);
			}
		}
    }
  
    void Update() {
//		if (!Input.GetMouseButton(0))
//			return;

		Ray r  = new Ray(
			MyArm.transform.position,
			MyArm.transform.forward
		);
		
		Debug.DrawRay(MyArm.transform.position, MyArm.transform.forward);
		
		RaycastHit hit;
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
		
		if (Input.GetKeyDown(KeyCode.Q)) {
			Reset();
		}
	}
	
	void Reset() {
		connectToLastPixel = false;
		for(int i = 0; i < tex.width; i++) {
			for(int j = 0; j < tex.height; j++) {
				tex.SetPixel(i,j, Color.white);
			}
		}
	}

	void OnApplicationQuit() {
		Reset ();
  	}
  
}
