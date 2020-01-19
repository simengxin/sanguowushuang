using UnityEngine;
using System.Collections;

public class HeroController : MonoBehaviour {
	protected UnityEngine.AI.NavMeshAgent m_NavAgent;
	protected CharacterController cc;
	protected Animator m_Animator;
	public float m_Speed = 3.0f;

	public Warrior m_Warrior;
	public static HeroController Instance = null;
	
	public StageController  m_SelectedObject = null;

	// Use this for initialization
	protected void Start () {
		m_NavAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ();
		m_Animator = gameObject.GetComponent<Animator> ();
		m_NavAgent.velocity = Vector3.zero;
		//cc = gameObject.GetComponent<CharacterController>();
		m_Speed = m_Warrior.Speed;
		m_Animator.Play("Base Layer.idle");
		
		Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateMove ();
	}

	void UpdateMove()
	{
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		if(JoyStickController.h !=0 || JoyStickController.v != 0)
		{
			h = JoyStickController.h;
			v = JoyStickController.v;
		}
		if (h != 0f || v != 0f)
		{
			Vector3 tempVector = new Vector3(h, 0f,v);
			tempVector.Normalize();
			
			Vector3 relative  = transform.InverseTransformPoint(transform.position + tempVector);
			float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;		
			transform.Rotate (Vector3.up * angle);	
			m_NavAgent.velocity = Vector3.zero;
			//cc.Move(tempVector * m_Speed * Time.deltaTime);
			m_NavAgent.Move(tempVector * m_Speed * Time.deltaTime);
			if (!m_Animator.GetBool("Move"))
			{
				m_Animator.SetBool("Move", true);
			}
		}
		else
		{
			m_NavAgent.Stop();
			if (m_Animator.GetBool("Move"))
			{
				m_Animator.SetBool("Move", false);
			}
		}
	}

	
	public void OnTriggerClick()
	{
		float minDistance = -1;
		m_SelectedObject = null;
// ע��  ������
		Collider[] colliders = Physics.OverlapSphere(transform.position, 3, 1 << LayerMask.NameToLayer("AttackNpc"));
		
		foreach (Collider c in colliders)
		{
			StageController unit = c.gameObject.GetComponent<StageController>();			
			if (unit == null)
			{
				continue;
			}
			
			float distance = Vector3.Distance(transform.position, unit.transform.position);
			if (minDistance < 0)
			{
				minDistance = distance;
				m_SelectedObject = unit;
			}
			else
			{
				if (distance < minDistance)
				{
					minDistance = distance;
					m_SelectedObject = unit;
				}
			}
		}
		
		if (m_SelectedObject != null)
		{
			m_SelectedObject.SendMessage("OnMouseClick", this.transform, SendMessageOptions.DontRequireReceiver);
			//StopMove();
			//RotateToTarget(m_SelectedObject.transform.position);
		}
	}
}
