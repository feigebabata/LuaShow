using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class CSTools :MonoBehaviour
{

	public static CSTools Instance;

	void Awake()
	{
		Instance = this;
	}

	public static string GetPlatformPath()
	{
		switch (Application.platform)
		{
		case RuntimePlatform.WindowsPlayer:
		case RuntimePlatform.WindowsEditor:
			return Application.streamingAssetsPath+"/LuaShow";
		case RuntimePlatform.Android:
			//string str = Application.dataPath + "!/assets";
			string str = Application.persistentDataPath+"/LuaShow";
			return str;
		default:
			return null;
		}
	}

	public void LoadText(string path,Action<string> callback)
	{
		StartCoroutine (wwwLoadText(path,callback));
	}

	private IEnumerator wwwLoadText(string path,Action<string> callback)
	{
		string filePath = "file:///" +GetPlatformPath ()+"/"+path;
		WWW www = new WWW (filePath);
		yield return www;
		if (www.isDone) 
		{
			callback (www.text);		
		} 
		else 
		{
			Debug.LogError (path + ". 加载失败！");
		}
	}

	public void LoadAudioClip(string path,Action<AudioClip> callback)
	{
		StartCoroutine (wwwLoadAudioClip(path,callback));
	}

	private IEnumerator wwwLoadAudioClip(string path,Action<AudioClip> callback)
	{
		string filePath = "file:///" +GetPlatformPath ()+"/"+path;
		WWW www = new WWW (filePath);
		yield return www;
		if (www.isDone) 
		{
			callback (www.audioClip);		
		} 
		else 
		{
			Debug.LogError (path + ". 加载失败！");
		}
	}

	public void LoadTexture(string path,Action<Texture> callback)
	{
		StartCoroutine (wwwLoadTexture(path,callback));
	}

	private IEnumerator wwwLoadTexture(string path,Action<Texture> callback)
	{
		string filePath = "file:///" +GetPlatformPath ()+"/"+path;
		WWW www = new WWW (filePath);
		yield return www;
		if (www.isDone) 
		{
			callback (www.texture);		
		} 
		else 
		{
			Debug.LogError (path + ". 加载失败！");
		}
	}

	public static void CopyDir(string fromDir, string toDir)
	{
		if (!Directory.Exists(fromDir))
			return;

		if (!Directory.Exists(toDir))
		{
			Directory.CreateDirectory(toDir);
		}

		string[] files = Directory.GetFiles(fromDir);
		foreach (string formFileName in files)
		{
			string fileName = Path.GetFileName(formFileName);
			string toFileName = Path.Combine(toDir, fileName);
			File.Copy(formFileName, toFileName);
		}
		string[] fromDirs = Directory.GetDirectories(fromDir);
		foreach (string fromDirName in fromDirs)
		{
			string dirName = Path.GetFileName(fromDirName);
			string toDirName = Path.Combine(toDir, dirName);
			CopyDir(fromDirName, toDirName);
		}
	}

	public static void MoveDir(string fromDir, string toDir)
	{
		if (!Directory.Exists(fromDir))
			return;

		CopyDir(fromDir, toDir);
		Directory.Delete(fromDir, true);
	}
}
