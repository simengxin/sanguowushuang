using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleLogicPVE : MonoBehaviour {
	public static BattleLogicPVE Instance = null;
	
	/// <summary>
	/// 当前战斗包数据;
	/// </summary>
	private static BattleData m_BattleData;
	/// <summary>
	/// 关卡数据;
	/// </summary>
	private static StageData m_StageData;
	/// <summary>
	/// 战斗单元管理;
	/// </summary>
	public static BattleUnitManagerPVE m_BattleUnitManagerPVE = null;

	/// <summary>
	/// 神将降临CD;
	/// </summary>
	public float BornCDTime = 1.5f;
	/// <summary>
	/// 开始增加降临力;
	/// </summary>
	private bool m_StartBp = false;
	/// <summary>
	/// 开始增加降临力时间;
	/// </summary>
	private float m_StartBpStime = 0;

	public static void LoadBattle(StageData stage){
		int battleId = DungeonLogic.GetBattleDataTimeByStage (stage).BattleID;
		m_BattleData = DataManager.s_BattleDataManager.GetData (battleId);
		if (m_BattleData == null) {
			return ;
		}
		m_StageData = stage;
		Debug.Log(m_BattleData.SceneId);
		GameStateManager.LoadScene (m_BattleData.SceneId);
	}
	// Use this for initialization
	void Start () {
		//GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
		m_BattleUnitManagerPVE = new BattleUnitManagerPVE ();
		m_BattleUnitManagerPVE.Init (m_BattleData.AttackNpcSpawnWaveList);

		Instance = this;

		GUIManager.ShowView("BattlePanel");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartBattle(){
		for (int i = 0; i < m_BattleData.AttackNpcSpawnWaveList.Count; i++) {
			AttackNpcSpawnWave wave = m_BattleData.AttackNpcSpawnWaveList[i];
			SpawnAttackNpcWave(wave);

		}

		for (int i = 0; i < m_BattleUnitManagerPVE.WarriorList.Count; i++) {
			m_BattleUnitManagerPVE.WarriorList[i].OnDeadEvent += HandleUnitDead;
			int battle_index = DungeonLogic.m_StageGrowth[m_StageData.ID];
			//if(m_StageData.BattleDatasList[battle_index].BattleType == Battle)
			SpawnWarrior(i,m_BattleData.WarriorSpawnList[i].Position,m_BattleData.WarriorSpawnList[i].Orient);
		}

		List<AttackNpc> npcList = new List<AttackNpc> (m_BattleUnitManagerPVE.AttackNpcDic.Values);
		for (int i = 0; i < npcList.Count; i++) {
			npcList[i].OnDeadEvent += HandleUnitDead;
		}
	} 
	/// <summary>
	/// Warrior死亡计数;
	/// </summary>
	private int m_WarriorDeadCount = 0;
	
	/// <summary>
	/// AttackNpc死亡计数;
	/// </summary>
	private int m_AttackNpcDeadCount = 0;

	void HandleUnitDead(CombatUnit unit){
		if (unit is Warrior) {
			m_WarriorDeadCount ++;
			if (m_WarriorDeadCount == m_BattleUnitManagerPVE.WarriorList.Count) {
				BattleFinishPanel.Success = false;
				StartCoroutine(ShowBattleFinishPanel());
				//m_BattleResult = false;
				//nstance.Run = false;
			}
		} else if (unit is AttackNpc) {
			m_AttackNpcDeadCount ++;
			if(m_AttackNpcDeadCount == m_BattleUnitManagerPVE.AttackNpcDic.Count){
				BattleFinishPanel.Success = true;
				StartCoroutine(ShowBattleFinishPanel());
				//m_BattleResult = true;
				//Instance.Run = false;
				return;
			}
		}
	}
	/// <summary>
	/// 战斗结束面板显示;
	/// </summary>
	IEnumerator ShowBattleFinishPanel()
	{
		yield return new WaitForSeconds(1);
		GUIManager.ShowView("BattleFinishPanel");
		if (BattleFinishPanel.Success)
		{
			//int loot = DungeonLogic.GetBattleDataTimeByStage(m_StageData).Loot;
			

		}
	}

	/// <summary>
	/// 投放神将;
	/// </summary>
	public void SpawnWarrior(int indexer,Vector3 position,float orient){
		int battle_index = DungeonLogic.m_StageGrowth [m_StageData.ID];
		m_BattleUnitManagerPVE.CreateWarrior (indexer, position, orient, m_StageData.BattleDatasList[battle_index].BattleType);
		if (!m_StartBp) {
			m_StartBp = true;
			m_StartBpStime = Time.time;
		}
	}


	/// <summary>
	/// AttackNpc出生;
	/// </summary>
	private void SpawnAttackNpcWave(AttackNpcSpawnWave wave){
		AttackNpcSpawnWave m_wave = wave;
		for (int i = 0; i < m_wave.AttackNpcSpawnList.Count; i++) {
			m_BattleUnitManagerPVE.CreateAttackNpc(wave.AttackNpcSpawnList[i]);
		}
	}





}
