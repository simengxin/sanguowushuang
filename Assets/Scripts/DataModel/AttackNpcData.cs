using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class AttackNpcData  {
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
	/// 高度;
	/// </summary>
	public float Height;
	
	/// <summary>
	/// 半径;
	/// </summary>
	public float Radius;
	
	/// <summary>
	/// 警戒范围;
	/// </summary>
	public float Spring;
	
	/// <summary>
	/// 速度;
	/// </summary>
	public float Speed;
	
	/// <summary>
	/// 等级;
	/// </summary>
	public int Level;
	
	/// <summary>
	/// 最大血量;
	/// </summary>
	public int MaxHp;
	
	/// <summary>
	/// 攻击力;
	/// </summary>
	public int Damage;
	
	/// <summary>
	/// 普通攻击CD;
	/// </summary>
	public float NormalAttackCDTime;
	
	/// <summary>
	/// 技能列表, 第一个为普通技能;
	/// </summary>
	public int[] SkillArray;
}

public class AttackNpcDataManager
{
	private Dictionary<int, AttackNpcData> m_AttackNPCDataDic = null;
	
	/// <summary>
	/// 获取NPC数据;
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public AttackNpcData GetData(int key)
	{
		if (m_AttackNPCDataDic == null)
		{
			LoadNpcData();
		}
		
		return m_AttackNPCDataDic.ContainsKey(key) ? m_AttackNPCDataDic[key] : null;
	}
	
	/// <summary>
	/// 加载NPC数据;
	/// </summary>
	public void LoadNpcData()
	{
		m_AttackNPCDataDic = new Dictionary<int, AttackNpcData>();
		string textAsset = ResourcesManager.Instance.LoadConfigXML("AttackNpcData").text;
		
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(textAsset);
		XmlNode equipXN = xmlDoc.SelectSingleNode("AttackNpcDatas");
		
		XmlNodeList list = equipXN.ChildNodes;
		if (list != null && list.Count > 0)
		{
			foreach (XmlNode node in list)
			{
				XmlElement element = node as XmlElement;
				if (element.Name.Equals("AttackNpcData"))
				{
					AttackNpcData info = new AttackNpcData();
					
					info.ID = CommonHelper.Str2Int(element.GetAttribute("ID"));
					info.Name = element.GetAttribute("Name");
					info.Desc = element.GetAttribute("Desc");
					info.Model = element.GetAttribute("Model");
					info.Speed = CommonHelper.Str2Float(element.GetAttribute("Speed"));
					info.Radius = CommonHelper.Str2Float(element.GetAttribute("Radius"));
					info.Spring = CommonHelper.Str2Float(element.GetAttribute("Spring"));
					info.Height = CommonHelper.Str2Float(element.GetAttribute("Height"));
					info.Level = CommonHelper.Str2Int(element.GetAttribute("Level"));
					info.MaxHp = CommonHelper.Str2Int(element.GetAttribute("MaxHp"));
					info.Damage = CommonHelper.Str2Int(element.GetAttribute("Damage"));
					info.NormalAttackCDTime = CommonHelper.Str2Float(element.GetAttribute("NormalAttackCDTime"));
					info.SkillArray = CommonHelper.Str2IntArray(element.GetAttribute("SkillArray"));
					if (!m_AttackNPCDataDic.ContainsKey(info.ID))
					{
						m_AttackNPCDataDic.Add(info.ID, info);
					}
				}
			}
		}
	}
}
