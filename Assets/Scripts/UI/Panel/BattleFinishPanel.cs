using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using SysUtils;

public class BattleFinishPanel : IView
{
	public static bool Success = false;
	
	private GameObject m_Fail;
	private GameObject m_Win;
	private TweenAlpha m_Star1;
	private TweenAlpha m_Star2;
	private TweenAlpha m_Star3;
	private UILabel m_GoldLabel;
	private UILabel m_ExpLabel;
	
	//public static VarList Args;
	
	//private List<LootClass> LootList = new List<LootClass>();
	private GameObject m_Template;
	private int m_Index = 0;
	
	private int m_Money = 0;
	private int m_Exp = 0;
	
	
	protected override void OnStart()
	{
		m_Fail = this.GetChild("FailGroup");
		m_Win = this.GetChild("WinGroup");
		
		m_Star1 = this.GetChild("imgWarStar1").GetComponent<TweenAlpha>();
		m_Star2 = this.GetChild("imgWarStar2").GetComponent<TweenAlpha>();
		m_Star3 = this.GetChild("imgWarStar3").GetComponent<TweenAlpha>();
		m_ExpLabel = this.GetChild("lblExpValue").GetComponent<UILabel>();
		m_GoldLabel = this.GetChild("lblMoneyValue").GetComponent<UILabel>();
	}
	

	
	protected override void OnShow()
	{
		m_Template = this.GetChild("ItemTemplate");
		m_Template.gameObject.SetActive(false);
		
		if (Success)
		{
			m_Fail.SetActive(false);
			m_Win.SetActive(true);
			m_Star1.gameObject.SetActive(true);
			m_Star2.gameObject.SetActive(true);
			m_Star3.gameObject.SetActive(true);
			m_Star1.ResetToBeginning();
			m_Star1.enabled = true;
			
			m_Star2.ResetToBeginning();
			m_Star2.enabled = true;
			
			m_Star3.ResetToBeginning();
			m_Star3.enabled = true;
		}
		else
		{
			m_Fail.SetActive(true);
			m_Win.SetActive(false);
		}
		
		//this.RegisterMessage(ServerCustomMsgs.S_CUSTOM_LOOTS, HandleLootMsg);
	}
	

	protected override void OnHide ()
	{
	}
	
	protected override void OnDestory ()
	{
	}
	

	
	protected override void OnDrag (GameObject sender, object param)
	{
	}

	
	protected override void OnPress (GameObject sender, object param)
	{
	}
	
	protected override void OnClick(GameObject sender, object param)
	{
		if (sender.name.Equals("OkButton"))
		{
			OnClickOkBtn();
		}
	}
	
	private void OnClickOkBtn()
	{
		GUIManager.HideView("BattleFinishPanel");
		//BattleLogicPVE.Instance.CloseBattle();
	}
}
