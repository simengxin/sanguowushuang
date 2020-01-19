using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class Prop
{
	public int		_base;
	public int		_add;
	public int		_grow;
}
public class WarriorData  {
	/// <summary>
	/// 数据编号;
	/// </summary>
	public int ID;
	
	/// <summary>
	/// 名字;
	/// </summary>
	public string Name;
	
	/// <summary>
	/// 描述
	/// </summary>
	public string Desc;
	
	/// <summary>
	/// 模型资源;
	/// </summary>
	public string Model;
	
	/// <summary>
	/// 头像
	/// </summary>
	public string Photo;
	
	/// <summary>
	/// 卡牌;
	/// </summary>
	public string Image;
	
	/// <summary>
	/// 高度;
	/// </summary>
	public float Height;
	
	/// <summary>
	/// 半径;
	/// </summary>
	public float Radius;
	
	/// <summary>
	/// 触发范围;
	/// </summary>
	public float Spring;
	
	
	/// <summary>
	/// 星级
	/// </summary>
	public int Star;
	
	/// <summary>
	/// 等级
	/// </summary>
	public int Level;
	
	/// <summary>
	/// 血量;
	/// </summary>
	public Prop HP = new Prop();
	
	/// <summary>
	/// 攻击力;
	/// </summary>
	public Prop Atk  = new Prop();
	
	/// <summary>
	/// 防御
	/// </summary>
	public Prop Def  = new Prop();
	
	/// <summary>
	/// 降临力回复速度
	/// </summary>
	public int BPSpeed;
	
	/// <summary>
	/// 移动速度;
	/// </summary>
	public float Speed;
	
	/// <summary>
	/// 普通攻击CD;
	/// </summary>
	public float NormalAttackCDTime;
	
	/// <summary>
	/// 技能列表, 第一个为普通技能;
	/// </summary>
	public int[] SkillArray;
	
	/// <summary>
	/// 阵法
	/// </summary>
	public int Tactics;
	
	/// <summary>
	/// 主将技
	/// </summary>
	public int LeaderSkill;
	
	public int Camp;
}
public class WarriorDataManager
{
	private Dictionary<int, WarriorData> m_WarriorDataDic = null;
	
	/// <summary>
	/// 获取NPC数据;
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public WarriorData GetData(int key)
	{
		if (m_WarriorDataDic == null)
		{
			LoadWarriorData();
		}
		
		return m_WarriorDataDic.ContainsKey(key) ? m_WarriorDataDic[key] : null;
	}
	
	//从武将列表中随机一个
	public int RandCard()
	{
		if(null == m_WarriorDataDic)
		{
			LoadWarriorData();
		}
		//int	npcCount = m_WarriorDataDic.Count;
		
		//随机一张
		Dictionary<int, WarriorData>.KeyCollection keyCols = m_WarriorDataDic.Keys;
		int[]		arrKeys =  new int[keyCols.Count];
		keyCols.CopyTo(arrKeys,0);
		
		int		key = arrKeys[Random.Range(0,keyCols.Count)];
		return m_WarriorDataDic[key].ID;			
	}
	
	/// <summary>
	/// 加载NPC数据;
	/// </summary>
	public void LoadWarriorData()
	{
		m_WarriorDataDic = new Dictionary<int, WarriorData>();
		string textAsset = ResourcesManager.Instance.LoadConfigXML("WarriorData").text;
		
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(textAsset);
		XmlNode equipXN = xmlDoc.SelectSingleNode("WarriorDatas");
		
		XmlNodeList list = equipXN.ChildNodes;
		if (list != null && list.Count > 0)
		{
			foreach (XmlNode node in list)
			{
				XmlElement element = node as XmlElement;
				if (element.Name.Equals("WarriorData"))
				{
					WarriorData info = new WarriorData();
					
					info.ID = CommonHelper.Str2Int(element.GetAttribute("ID"));
					info.Name = element.GetAttribute("Name");
					info.Desc = element.GetAttribute("Desc");
					info.Image = element.GetAttribute("Image");
					info.Photo = element.GetAttribute("Photo");
					info.Height = CommonHelper.Str2Float(element.GetAttribute("Height"));
					info.Radius = CommonHelper.Str2Float(element.GetAttribute("Radius"));
					info.Spring = CommonHelper.Str2Float(element.GetAttribute("Spring"));
					info.Model = element.GetAttribute("Model");
					
					info.HP._base = CommonHelper.Str2Int(element.GetAttribute("HP"));
					info.Atk._base = CommonHelper.Str2Int(element.GetAttribute("Atk"));
					info.Def._base = CommonHelper.Str2Int(element.GetAttribute("Def"));		
					
					info.BPSpeed = CommonHelper.Str2Int(element.GetAttribute("BPSpeed"));
					info.Speed = CommonHelper.Str2Float(element.GetAttribute("Speed"));
					info.NormalAttackCDTime = CommonHelper.Str2Float(element.GetAttribute("NormalAttackCDTime"));
					info.SkillArray = CommonHelper.Str2IntArray(element.GetAttribute("SkillArray"));
					info.Tactics = CommonHelper.Str2Int(element.GetAttribute("Tactics"));
					info.LeaderSkill = CommonHelper.Str2Int(element.GetAttribute("LeaderSkill"));
					
					info.Level = CommonHelper.Str2Int(element.GetAttribute("Level"));
					info.Star= CommonHelper.Str2Int(element.GetAttribute("Star"));		
					info.Camp = CommonHelper.Str2Int(element.GetAttribute("Camp"));
					
					
					if (!m_WarriorDataDic.ContainsKey(info.ID))
					{
						m_WarriorDataDic.Add(info.ID, info);
					}
				}
			}
		}
	}
}







