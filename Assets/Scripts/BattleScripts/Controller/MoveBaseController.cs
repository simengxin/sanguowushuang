using UnityEngine;
using System.Collections;

public class MoveBaseController : MonoBehaviour {

	protected UnityEngine.AI.NavMeshAgent m_NavAgent;
	protected Animator m_Animator;
	protected Vector3 m_SpawnPos;
	public float m_Speed = 3.0f;
	protected bool m_CrossFade = true;


	// Use this for initialization
	protected virtual void Start () {
		m_NavAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ();
		m_Animator = gameObject.GetComponent<Animator> ();
		m_NavAgent.velocity = Vector3.zero;

	}

	public void NavmeshMove(){
		Vector3 lookat = m_NavAgent.steeringTarget - transform.position;
		lookat.y = 0;
		if (lookat != Vector3.zero)
		{		
			Quaternion to = Quaternion.LookRotation(lookat, Vector3.up);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, to, m_Speed * 4.0f);	
		}
		
		m_NavAgent.Move(transform.forward * m_Speed * Time.deltaTime);
	}
	public bool NeedMove{
		get{
			if(m_NavAgent.remainingDistance <= m_NavAgent.stoppingDistance){
				return false;
			}
			return true;
		}
	}

	/// <summary>
	/// 设置移动目标位置;
	/// </summary>
	public void SetDestination(Vector3 pos)
	{
		m_NavAgent.SetDestination(pos);
		//m_NavAgent.Stop();
	}
	
	/// <summary>
	/// 停止移动;
	/// </summary>
	protected void StopMove()
	{
		SetDestination(transform.position);
	}
	
	/// <summary>
	/// 旋转正对目标;
	/// </summary>
	public void RotateToTarget(Vector3 pos) 
	{
		Vector3 relative  = transform.InverseTransformPoint(pos);
		float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;		
		transform.Rotate (Vector3.up * angle);	
	}

	void OnAnimatorMove(){
		if (m_Animator.speed == 0) {
			return;
		}

		if (m_NavAgent.enabled) {
			if(NeedMove){
				if(!m_Animator.GetBool("Move")){
					m_Animator.SetBool("Move",true);
				}

				if(m_CrossFade){
					AnimatorStateInfo stateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
					if(stateInfo.IsName("Base Layer.run") && !m_Animator.IsInTransition(0)){
						NavmeshMove();
					}
				}else{
					NavmeshMove();
				}
			}else{
				if(m_Animator.GetBool("Move")){
					m_NavAgent.velocity = Vector3.zero;
					m_Animator.SetBool("Move",false);
				}
			}
		}
	}
}
