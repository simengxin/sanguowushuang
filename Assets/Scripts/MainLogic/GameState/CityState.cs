using UnityEngine;
using System.Collections;

public class CityState : GameState {
	protected override void OnStart(){

	}

	protected override void OnStop(){
		GUIManager.HideView("PlayerPanel");
	}
	protected override void OnLoadComplete(){
		GUIManager.ShowView("PlayerPanel");
	}
}
