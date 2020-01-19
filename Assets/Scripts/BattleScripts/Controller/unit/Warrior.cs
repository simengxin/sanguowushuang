using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Warrior : CombatUnit {
	protected WarriorData m_WarriorBaseData;
	public WarriorData WarriorBaseData{
		get{
			return m_WarriorBaseData;
		}
	}

	public virtual string Model{
		get{
			if(m_WarriorBaseData!= null){
				return m_WarriorBaseData.Model;
			}else{
				return "";
			}
		}
	}

	/// <summary>
	/// 卡牌;
	/// </summary>
	public virtual string Image
	{
		get
		{
			if (m_WarriorBaseData != null)
			{
				return m_WarriorBaseData.Image;
			}
			else
			{
				return "";
			}
		}
	}

	/// <summary>
	/// 头像;
	/// </summary>
	public virtual string Photo
	{
		get
		{
			if (m_WarriorBaseData != null)
			{
				return m_WarriorBaseData.Photo;
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
			if (m_WarriorBaseData != null)
			{
				return m_WarriorBaseData.Name;
			}
			else
			{
				return "";
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
			if (m_WarriorBaseData != null)
			{
				return m_WarriorBaseData.NormalAttackCDTime;
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
			if (m_WarriorBaseData != null)
			{
				return m_WarriorBaseData.Atk._base;
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
			if (m_WarriorBaseData != null)
			{
				return m_WarriorBaseData.Spring;
			}
			else
			{
				return 0;
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
			if (m_WarriorBaseData != null)
			{
				return m_WarriorBaseData.HP._base;
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
			if (m_WarriorBaseData != null)
			{
				return m_WarriorBaseData.Height;
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
			if (m_WarriorBaseData != null)
			{
				return m_WarriorBaseData.Radius;
			}
			else
			{
				return 0;
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
			if (m_WarriorBaseData != null)
			{
				return m_WarriorBaseData.Speed;
			}
			else
			{
				return 0;
			}
		}
	}
	
	/// <summary>
	/// 降临力回复速度速度;
	/// </summary>
	public int BpSpeed
	{
		get
		{
			if (m_WarriorBaseData != null)
			{
				return m_WarriorBaseData.BPSpeed;
			}
			else
			{
				return 0;
			}
		}
	}
	
	/// <summary>
	/// 最大降临力;
	/// </summary>
	protected int m_MaxBp = 100;
	public int MaxBP
	{
		get {return m_MaxBp;}
		set { m_MaxBp = value;}
	}
	
	/// <summary>
	/// 当前降临力;
	/// </summary>
	protected int m_Bp;
	public virtual int Bp
	{
		get {return m_Bp;}
		set 
		{ 
			m_Bp = value >= 0 ? value : 0;
			
			//if (OnBpChangeEvent != null)
			//{
			//	OnBpChangeEvent(this);
			//}
		}
	}

	// <summary>
	/// 是否释放过大招技能;
	/// </summary>
	public bool m_HasCastSuperSkill = false;
	
	/// <summary>
	/// 血量更改事件;
	/// </summary>
	public delegate void BpChangeEventHandler(Warrior unit);
	public event BpChangeEventHandler OnBpChangeEvent;
	
	/// <summary>
	/// 大招技能;
	/// </summary>
	public List<int> SuperSkillList = new List<int>();
	
	public Warrior(int dataId)
	{
		m_WarriorBaseData = DataManager.s_WarriorDataManager.GetData(dataId);
		if (m_WarriorBaseData == null)
		{
			Debug.LogError("WarriorData is null: " + dataId);
			return;
		}
		
		m_SkillList = new List<Skill>();
		for (int i = 0; i < m_WarriorBaseData.SkillArray.Length; i++)
		{
			Skill skill = new Skill(m_WarriorBaseData.SkillArray[i]);
			m_SkillList.Add(skill);
			if (skill.SkillType == SkillTypes.NormalSkill)
			{
				NormalSkillList.Add(skill.SkillId);
			}
			
			if (skill.SkillType == SkillTypes.AttackSkill)
			{ 
				AttackSkillList.Add(skill.SkillId);
			}
			
			if (skill.SkillType == SkillTypes.SuperSkill)
			{ 
				SuperSkillList.Add(skill.SkillId);
			}
		}
		
		Hp = MaxHp;
	}

	public Skill GetSuperSkill()
	{
		if (m_HasCastSuperSkill)
		{
			return null;
		}
		
		int bpNeed = 0;
		Skill result = null;
		for(int i = 0; i < SuperSkillList.Count; i++)
		{
			Skill skill = GetSkill(SuperSkillList[i]);
			if (skill.BpNeed <= this.Bp)
			{
				if (skill.BpNeed >= bpNeed)
				{
					result = skill;
					bpNeed = skill.BpNeed;
				}
			}
		}
		
		if (result != null)
		{
			m_HasCastSuperSkill = true;
		}
		
		return result;
	}
}
