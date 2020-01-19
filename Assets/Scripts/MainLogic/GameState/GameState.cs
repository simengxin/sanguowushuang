using UnityEngine;
using System.Collections;

public abstract class GameState{
	public void Start(){
		GUIManager.ShowView ("LoadingPanel");
		OnStart ();
	}
	public void Stop(){
		OnStop ();
	}
	public void LoadComplete(params object[] args){
		OnLoadComplete ();
		GUIManager.HideView("LoadingPanel");
	}

	protected abstract void OnStart();
	protected abstract void OnStop();
	protected abstract void OnLoadComplete(); 
}
