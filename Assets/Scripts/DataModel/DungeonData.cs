using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.IO;


public enum BattleTypes
{
	DragPosition = 0,  // 拖动降临
	FixedPosition = 1, // 固定地点降临
};

public enum NpcTypes
{
	Battle = 0,  	// 战斗类型
	TreasureBox = 1,// 宝箱
	Crystal = 2,  	// 水晶
	Stone = 3,		// 石头
	Wood = 4,		// 木头
};

public class DungeonData
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
	/// 描述;
	/// </summary>
	public string Desc;

	/// <summary>
	/// 场景Id;
	/// </summary>
	public int SceneId;

	/// <summary>
	/// 主将出生位置;
	/// </summary>
	public Vector3 Position;
	public float Orient;

	/// <summary>
	/// 资源产出;
	/// </summary>
	public int[] CapitalProducts;

	/// <summary>
	/// 道具产出;
	/// </summary>
	public int[] ItemProducts;

	/// <summary>
	/// 关卡npc数据;
	/// </summary>
	public List<StageData> StageDataList;
}

public class StageData
{
	/// <summary>
	/// 数据编号;
	/// </summary>
	public int ID;

	/// <summary>
	/// 生成器编号;
	/// </summary>
	public int NpcID;

	/// <summary>
	/// Npc类型;
	/// </summary>
	public NpcTypes NpcType;

	/// <summary>
	/// 位置;
	/// </summary>
	public Vector3 Position;
	public float Orient;

	/// <summary>
	/// 是否boss战
	/// </summary>
	public int IsBossBattle;

	/// <summary>
	/// 关卡npc战斗包数据;
	/// </summary>
	public List<BattleDataByTime> BattleDatasList;
}

public class BattleDataByTime
{
	/// <summary>
	/// 时间编号(秒为单位);
	/// </summary>
	public float GrowthTime;
	
	/// <summary>
	/// 战斗包编号;
	/// </summary>
	public int BattleID;
	
	/// <summary>
	/// 战斗类型;
	/// </summary>
	public BattleTypes BattleType;

	/// <summary>
	/// 掉落;
	/// </summary>
	public int Loot;
};

public class DungeonDataManager
{
	private List<string> m_DungeonDataComment = new List<string>();
	private Dictionary<int, DungeonData> m_DungeonDataDic = null;

	/// <summary>
	/// 获取副本数据;
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public DungeonData GetData(int key)
	{
		if (m_DungeonDataDic == null)
		{
			LoadDungeonData();
		}
		
		return m_DungeonDataDic.ContainsKey(key) ? m_DungeonDataDic[key] : null;
	}

	/// <summary>
	/// 获取所有副本数据;
	/// </summary>
	/// <returns></returns>
	public List<DungeonData> GetAll()
	{
		if (m_DungeonDataDic == null)
		{
			LoadDungeonData();
		}

		return new List<DungeonData>(m_DungeonDataDic.Values);
	}


    /// <summary>
    /// 修改对应场景的npc数据
    /// </summary>
    /// <param name="sceneid"></param>
    /// <param name="arrStage"></param>
    public void SetDungeonDataList(int sceneid, StageData[] arrStage)
    {
        DungeonData dungeon = GetDataByScene(sceneid);

        //清空旧的
        dungeon.StageDataList.Clear();

        //写入新的
        dungeon.StageDataList.AddRange(arrStage);

        //保存整个xml
        SaveDungeonData();
    }

    /// <summary>
    /// 根据场景ID获取npc数据
    /// </summary>
    /// <param name="sceneid"></param>
    public DungeonData GetDataByScene(int sceneid)
    {
        if (m_DungeonDataDic == null)
        {
            LoadDungeonData();
        }

        foreach(DungeonData data in m_DungeonDataDic.Values)
        {
            if(data.SceneId == sceneid)
            {
                return data;
            }
        }

        return null;
    }

    /// <summary>
    /// 保存npc数据
    /// </summary>
    public void SaveDungeonData()
    {
        FileStream ms = new FileStream(
            "Assets/Resources/Config/" +
			"DungeonData" + ".xml",
            FileMode.OpenOrCreate);
        XmlWriterSettings settings = new XmlWriterSettings(); 

        //要求缩进 
        settings.Indent = true; 
        //注意如果不设置encoding默认将输出utf-16
       
        //注意这儿不能直接用Encoding.UTF8如果用Encoding.UTF8将在输出文本的最前面添加4个字节的非xml内容 
        settings.Encoding = new UTF8Encoding(false); 

        //设置换行符
        settings.NewLineChars = "\r\n"; 

        XmlWriter xmlWriter = XmlWriter.Create(ms, settings);

        //写xml文件开始<?xml version="1.0" encoding="utf-8" ?> 
        xmlWriter.WriteStartDocument(false);

        //写注释
        WriteComment(xmlWriter);

        //写内容
        WriteDungeonDatas(xmlWriter);
        
        xmlWriter.WriteEndDocument();

        xmlWriter.Flush();

    }

    /// <summary>
    /// 写注释
    /// </summary>
    /// <param name="xmlWriter"></param>
    private void WriteComment(XmlWriter xmlWriter)
    {
		for( int i = 0; i < m_DungeonDataComment.Count; i++ )
		{
			xmlWriter.WriteComment(m_DungeonDataComment[i]);
		}
        //xmlWriter.WriteComment(" 被2011 rel. 2 sp1 (wangxm) 使用XMLSpy 编辑的 (http://www.altova.com) by ");
        //xmlWriter.WriteComment("【DungeonData：副本数据】【SceneId:场景Id】【Name:名字】【Desc：描述】 【CapitalProducts：副本可获得资源】【ItemProducts：副本可获得道具】 【Pos： 主将出生坐标】【Orient：朝向】");
        //xmlWriter.WriteComment("【StageData：关卡数据】 【NpcID: 关卡NpcId】【NpcType：0 是怪物， 1是宝箱】【Pos：怪物所在坐标】");
        //xmlWriter.WriteComment("【GrowthTime：经过X时间后，调用新的战斗包】【BattleId：战斗包Id】【BattleType：战斗类型1为正常出现，0为丢将】【Loot：奖励ID】");
    }

    /// <summary>
    /// 写副本数据
    /// </summary>
    /// <param name="xmlWriter"></param>
    private void WriteDungeonDatas(XmlWriter xmlWriter)
    {
        xmlWriter.WriteStartElement("DungeonDatas");
        
        foreach(DungeonData data in m_DungeonDataDic.Values)
        {
              WriteDungeonData(xmlWriter,data);
        }     

        xmlWriter.WriteEndElement();
    }

    /// <summary>
    /// 写单个场景信息
    /// </summary>
    /// <param name="xmlWriter"></param>
    /// <param name="data"></param>
    private void WriteDungeonData(XmlWriter xmlWriter,DungeonData data)
    {
        xmlWriter.WriteStartElement("DungeonData");

        xmlWriter.WriteAttributeString("ID", data.ID.ToString());
        xmlWriter.WriteAttributeString("Name", data.Name.ToString());
        xmlWriter.WriteAttributeString("Desc", data.Desc.ToString());
        xmlWriter.WriteAttributeString("SceneId", data.SceneId.ToString());
        xmlWriter.WriteAttributeString("PosX", data.Position.x.ToString());
        xmlWriter.WriteAttributeString("PosY", data.Position.y.ToString());
        xmlWriter.WriteAttributeString("PosZ", data.Position.z.ToString());
        xmlWriter.WriteAttributeString("Orient", data.Orient.ToString());
        xmlWriter.WriteAttributeString("CapitalProducts", CommonHelper.Array2Str(data.CapitalProducts));
        xmlWriter.WriteAttributeString("ItemProducts", CommonHelper.Array2Str(data.ItemProducts));

		if( data.StageDataList != null )
		{
	        foreach (StageData stageData in data.StageDataList)
	        {
	            WriteStageData(xmlWriter, stageData);
	        }   
		}

        xmlWriter.WriteEndElement();
    }

    /// <summary>
    /// 写单个npc信息
    /// </summary>
    /// <param name="xmlWriter"></param>
    /// <param name="data"></param>
    private void WriteStageData(XmlWriter xmlWriter, StageData data)
    {
        xmlWriter.WriteStartElement("StageData");

        xmlWriter.WriteAttributeString("ID", data.ID.ToString());
        xmlWriter.WriteAttributeString("NpcID", data.NpcID.ToString());
        xmlWriter.WriteAttributeString("NpcType", ((int)data.NpcType).ToString());
        xmlWriter.WriteAttributeString("PosX", data.Position.x.ToString());
        xmlWriter.WriteAttributeString("PosY", data.Position.y.ToString());
        xmlWriter.WriteAttributeString("PosZ", data.Position.z.ToString());
        xmlWriter.WriteAttributeString("Orient", data.Orient.ToString());
        xmlWriter.WriteAttributeString("IsBossBattle", data.IsBossBattle.ToString());

        foreach (BattleDataByTime battleData in data.BattleDatasList)
        {
            WriteBattleData(xmlWriter, battleData);
        }  

        xmlWriter.WriteEndElement();
    }
	
    /// <summary>
    /// 写战斗增长数据
    /// </summary>
    /// <param name="xmlWriter"></param>
    /// <param name="data"></param>
    private void WriteBattleData(XmlWriter xmlWriter, BattleDataByTime data)
    {
        xmlWriter.WriteStartElement("BattleData");

        xmlWriter.WriteAttributeString("GrowthTime", data.GrowthTime.ToString());
        xmlWriter.WriteAttributeString("BattleID", data.BattleID.ToString());
        xmlWriter.WriteAttributeString("BattleType", ((int)data.BattleType).ToString());
        xmlWriter.WriteAttributeString("Loot", data.Loot.ToString());

        xmlWriter.WriteEndElement();
    }

	/// <summary>
	/// 加载NPC数据;
	/// </summary>
	public void LoadDungeonData()
	{
		m_DungeonDataDic = new Dictionary<int, DungeonData>();
		string textAsset = ResourcesManager.Instance.LoadConfigXML("DungeonData").text;

//		XmlReaderSettings settings = new XmlReaderSettings();
//		settings.ConformanceLevel = ConformanceLevel.Fragment;
//		settings.IgnoreWhitespace = false;
//		settings.IgnoreComments = false;
//		XmlReader reader = XmlReader.Create(textAsset, settings);
		//Debug.Log(textAsset);

		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(textAsset);

		m_DungeonDataComment.Clear();
		XmlNodeList nodeList = xmlDoc.ChildNodes;
		foreach( XmlNode node in nodeList )
		{
			switch( node.NodeType )
			{
			case XmlNodeType.Comment:
				//Debug.Log("Commnent: " + node.Value);
				m_DungeonDataComment.Add(node.Value);
				break;
//			case XmlNodeType.Element:
//				Debug.Log("Element: " + node.Name);
//				break;
//			case XmlNodeType.Text:
//				Debug.Log("Text: " + node.Value);
//				break;
//			case XmlNodeType.XmlDeclaration:
//				Debug.Log("XmlDeclaration: " + node.Name + " " + node.Value);
//				break;
			default:
				break;
			}
		}

		XmlNode datas = xmlDoc.SelectSingleNode("DungeonDatas");
		
		XmlNodeList list = datas.ChildNodes;
		if (list == null || list.Count < 1)
		{
			return ;
		}

		foreach (XmlNode node in list)
		{
			XmlElement element = node as XmlElement;

			if (!element.Name.Equals("DungeonData"))
			{
				continue ;
			}
			DungeonData info = new DungeonData();
			
			info.ID = CommonHelper.Str2Int(element.GetAttribute("ID"));
			info.Name = element.GetAttribute("Name");
			info.Desc = element.GetAttribute("Desc");
			info.SceneId = CommonHelper.Str2Int(element.GetAttribute("SceneId"));
			float x = CommonHelper.Str2Float(element.GetAttribute("PosX"));
			float y = CommonHelper.Str2Float(element.GetAttribute("PosY"));
			float z = CommonHelper.Str2Float(element.GetAttribute("PosZ"));
			info.Position = new Vector3(x, y, z);
			info.Orient = CommonHelper.Str2Float(element.GetAttribute("Orient"));
			info.CapitalProducts = CommonHelper.Str2IntArray(element.GetAttribute("CapitalProducts"));
			info.ItemProducts = CommonHelper.Str2IntArray(element.GetAttribute("ItemProducts"));
			
			XmlNodeList data = element.ChildNodes;
			if (data == null )
			{
				continue ;
			}

			foreach (XmlNode subNode in data)
			{
				XmlElement subElement = subNode as XmlElement;
				if (!subElement.Name.Equals("StageData"))
				{
					continue ;
				}
				StageData stageData = new StageData();
				
				stageData.ID			= CommonHelper.Str2Int(subElement.GetAttribute("ID"));
				stageData.NpcID			= CommonHelper.Str2Int(subElement.GetAttribute("NpcID"));
				stageData.NpcType			= (NpcTypes)CommonHelper.Str2Int(subElement.GetAttribute("NpcType"));
				float subx			= CommonHelper.Str2Float(subElement.GetAttribute("PosX"));
				float suby			= CommonHelper.Str2Float(subElement.GetAttribute("PosY"));
				float subz			= CommonHelper.Str2Float(subElement.GetAttribute("PosZ"));
				stageData.Position = new Vector3(subx, suby, subz);
				stageData.Orient		= CommonHelper.Str2Float(subElement.GetAttribute("Orient"));
				stageData.IsBossBattle	= CommonHelper.Str2Int(subElement.GetAttribute("IsBossBattle"));
				
				XmlNodeList battledata = subElement.ChildNodes;
				if (battledata == null )
				{
					continue ;
				}

				if (stageData.BattleDatasList == null)
				{
					stageData.BattleDatasList = new List<BattleDataByTime>();
				}
				foreach (XmlNode nextNode in battledata)
				{
					XmlElement nextElement = nextNode as XmlElement;
					if (!nextElement.Name.Equals("BattleData"))
					{
						continue;
					}

					BattleDataByTime battleData = new BattleDataByTime();

					battleData.GrowthTime = CommonHelper.Str2Float(nextElement.GetAttribute("GrowthTime"));
					battleData.BattleID = CommonHelper.Str2Int(nextElement.GetAttribute("BattleID"));
					battleData.BattleType = (BattleTypes)CommonHelper.Str2Int(nextElement.GetAttribute("BattleType"));
					battleData.Loot		= CommonHelper.Str2Int(nextElement.GetAttribute("Loot"));

					//加入战斗包信息
					stageData.BattleDatasList.Add(battleData);
				}

				stageData.BattleDatasList.Sort(delegate (BattleDataByTime a, BattleDataByTime b){
					if (a.GrowthTime < b.GrowthTime) return -1;
					if (a.GrowthTime > b.GrowthTime) return 1;
					return 0;
				});

				//加入npc信息
				if (info.StageDataList == null)
				{
					info.StageDataList = new List<StageData>();
				}
				info.StageDataList.Add(stageData);
			}

			//加入副本信息
			if (!m_DungeonDataDic.ContainsKey(info.ID))
			{
				m_DungeonDataDic.Add(info.ID, info);
			}
		}
	}
}

