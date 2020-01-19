using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum NavmeshPriority : int{
	DEAD = 0,
	HERO = 50,
	NPC = 40,
	WARRIOR=10,
	ATTACKNPC = 20,
}

public class DungeonUnitManager  {
	/// <summary>
	/// 主将控制器;
	/// </summary>
	private HeroController m_HeroController;
	public HeroController HeroController{
		get{
			return m_HeroController;
		}
	}
	/// <summary>
	/// 创建主将;
	/// </summary>
	/// <returns></returns>
	public void CreateHero(int warriorId, Vector3 pos, float orient)
	{
		WarriorData warriorData = DataManager.s_WarriorDataManager.GetData (warriorId);
		if (warriorData == null)
			return;

		Warrior warrior = new Warrior (warriorId);

		Quaternion rotate = Quaternion.Euler (new Vector3 (0, orient * Mathf.Rad2Deg, 0));
		GameObject prefab = ResourcesManager.Instance.LoadUnitObject (warriorData.Model, UNIT_TYPE.HERO);
		GameObject hero = GameObject.Instantiate (prefab) as GameObject;

		hero.transform.position = pos;
		hero.transform.rotation = rotate;

		UnityEngine.AI.NavMeshAgent agent = hero.AddComponent<UnityEngine.AI.NavMeshAgent>();
		agent.speed = 0;
		agent.acceleration = 0;
		agent.angularSpeed = 0;
		agent.avoidancePriority = (int)NavmeshPriority.HERO;
		agent.height = warriorData.Height;
		agent.radius = warriorData.Radius;
		agent.stoppingDistance = agent.radius;
		
		hero.SetLayerRecursively(LayerMask.NameToLayer("Warrior"));
		
		m_HeroController = hero.AddComponent<HeroController>();
		m_HeroController.m_Warrior = warrior;
	}

	/// <summary>
	/// 关卡Npc控制器列表;
	/// </summary>
	private Dictionary<int, StageController> m_StageNpcControllerDic = new Dictionary<int, StageController>();
	public Dictionary<int, StageController> StageNpcControllerDic
	{
		get {return m_StageNpcControllerDic;}
	}	

	/// <summary>
	/// 创建关卡Npc;
	/// </summary>
	/// <returns></returns>
	public void CreateStageNpc(StageData stage)
	{
		if (stage == null)
		{
			return;
		}
		
		NpcData npcData = DataManager.s_NpcDataManager.GetData(stage.NpcID);
		if (npcData == null)
		{
			return;
		}
		
		Npc npc = new Npc(stage.NpcID);
		
		Quaternion rotate = Quaternion.Euler(new Vector3(0, stage.Orient * Mathf.Rad2Deg, 0));
		GameObject prefab = ResourcesManager.Instance.LoadUnitObject(npcData.Model, UNIT_TYPE.NPC);
		GameObject npcobj = GameObject.Instantiate (prefab) as GameObject;
		
		npcobj.transform.position = stage.Position; 
		npcobj.transform.rotation = rotate;
		
		UnityEngine.AI.NavMeshAgent agent = npcobj.AddComponent<UnityEngine.AI.NavMeshAgent>();
		agent.speed = 0;
		agent.acceleration = 0;
		agent.angularSpeed = 0;
		agent.avoidancePriority = (int)NavmeshPriority.NPC;
		agent.height = npcData.Height;
		agent.radius = npcData.Radius;
		agent.stoppingDistance = agent.radius;
		/*NavMeshObstacle obstacle = npcobj.AddComponent<NavMeshObstacle>();
		obstacle.height = npcData.Height;
		obstacle.radius = npcData.Radius;
		obstacle.carving = true;*/
		
		CapsuleCollider collider = npcobj.AddComponent<CapsuleCollider>();
		collider.height = npcData.Height;
		collider.radius = npcData.Radius;
		collider.center = Vector3.up * (Mathf.Max(collider.height / 2.0f, collider.radius) + 0.03f);
		
		npcobj.SetLayerRecursively(LayerMask.NameToLayer("AttackNpc"));

		if (stage.NpcType == NpcTypes.Battle)
		{
			NpcController controller = npcobj.AddComponent<NpcController>();
			controller.m_Npc = npc;
			controller.m_CurStageData = stage;
			m_StageNpcControllerDic.Add(stage.ID, controller);
		}
		else if (stage.NpcType == NpcTypes.TreasureBox)
		{
			TreasureBoxController controller = npcobj.AddComponent<TreasureBoxController>();
			controller.m_Npc = npc;
			controller.m_CurStageData = stage;
			m_StageNpcControllerDic.Add(stage.ID, controller);
		}
		else if (stage.NpcType == NpcTypes.Stone || stage.NpcType == NpcTypes.Crystal ||
		         stage.NpcType == NpcTypes.Wood)
		{
			DungeonCollectController controller = npcobj.AddComponent<DungeonCollectController>();
			controller.m_Npc = npc;
			controller.m_CurStageData = stage;
			m_StageNpcControllerDic.Add(stage.ID, controller);
		}
	}
}
