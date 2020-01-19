using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleUnitManagerPVE {
	/// <summary>
	/// 出场神将列表;
	/// </summary>
	private List<Warrior> m_WarriorList = new List<Warrior> ();
	public List<Warrior> WarriorList{
		get{
			return m_WarriorList;
		}
	}
	/// <summary>
	/// 出场AttackNpc列表;
	/// </summary>
	private Dictionary<int,AttackNpc> m_AttackNpcDic = new Dictionary<int, AttackNpc> ();
	public Dictionary<int,AttackNpc> AttackNpcDic{
		get{
			return m_AttackNpcDic;
		}
	}
	/// <summary>
	/// 神将控制器列表;
	/// </summary>
	private Dictionary<int, WarriorController> m_WarriorControllerDic = new Dictionary<int, WarriorController>();
	public Dictionary<int, WarriorController> WarriorControllerDic
	{
		get {return m_WarriorControllerDic;}
	}
	
	/// <summary>
	/// AttackNpc控制器列表;
	/// </summary>
	private Dictionary<int, AttackNpcController> m_AttackNpcControllerDic = new Dictionary<int, AttackNpcController>();
	public Dictionary<int, AttackNpcController> AttackNpcControllerDic
	{
		get {return m_AttackNpcControllerDic;}
	}
	public int SpawnWaveCount = 0;

	public void Init(List<AttackNpcSpawnWave> spawnWaveList){
		for (int i = 0; i < spawnWaveList.Count; i++) {
			AttackNpcSpawnWave wave = spawnWaveList[i];
			for (int j = 0; j < wave.AttackNpcSpawnList.Count; j++) {
				InitAttackNpc(wave.AttackNpcSpawnList[j]);
			}
		}


		List<int> warriorList = GetWarriorList ();
		for (int i = 0; i < warriorList.Count; i++) {
			InitWarrior(warriorList[i]); 
		}

	}
	/// <summary>
	/// 初始AttackNpc数据;
	/// </summary>
	/// <returns></returns>
	void InitAttackNpc(AttackNpcSpawnData spawnData ){
		AttackNpcData attackNpcData = DataManager.s_AttackNpcDataManager.GetData (spawnData.AttackNpcID);
		if (attackNpcData == null) {
			return;
		}
		AttackNpc attackNpc = new AttackNpc (spawnData.AttackNpcID);
		if (attackNpc != null) {
			m_AttackNpcDic.Add(spawnData.ID,attackNpc);
		}
	}

	void InitWarrior(int id){
		WarriorData warriorData = DataManager.s_WarriorDataManager.GetData (id);
		if (warriorData == null)
			return;

		Warrior warrior = new Warrior (id);
		if (warrior != null) {
			m_WarriorList.Add(warrior);
		}
	}

	/// <summary>
	/// 获得出场神将;
	/// </summary>
	List<int> GetWarriorList(){
		List<int> result = new List<int> ();
		result.Add (1001);
		result.Add (1002);
		result.Add (1003);
		result.Add (1004);
		

		return result;
	}

	public WarriorController GetWarriorControllerByIndex(int index)
	{
		WarriorController c = null;
		WarriorControllerDic.TryGetValue(index, out c);
		return c;
	}

	/// <summary>
	/// 创建神将;
	/// </summary>
	/// <returns></returns>
	public WarriorController CreateWarrior(int index,Vector3 pos,float orient,BattleTypes battleType){
		Warrior warrior = m_WarriorList [index];

		Quaternion rotate = Quaternion.Euler (new Vector3 (0, orient * Mathf.Rad2Deg, 0));
		GameObject prefab = ResourcesManager.Instance.LoadUnitObject (warrior.Model, UNIT_TYPE.WARRIOR);
		GameObject warriorObj = GameObject.Instantiate (prefab) as GameObject;

		warriorObj.transform.position = pos;
		warriorObj.transform.rotation = rotate;

		UnityEngine.AI.NavMeshAgent agent = warriorObj.AddComponent<UnityEngine.AI.NavMeshAgent>();
		agent.speed = 0;
		agent.acceleration = 0;
		agent.angularSpeed = 0;
		//agent.avoidancePriority = (int)NavmeshPriority.WARRIOR;
		agent.height = warrior.Height;
		agent.radius = warrior.Radius;
		agent.stoppingDistance = 0.5f;
		//agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
		
		CharacterController collider = warriorObj.AddComponent<CharacterController>();
		collider.height = warrior.Height;
		collider.radius = warrior.Radius;
		collider.center = Vector3.up * (Mathf.Max(collider.height / 2.0f, collider.radius) + 0.03f);
		warriorObj.SetLayerRecursively(LayerMask.NameToLayer("Warrior"));
	
		WarriorController controller = warriorObj.AddComponent<WarriorController>();
		m_WarriorControllerDic.Add(index, controller);
		controller.CurCombatUnit = warrior;
		//controller.m_BattleType = battleType;
		
		return controller;
	}

	/// <summary>
	/// 创建AttackNpc;
	/// </summary>
	/// <returns></returns>
	public void CreateAttackNpc(AttackNpcSpawnData spawnData){
		if (spawnData == null) {
			return;
		}

		AttackNpcData attackNpcData = DataManager.s_AttackNpcDataManager.GetData (spawnData.AttackNpcID);
		if (attackNpcData == null) {
			return;
		}

		AttackNpc attackNpc = m_AttackNpcDic [spawnData.ID];
		Quaternion rotate = Quaternion.Euler (new Vector3 (0, spawnData.Orient * Mathf.Rad2Deg, 0));
		GameObject prefab = ResourcesManager.Instance.LoadUnitObject (attackNpcData.Model, UNIT_TYPE.ATTACK_NPC);
		GameObject npcobj = GameObject.Instantiate (prefab) as GameObject;

		npcobj.transform.position = spawnData.Position;
		npcobj.transform.rotation = rotate;

		UnityEngine.AI.NavMeshAgent agent = npcobj.AddComponent<UnityEngine.AI.NavMeshAgent> ();
		agent.speed = 0;
		agent.acceleration = 0;
		agent.angularSpeed = 0;
		//agent.avoidancePriority =
		agent.height = attackNpcData.Height;
		agent.radius = attackNpcData.Radius;
		agent.stoppingDistance = 0.5f;

		CharacterController collider = npcobj.AddComponent<CharacterController> ();
		collider.height = attackNpcData.Height;
		collider.radius = attackNpcData.Radius;
		collider.center = Vector3.up * Mathf.Max (collider.height / 2.0f, collider.radius);
		npcobj.SetLayerRecursively(LayerMask.NameToLayer("AttackNpc"));

		AttackNpcController controller = npcobj.AddComponent<AttackNpcController> ();
		m_AttackNpcControllerDic.Add (spawnData.ID, controller);
		controller.CurCombatUnit = attackNpc;
		controller.PatrolRadius = spawnData.PatrolRadius;
	}
}














