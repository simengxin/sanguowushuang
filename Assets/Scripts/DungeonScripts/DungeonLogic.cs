using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DungeonLogic : MonoBehaviour {
	/// <summary>
	/// 副本数据;
	/// </summary>
	public static DungeonData m_CurDungeonData;
	/// <summary>
	/// 主将位置;
	/// </summary>
	public static Vector3 m_HeroPosition;
	public static float m_HeroOrient;

	public Vector3 HeroPosition
	{
		get
		{
			return m_HeroPosition;
		}
	}
	/// <summary>
	/// 战斗场景返回;
	/// </summary>
	public static void ReLoadDungeon(bool result, int stageId = 0)
	{
		if (result)
		{
			m_StagePassed.Add(stageId);
		}
		
		if (m_CurDungeonData != null)
		{
			GameStateManager.LoadScene(m_CurDungeonData.SceneId);
		}
		
		TimerManager.Instance.ResumeTimerWithPrefix("StageGrowth");
	}

	/// <summary>
	/// 已过关卡;
	/// </summary>
	private static List<int> m_StagePassed;
	
	/// <summary>
	/// 关卡成长包;
	/// </summary>
	public static Dictionary<int, int> m_StageGrowth;

	private DungeonUnitManager m_DungeonUnitManager;

	/// <summary>
	/// 定时器标记(防止重复添加);
	/// </summary>
	private static bool m_DungeonStart = false;
	
	private static bool m_DungeonPass = false;
	
	//private DungeonUnitManager m_DungeonUnitManager;
	
	private float FogOutterRaidus = 7.0f;
	
	public static DungeonLogic Instance;
	public static void LoadDungeon(int dungeonId){
		m_CurDungeonData = DataManager.s_DungeonDataManager.GetData (dungeonId);
		if (m_CurDungeonData == null) {
			return;
		}
		m_HeroPosition = m_CurDungeonData.Position;
		m_HeroOrient = m_CurDungeonData.Orient;
		m_StagePassed = new List<int> ();

		m_StageGrowth = new Dictionary<int, int> ();
		int count = m_CurDungeonData.StageDataList.Count;
		for (int i = 0; i < count; i++) {
			m_StageGrowth.Add(m_CurDungeonData.StageDataList[i].ID,0);
		}

		m_DungeonStart = true;
		GameStateManager.LoadScene (m_CurDungeonData.SceneId);

	}
	/// <summary>
	/// 根据关卡获得战斗包;
	/// </summary>
	public static BattleDataByTime GetBattleDataTimeByStage(StageData stage){
		int index = m_StageGrowth[stage.ID];
		return stage.BattleDatasList[index];
	}

	public static int GetMainWarriorId(){
		return 1001;
	}

	// Use this for initialization
	void Start () {
		m_DungeonUnitManager = new DungeonUnitManager ();

		int mainWarriorId = GetMainWarriorId ();
		m_DungeonUnitManager.CreateHero (mainWarriorId, m_HeroPosition, m_HeroOrient);
		if (m_DungeonUnitManager.HeroController == null) {
			Debug.LogError("hero is null");
			return;
		}
		GameObject carmera = GameObject.FindWithTag("MainCamera");
		DungeonCamera dungeonCamera = carmera.GetComponent<DungeonCamera> ();
		dungeonCamera.target = m_DungeonUnitManager.HeroController.transform;

	}
	
	// Update is called once per frame
	void Update () {
		if (m_DungeonUnitManager.HeroController != null
		    && m_CurDungeonData != null && m_CurDungeonData.StageDataList.Count>0) {
			m_HeroPosition = m_DungeonUnitManager.HeroController.transform.position;
			m_HeroOrient = m_DungeonUnitManager.HeroController.transform.localEulerAngles.y/180*Mathf.PI;

			foreach (StageData stage in m_CurDungeonData.StageDataList) {
				if(!m_DungeonUnitManager.StageNpcControllerDic.ContainsKey(stage.ID) && 
				   !m_StagePassed.Contains(stage.ID)){

					//if(Vector3.Distance(m_HeroPosition,stage.Position)<)
					m_DungeonUnitManager.CreateStageNpc(stage);
				}
			}
		}
	}
}










