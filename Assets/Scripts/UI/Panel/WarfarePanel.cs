using UnityEngine;
using System.Collections;

public class WarfarePanel : IView {
	public static StageData m_StageData;
	protected override void OnStart()
	{
		Debug.LogError("WarfarePanel onstart");
		
	}
	
	protected override void OnShow()
	{
		
	}
	
	protected override void OnHide()
	{
		
	}
	
	protected override void OnDestory()
	{
		
	}
	protected override void OnDrag(GameObject sender, object param)
	{
		
	}
	
	protected override void OnPress(GameObject sender, object param)
	{
		
	}
	
	protected override void OnClick(GameObject sender, object param)
	{

		if (sender.name.Equals("BtnReturn"))
		{
			OnClickBtnReturn();
		}
		if (sender.name.Equals("BtnFormation"))
		{
			OnClickBtnFormation();
		}
		if (sender.name.Equals("BtnBattle"))
		{
			OnClickBtnBattle();
		}
	}

	private void OnClickBtnReturn()
	{
		GUIManager.HideView("WarfarePanel");
	}
	
	private void OnClickBtnFormation()
	{
		GUIManager.ShowView("PlaceTeamPanel");
		GUIManager.HideView("WarfarePanel");
	}
	
	private void OnClickBtnBattle()
	{
		BattleLogicPVE.LoadBattle(m_StageData);
		GUIManager.HideView("WarfarePanel");
	}
}
