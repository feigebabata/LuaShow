using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System;
using System.IO;

public class Main : MonoBehaviour {

	void Start()
	{
		main ();
	}

	void main()
	{
		if(!Directory.Exists(CSTools.GetPlatformPath()))
		{
			Directory.CreateDirectory (CSTools.GetPlatformPath());
			FileStream fs = new FileStream(CSTools.GetPlatformPath()+"/main.lua", FileMode.Create);
			StreamWriter sw = new StreamWriter(fs);
			string text = "function main() print('hello LuaShow！') end";
			sw.Write(text);
			sw.Flush();
			sw.Close();
			fs.Close();
		}
		LuaEnv luaEnv = new LuaEnv ();
		CSTools.Instance.LoadText ("main.lua", (luaScript) => {
			LuaTable scriptEnv = luaEnv.NewTable ();
			LuaTable meta = luaEnv.NewTable ();
			meta.Set ("__index", luaEnv.Global);
			scriptEnv.SetMetaTable (meta);
			meta.Dispose ();
			scriptEnv.Set ("self", this);
			luaEnv.DoString (luaScript, "main.lua", scriptEnv);
			Action luaMain = scriptEnv.Get<Action> ("main");
			luaMain ();
		});
	}
}
