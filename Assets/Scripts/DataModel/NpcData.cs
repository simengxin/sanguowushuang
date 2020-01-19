using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class NpcData
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
	/// 描述
	/// </summary>
	public string Desc;
	
	/// <summary>
	/// 模型资源;
	/// </summary>
	public string Model;
	
	/// <summary>
	/// 等级;
	/// </summary>
	public int Level;
	
	/// <summary>
	/// 高度;
	/// </summary>
	public float Height;
	
	/// <summary>
	/// 半径;
	/// </summary>
	public float Radius;
	
	/// <summary>
	/// 移动速度;
	/// </summary>
	public float Speed;
}

public class NpcDataManager
{
	private Dictionary<int, NpcData> m_NPCDataDic = null;
	
	/// <summary>
	/// 获取NPC数据;
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public NpcData GetData(int key)
	{
		if (m_NPCDataDic == null)
		{
			LoadNpcData();
		}
		
		return m_NPCDataDic.ContainsKey(key) ? m_NPCDataDic[key] : null;
	}
	
	//获得字典
	public Dictionary<int, NpcData> GetDict()
	{
		return m_NPCDataDic;
	}
	
	/// <summary>
	/// 加载NPC数据;
	/// </summary>
	public void LoadNpcData()
	{
		m_NPCDataDic = new Dictionary<int, NpcData>();
		string textAsset = ResourcesManager.Instance.LoadConfigXML("NpcData").text;
		
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(textAsset);
		XmlNode equipXN = xmlDoc.SelectSingleNode("NpcDatas");
		
		XmlNodeList list = equipXN.ChildNodes;
		if (list != null && list.Count > 0)
		{
			foreach (XmlNode node in list)
			{
				XmlElement element = node as XmlElement;
				if (element.Name.Equals("NpcData"))
				{
					NpcData info = new NpcData();
					
					info.ID = CommonHelper.Str2Int(element.GetAttribute("ID"));
					info.Name = element.GetAttribute("Name");
					info.Desc = element.GetAttribute("Desc");
					info.Model = element.GetAttribute("Model");
					info.Level = CommonHelper.Str2Int(element.GetAttribute("Level"));
					info.Radius = CommonHelper.Str2Float(element.GetAttribute("Radius"));
					info.Height = CommonHelper.Str2Float(element.GetAttribute("Height"));
					info.Speed = CommonHelper.Str2Float(element.GetAttribute("Speed"));
					if (!m_NPCDataDic.ContainsKey(info.ID))
					{
						m_NPCDataDic.Add(info.ID, info);
					}
				}
			}
		}
	}
}
