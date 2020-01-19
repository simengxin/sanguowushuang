using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BattleScale : MonoBehaviour 
{
	void OnTriggerEnter(Collider collider)
	{
		Transform colliderTransform = collider.transform;
		AttackNpcController controller = colliderTransform.GetComponent<AttackNpcController>();
		if (controller != null)
		{
			controller.CantAttack = 0;
			controller.CantbeAttack = 0;
		}
	}
}