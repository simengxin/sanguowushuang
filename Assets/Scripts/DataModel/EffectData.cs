using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;


/// <summary>
/// @特效数据;
/// </summary>
public class EffectData
{
    public int ID
    {
        get;
        set;
    }

	public int EffectType
	{
		get;
		set;
	}

    public string ArtRes
    {
        get;
        set;
    }

    public string[] Dot
    {
        get;
        set;
    }

    public float LiveTime
    {
        get;
        set;
    }

    public bool Save
    {
        get;
        set;
    }

	public bool Bind
	{
		get;
		set;
	}

	public bool BindRotate
	{
		get;
		set;
	}


	public float Speed
	{
		get;
		set;
	}
	
	public float Radius
	{
		get;
		set;
	}
	
	public bool DestroyOnHit
	{
		get;
		set;
	}

	public string HitDot
	{
		get;
		set;
	}
	
	public string Audio
    {
        get;
        set;
    }
}

public class EffectDataManager
{
	private Dictionary<int, EffectData> m_EffectDataDic = null;
	
	/// <summary>
	/// 特效数据;
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public EffectData GetData(int key)
	{
		if (m_EffectDataDic == null)
			LoadEffectData();
		
		return m_EffectDataDic.ContainsKey(key) ? m_EffectDataDic[key] : null;
	}
	
	public void LoadEffectData()
	{
		m_EffectDataDic = new Dictionary<int, EffectData>();
		string textAsset = ResourcesManager.Instance.LoadConfigXML("EffectData").text;
		
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(textAsset);
		XmlNode equipXN = xmlDoc.SelectSingleNode("EffectDatas");
		
		XmlNodeList list = equipXN.ChildNodes;
		if (list != null && list.Count > 0)
		{
			foreach (XmlNode node in list)
			{
				XmlElement element = node as XmlElement;
				if (element.Name.Equals("EffectData"))
				{
					EffectData info = new EffectData();
					
					info.ID = CommonHelper.Str2Int(element.GetAttribute("ID"));
					info.EffectType = CommonHelper.Str2Int(element.GetAttribute("EffectType"));
					info.ArtRes = element.GetAttribute("ArtRes");
					info.Dot = CommonHelper.Str2StringArray(element.GetAttribute("Dot"));
					info.LiveTime = CommonHelper.Str2Float(element.GetAttribute("LiveTime"));
					info.Save = CommonHelper.Str2Boolean(element.GetAttribute("Save"));
					info.Bind = CommonHelper.Str2Boolean(element.GetAttribute("Bind"));
					info.BindRotate = CommonHelper.Str2Boolean(element.GetAttribute("BindRotate"));
					info.Speed = CommonHelper.Str2Float(element.GetAttribute("Speed"));
					info.Radius = CommonHelper.Str2Float(element.GetAttribute("Radius"));
					info.DestroyOnHit = CommonHelper.Str2Boolean(element.GetAttribute("DestroyOnHit"));
					info.HitDot = element.GetAttribute("HitDot");
					info.Audio = element.GetAttribute("Audio");

					if (!m_EffectDataDic.ContainsKey(info.ID))
					{
						m_EffectDataDic.Add(info.ID, info);
					}
				}
			}
		}
	}
}
