using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 战斗Npc基类;
/// </summary>
public class AttackNpc : CombatUnit {
	/// <summary>
	/// NPC基础数据;
	/// </summary>
	protected AttackNpcData m_AttackNPCBaseData;
	public AttackNpcData AttackNpcBaseData{
		get{return m_AttackNPCBaseData;}
	}

	/// <summary>
	/// 模型包;
	/// </summary>
	public virtual string Model
	{
		get
		{
			if (m_AttackNPCBaseData != null)
			{
				return m_AttackNPCBaseData.Model;
			}
			else
			{
				return "";
			}
		}
	}
	
	/// <summary>
	/// 移动速度;
	/// </summary>
	public override float Speed
	{
		get
		{
			if (m_AttackNPCBaseData != null)
			{
				return m_AttackNPCBaseData.Speed;
			}
			else
			{
				return 0;
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
			if (m_AttackNPCBaseData != null)
			{
				return m_AttackNPCBaseData.Name;
			}
			else
			{
				return "";
			}
		}
	}
	
	/// <summary>
	/// 最大血量;
	/// </summary>
	public override int MaxHp
	{
		get
		{
			if (m_AttackNPCBaseData != null)
			{
				return m_AttackNPCBaseData.MaxHp;
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
			if (m_AttackNPCBaseData != null)
			{
				return m_AttackNPCBaseData.Height;
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
			if (m_AttackNPCBaseData != null)
			{
				return m_AttackNPCBaseData.Radius;
			}
			else
			{
				return 0;
			}
		}
	}
	
	/// <summary>
	/// 敌人触发范围;
	/// </summary>
	public override float Spring
	{
		get
		{
			if (m_AttackNPCBaseData != null)
			{
				return m_AttackNPCBaseData.Spring;
			}
			else
			{
				return 0;
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
			if (m_AttackNPCBaseData != null)
			{
				return m_AttackNPCBaseData.Level;
			}
			else
			{
				return 0;
			}
		}
	}
	
	/// <summary>
	/// 普通攻击CD;
	/// </summary>
	public override float NormalAttackCDTime
	{
		get
		{
			if (m_AttackNPCBaseData != null)
			{
				return m_AttackNPCBaseData.NormalAttackCDTime;
			}
			else
			{
				return 0;
			}
		}
	}
	
	/// <summary>
	/// 伤害;
	/// </summary>
	public override int Damage
	{
		get
		{
			if (m_AttackNPCBaseData != null)
			{
				return m_AttackNPCBaseData.Damage;
			}
			else
			{
				return 0;
			}
		}
	}
	
	public AttackNpc(int dataId)
	{
		m_AttackNPCBaseData = DataManager.s_AttackNpcDataManager.GetData(dataId);
		if (m_AttackNPCBaseData == null)
		{
			Debug.LogError("AttackNPCData is null: " + dataId);
			return;
		}
		
		m_SkillList = new List<Skill>();
		for (int i = 0; i < m_AttackNPCBaseData.SkillArray.Length; i++)
		{
			Skill skill = new Skill(m_AttackNPCBaseData.SkillArray[i]);
			m_SkillList.Add(skill);
			if (skill.SkillType == SkillTypes.NormalSkill)
			{
				NormalSkillList.Add(skill.SkillId);
			}
			else if (skill.SkillType == SkillTypes.AttackSkill)
			{ 
				AttackSkillList.Add(skill.SkillId);
			}
		}
		
		Hp = MaxHp;
	}
}
