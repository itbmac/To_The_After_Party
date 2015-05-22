using UnityEngine;
using System.Collections;

public class ColorSelector : MyMonoBehaviour {

	public Color32 MyColor;

	// Use this for initialization
	void Start () {
		renderer.material = Instantiate(renderer.material);
		renderer.material.color = MyColor;
		
		renderer.enabled = GameManager.Instance.OnePlayerMode;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
