using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XLua;
using System;


[LuaCallCSharp]
public class XLuaBehaviour : MonoBehaviour {

    internal static LuaEnv luaEnv = new LuaEnv(); //all lua behaviour shared one luaenv only!
    internal static float lastGCTime = 0;
    internal const float GCInterval = 1;//1 second 

    private Action luaStart;
    private Action luaUpdate;
    private Action luaOnDestroy;
	private Action luaLateUpdate;
	private Action luaOnTransformChildrenChanged;
	private Action luaOnTransformParentChanged;
	private Action luaOnEnable;
	private Action luaOnDisable;

	private Action<Collider> luaOnTriggerEnter;
	private Action<Collider> luaOnTriggerStay;
	private Action<Collider> luaOnTriggerExit;

	private Action<Collision> luaOnCollisionEnter;
	private Action<Collision> luaOnCollisionStay;
	private Action<Collision> luaOnCollisionExit;

	private Action luaOnBecameInvisible;
	private Action luaOnBecameVisible;

    private LuaTable scriptEnv;

	public void Init(string luaScript)
	{
		scriptEnv = luaEnv.NewTable();

		LuaTable meta = luaEnv.NewTable();
		meta.Set("__index", luaEnv.Global);
		scriptEnv.SetMetaTable(meta);
		meta.Dispose();

		scriptEnv.Set("self", this);

		luaEnv.DoString(luaScript, "XLuaBehaviour", scriptEnv);

		scriptEnv.Get("start", out luaStart);
		scriptEnv.Get("update", out luaUpdate);
		scriptEnv.Get("ondestroy", out luaOnDestroy);
		scriptEnv.Get("lateupdate", out luaLateUpdate);
		scriptEnv.Get("ontransformchildrenchanged", out luaOnTransformChildrenChanged);
		scriptEnv.Get("ontransformparentchanged", out luaOnTransformParentChanged);
		scriptEnv.Get("onenable", out luaOnEnable);
		scriptEnv.Get("ondisable", out luaOnDisable);
		
		scriptEnv.Get("ontriggerenter", out luaOnTriggerEnter);
		scriptEnv.Get("ontriggerstay", out luaOnTriggerStay);
		scriptEnv.Get("ontriggerexit", out luaOnTriggerExit);
		
		scriptEnv.Get("oncollisionenter", out luaOnCollisionEnter);
		scriptEnv.Get("oncollisionstay", out luaOnCollisionStay);
		scriptEnv.Get("oncollisionexit", out luaOnCollisionExit);
		
		scriptEnv.Get("onbecameinvisible", out luaOnBecameInvisible);
		scriptEnv.Get("onbecamevisible", out luaOnBecameVisible);
	}

	public void InitByFile(string path)
	{
		string luaScript = "";

		Init (luaScript);
	}
	void OnEnable()
	{
		if (luaOnEnable != null)
		{
			luaOnEnable();
		}
	}
	// Use this for initialization
	void Start ()
    {
        if (luaStart != null)
        {
            luaStart();
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (luaUpdate != null)
        {
            luaUpdate();
        }
        if (Time.time - LuaBehaviour.lastGCTime > GCInterval)
        {
            luaEnv.Tick();
            LuaBehaviour.lastGCTime = Time.time;
        }
	}

	void LateUpdate()
	{
		if (luaLateUpdate != null)
		{
			luaLateUpdate();
		}
	}

	void OnTransformChildrenChanged() 
	{
		if (luaOnTransformChildrenChanged != null)
		{
			luaOnTransformChildrenChanged();
		}
	}
	
	void OnTransformParentChanged() 
	{
		if (luaOnTransformParentChanged != null)
		{
			luaOnTransformParentChanged();
		}
	}
	
	void OnTriggerEnter(Collider other) //ontriggerenter
	{
		if (luaOnTriggerEnter != null) 
		{
			luaOnTriggerEnter (other);
		}
	}
	
	void OnTriggerStay(Collider other)
	{
		if (luaOnTriggerStay != null) 
		{
			luaOnTriggerStay (other);
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if (luaOnTriggerExit != null) 
		{
			luaOnTriggerExit (other);
		}
	}
	
	void OnCollisionEnter(Collision collision)	//oncollisionenter
	{
		if (luaOnCollisionEnter != null) 
		{
			luaOnCollisionEnter (collision);
		}
	}
	
	void OnCollisionStay(Collision collisionInfo)
	{
		if (luaOnCollisionStay != null) 
		{
			luaOnCollisionStay (collisionInfo);
		}
	}
	
	void OnCollisionExit(Collision collisionInfo)
	{
		if (luaOnCollisionExit != null) 
		{
			luaOnCollisionExit (collisionInfo);
		}
	}
	
	void OnBecameInvisible()
	{
		if (luaOnBecameInvisible != null) 
		{
			luaOnBecameInvisible ();
		}
	}
	
	void OnBecameVisible() 
	{
		if (luaOnBecameVisible != null) 
		{
			luaOnBecameVisible ();
		}
	}

	void OnDisable()
	{
		if (luaOnDisable != null) 
		{
			luaOnDisable ();
		}
	}

    void OnDestroy()
    {
        if (luaOnDestroy != null)
        {
            luaOnDestroy();
        }
        luaOnDestroy = null;
        luaUpdate = null;
        luaStart = null;
		if (scriptEnv != null) 
		{
			scriptEnv.Dispose();
		}
    }
}
