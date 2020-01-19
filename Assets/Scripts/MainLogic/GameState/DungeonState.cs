using UnityEngine;
using System.Collections;

public class DungeonState : GameState {
	protected override void OnStart(){

	}
	protected override void OnStop(){
		GUIManager.HideView ("DungeonPanel");
	}
	protected override void OnLoadComplete(){
		GameObject logicObj = new GameObject("DungeonLogic");
		logicObj.AddComponent<DungeonLogic>();
		GUIManager.ShowView("DungeonPanel");
	}
 }
