using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattlePanel : IView
{
	public class IconWidget
	{
		/// <summary>
		/// 神将控件;
		/// </summary>
		public GameObject ControlObject;
		
		/// <summary>
		/// 神将图标;
		/// </summary>
		public UITexture IconTexture;
		
		/// <summary>
		/// 背景图标;
		/// </summary>
		public UISprite BackTexture;
		
		/// <summary>
		/// 血条;
		/// </summary>
		public UISlider HpSlider;
		
		/// <summary>
		/// 降临力;
		/// </summary>
		public UISlider BPSlider;
		
		/// <summary>
		/// CDLabel;
		/// </summary>
		public UILabel CDTimeLabel;
		
		/// <summary>
		/// 拖动时控件效果;
		/// </summary>
		public GameObject AniSprite;
		
		/// <summary>
		/// BPTweenColor;
		/// </summary>
		public TweenColor BPTweenColor;
		
		/// <summary>
		/// 神将Index;
		/// </summary>
		private int m_Index = 0;
		
		/// <summary>
		/// CD时间;
		/// </summary>
		private float m_CDTime = 0;
		
		/// <summary>
		/// CD已近过去时间;
		/// </summary>
		private float m_ElaspedTime = 0;
		
		/// <summary>
		/// 是否初始化;
		/// </summary>
		public bool Init = false;
		
		/// <summary>
		/// 是否出身;
		/// </summary>
		public bool Spawn = false;
		
		/// <summary>
		/// 是否死亡;
		/// </summary>
		public bool Dead = false;

		public void InitIcon(int index){
			m_Index = index;
			List<Warrior> warriorList = BattleLogicPVE.m_BattleUnitManagerPVE.WarriorList;
			Warrior warrior = warriorList [m_Index];

			IconTexture.mainTexture = ResourcesManager.Instance.GetWarriorIcon (warrior.Photo);


			CDTimeLabel.gameObject.SetActive (false);
			HpSlider.value = 1.0f;
			AniSprite.SetActive (false);

		}

		public void OnClick(){
			List<Warrior> warriorList = BattleLogicPVE.m_BattleUnitManagerPVE.WarriorList;
			Warrior warrior = warriorList [m_Index];
			if (!warrior.m_HasCastSuperSkill) {
				WarriorController controller = BattleLogicPVE.m_BattleUnitManagerPVE.GetWarriorControllerByIndex(m_Index);
				if(controller != null){
					controller.CastSuperSkill();
				}
			}
		}

		public void OnSpawn(CombatUnit unit){
			Spawn = true;
			AniSprite.SetActive (false);
		}

		public void OnDead(CombatUnit unit){
			if (Init) {
				IconTexture.material.SetFloat("_CD",1.0f);
				unit.OnDeadEvent -=OnDead;
				Dead = true;

				CDTimeLabel.gameObject.SetActive(false);
				BPSlider.value = 0;
			}
		}

		/// <summary>
		/// 血量变化;
		/// </summary>
		private void OnHpChange(CombatUnit unit)
		{
			if (Dead)
			{
				return;
			}
			
			HpSlider.value = (float)unit.Hp / (float)unit.MaxHp;
		}
		
		/// <summary>
		/// 降临力变化;
		/// </summary>
		private void OnBpChange(Warrior warrior)
		{
			if (Dead)
			{
				return;
			}
			
			BPSlider.value = (float)warrior.Bp / (float)warrior.MaxBP;
			
			if (warrior.Bp >= warrior.MaxBP)
			{
				if (BPTweenColor.enabled == false)
				{
					BPTweenColor.enabled = true;
					BPTweenColor.style = UITweener.Style.PingPong;
				}
			}
			else
			{
				if (BPTweenColor.enabled == true)
				{
					BPTweenColor.enabled = false;
				}
			}
		}
		
		/// <summary>
		/// 设置出身CD;
		/// </summary>
		public void SetBpCD()
		{
			if (Init && !Spawn)
			{
				m_CDTime = BattleLogicPVE.Instance.BornCDTime;
				m_ElaspedTime = 0;
				CDTimeLabel.gameObject.SetActive(true);
			}
		}
		
		/// <summary>
		/// 是否CD中;
		/// </summary>
		public bool BpCD
		{
			get {return m_ElaspedTime < m_CDTime;}
		}
	

		public void Update()
		{
			if (Init && !Dead && m_CDTime > 0)
			{
				m_ElaspedTime += Time.deltaTime;
				float normalized = 1.0f - m_ElaspedTime / m_CDTime;
				if (m_ElaspedTime > m_CDTime)
				{
					m_CDTime = 0;
					normalized = 0.0f;
					CDTimeLabel.gameObject.SetActive(false);
				}
				
				float leftTime = m_CDTime - m_ElaspedTime;
				CDTimeLabel.text = leftTime.ToString("0.0");
				IconTexture.material.SetFloat("_CD", normalized);
				IconTexture.MarkAsChanged();
				IconTexture.panel.Refresh();
			}
		}
	}



	private List<IconWidget> m_IconWidgetList = new List<IconWidget>();

	private GameObject m_PauseBtn;
	private GameObject m_ResumeBtn;

	protected override void OnStart()
	{
		//Debug.LogError("BattlePanel onstart");
		m_PauseBtn = this.GetChild("PauseBtn");
		m_ResumeBtn = this.GetChild("ResumeBtn");

		IconWidget widget1 = new IconWidget ();
		widget1.ControlObject = this.GetChild("Warrior1");
		widget1.IconTexture = this.GetChild ("WarriorIcon1").GetComponent<UITexture> ();
		widget1.BackTexture = this.GetChild ("WarriorBack1").GetComponent<UISprite> ();
		widget1.HpSlider = this.GetChild("HpSlider1").GetComponent<UISlider>();
		widget1.BPSlider = this.GetChild("BpSlider1").GetComponent<UISlider>();
		Transform f1 = widget1.BPSlider.transform.FindRecursively("Foreground");
		widget1.BPTweenColor = f1.gameObject.GetComponent<TweenColor>();
		widget1.CDTimeLabel = this.GetChild("WarriorLabel1").GetComponent<UILabel>();
		widget1.AniSprite = this.GetChild("AniSprite1");
		m_IconWidgetList.Add(widget1);

		IconWidget widget2 = new IconWidget();
		widget2.ControlObject = this.GetChild("Warrior2");
		widget2.IconTexture = this.GetChild("WarriorIcon2").GetComponent<UITexture>();
		widget2.BackTexture = this.GetChild("WarriorBack2").GetComponent<UISprite>();
		widget2.HpSlider = this.GetChild("HpSlider2").GetComponent<UISlider>();
		widget2.BPSlider = this.GetChild("BpSlider2").GetComponent<UISlider>();
		Transform f2 = widget2.BPSlider.transform.FindRecursively("Foreground");
		widget2.BPTweenColor = f2.gameObject.GetComponent<TweenColor>();
		widget2.CDTimeLabel = this.GetChild("WarriorLabel2").GetComponent<UILabel>();
		widget2.AniSprite = this.GetChild("AniSprite2");
		m_IconWidgetList.Add(widget2);
		
		IconWidget widget3 = new IconWidget();
		widget3.ControlObject = this.GetChild("Warrior3");
		widget3.IconTexture = this.GetChild("WarriorIcon3").GetComponent<UITexture>();
		widget3.BackTexture = this.GetChild("WarriorBack3").GetComponent<UISprite>();
		widget3.HpSlider = this.GetChild("HpSlider3").GetComponent<UISlider>();
		widget3.BPSlider = this.GetChild("BpSlider3").GetComponent<UISlider>();
		Transform f3 = widget3.BPSlider.transform.FindRecursively("Foreground");
		widget3.BPTweenColor = f3.gameObject.GetComponent<TweenColor>();
		widget3.CDTimeLabel = this.GetChild("WarriorLabel3").GetComponent<UILabel>();
		widget3.AniSprite = this.GetChild("AniSprite3");
		m_IconWidgetList.Add(widget3);
		
		IconWidget widget4 = new IconWidget();
		widget4.ControlObject = this.GetChild("Warrior4");
		widget4.IconTexture = this.GetChild("WarriorIcon4").GetComponent<UITexture>();
		widget4.BackTexture = this.GetChild("WarriorBack4").GetComponent<UISprite>();
		widget4.HpSlider = this.GetChild("HpSlider4").GetComponent<UISlider>();
		widget4.BPSlider = this.GetChild("BpSlider4").GetComponent<UISlider>();
		Transform f4 = widget4.BPSlider.transform.FindRecursively("Foreground");
		widget4.BPTweenColor = f4.gameObject.GetComponent<TweenColor>();
		widget4.CDTimeLabel = this.GetChild("WarriorLabel4").GetComponent<UILabel>();
		widget4.AniSprite = this.GetChild("AniSprite4");
		m_IconWidgetList.Add(widget4);

	}
	
	protected override void OnShow()
	{
		m_PauseBtn.SetActive (true);
		m_ResumeBtn.SetActive (false);
		for (int i = 0; i < m_IconWidgetList.Count; i++) {
			m_IconWidgetList[i].ControlObject.SetActive(false);
		}

		if (BattleLogicPVE.Instance != null) {
			AddWarriorIcon();
		}

		ShowBattleMain ();
	}

	private void ShowBattleMain(params object[] args){
		BattleLogicPVE.Instance.StartBattle ();
	}

	private void AddWarriorIcon(){
		if (BattleLogicPVE.Instance == null) {
			return;
		}

		List<Warrior> warriorList = BattleLogicPVE.m_BattleUnitManagerPVE.WarriorList;
		for (int i = 0; i < warriorList.Count; i++) {
			m_IconWidgetList[i].ControlObject.SetActive(true);
			m_IconWidgetList[i].InitIcon(i);
		}
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

		Debug.LogError ("1111");
		if (sender.name.Equals ("PauseBtn")) {
			OnClickPauseBtn ();
		} else if (sender.name.Equals ("ResumeBtn")) {
		
			OnClickResumeBtn ();
		} else if (sender.name.Equals ("WarriorIcon1")) {
			ClickIcon(0);
		
		}else if (sender.name.Equals ("WarriorIcon2")) {
			ClickIcon(1);
			
		}else if (sender.name.Equals ("WarriorIcon3")) {
			ClickIcon(2);
			
		}else if (sender.name.Equals ("WarriorIcon4")) {
			ClickIcon(3);
			
		}
	}

	private void OnClickPauseBtn(){
		Time.timeScale = 0;
		m_PauseBtn.SetActive (false);
		m_ResumeBtn.SetActive (true);
	}

	
	private void OnClickResumeBtn(){
		Time.timeScale = 1;
		m_PauseBtn.SetActive (true);
		m_ResumeBtn.SetActive (false);
	}

	public void ClickIcon(int index){
		if (!m_IconWidgetList [index].Spawn) {
			return;
		}
		m_IconWidgetList [index].OnClick ();

	}
}







