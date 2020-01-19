
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

/// <summary>
/// @物品数据;
/// </summary>
public class ItemData
{
    // 物品ID
	public int ID
	{
		get;
		set;
	}

    // 物品名称
	public string Name
	{
		get;
		set;
	}
	
    // 物品类型
    public int ItemType
    {
        get;
        set;
    }

    // 描述
	public string Desc
	{
		get;
		set;
	}
	
    // 图标
	public string Icon
	{
		get;
		set;
	}

    // 是否不可卖
    public int CantSell
	{
		get;
		set;
	}
	
    // 售卖货币类型
    public int SellType
	{
		get;
		set;
	}

    // 价格
    public int Price
	{
		get;
		set;
	}
}

public class ItemDataManager
{
	private Dictionary<int, ItemData> m_ItemDataDic = null;
	
	/// <summary>
	/// 技能数据;
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public ItemData GetData(int key)
	{
		if (m_ItemDataDic == null)
			LoadItemData();
		
		return m_ItemDataDic.ContainsKey(key) ? m_ItemDataDic[key] : null;
	}
	
	public void LoadItemData()
	{
		m_ItemDataDic = new Dictionary<int, ItemData>();
		string textAsset = ResourcesManager.Instance.LoadConfigXML("ItemData").text;
		
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(textAsset);
		XmlNode equipXN = xmlDoc.SelectSingleNode("Object");
		
		XmlNodeList list = equipXN.ChildNodes;
		if (list != null && list.Count > 0)
		{
			foreach (XmlNode node in list)
			{
				XmlElement element = node as XmlElement;
                if (null != element && element.Name.Equals("Property"))
				{
					ItemData info = new ItemData();
                    					
					info.ID = CommonHelper.Str2Int(element.GetAttribute("ID"));
					info.Name = element.GetAttribute("Name");
                    info.ItemType = CommonHelper.Str2Int(element.GetAttribute("ItemType"));
					info.Desc = element.GetAttribute("Desc");
					info.Icon = element.GetAttribute("Icon");  
                    info.CantSell = CommonHelper.Str2Int(element.GetAttribute("CantSell"));
                    info.SellType = CommonHelper.Str2Int(element.GetAttribute("SellType"));
                    info.Price = CommonHelper.Str2Int(element.GetAttribute("Price"));				
					
					if (!m_ItemDataDic.ContainsKey(info.ID))
					{
						m_ItemDataDic.Add(info.ID, info);
					}
				}
			}
		}
	}
}