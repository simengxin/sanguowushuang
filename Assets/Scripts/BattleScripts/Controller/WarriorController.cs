using UnityEngine;
using System.Collections;

public class WarriorController : CombatUnitController {
	public Warrior CurWarrior{
		get{
			return m_CurCombatUnit  as Warrior;
		}
	}
	protected bool m_CastSuperSkill = false;

	public override int EnemyLayer{
		get{
			return 1<<LayerMask.NameToLayer("AttackNpc");
		}
	}

	public BattleTypes m_BattleTypes;

	protected override void Start(){
		CantAttack = 0;
		CantbeAttack = 0;

		base.Start ();

		//if(m_BattleT)
	}

	public void CastSuperSkill(){
		m_CastSuperSkill = true;
	}

	/// <summary>
	/// 自动战斗Update;
	/// </summary>
	protected override void AutoFightUpdate()
	{
		if (!IsIdleToAttack)
		{
			return;
		}
		
		if (m_CastSuperSkill && !CurWarrior.m_HasCastSuperSkill)
		{
			m_CurSelectedSkill = CurWarrior.GetSuperSkill();
			m_CastSuperSkill = false;
			CurWarrior.Bp = 0;
		}
		
		if (m_CurSelectedSkill == null)
		{
			m_CurSelectedSkill = CurWarrior.GetAutoSkillId();
		}
		
		if (m_CurSelectedSkill == null && !m_NormalAttackCD)
		{
			m_CurSelectedSkill = CurWarrior.GetRandomNormalSkill();
		}
		
		SelectTargetBySkill();
		
		if (m_CurSelectedSkill != null && m_Target != null)
		{
			TryCastSkill();
			if (m_NeedMoveToTarget)
			{
				MoveToTarget();
			}
		}
		
		if (m_Target == null)
		{
			FreeUpdate();
		}
	}
	
	/// <summary>
	/// Idel情况下Update;
	/// </summary>
	void FreeUpdate()
	{
		SetDestination(m_SpawnPos);
	}
}
