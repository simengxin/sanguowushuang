using UnityEngine;
using System.Collections;

public class Npc : BaseUnit
{
    /// <summary>
    /// NPC基础数据;
    /// </summary>
    protected NpcData m_NPCBaseData;
	public NpcData NPCBaseData
    {
        get { return m_NPCBaseData; }
    }

	/// <summary>
	/// 模型包;
	/// </summary>
	public virtual string Model
	{
		get
		{
			if (m_NPCBaseData != null)
			{
				return m_NPCBaseData.Model;
			}
			else
			{
				return "";
			}
		}
	}

	/// <summary>
	/// 名字;
	/// </summary>
	public override string Name
	{
		get
		{
			if (m_NPCBaseData != null)
			{
				return m_NPCBaseData.Name;
			}
			else
			{
				return "";
			}
		}
	}

	/// <summary>
	/// 等级;
	/// </summary>
	public override int Level
	{
		get
		{
			if (m_NPCBaseData != null)
			{
				return m_NPCBaseData.Level;
			}
			else
			{
				return 0;
			}
		}
	}

	/// <summary>
	/// 高度;
	/// </summary>
	public override float Height
	{
		get
		{
			if (m_NPCBaseData != null)
			{
				return m_NPCBaseData.Height;
			}
			else
			{
				return 0;
			}
		}
	}

	/// <summary>
	/// 半径;
	/// </summary>
	public override float Radius
	{
		get
		{
			if (m_NPCBaseData != null)
			{
				return m_NPCBaseData.Radius;
			}
			else
			{
				return 0;
			}
		}
	}

	public Npc(int dataId)
    {
		m_NPCBaseData = DataManager.s_NpcDataManager.GetData(dataId);
		if (m_NPCBaseData == null)
		{
			Debug.LogError("NPCData is null: " + dataId);
			return;
		}
    }
}
