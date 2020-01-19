using UnityEngine;
using System.Collections;

public class BattleState : GameState {
	protected override void OnStart(){

	}
	protected override void OnStop(){
		//GUIManager.HideView ("");
	}
	protected override void OnLoadComplete(){
		GameObject logicObj = new GameObject("BattleLogicPVE");
		logicObj.AddComponent<BattleLogicPVE>();
	}
}
