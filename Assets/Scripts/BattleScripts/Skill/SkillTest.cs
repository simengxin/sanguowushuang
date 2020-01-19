using UnityEngine;
using System.Collections;

public class SkillTest : MonoBehaviour {
	private UnityEngine.AI.NavMeshAgent m_NavAgent;
	public Animator m_Animator;
	public float m_Speed = 3.0f;
	private int SkillId = 1016;

	protected SkillTest m_Target;
	private Skill m_Skill = null;
	private float hitBackAcc = -80.0f;

	void Awake(){
		m_NavAgent = gameObject.AddComponent<UnityEngine.AI.NavMeshAgent> ();
		m_NavAgent.speed = 0;
		m_NavAgent.acceleration = 0;
		m_NavAgent.angularSpeed = 0;
		//m_NavAgent.avoidancePriority = 
		m_NavAgent.height = 2.0f;
		m_NavAgent.radius = 1.5f;
		m_NavAgent.stoppingDistance = 2.0f;

		CapsuleCollider collider = gameObject.AddComponent<CapsuleCollider> ();
		collider.height = m_NavAgent.height;
		collider.radius = m_NavAgent.radius;
		collider.center = Vector3.up * (Mathf.Max (collider.height / 2.0f, collider.radius) + 0.03f);

		m_Animator = gameObject.GetComponent<Animator> ();
	}

	void Start () {
		if (SkillId > 0) {
			m_Skill = new Skill(SkillId);
		}

		//TODO
	}

	void Update () {
		if (gameObject.name == "zhangfei_2") {
			return;
		}
		FightStateUpdate ();
	}
	void FightStateUpdate(){
		if (m_Target == null) {
			m_Target = FindTargetInradius ();
		} else {
			bool moveToTarget = false;
			TryAttack(out moveToTarget);
			if(moveToTarget){
				MoveToTarget();
			}
		}

	}

	void MoveToTarget(){
		SetDestination (m_Target.transform.position);
	}

	public SkillTest FindTargetInradius(){
		Collider[] colliders = Physics.OverlapSphere(transform.position,100,1<<LayerMask.NameToLayer("Warrior"));

		foreach (Collider c in colliders) {
			SkillTest unit = c.gameObject.GetComponent<SkillTest>();
			if(unit == null){
				continue;
			}
			if(unit == this){
				continue;
			}
			return unit;
		}
		return null;
	} 

	public bool IsIdleToUseSkill{
		get{
			int curSkillId = m_Animator.GetInteger("SkillId");
			AnimatorStateInfo stateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
			if(curSkillId == 0 && stateInfo.IsName("Base Layer.idle") && !m_Animator.IsInTransition(0)){
				return true;
			}
			return false;
		}

	}
	public void TryAttack(out bool moveToTarget){
		moveToTarget = false;
		if (m_Skill != null) {
			if(!m_Skill.CoolDown){
				if((Vector3.Distance(transform.position,m_Target.transform.position)) >= m_Skill.AttackDist){
					moveToTarget = true;
					return;
				}else{
					PrepareAttack();
				}
				if(IsIdleToUseSkill){
					m_Animator.SetInteger("SkillId",m_Skill.SkillId);
				}

				return;
			}
		}
		return;
	}

	private void PrepareAttack(){
		if (m_Target != null) {
			RotateToTarget(m_Target.transform.position);
		}
		StopMove ();
	}

	private void StopMove(){
	
		SetDestination (transform.position);
	}

	void SetDestination(Vector3 pos){
		m_NavAgent.SetDestination (pos);
		m_NavAgent.Stop();
	}
	protected void RotateToTarget(Vector3 pos){
		Vector3 relative = transform.InverseTransformPoint(pos);
		float angle = Mathf.Atan2 (relative.x, relative.z) * Mathf.Rad2Deg;
		transform.Rotate (Vector3.up * angle);
	}

    void NavmeshMove()
    {
        m_NavAgent.Move(transform.forward * m_Speed * Time.deltaTime);

        Vector3 lookat = m_NavAgent.steeringTarget - transform.position;
        lookat.y = 0;
        if (lookat != Vector3.zero)
        {
            Quaternion to = Quaternion.LookRotation(lookat, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, to, 10);
        }
    }
    public bool NeedMove
    {
        get
        {
            if (m_NavAgent.remainingDistance <= m_NavAgent.stoppingDistance)
            {
                return false;
            }
            return true;
        }
    }
    void OnAnimatorMove()
    {
        if (NeedMove)
        {
            if (!m_Animator.GetBool("Move"))
            {
                m_Animator.SetBool("Move", true);
            }
            AnimatorStateInfo stateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Base Layer.run") && !m_Animator.IsInTransition(0))
            {
                NavmeshMove();
            }
        }
        else
        {
            if (m_Animator.GetBool("Move"))
            {
                m_NavAgent.velocity = Vector3.zero;
                m_Animator.SetBool("Move", false);
            }
        }
    }

	void OnSkillCast(int skillId){
		m_Skill.SetSkillCaster (this.gameObject, m_Target.gameObject);
		m_Animator.SetInteger ("SkillId", 0);
		m_Skill.SetCD ();
	}

	public void OnAttackEffect(int effectId){
		Debug.LogError ("skillTest,effectId:" + effectId);
		GameObject effect = EffectManager.Instance.CreateEffect (effectId, gameObject.transform);
		EffectData info = DataManager.s_EffectDataManager.GetData (effectId);
		if (effect == null || info == null) {
			return;
		}
	}

	public void Freezed(float time){
		StartCoroutine (FreezedInternal (time));
	}
	IEnumerator FreezedInternal(float time){
		float m_speed = 0;
		if (m_Animator != null) {
			m_speed = m_Animator.speed;
			m_Animator.speed = 0;
		}

		yield return new WaitForSeconds (time);
		if (m_Animator != null) {
			m_Animator.speed = m_speed;
		}
	}

	public void HitBack(Vector3 hitBackVel){
		StartCoroutine (HitingBack (hitBackVel));
	}
	//private float hitBackAcc = -80.0f;
	IEnumerator HitingBack(Vector3 hitBackVel){
		Vector3 facing = -hitBackVel;
		facing.Normalize ();
		transform.rotation = Quaternion.LookRotation (facing);

		while (true) {
			m_NavAgent.Move(hitBackVel*Time.deltaTime);

			float newVelMag = hitBackVel.magnitude + hitBackAcc*Time.deltaTime;
			if(newVelMag < 0.02f){
				break;
			}

			hitBackVel  = hitBackVel.normalized * newVelMag;

			yield return null;
		}
		yield break;
	}
}























