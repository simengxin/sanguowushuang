using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SkillTypes{
	BornSkill = 0, 		//降临技能
	NormalSkill = 1, 	// 普通攻击技能
	AttackSkill = 2,	// 攻击技能
	SuperSkill  = 3, 	// 大招
}

public enum SkillHitTypes
{   //HitType: 击中类型(0:对敌方目标 1:敌方范围 2:友方目标 3:友方范围 4:自己);
	SelectedEnemy = 0,
	ScaleEnemy = 1,
	SelectedFriend = 2,
	ScaleFriend = 3,
	Self = 4,
};
public enum SkillHitSharpTypes
{//HitSharpType: 攻击范围类型(0：对目标 1：圆形 2：扇形);
	None = 0,
	Circle = 1,
	Fan = 2,
};

public enum SkillEffectTypes
{
	ImmedDamage = 0,				// 立即伤害
	ImmedHeal = 1,					// 治疗
	AddBuff = 2, 				// 添加buff
	Sneer = 3, 						// 嘲讽
	HitDown = 4,				// 击倒
	HitOut = 5,				// 击飞
};



public class Skill  {
	//public float AttackDist;

	public int SkillId ;
	private SkillData  m_SkillBaseData;
	public SkillData BaseData{
		get{
			return m_SkillBaseData;
		}
	}

	public string SkillName{
		get{return m_SkillBaseData.Name;}
	}

	public SkillTypes SkillType{
		get{return (SkillTypes)m_SkillBaseData.SkillType;}

	}
	public string Icon{
		get{return m_SkillBaseData.Icon;}
	}
	public bool CoolDown = false;
	public float CDTime{
		get{return m_SkillBaseData.CDTime;}
	}
	public float AttackDist{
		get{return m_SkillBaseData.AttackDist;}
	}
	public int HitNum{
		get{return m_SkillBaseData.HitNum;}
	}
	public float AttackRadius{
		get{return m_SkillBaseData.AttackRadius;}
	}
	public float AttackAngel{
		get{return m_SkillBaseData.AttackAngle;}
	}
	public SkillHitTypes HitType{
		get{return (SkillHitTypes)m_SkillBaseData.HitType;}
	}
	public SkillHitSharpTypes HitSharpType{
		get{return (SkillHitSharpTypes)m_SkillBaseData.HitSharpType;}
	}
	public int BpNeed{
		get{return m_SkillBaseData.BpNeed;}
	}
	public List<SkillEffectData> EffectList{
		get{return m_SkillBaseData.EffectList;}
	}
	public float AttackAngle
	{
		get {return m_SkillBaseData.AttackAngle;}
	}

	public Skill(int dataId){
		SkillId = dataId;
		m_SkillBaseData = DataManager.s_SkillDataManager.GetData(dataId); 
		if (m_SkillBaseData == null) {
			Debug.LogError("SkillData is null,dataID="+dataId);
			return ;
		}
		//return m_SkillBaseData;
	}

	private GameObject m_SkillCaster= null;
	private GameObject m_SkillTarget = null;
	public void SetSkillCaster(GameObject m,GameObject tar){
		m_SkillCaster = m;
		m_SkillTarget = tar;
	}

	public void SetCD(){
		CoolDown = true;
		TimerManager.Instance.AddTimer ("Skill" + m_SkillCaster.gameObject.name + SkillId.ToString (), CDTime, ResetCD, null);
	}

	void ResetCD(params object[] args){
		CoolDown = false;
	}

	/// <summary>
	/// 击中目标;
	/// </summary>
	public void HitTarget(GameObject target, int skillEffectIndex = -1)
	{
		if (m_SkillCaster == null || target == null)
		{
			return;
		}
		
		CombatUnitController attacker = m_SkillCaster.GetComponent<CombatUnitController>();
		CombatUnitController unit = target.GetComponent<CombatUnitController>();
		
		if (skillEffectIndex == -1)
		{
			for (int i = 0; i < EffectList.Count; i ++)
			{
				SkillEffectData effect = EffectList[i];
				ExecuteEffect(effect, unit, attacker);
			}
		}
		else
		{
			if (skillEffectIndex >= 0 && skillEffectIndex < EffectList.Count)
			{
				SkillEffectData effect = EffectList[skillEffectIndex];
				ExecuteEffect(effect, unit, attacker);
			}
		}
	}
	private void ExecuteEffect(SkillEffectData effect, CombatUnitController unit, CombatUnitController attacker)
	{
		switch((SkillEffectTypes)effect.Type)
		{
		case SkillEffectTypes.ImmedDamage:
			ImmedDamage(unit, attacker, effect);
			break;
		case SkillEffectTypes.ImmedHeal:
			ImmedHeal(unit, attacker, effect);
			break;
		case SkillEffectTypes.AddBuff:
			AddBuff(unit, attacker, effect);
			break;
		case SkillEffectTypes.Sneer:
			Sneer(unit, attacker);
			break;
		case SkillEffectTypes.HitDown:
			HitDown(unit, attacker, effect);
			break;
		case SkillEffectTypes.HitOut:
			HitOut(unit, attacker, effect);
			break;
		default:
			break;
		}
	}

	/// <summary>
	/// 立即伤害效果;
	/// </summary>
	private void ImmedDamage(CombatUnitController unit, CombatUnitController attacker, SkillEffectData effect)
	{
		if (unit != null)
		{
			unit.OnDamaged(attacker, effect.Para1, effect.Para2, effect.Para3, effect.HitEffect);
		}
	}

	/// <summary>
	/// 立即治疗效果;
	/// </summary>
	private void ImmedHeal(CombatUnitController unit, CombatUnitController attacker, SkillEffectData effect)
	{
		if (unit != null)
		{
			unit.OnHeal(attacker, (int)effect.Para1, effect.HitEffect);
		}
	}
	
	/// <summary>
	/// 添加Buff;
	/// </summary>
	private void AddBuff(CombatUnitController unit, CombatUnitController attacker, SkillEffectData effect)
	{
		if (unit != null)
		{
			unit.OnAddBuff(attacker, (int)effect.Para1);
		}
	}
	
	/// <summary>
	/// 讥讽效果;
	/// </summary>
	private void Sneer(CombatUnitController unit, CombatUnitController attacker)
	{
		if (unit != null)
		{
			unit.OnSneer(attacker);
		}
	}
	
	/// <summary>
	/// 击倒效果;
	/// </summary>
	private void HitDown(CombatUnitController unit, CombatUnitController attacker, SkillEffectData effect)
	{
		if (unit != null)
		{
			unit.OnHitDown(attacker, (float)effect.Para1, (float)effect.Para2);
		}
	}
	
	/// <summary>
	/// 击飞效果;
	/// </summary>
	private void HitOut(CombatUnitController unit, CombatUnitController attacker, SkillEffectData effect)
	{
		if (unit != null)
		{
			unit.OnHitOut(attacker, (float)effect.Para1, (float)effect.Para2);
		}
	}
	public void SkillHit(int skillEffectIndex){
		if (m_SkillCaster == null)
		{
			return;
		}
		
		CombatUnitController attacker = m_SkillCaster.GetComponent<CombatUnitController>();
		
		switch (HitType)
		{
		case SkillHitTypes.SelectedEnemy:
		case SkillHitTypes.SelectedFriend:
		case SkillHitTypes.Self:
		{
			HitTarget(m_SkillTarget, skillEffectIndex);
		}
			break;
		case SkillHitTypes.ScaleEnemy:
		case SkillHitTypes.ScaleFriend:
		{
			switch (HitSharpType)
			{
			case SkillHitSharpTypes.Circle:
				HitTargetsInCircle(attacker, skillEffectIndex);
				break;
			case SkillHitSharpTypes.Fan:
				HitTargetsInFan(attacker, skillEffectIndex);
				break;
			default:
				break;
			}
		}
			break;
		default:
			break;
		}

	}

	/// <summary>
	/// 查询扇形范围目标;
	/// </summary>
	private void HitTargetsInFan(CombatUnitController attacker, int skillEffectIndex)
	{
		int count = 0;
		Vector3 dir = m_SkillCaster.transform.forward;
		Collider[] colliders = Physics.OverlapSphere(m_SkillCaster.transform.position,  AttackRadius + 0.1f, TargetLayerMask);
		
		foreach (Collider c in colliders)
		{
			CombatUnitController unit = c.GetComponent<CombatUnitController>();
			if (unit != null)
			{
				Vector3 testDir = c.transform.position - m_SkillCaster.transform.position;
				testDir.y = 0;
				if (testDir == Vector3.zero)
				{
					continue;
				}
				
				testDir.Normalize();
				
				float dotProduct = Vector3.Dot(testDir, dir);
				
				if (dotProduct > 0)
				{
					float angleCos = Mathf.Cos(AttackAngle/2.0f);
					if (dotProduct >= angleCos)
					{
						if (HitNum > -1)
						{
							if (count >= HitNum)
							{
								break;
							}
						}
						HitTarget(unit.gameObject, skillEffectIndex);
						count ++;
					}
				}
			}
		}		
	}
	
	/// <summary>
	/// 查询圆形范围目标;
	/// </summary>
	private void HitTargetsInCircle(CombatUnitController attacker, int skillEffectIndex)
	{
		int count = 0;
		Collider[] colliders = Physics.OverlapSphere(m_SkillCaster.transform.position, AttackRadius + 0.1f, TargetLayerMask);
		
		foreach (Collider c in colliders)
		{
			CombatUnitController unit = c.GetComponent<CombatUnitController>();
			if (unit != null)
			{
				if (HitNum > -1)
				{
					if (count >= HitNum)
					{
						break;
					}
				}
				HitTarget(unit.gameObject, skillEffectIndex);
				count ++;
			}
		}
	}

	/// <summary>
	/// 根据技能选择目标层;
	/// </summary>
	public int TargetLayerMask
	{
		get
		{
			if (m_SkillCaster == null)
			{
				return 0;
			}
			
			CombatUnitController controller = m_SkillCaster.GetComponent<CombatUnitController>();
			if (controller == null)
			{
				return 0;
			}
			
			if (HitType == SkillHitTypes.ScaleEnemy ||
			    HitType == SkillHitTypes.SelectedEnemy)
			{
				return controller.EnemyLayer;
			}
			else if (HitType == SkillHitTypes.ScaleFriend ||
			         HitType == SkillHitTypes.SelectedFriend)
			{
				return controller.FriendLayer;
			}
			
			return 0;
		}
	}
}




















