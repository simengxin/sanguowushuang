using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatUnitController : MoveBaseController {
	/// <summary>
	/// 客户战斗对象数据;

	protected CombatUnit m_CurCombatUnit;
	public CombatUnit CurCombatUnit{
		get{return m_CurCombatUnit;}
		set{
			m_CurCombatUnit = value;
			m_CurCombatUnit.OnSpawn();
		}
	}

	// <summary>
	/// 对象死否死亡;
	/// </summary>
	protected bool m_IsDead = false;
	public bool IsDead
	{
		get { return m_IsDead; }
	}
	
	/// <summary>
	/// 能否攻击;
	/// </summary>
	public int CantAttack
	{
		get {return CurCombatUnit.CantAttack;}
		set { CurCombatUnit.CantAttack = value;}
	}
	
	/// <summary>
	/// 能否被攻击;
	/// </summary>
	public int CantbeAttack	
	{
		get {return CurCombatUnit.CantbeAttack;}
		set { CurCombatUnit.CantbeAttack = value;}
	}
	
	/// <summary>
	/// Hp;
	/// </summary>
	public int Hp	
	{
		get {return CurCombatUnit.Hp;}
		set { CurCombatUnit.Hp = value;}
	}
	
	/// <summary>
	/// HpRatio;
	/// </summary>
	public float HpRatio	
	{
		get {return (float)CurCombatUnit.Hp / (float)CurCombatUnit.MaxHp;}
	}
	
	/// <summary>
	/// Hp;
	/// </summary>
	public int MaxHp	
	{
		get {return CurCombatUnit.MaxHp;}
	}
	
	/// <summary>
	/// 头顶血条Prefab名字;
	/// </summary>
	public virtual string HUDPrefabName
	{
		get {return "NpcHUD";}
	}
	
	/// <summary>
	/// 头顶血条;
	/// </summary>
	//protected NpcHUD m_NpcHud;
	///public virtual Color HUDColor
	//{
	///	get {return Color.white;}
	//}
	
	/// <summary>
	/// 战斗目标;
	/// </summary>
	protected CombatUnitController m_Target;
			  //CombatUnitController
	/// <summary>
	/// 敌方层;
	/// </summary>
	public virtual int EnemyLayer
	{
		get {return 0;}
	}
	
	/// <summary>
	/// 友方层;
	/// </summary>
	public virtual int FriendLayer
	{
		get {return 1 << gameObject.layer;}
	}
	
	/// <summary>
	/// 普通攻击CD;
	/// </summary>
	protected bool m_NormalAttackCD = false;
	
	/// <summary>
	/// 当前选中的技能;
	/// </summary>
	protected Skill m_CurSelectedSkill = null;
	
	/// <summary>
	/// 当前正在使用的技能;
	/// </summary>
	protected Skill m_CurSkill = null;
	
	/// <summary>
	/// 轮廓光组件;
	/// </summary>
	//private HighlightableObject m_HighLight;
	
	/// <summary>
	/// 是否需要移动;
	/// </summary>
	protected bool m_NeedMoveToTarget = false;

	protected override void Start(){
		base.Start ();
		if (CurCombatUnit != null) {
			m_Speed = CurCombatUnit.Speed;
		}
	}

	/// <summary>
	/// 查找最近距离单位;
	/// </summary>
	public virtual CombatUnitController FindTargetInRadius_MinDistance(int layer,bool includeCantBeAttack = false){
		float minDistance = -1;
		CombatUnitController selectedUnit = null;

		Collider[] colliders = Physics.OverlapSphere (transform.position, m_CurCombatUnit.Spring, layer);
		foreach (Collider c in colliders) {
			CombatUnitController unit = c.gameObject.GetComponent<CombatUnitController>();
			if(unit == null || unit.IsDead){
				continue;
			}
			if(!includeCantBeAttack && unit.CantbeAttack>0){
				continue;
			}

			float distance = Vector3.Distance(transform.position,unit.transform.position);
			if(minDistance <0){
				minDistance = distance;
				selectedUnit = unit;
			}else{
				if(distance<minDistance){
					minDistance = distance;
					selectedUnit = unit;
				}
			}

		}
		return selectedUnit;
	}

	/// <summary>
	/// 查找随机单位;
	/// </summary>
	public virtual CombatUnitController FindTargetInRadius_Random(int layer,bool includeCantbeAttack = false){
		Collider[] colliders = Physics.OverlapSphere (transform.position, CurCombatUnit.Spring, layer);
		List<object> allUnit = new List<object> ();
		foreach (Collider c in colliders) {
			CombatUnitController unit = c.gameObject.GetComponent<CombatUnitController>();
			if(unit == null || unit.IsDead){
				continue;
			}
			if(!includeCantbeAttack && unit.CantbeAttack>0){
				continue;
			}

			allUnit.Add(unit);
		}
		return CommonHelper.RandomObject (allUnit) as CombatUnitController;
	}
	/// <summary>
	/// 查找Hp最少单位;
	/// </summary>
	public virtual CombatUnitController FindTargetInRadius_MinHp(int layer,bool includeCantbeAttack = false){
		float minHp = 0;
		CombatUnitController selectedUnit = null;
		
		Collider[] colliders = Physics.OverlapSphere (transform.position, m_CurCombatUnit.Spring, layer);
		foreach (Collider c in colliders) {
			CombatUnitController unit = c.gameObject.GetComponent<CombatUnitController>();
			if(unit == null || unit.IsDead){
				continue;
			}
			if(!includeCantbeAttack && unit.CantbeAttack>0){
				continue;
			}
			
			float hp = unit.HpRatio;
			if(minHp  == 0){
				minHp = hp;
				selectedUnit = unit;
			}else{
				if(hp<minHp){
					minHp = hp;
					selectedUnit = unit;
				}
			}
			
		}

		if (selectedUnit.Hp >= selectedUnit.MaxHp) {
			selectedUnit = null;
		}
		return selectedUnit;
	}

	/// <summary>
	/// 根据技能配置查找目标单位;
	/// </summary>
	protected void SelectTargetBySkill(){
		if (m_CurSelectedSkill == null) {
			return;
		}

		if (m_CurSelectedSkill.HitType == SkillHitTypes.ScaleEnemy ||
			m_CurSelectedSkill.HitType == SkillHitTypes.SelectedEnemy) {
			if (m_Target != null) {
				if (m_Target.IsDead) {
					m_Target = null;
				} else {
					if (m_Target.CantbeAttack > 0) {
						m_Target = null;
					}

					if ((1 << m_Target.gameObject.layer) != EnemyLayer) {
						m_Target = null;
					}
				}
			}

			if (m_Target == null) {
				m_Target = FindTargetInRadius_MinDistance (EnemyLayer);
			}

		} else if (m_CurSelectedSkill.HitType == SkillHitTypes.ScaleFriend || 
			m_CurSelectedSkill.HitType == SkillHitTypes.SelectedFriend) {
			m_Target = FindTargetInRadius_MinHp (FriendLayer, true);
		} else if (m_CurSelectedSkill.HitType == SkillHitTypes.Self) {
			m_Target = this;
		}
	}

	/// <summary>
	/// 移动到选中目标;
	/// </summary>
	protected void MoveToTarget()
	{
		SetDestination(m_Target.transform.position);
	}

	//-------------------Skill HitEffect  Start---------------------------------
	/// <summary>
	/// 击中目标受到伤害;
	/// </summary>
	public virtual void OnDamaged(CombatUnitController attacker,float hp,float burstRatio,float burstMul,List<int> hitEffects){
		if (attacker == null || m_IsDead) {
			return;
		}

		if (CantbeAttack > 0) {
			return;
		}
		bool burst = false;
		if (CommonHelper.OnProbability (burstRatio)) {
			burst = true;
			hp = hp*burstMul;
		}

		Hp = Mathf.Max (0, Hp - (int)hp);
		if (Hp == 0) {
			OnDead ();
		} else if (Hp > 0) {
			OnHurt(hitEffects);
			if(m_Target == null){
				m_Target = attacker;
			}
		}
	}
	/// <summary>
	/// 击中目标受到治疗;
	/// </summary>
	public virtual void OnHeal(CombatUnitController attacker,int hp,List<int>hitEffects){
		if (attacker == null || m_IsDead) {
			return;
		}
		Hp = Mathf.Min (MaxHp, Hp + hp);
		for (int i = 0; i < hitEffects.Count; i++) {
			if(hitEffects[i]>0){
				EffectManager.Instance.CreateEffect(hitEffects[i],this.transform);
			}
		}
	}
	/// <summary>
	/// 添加Buff;
	/// </summary>
	public void OnAddBuff(CombatUnitController attacker,int buffId){
		if (attacker == null || m_IsDead) {
			return;
		}
		//todo

	}
	/// <summary>
	/// 击飞;
	/// </summary>
	public virtual void OnHitOut(CombatUnitController attacker,float dist,float time){
		if (attacker == null || m_IsDead) {
			return;
		}
		if (m_Animator == null || m_Animator.speed == 0) {
			return;
		}
		AnimatorStateInfo stateInfo = m_Animator.GetCurrentAnimatorStateInfo (0);
		if (stateInfo.IsName ("Base Layer.hitdown") ||
			stateInfo.IsName ("Base Layer.hitout") ||
			stateInfo.IsName ("Base Layer.downup")) {
			return;
		}

		m_Animator.SetTrigger("HitOut");
		HitBack (attacker, dist, time);
	}

	/// <summary>
	/// 击退;
	/// </summary>
	void HitBack(CombatUnitController attacker,float dist,float time){
		Vector3 backDist = transform.position - attacker.transform.position;
		if (backDist == Vector3.zero) {
			return;
		}
		backDist.Normalize ();
		Vector3 hitBackVel = backDist * dist;
		float hitBackAcc = -dist / time;
		StartCoroutine(HitingBack(hitBackVel,hitBackAcc));
	}

	IEnumerator HitingBack(Vector3 hitBackVel,float hitBackAcc){
		yield return new WaitForSeconds (0.05f);

		Vector3 facing = hitBackVel;
		facing.Normalize ();
		transform.rotation = Quaternion.LookRotation (facing);
		while(true){
			if(!m_NavAgent.enabled){
				break;
			}
			m_NavAgent.Move(hitBackVel*Time.deltaTime);

			float newVelMag = hitBackVel.magnitude +hitBackAcc *Time.deltaTime;
			if(newVelMag < 0.02f){
				break;
			}
			hitBackVel = hitBackVel.normalized* newVelMag;
			yield return null;
		}
		yield break;
	}
	// <summary>
	/// 击倒;
	/// </summary>
	public virtual void OnHitDown(CombatUnitController attacker,float dist,float time){
		if (attacker == null || m_IsDead || m_Animator == null) {
			return ;
		}

		if (m_Animator.speed == 0) {
			return;
		}

		AnimatorStateInfo stateInfo = m_Animator.GetCurrentAnimatorStateInfo (0);
		if (stateInfo.IsName ("Base Layer.hitdown") ||
			stateInfo.IsName ("Base Layer.hitout") ||
			stateInfo.IsName ("Base Layer.downup")) {
			return;
		}
		if (time <= 0 || dist <= 0) {
			return;
		}

		//TODO

		m_Animator.SetTrigger("HitDown");
		HitBack (attacker, dist, time);
	
	}
	/// <summary>
	/// 嘲讽;
	/// </summary>
	public virtual void OnSneer(CombatUnitController attacker){
		if (attacker == null || m_IsDead) {
			return;
		}

		m_Target = attacker;
		
	}

	//-------------------Skill HitEffect  End---------------------------------
	/// <summary>
	/// 播放伤害特效和动作;
	/// </summary>
	public void OnHurt(List<int> hitEffects){
		for (int i = 0; i < hitEffects.Count; i++) {
			if(hitEffects[i]>0){
				EffectManager.Instance.CreateEffect(hitEffects[i],this.transform);
			}
		}

		if (m_Animator == null) {
			return;
		}
		AnimatorStateInfo stateInfo = m_Animator.GetCurrentAnimatorStateInfo (0);
		if (stateInfo.IsName ("Base Layer.idle") && !m_Animator.IsInTransition (0)) {
			m_Animator.SetTrigger("Hurt");
		}
	}

	/// <summary>
	/// 设置死亡;
	/// </summary>
	public void SetDead(bool value)
	{
		if (m_Animator == null)
		{
			return;
		}
		
		m_IsDead = value;
		if (value)
		{
			m_Animator.SetTrigger("Dead");
			m_NavAgent.enabled = false;
			CapsuleCollider collide = GetComponent<CapsuleCollider>();
			if (collide != null)
			{
				MonoBehaviour.Destroy(collide);
			}
			
			CharacterController control = GetComponent<CharacterController>();
			if (control != null)
			{
				MonoBehaviour.Destroy(control);
			}
			
			//Transform blob = transform.FindRecursively("Blob Shadow Projector");
			//if (blob != null)
			//{
			//	blob.gameObject.SetActive(false);
			//}
		}
	}
	protected virtual void OnDead(){
		SetDead(true);
		//BufferManager.RemoveAll(this);
	}
	/// <summary>
	/// 设置普通攻击CD;
	/// </summary>
	protected void SetNormalAttackCD(){
		m_NormalAttackCD = true;
		TimerManager.Instance.AddTimer ("NormalSkill"+gameObject.GetInstanceID().ToString(),CurCombatUnit.NormalAttackCDTime,ResetNormalAttackCD,null);

	}
	/// <summary>
	/// 重设普通攻击CD;
	/// </summary>
	public void ResetNormalAttackCD(params object[] args){
		m_NormalAttackCD = false;
	}
	/// <summary>
	/// 判断是否处于空闲状态来使用技能;
	/// </summary>
	public bool IsIdleToUseSkill{
		get{
			int curSkillId = m_Animator.GetInteger("SkillId");
			AnimatorStateInfo stateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
			if(curSkillId == 0 && stateInfo.IsName("Base Layer.idle")&&!m_Animator.IsInTransition(0)){
				return true;
			}
			return false;
		}

	}

	/// <summary>
	/// 能否进行战斗;
	/// </summary>
	public bool IsIdleToAttack
	{
		get
		{
			if (CantAttack > 0)
			{
				return false;
			}
			
			AnimatorStateInfo stateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
			//Debug.LogError("a:" + stateInfo.IsName("Base Layer.idle") + "  b:" + stateInfo.IsName("Base Layer.run") +
			//        "    c:" + !m_Animator.IsInTransition(0));
			if ((stateInfo.IsName("Base Layer.idle") || stateInfo.IsName("Base Layer.run")) && !m_Animator.IsInTransition(0))
			{
				//Debug.LogError("1:" + stateInfo.IsName("Base Layer.idle") + "  2:" + stateInfo.IsName("Base Layer.run") +
				//    "    3:" + !m_Animator.IsInTransition(0));
				return true;
			}
			
			return false;
		}
	}
	void Update()
	{
		if (IsDead) {
			return;
		}
		AutoFightUpdate ();
	}

	/// <summary>
	/// 自动战斗Update;
	/// </summary>
	protected virtual void AutoFightUpdate()
	{
		return;
	}
	/// <summary>
	/// 尝试使用技能，有可能还不能使用;
	/// </summary>
	protected virtual void TryCastSkill()
	{
		if ((Vector3.Distance(transform.position, m_Target.transform.position) >= m_CurSelectedSkill.AttackDist - 0.1f))
		{
			m_NeedMoveToTarget = true;
			return;
		}
		else
		{
			m_NeedMoveToTarget = false;
			
			PerpareAttack();
		}
		
		if (IsIdleToUseSkill)
		{
			m_Animator.SetInteger("SkillId", m_CurSelectedSkill.SkillId);
		}
		
		return;
	}

	/// <summary>
	/// 准备使用技能，停止移动，正对目标;
	/// </summary>
	protected void PerpareAttack()
	{
		if (m_Target != null) 
		{
			RotateToTarget(m_Target.transform.position);
		}
		StopMove();
	}

	/// <summary>
	/// 被冰冻标记量;
	/// </summary>
	private int m_AnimatorFreeze = 0;
	public int AnimatorFreeze
	{
		get { return m_AnimatorFreeze;}
		set
		{
			m_AnimatorFreeze = value;
			if (m_AnimatorFreeze > 0)
			{
				m_Animator.speed = 0;
			}
			else
			{
				m_Animator.speed = 1;
			}
		}
	}
	/// <summary>
	/// 被冰冻;
	/// </summary>
	public void Freezed(float time){
	
		StartCoroutine (FreezedInternal (time));
	}
	IEnumerator FreezedInternal(float time){
		AnimatorFreeze = AnimatorFreeze + 1;
		yield return new WaitForSeconds (time);

		if (m_Animator != null) {
			AnimatorFreeze = AnimatorFreeze - 1;
		
		}

	}
	/// <summary>
	/// 冲刺击中目标;
	/// </summary> 
	public void OnSprintHit(GameObject hitTarget,params object[] parm){
		Skill skill = parm [0] as Skill;
		skill.HitTarget (hitTarget);

	}
	/// <summary>
	/// 发射的子弹特效击中目标;
	/// </summary>
	void OnBulletHit(GameObject hitTarget, params object[] param)
	{
		Skill skill = param[0] as Skill;
		skill.HitTarget(hitTarget);
	}

	/// <summary>
	/// 发射的导弹特效击中目标;
	/// </summary>
	void OnMissileHit(GameObject hitTarget, params object[] param)
	{
		Skill skill = param[0] as Skill;
		skill.HitTarget(hitTarget);
	}

	/// <summary>
	/// 大招特效，全黑，显示自己;
	/// </summary>
	public void Focus(float time)
	{
		StartCoroutine(FocusInternal(time));
	}
	
	/// <summary>
	/// 大招特效，全黑，显示自己协程;
	/// </summary>
	IEnumerator FocusInternal(float time)
	{
		//m_HighLight.FocusOnImmediate();
		
		yield return new WaitForSeconds(time);
		
		//m_HighLight.FocusOffImmediate();
		yield break;
	}


	/// <summary>
	/// 技能开始;
	/// </summary>
	void OnSkillCast(int skillId)
	{
		m_Animator.SetInteger("SkillId", 0);
		
		Skill skill = CurCombatUnit.GetSkill(skillId);
		if (skill == null)
		{
			Debug.Log("OnSkillCase Event cant Find the Skill, id = " + skillId.ToString());
			return;
		}
		
		if (m_Target == null)
		{
			skill.SetSkillCaster(this.gameObject, null);
		}
		else
		{
			skill.SetSkillCaster(this.gameObject, m_Target.gameObject);
		}
		
		if (skill.SkillType == SkillTypes.NormalSkill)
		{
			SetNormalAttackCD();
		}
		else
		{
			skill.SetCD();
		}
		
		m_CurSkill = skill;
		
		m_CurSelectedSkill = null;
	}
	/// <summary>
	/// 战斗特效，绑定自己;
	/// </summary>
	void OnAttackEffect(int effectId){
		GameObject effect = EffectManager.Instance.CreateEffect (effectId, this.transform);

		EffectData info = DataManager.s_EffectDataManager.GetData (effectId);
		if (effect == null || info == null)
		{
			return;
		}
		
		if ((EffectTypes)(info.EffectType) == EffectTypes.Bullet)
		{
			//EffectManager.Instance.BindBullet(effect.transform, info, this.transform, EnemyLayer, OnBulletHit, m_CurSkill);
		}
		
		if ((EffectTypes)(info.EffectType) == EffectTypes.Missile)
		{
			if (m_Target != null)
			{
				//EffectManager.Instance.BindMissile(effect.transform, info, this.transform, m_Target.transform, OnMissileHit, m_CurSkill);
			}
			else
			{
				Debug.Log("Launch Missile but no Target!");
			}
		}
	}

	/// <summary>
	/// 大招冻结其他人;
	/// </summary>
	void OnAttackFreezed(float time)
	{
		int layerMask = 1 << LayerMask.NameToLayer("Warrior") 
			| 1 << LayerMask.NameToLayer("AttackNpc");
		Collider[] colliders = Physics.OverlapSphere(transform.position, 100, layerMask );
		
		foreach (Collider c in colliders)
		{
			CombatUnitController unit = c.GetComponent<CombatUnitController>();
			//CombatUnitContrller unit = c.gameObject.GetComponent<CombatUnitContrller>();			
			if (unit == null)
			{
				continue;
			}
			
			if (unit == this)
			{
				continue;
			}
			
			unit.Freezed(time);
		}
	}
	
	/// <summary>
	/// 大招全部冻结;
	/// </summary>
	void OnAttackFreezedAll(float time)
	{
		int layerMask = 1 << LayerMask.NameToLayer("Warrior")
			| 1 << LayerMask.NameToLayer("AttackNpc");
		Collider[] colliders = Physics.OverlapSphere(transform.position, 100, layerMask );
		
		foreach (Collider c in colliders)
		{
			CombatUnitController unit = c.gameObject.GetComponent<CombatUnitController>();			
			if (unit == null)
			{
				continue;
			}
			
			unit.Freezed(time);
		}
	}
	
	/// <summary>
	/// 大招全屏黑，只显示自己;
	/// </summary>
	void OnFocus(float time)
	{
		this.Focus(time);
	}
	
	
	/// <summary>
	/// 技能击中事件;
	/// </summary>
	void OnHitTarget(int index)
	{
		if (m_CurSkill != null)
		{
			m_CurSkill.SkillHit(index);
		}
	}
	
	/// <summary>
	/// 2D特效;
	/// </summary>
	void On2DEffect(int id)
	{
		//if (DataManager.s_Skill2DEffectDataManager.GetData(id) == null)
		//{
		//	return;
		//}
		
		//BattlePanel panel = GUIManager.FindView<BattlePanel>("BattlePanel");
		//if (panel != null)
	//	{
		//	panel.Show2DEffect(id);
		//}
	}
	
	/// <summary>
	/// 冲刺;
	/// </summary>
	void OnSprint(int buffId)
	{
	/*	BuffData buff = DataManager.s_BuffDataManager.GetData(buffId);
		if (buff == null)
		{
			Debug.LogError("Cant find the buffer, id = " + buffId.ToString());
		}
		
		if ((BuffTypes)buff.Type != BuffTypes.Sprint)
		{
			Debug.LogError("On Sprint Event Added the InCorrect Buff, id = " + buffId.ToString());
		}
		
		object[] args = new object[] {buff.EffectId, buff.Para1, buff.Para2, EnemyLayer, m_CurSkill};
		BufferManager.AddBuff(this, (BuffTypes)buff.Type, args);*/
	}





}
