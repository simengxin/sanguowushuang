using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackNpcController : CombatUnitController {
	/// <summary>
	/// 客户战斗对象数据;
	/// </summary>
	public AttackNpc CurAttackNpc{
		get{
			return m_CurCombatUnit as AttackNpc;
		}
	}
	/// <summary>
	/// 敌方层;
	/// </summary>
	public override int EnemyLayer{
		get{
			return 1<< LayerMask.NameToLayer("Warrior");
		}
	}

	public float PatrolRadius = 0;

	private float m_ReFindTargetTime = 10.0f;
	private float m_ReFindTime = 0;

	public override void OnDamaged(CombatUnitController attacker,float hp,float burstRatio,float burstMul,List<int> hitEffects){
		base.OnDamaged (attacker, hp, burstRatio,burstMul, hitEffects);

		if (!IsDead) {
			//TODO
		}
	}
	/// <summary>
	/// 死亡;
	/// </summary>
	protected override void OnDead(){
		base.OnDead ();
		StartCoroutine (Sink ());
	}

	private Vector3 m_SinkDest = Vector3.zero;
	IEnumerator Sink(){
		m_SinkDest = transform.position + Vector3.down * 5;
		yield return new WaitForSeconds (3.0f);
		while (Mathf.Abs(transform.position.y  - m_SinkDest.y)>0.5f) {
			transform.position = Vector3.Lerp(transform.position,m_SinkDest,Time.deltaTime*0.1f);
			yield return null;
		}
		yield break;

	}
	/// <summary>
	/// 自动战斗Update;
	/// </summary>
	protected override void AutoFightUpdate()
	{
		m_ReFindTime += Time.deltaTime;
		if (m_ReFindTime > m_ReFindTargetTime)
		{
			m_ReFindTime = 0;
			m_Target = null;
		}
		
		if (!IsIdleToAttack)
		{
			return;
		}
		
		if (!m_NormalAttackCD && m_CurSelectedSkill == null)
		{
			m_CurSelectedSkill = CurAttackNpc.GetRandomNormalSkill();
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
		else if (m_Target == null)
		{
			if (PatrolRadius > 0)
			{
				if (!NeedMove)
				{
					SetDestination(GetRandomPosition());
				}
			}
		}
	}

	protected Vector3 GetRandomPosition()
	{
		Ray ray = new Ray();
		for (int i = 0; i < 10; i++)
		{
			ray.origin = new Vector3(Random.Range(-PatrolRadius,PatrolRadius) + m_SpawnPos.x, m_SpawnPos.y + 10, m_SpawnPos.z + Random.Range(-PatrolRadius,PatrolRadius));
			ray.direction = -Vector3.up;
			RaycastHit hitInfo;		
			int mask = 1 << LayerMask.NameToLayer("Default");			
			if (Physics.Raycast(ray, out hitInfo, 50, mask))
			{	
				return hitInfo.point + Vector3.up * 0.04f;
			}
		}
		return transform.position;	
	}

}
