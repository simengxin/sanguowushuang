using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 战斗对象基类;
/// </summary>
public class CombatUnit : BaseUnit {
	/// <summary>
	/// 最大血量;
	/// </summary>
	protected int m_MaxHp = 0;
	public virtual int MaxHp
	{
		get {return m_MaxHp;}
		set { m_MaxHp = value;}
	}
	
	/// <summary>
	/// 当前血量;
	/// </summary>
	protected int m_Hp;
	public virtual int Hp
	{
		get {return m_Hp;}
		set 
		{ 
			m_Hp = value >= 0 ? value : 0;
			if (OnHpChangeEvent != null)
			{
				OnHpChangeEvent(this);
			}
			
			if (Hp == 0)
			{
				if (OnDeadEvent != null)
				{
					OnDeadEvent(this);
				}
			}
		}
	}
	
	/// <summary>
	/// 攻击力;
	/// </summary>
	protected int m_Damage;
	public virtual int Damage
	{
		get {return m_Damage;}
		set { m_Damage = value;}
	}
	
	/// <summary>
	/// 敌人触发范围;
	/// </summary>
	protected float m_Spring = 100;
	public virtual float Spring
	{
		get {return m_Spring;}
		set { m_Spring = value;}
	}
	
	/// <summary>
	/// 普通攻击CD;
	/// </summary>
	protected float m_NormalAttackCDTime = 0;
	public virtual float NormalAttackCDTime
	{
		get {return m_NormalAttackCDTime;}
		set { m_NormalAttackCDTime = value;}
	}
	
	/// <summary>
	/// 能否攻击;
	/// </summary>
	public int CantAttack = 1;
	
	/// <summary>
	/// 能否被攻击;
	/// </summary>
	public int CantbeAttack = 1;
	
	/// <summary>
	/// 技能实例;
	/// </summary>
	protected List<Skill> m_SkillList = null;
	public List<Skill> SkillList
	{
		get {return m_SkillList;}
	}
	
	/// <summary>
	/// 普通攻击技能;
	/// </summary>
	public List<int> NormalSkillList = new List<int>();
	
	/// <summary>
	/// 攻击技能;
	/// </summary>
	public List<int> AttackSkillList = new List<int>();
	
	/// <summary>
	/// 出生事件;
	/// </summary>
	public delegate void SpawnEventHandler(CombatUnit unit);
	public event SpawnEventHandler OnSpawnEvent;
	
	/// <summary>
	/// 死亡事件;
	/// </summary>
	public delegate void DeadEventHandler(CombatUnit unit);
	public event DeadEventHandler OnDeadEvent;
	
	/// <summary>
	/// 血量更改事件;
	/// </summary>
	public delegate void HpChangeEventHandler(CombatUnit unit);
	public event HpChangeEventHandler OnHpChangeEvent;
	
	/// <summary>
	/// 获得技能;
	/// </summary>
	public Skill GetSkill(int id)
	{
		if (m_SkillList != null && m_SkillList.Count > 0)
		{
			for (int i = 0; i < m_SkillList.Count; i++)
			{
				if (m_SkillList[i].SkillId == id)
				{
					return m_SkillList[i];
				}
			}
		}
		return null;
	}
	
	/// <summary>
	/// 获得普通攻击技能;
	/// </summary>
	public Skill GetRandomNormalSkill()
	{
		List<object> allSkill = new List<object>();
		for (int i = 0; i < NormalSkillList.Count; i++)
		{
			Skill skill = GetSkill(NormalSkillList[i]);
			allSkill.Add(skill);
		}
		
		return CommonHelper.RandomObject(allSkill) as Skill;
	}
	
	/// <summary>
	/// 自动获得可以释放的技能;
	/// </summary>
	public Skill GetAutoSkillId()
	{
		List<object> allSkill = new List<object>();
		for (int i = 0; i < AttackSkillList.Count; i++)
		{
			Skill skill = GetSkill(AttackSkillList[i]);
			if (!skill.CoolDown)
			{
				allSkill.Add(skill);
			}
		}
		
		return CommonHelper.RandomObject(allSkill) as Skill;
	}
	
	public  void Init()
	{
	}
	
	public  void Destroy()
	{    
	}
	
	public void OnSpawn()
	{
		if (OnSpawnEvent != null)
		{
			OnSpawnEvent(this);
		}
	}
}
