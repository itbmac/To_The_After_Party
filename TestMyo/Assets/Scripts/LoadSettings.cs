using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using System.IO; 
using System.Linq;

// LoadSettings.cs by Hayden Scott-Baron (@docky)
// needs a settings.txt file.
// see the Unity wiki for usage: http://wiki.unity3d.com/index.php/LoadSettings
// send me a tweet if you use this, please :) 

public class LoadSettings : MonoBehaviour 
{
	public string filename = "settings.txt";
	
	// after 20 seconds the script times out and reports an error; 
	public float timeOut = 20f; 
	
	// keep track of when the file has finished loading, and whether an error has occured
	public bool fileReady = false; 
	public bool fileError = false; 
	public bool loadStarted = false;
	
	// data for the content
	public Dictionary<string, string> dataPairs = new Dictionary<string,string>(); 
	
	// singleton
	public static LoadSettings instance; 
	public bool DebugMode = false;
	
	void Awake ()
	{
		// set the singleton, to allow static references
		instance = this; 
	}
	
	void Start () 
	{
		LoadFile (filename);
	}
	
	// this allows the file loading to time out in case of a file io problem
	IEnumerator TimeOut (float timer)
	{
		yield return new WaitForSeconds (timer); 
		
		if ( !FileReady() )
		{
			fileError = true; 	
			StopAllCoroutines(); 
		}
	}
	
	static void DebugModeLog(string msg) {
		if (instance != null && instance.DebugMode)
			print (msg);
	}
	
	// load the file
	void LoadFile (string filename)
	{
		loadStarted = true;
		StartCoroutine ("TimeOut", timeOut); 
		
		// reformat the filepath
//		string path = Application.dataPath + "\\..\\" + filename;
		
		var files = Directory.GetParent(Application.dataPath).GetFiles(filename, SearchOption.AllDirectories);
		if (files.Length == 0) {
			Debug.LogError("Could not find " + filename);
			fileError = true;
			return;
		}
		
		string path = files.First().FullName;
		
		// load the file, and grab the text
		StreamReader newStream = new StreamReader( path );
		string allText = newStream.ReadToEnd(); 
		
		DebugModeLog("found the following text:\n" + allText); 
		
		// clean up the text file
		allText = allText.Replace (" ", ""); 
		allText = allText.Replace ("\t", ""); 
		// split the comments onto new lines
		allText = allText.Replace ("//", "\n//"); 
		
		// split into lines
		string[] lines = {}; 
		lines = allText.Split("\n".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries); 
		
		// abort if there aren't any lines
		if (lines.Length < 1)
		{
			fileError = true; 
			return;
		}
		
		// keep count of successes
		int totalLines = 0; 
		
		// iterate through the lines, and check for valid ones
		foreach (string item in lines)
		{
			if ( !item.Contains("//") )
			{
				if (item.Contains ("="))
				{
					string[] pieces = item.Split ("=".ToCharArray()); 
					if (pieces.Length == 2)	
					{
						string key 			= pieces[0]; 
						string dataValue 	= pieces[1]; 
						
						DebugModeLog("found data: " + key + " = " + dataValue); 
						
						totalLines++; 
						dataPairs.Add (key, dataValue.ToLower() ); 
					}
				}
			}			
		}
		
		DebugModeLog("Finished reading file. " + totalLines + " items found.");
		fileReady = true; 
	}
	
	public static bool FileError()
	{
		if (!instance)
			return false; 
		
		return instance.fileError; 
	}
	
	public static bool FileReady()
	{
		if (!instance)
			return false; 
		
		return instance.fileReady; 
	}
	
	public static int GetInt (string id)
	{
		id = id.ToLower().Trim(); 
		DebugModeLog("id = '" + id + "'"); 
		
		if (instance == null)
		{
			DebugModeLog("no LoadSettings instance");
			return -1; 
		}
		
		bool keyExists = (instance.dataPairs.ContainsKey (id)); 
		if (!keyExists)
		{
			DebugModeLog("no matching key");
			return -1; 
		}
		
		string dataForKey = instance.dataPairs[id]; 
		int outputData = -1; 
		
		bool canParse = int.TryParse (dataForKey, out outputData); 
		if (!canParse)
		{
			DebugModeLog("can't parse the line");
			return -1; 
		}
		
		return outputData; 
	}
	
	public static float GetFloat (string id)
	{
		id = id.ToLower().Trim(); 
		
		if (instance == null)
		{
			DebugModeLog("no LoadSettings instance");
			return -1; 
		}
		
		bool keyExists = (instance.dataPairs.ContainsKey (id)); 
		if (!keyExists)
		{
			DebugModeLog("no matching key");
			return -1; 
		}
		
		string dataForKey = instance.dataPairs[id]; 
		float outputData = -1; 
		
		bool canParse = float.TryParse (dataForKey, out outputData); 
		if (!canParse)
		{
			DebugModeLog("can't parse the line");
			return -1; 
		}
		
		return outputData; 
	}
	
	public static string GetString (string id)
	{
		id = id.ToLower().Trim(); 
		
		
		if (instance == null)
			return ""; 
		
		bool keyExists = (instance.dataPairs.ContainsKey (id)); 
		if (!keyExists)
			return ""; 
		
		string dataForKey = instance.dataPairs[id]; 
		
		return dataForKey; 
	}
	
}