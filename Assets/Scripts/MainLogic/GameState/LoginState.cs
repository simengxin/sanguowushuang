using UnityEngine;
using System.Collections;

public class LoginState : GameState {
	protected override void OnStart(){


	}
	protected override void OnStop(){
		GUIManager.HideView("LoginPanel");
	}
	protected override void OnLoadComplete(){
		GUIManager.ShowView("LoginPanel");
	}
}
