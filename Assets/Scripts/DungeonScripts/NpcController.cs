using UnityEngine;
using System.Collections;

public class NpcController : StageController {

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	protected override void OnMouseClick(Transform hero){
		WarfarePanel.m_StageData = m_CurStageData;
		GUIManager.ShowView("WarfarePanel");
	}
}
