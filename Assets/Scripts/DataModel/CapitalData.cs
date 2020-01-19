using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

//货币类型定义
public enum CapitalType
{
	CAPITAL_GOLD            = 1,  //金币
	CAPITAL_CASH            = 2,  //元宝
	
	CAPITAL_STONE           = 3,  //石头
	CAPITAL_WOOD            = 4,  //木头
	CAPITAL_CRYSTAL         = 5,  //水晶    
	
	CAPITAL_MAX             = 6,
};

/// <summary>
/// @道具数据;
/// </summary>
public class CapitalData
{
	public CapitalType ID;
	
	public string Name;
	
	public string Desc;

	public string Icon;
}

public class CapitalDataManager
{
	private Dictionary<CapitalType, CapitalData> m_CapitalDataDic = null;
	
	/// <summary>
	/// 技能数据;
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public CapitalData GetData(CapitalType key)
	{
		if (m_CapitalDataDic == null)
			LoadCapitalData();
		
		return m_CapitalDataDic.ContainsKey(key) ? m_CapitalDataDic[key] : null;
	}
	
	public void LoadCapitalData()
	{
		m_CapitalDataDic = new Dictionary<CapitalType, CapitalData>();
		string textAsset = ResourcesManager.Instance.LoadConfigXML("CapitalData").text;
		
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(textAsset);
		XmlNode equipXN = xmlDoc.SelectSingleNode("CapitalDatas");
		
		XmlNodeList list = equipXN.ChildNodes;
		if (list != null && list.Count > 0)
		{
			foreach (XmlNode node in list)
			{
				XmlElement element = node as XmlElement;
				if (element.Name.Equals("CapitalData"))
				{
					CapitalData info = new CapitalData();
					
					info.ID = (CapitalType)CommonHelper.Str2Int(element.GetAttribute("ID"));
					info.Name = element.GetAttribute("Name");
					info.Desc = element.GetAttribute("Desc");
					info.Icon = element.GetAttribute("Icon");
					
					if (!m_CapitalDataDic.ContainsKey(info.ID))
					{
						m_CapitalDataDic.Add(info.ID, info);
					}
				}
			}
		}
	}
}
