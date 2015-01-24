using UnityEngine;
using System.Collections;

public class Painting : MonoBehaviour {

	public Camera mainCamera;
	private Texture2D tex;
	private Vector2 pixelUV;

  	void Start() {
		tex = renderer.material.mainTexture as Texture2D;

		for(int i = 0; i < tex.width; i++) {
			for(int j = 0; j < tex.height; j++) {
				tex.SetPixel(i,j, Color.white);
			}
    	}
    }
  
    void Update() {
		if (!Input.GetMouseButton(0))
			return;
		
		RaycastHit hit;
		if (!Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit))
			return;
		
		Renderer renderer = hit.collider.renderer;
		MeshCollider meshCollider = hit.collider as MeshCollider;
		if (renderer == null || renderer.sharedMaterial == null || renderer.sharedMaterial.mainTexture == null || meshCollider == null)
			return;
		
		pixelUV = hit.textureCoord;
		pixelUV.x *= tex.width;
		pixelUV.y *= tex.height;

		for(int i = -30; i < 30; i++) {
		  for(int j = -30; j < 30; j++) {
		    tex.SetPixel((int) pixelUV.x+i,(int) pixelUV.y+j, Color.black);
		  }
		}
		tex.Apply();
	}

	void OnApplicationQuit() {
		for(int i = 0; i < tex.width; i++) {
			for(int j = 0; j < tex.height; j++) {
				tex.SetPixel(i,j, Color.black);
			}
    	}
  	}
  
}
