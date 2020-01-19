using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum DungeonStateTypes{
	DungeonNone = 0,
	DungeonSetClose = 1,
	DungeonSetOpen = 2,
}
public class WarfareIconTemplate{
	public GameObject ControlObj;
	public UILabel NameLabel;
	public UITexture Icon;
}

public class WarfareInfoPanel{
	public GameObject PanelObj;
	public UILabel NameLabel;
	public UILabel DescLabel;
	public List<WarfareIconTemplate> ProductLabelList = new List<WarfareIconTemplate> ();
	private int m_DungeonId = 0;


	public void ShowPanel(int dungeonId){
		m_DungeonId = dungeonId;
		DungeonData data = DataManager.s_DungeonDataManager.GetData (m_DungeonId);
		if (data != null) {
			this.PanelObj.SetActive(true);
			for (int i = 0; i < ProductLabelList.Count; i++) {
				ProductLabelList[i].ControlObj.SetActive(false);
			}
			
			NameLabel.text = data.Name;
			DescLabel.text = data.Desc;
			
			int ControlIndex = 0;
			for (int i = 0; i < data.CapitalProducts.Length; i++) {
				if(ControlIndex >= ProductLabelList.Count){
					return;
				}
				CapitalData cData = DataManager.s_CapitalDataManager.GetData((CapitalType)data.CapitalProducts[i]);
				if(cData == null){
					return;
				}
				ProductLabelList[ControlIndex].ControlObj.SetActive(true);
				ProductLabelList[ControlIndex].NameLabel.text = cData.Name;
				ProductLabelList[ControlIndex].Icon.mainTexture = ResourcesManager.Instance.GetCapital(cData.Icon);
				ControlIndex ++;
				
			}
			
			for (int i = 0; i < data.ItemProducts.Length; i++) {
				if(ControlIndex >= ProductLabelList.Count){
					return;
				}
				ItemData iData = DataManager.s_ItemDataManager.GetData(data.ItemProducts[i]);
				if(iData == null)
					return;
				ProductLabelList[ControlIndex].ControlObj.SetActive(true);
				ProductLabelList[ControlIndex].NameLabel.text= iData.Name;
				ProductLabelList[ControlIndex].Icon.mainTexture = ResourcesManager.Instance.GetItem(iData.Icon);
				ControlIndex++;
			}
			
		}
	}

	public void HidePanel(){
		this.PanelObj.SetActive(false);
	}

	public void OpendDungeon(){
		//TODO
		int warriorId = DungeonLogic.GetMainWarriorId ();
		WarriorData warriorData = DataManager.s_WarriorDataManager.GetData (warriorId);
		if (warriorData == null) {
		
			Debug.LogError ("no hero");
		} else {
			DungeonLogic.LoadDungeon(m_DungeonId);
		}

	}
}

public class MapPanel : IView
{
	private WarfareInfoPanel m_WarfareInfoPanel;

	protected override void OnStart()
	{
		Debug.LogError("MapPanel onstart");
		
	}
	/// <summary>
	/// 大地图副本类型初始化
	/// </summary>
	private void MapDungeonInit(){
		GameObject m_MapDungeonPanel = this.GetChild ("Panel");

		UIButton[] buttomList = m_MapDungeonPanel.GetComponentsInChildren<UIButton> (true);

		int len = buttomList.Length;
		for (int i = 0; i < len; i++) {
			buttomList[i].isEnabled = false;
			buttomList[i].SetState(UIButton.State.Disabled,true);
		}
	}
	/// <summary>
	/// 大地图副本类型设置
	/// </summary>
	private void SetDungeonState(UIButton button,DungeonStateTypes state){
		switch(state){
		case DungeonStateTypes.DungeonNone:
			button.isEnabled = false;
			button.SetState(UIButton.State.Disabled,true);
			break;
		case DungeonStateTypes.DungeonSetClose:
			button.isEnabled = false;
			button.SetState(UIButton.State.Disabled,true);
			break;
		case DungeonStateTypes.DungeonSetOpen:
			button.isEnabled = true;
			button.SetState(UIButton.State.Normal,true);
			break;
		default:
			break;
		}
	}


	protected override void OnShow()
	{
		MapDungeonInit ();

		List<DungeonData> datas = DataManager.s_DungeonDataManager.GetAll ();
		for (int i = 0; i < datas.Count; i++) {
			GameObject ButtonObj = this.GetChild("Dungeon"+datas[i].ID);
			if(ButtonObj != null){
				UIButton dungeon = ButtonObj.GetComponent<UIButton>();
				SetDungeonState(dungeon,DungeonStateTypes.DungeonSetOpen);
			}
		}

		if (m_WarfareInfoPanel == null) {
			m_WarfareInfoPanel = new WarfareInfoPanel();
			m_WarfareInfoPanel.PanelObj = this.GetChild("WarfareInfoPanel");
			m_WarfareInfoPanel.NameLabel = this.GetChild("WarfareName").GetComponent<UILabel>();
			m_WarfareInfoPanel.DescLabel = this.GetChild("WarfareInfo").GetComponent<UILabel>();

			WarfareIconTemplate t = new WarfareIconTemplate();
			t.ControlObj = this.GetChild("Type1");
			t.NameLabel = this.GetChild("TypeNameImg1").GetComponent<UILabel>();
			t.Icon = this.GetChild("TypeResImg1").GetComponent<UITexture>();
			m_WarfareInfoPanel.ProductLabelList.Add(t);
			
			t = new WarfareIconTemplate();
			t.ControlObj = this.GetChild("Type2");
			t.NameLabel = this.GetChild("TypeNameImg2").GetComponent<UILabel>();
			t.Icon = this.GetChild("TypeResImg2").GetComponent<UITexture>();
			m_WarfareInfoPanel.ProductLabelList.Add(t);
			
			t = new WarfareIconTemplate();
			t.ControlObj = this.GetChild("Type3");
			t.NameLabel = this.GetChild("TypeNameImg3").GetComponent<UILabel>();
			t.Icon = this.GetChild("TypeResImg3").GetComponent<UITexture>();
			m_WarfareInfoPanel.ProductLabelList.Add(t);
			
			t = new WarfareIconTemplate();
			t.ControlObj = this.GetChild("Type4");
			t.NameLabel = this.GetChild("TypeNameImg4").GetComponent<UILabel>();
			t.Icon = this.GetChild("TypeResImg4").GetComponent<UITexture>();
			m_WarfareInfoPanel.ProductLabelList.Add(t);
		}
		m_WarfareInfoPanel.HidePanel();
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

		if (sender.name.Contains("Dungeon"))
		{
			string dungeon = sender.name.Substring(7);
			int dungeonId = CommonHelper.Str2Int(dungeon);
			m_WarfareInfoPanel.ShowPanel(dungeonId);
		}
		else if (sender.name.Contains("BtnReturn"))
		{
			GUIManager.HideView("MapPanel");
		}
		else if (sender.name.Contains("BtnWarfareClose"))
		{
			m_WarfareInfoPanel.HidePanel();
		}
		else if (sender.name.Contains("BtnWarfareOpen"))
		{
			m_WarfareInfoPanel.OpendDungeon();
		}
	}
}























