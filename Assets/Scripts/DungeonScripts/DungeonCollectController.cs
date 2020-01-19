using UnityEngine;
using System.Collections;

public class DungeonCollectController : StageController 
{
	protected override void Start()
	{
		base.Start();
	}

	protected override void OnMouseClick (Transform hero)
	{

	}

	private void Update () 
	{
#if UNITY_IPHONE || UNITY_ANDROID
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
			RaycastHit hitInfo;
			
			if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, 1 << LayerMask.NameToLayer("AttackNpc")))
			{
				DungeonCollectController dungeon_collect = hitInfo.transform.GetComponent<DungeonCollectController>();
				if (null == dungeon_collect)
				{
					return;
				}
				UpdateCollect();	
			}
		}
#else
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			
			if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, 1 << LayerMask.NameToLayer("AttackNpc")))
			{
				DungeonCollectController dungeon_collect = hitInfo.transform.GetComponent<DungeonCollectController>();
				if (null == dungeon_collect)
				{
					return;
				}
				UpdateCollect();	
			}
		}
#endif
	}

	void UpdateCollect()
	{
		float max_distance = 1.5f;
		Vector3 heroPosition = DungeonLogic.Instance.HeroPosition;
		float distance = Vector3.Distance(transform.position, heroPosition);
		if (distance > max_distance)
		{
			return;
		}

		//Transform root = gameObject.transform.FindRecursively("Crystal(Clone)");
		string name = gameObject.transform.name.Replace("(Clone)", "");
		bool bDestroy = false;
		int count = gameObject.transform.childCount;
		for (int i = 0; i < count; ++i)
		{
			Transform child = gameObject.transform.FindRecursively(name + i);
			if (child.gameObject.activeInHierarchy)
			{
				child.gameObject.SetActive(false);
				//DungeonLogic.CollectResource(m_CurStageData);
				if (i >= count - 1)
				{
					bDestroy = true;
				}
				
				break;
			}
		}
		
		if (bDestroy)
		{
			GameObject.Destroy(gameObject, 1.0f);
		}
	}
}