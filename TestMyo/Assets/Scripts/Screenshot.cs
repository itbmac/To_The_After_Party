using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class Screenshot : MonoBehaviour {

	//vars
	public Texture2D painting;
	
	public static Screenshot Instance {
		get; private set;
	}
	
	void Awake() {
		Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		//save screenshot when pressing S
		if(Input.GetKeyDown(KeyCode.S)) {
			SavePainting();
		}
	}
	
	public void SavePainting() {
		SaveTextureToFile(painting,"Painting "+System.DateTime.Now.ToString("yyyy-MM-dd HH-mm-s")+".png");
	}
	
	string GetSavePath() {
		var custom = LoadSettings.GetString("screenshot_directory");
		if (custom == null)
			return Application.dataPath;
		else
			return custom;
	}
	
	//function to save texture to file (from user Fabkins)
	//Link: http://answers.unity3d.com/questions/245600/saving-a-png-image-to-hdd-in-standalone-build.html
	void SaveTextureToFile( Texture2D texture, string fileName) {
		byte[] bytes=texture.EncodeToPNG();
		var dirPath = GetSavePath();
		var path = dirPath + "/" + fileName;
		print ("Saving to " + path);
		
		Stream file = System.IO.File.Open(path, FileMode.Create);
		BinaryWriter binary= new BinaryWriter(file);
		binary.Write(bytes);
		file.Close();
	}

}
