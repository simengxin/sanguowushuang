using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class BattleData
{
	/// <summary>
	/// 数据编号;
	/// </summary>
	public int ID;
	
	/// <summary>
	/// 名字;
	/// </summary>
	public string Name;
	
	/// <summary>
	/// 场景Id;
	/// </summary>
	public int SceneId;
	
	/// <summary>
	/// 关卡npc数据
	/// </summary>
	public List<AttackNpcSpawnWave> AttackNpcSpawnWaveList;

	/// <summary>
	/// 关卡神将出生数据
	/// </summary>
	public List<WarriorSpawnPos> WarriorSpawnList = null;
}

public class AttackNpcSpawnWave
{
	/// <summary>
	/// 出生时间
	/// </summary>
	public float SpawnTime;

	/// <summary>
	/// 关卡npc数据
	/// </summary>
	public List<AttackNpcSpawnData> AttackNpcSpawnList = new List<AttackNpcSpawnData>();
}

public class AttackNpcSpawnData
{	
	/// <summary>
	/// 数据编号;
	/// </summary>
	public int ID;

	/// <summary>
	/// 生成器编号;
	/// </summary>
	public int AttackNpcID;
	
	/// <summary>
	/// 位置;
	/// </summary>
	public Vector3 Position;
	public float Orient;

	/// <summary>房
	/// 巡逻半径;
	/// </summary>
	public float PatrolRadius;

	/// <summary>
	/// AI;
	/// </summary>
	public int AI;
}

public class WarriorSpawnPos
{
	/// <summary>
	/// 位置
	/// </summary>
	public Vector3 Position;
	public float Orient;
}

public class BattleDataManager
{
	private Dictionary<int, BattleData> m_BattleDataDic = null;

	/// <summary>
	/// 获取副本数据;
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public BattleData GetData(int key)
	{
		if (m_BattleDataDic == null)
		{
			LoadBattleData();
		}
		
		return m_BattleDataDic.ContainsKey(key) ? m_BattleDataDic[key] : null;
	}
	
	/// <summary>
	/// 加载NPC数据;
	/// </summary>
	public void LoadBattleData()
	{
		m_BattleDataDic = new Dictionary<int, BattleData>();
		string textAsset = ResourcesManager.Instance.LoadConfigXML(COMMDEF.XML_BATTLE_TEXT).text;
		
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(textAsset);
		XmlNode datas = xmlDoc.SelectSingleNode("BattleDatas");
		
		XmlNodeList list = datas.ChildNodes;
		if (list == null || list.Count < 1)
		{
			return ;
		}
		
		foreach (XmlNode node in list)
		{
			XmlElement element = node as XmlElement;
			if (!element.Name.Equals("BattleData"))
			{
				continue;
			}
			BattleData info = new BattleData();
			
			info.ID = CommonHelper.Str2Int(element.GetAttribute("ID"));
			info.Name = element.GetAttribute("Name");
			info.SceneId = CommonHelper.Str2Int(element.GetAttribute("SceneId"));
			
			XmlNodeList data = element.ChildNodes;
			if (data == null || data.Count < 1)
			{
				continue;
			}
			
			foreach (XmlNode subNode in data)
			{
				XmlElement subElement = subNode as XmlElement;
				if (subElement.Name.Equals("WarriorSpawnData"))
				{
					XmlNodeList spawnDatas = subElement.ChildNodes;
					if (spawnDatas == null || spawnDatas.Count < 1)
					{
						continue;
					}

					foreach (XmlNode ssubNode in spawnDatas)
					{
						XmlElement ssubElement = ssubNode as XmlElement;
						if (ssubElement.Name.Equals("WarriorSpawnPos"))
						{
							WarriorSpawnPos spawnPos = new WarriorSpawnPos();
							
							float subx = CommonHelper.Str2Float(ssubElement.GetAttribute("PosX"));
							float suby = CommonHelper.Str2Float(ssubElement.GetAttribute("PosY"));
							float subz = CommonHelper.Str2Float(ssubElement.GetAttribute("PosZ"));
							spawnPos.Position = new Vector3(subx, suby, subz);
							spawnPos.Orient = CommonHelper.Str2Float(ssubElement.GetAttribute("Orient"));
							
							//加入npc信息
							if (info.WarriorSpawnList == null)
							{
								info.WarriorSpawnList = new List<WarriorSpawnPos>();
							}
							info.WarriorSpawnList.Add(spawnPos);
						}
					}
				}
				else if (subElement.Name.Equals("AttackNpcWaveData"))
				{
					XmlNodeList npcWave = subElement.ChildNodes;
					if (npcWave == null || npcWave.Count < 1)
					{
						continue;
					}

					if (info.AttackNpcSpawnWaveList == null)
					{
						info.AttackNpcSpawnWaveList = new List<AttackNpcSpawnWave>();
					}

					AttackNpcSpawnWave wave = new AttackNpcSpawnWave();
					wave.SpawnTime = CommonHelper.Str2Float(subElement.GetAttribute("SpawnTime"));

					foreach (XmlNode ssubNode in npcWave)
					{
						XmlElement ssubElement = ssubNode as XmlElement;
						if (ssubElement.Name.Equals("AttackNpcSpawnData"))
						{
							AttackNpcSpawnData spawnData = new AttackNpcSpawnData();
							spawnData.ID = CommonHelper.Str2Int(ssubElement.GetAttribute("ID"));
							spawnData.AttackNpcID = CommonHelper.Str2Int(ssubElement.GetAttribute("AttackNpcID"));
							float subx = CommonHelper.Str2Float(ssubElement.GetAttribute("PosX"));
							float suby = CommonHelper.Str2Float(ssubElement.GetAttribute("PosY"));
							float subz = CommonHelper.Str2Float(ssubElement.GetAttribute("PosZ"));
							spawnData.Position = new Vector3(subx, suby, subz);
							spawnData.Orient = CommonHelper.Str2Float(ssubElement.GetAttribute("Orient"));
							spawnData.PatrolRadius = CommonHelper.Str2Float(ssubElement.GetAttribute("PatrolRadius"));
							spawnData.AI = CommonHelper.Str2Int(ssubElement.GetAttribute("AI"));
							wave.AttackNpcSpawnList.Add(spawnData);
						}
					}

					info.AttackNpcSpawnWaveList.Add(wave);
				}
			}

			info.AttackNpcSpawnWaveList.Sort(delegate(AttackNpcSpawnWave a, AttackNpcSpawnWave b)
         	{
				return a.SpawnTime.CompareTo(b.SpawnTime);
			});

			//加入副本信息
			if (!m_BattleDataDic.ContainsKey(info.ID))
			{
				m_BattleDataDic.Add(info.ID, info);
			}
		}
	}
}

