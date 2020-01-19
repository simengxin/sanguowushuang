using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LootStarsClass
{
	public GameObject ObjControl;
	public UISprite StarBack;
	public UISprite StarFront;
}

public class DungeonPanel : IView
{
	private List<LootStarsClass> LootListStar = new List<LootStarsClass>();
	private GameObject m_TemplateStar;

	private static int m_Index = 0;
	private int m_curStageID = 0;

	private UILabel m_TimeLabel;
	private UILabel m_nextUpLable;
	private UILabel m_NpcNameLable;
	
	protected override void OnStart()
	{
		UILayer = UIPanelLayers.MainLayer;

		m_TimeLabel = this.GetChild("lblTimes").GetComponent<UILabel>();
		m_nextUpLable = this.GetChild("nextUp").GetComponent<UILabel>();
		m_NpcNameLable = this.GetChild("npcName").GetComponent<UILabel>();

		for (int i = 0; i < DungeonLogic.m_CurDungeonData.StageDataList.Count; i++)
		{
			StageData stageData = DungeonLogic.m_CurDungeonData.StageDataList[i];
			if (stageData.BattleDatasList.Count > 1)
			{
				m_curStageID = stageData.ID;
				m_NpcNameLable.text = DataManager.s_NpcDataManager.
									  GetData(stageData.NpcID).Name.ToString() + ":";   
				break;
			}
		}
	}

	protected void CreateStars(int index)
	{
		LootStarsClass loot = new LootStarsClass();
		
		loot.ObjControl = GameObject.Instantiate(m_TemplateStar) as GameObject;
		loot.ObjControl.name = "LootObj" + (index + 1001);
		loot.ObjControl.SetActive(true);
		
		//loot.StarBack = loot.ObjControl.transform.FindRecursively("StarBack").gameObject.GetComponent<UISprite>();
		loot.StarFront = loot.ObjControl.transform.FindRecursively("StarFront").gameObject.GetComponent<UISprite>();
		if (m_Index - 1 < index)
		{
			loot.StarFront.gameObject.SetActive(false);
		}
		
		loot.ObjControl.transform.parent = m_TemplateStar.transform.parent;
		loot.ObjControl.transform.localScale = Vector3.one;
		
		LootListStar.Add(loot);
	}

	protected override void OnShow()
	{
		m_TemplateStar = this.GetChild("StarsTemplate");
		m_TemplateStar.gameObject.SetActive(false);

		for (int i = 0; i < DungeonLogic.m_CurDungeonData.StageDataList.Count; i++)
		{
			StageData stageData = DungeonLogic.m_CurDungeonData.StageDataList[i];
			if (stageData.BattleDatasList.Count > 1)
			{
				for (int j = 0; j < stageData.BattleDatasList.Count; j++)
				{
					CreateStars(j);
				}
			}
		}

		UIGrid gridStar = this.GetChild("LootList").GetComponent<UIGrid>();
		gridStar.Reposition();
		
		//this.RegisterMessage(ServerCustomMsgs.S_CUSTOM_LOOTS, HandleLootMsg);
		//this.RegisterMessage(ServerCustomMsgs.S_CUSTOM_COLLECTS, HandleOtherLootMsg);
	}

	protected override void OnHide ()
	{
	}

	protected override void OnDestory()
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
		if (sender.name.Equals("ReturnMainBtn"))
		{
			OnClickReturnMainBtn();
		}
		else if (sender.name.Equals("TriggerBtn"))
		{
			OnClickTrigger();
		}
	}

	public override void Update ()
	{
		int count = LootListStar.Count;
		if (m_Index >= count)
		{
			m_TimeLabel.gameObject.SetActive(false);
			m_nextUpLable.gameObject.SetActive(false);
		}
		else
		{
			int index = DungeonLogic.m_StageGrowth[m_curStageID];
//			float second = TimerManager.Instance.GetTimerLeftWithPrefix("StageGrowth"
//			                            + m_curStageID.ToString() + index.ToString());

			//m_TimeLabel.text = second.ToString("0") + "秒";
		}
	}

	public void SetStarsLight()
	{
		int count = LootListStar.Count;
		if (m_Index < count)
		{
			LootListStar[m_Index].StarFront.gameObject.SetActive(true);
			m_Index ++;
		}
	}

	private void OnClickReturnMainBtn()
	{
		m_Index = 0;
		//DungeonLogic.LeaveDungeon();
	}

	private void OnClickTrigger()
	{
		HeroController.Instance.OnTriggerClick();
	}




}