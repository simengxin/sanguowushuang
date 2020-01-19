using UnityEngine;
using System.Collections;

public class TreasureBoxController : StageController 
{
	protected override void Start()
	{
		base.Start();

		ClosedBox();
	}

	protected override void OnMouseClick(Transform hero)
	{
		OpenBox();
		//DungeonLogic.OpenTreasurebox(m_CurStageData);
	}

	private void OpenedBox()
	{
		Animation[] animations = GetComponents<Animation>();
		foreach (Animation animation in animations)		
		{
			animation.Play("openloop");
		}
		CapsuleCollider capsuleCollider = transform.gameObject.GetComponent<CapsuleCollider>();
		if (capsuleCollider != null)
		{
			capsuleCollider.enabled = false;
		}			
	}
	
	private void ClosedBox()
	{
		Animation[] animations = GetComponents<Animation>();
		foreach (Animation animation in animations)	
		{
			animation.Play("closeloop");
		}
	}
	
	private void OpenBox()
	{
		Animation[] animations = GetComponents<Animation>();
		foreach (Animation animation in animations)		
		{
			if (animation.IsPlaying("close"))
			{
				animation.CrossFade("open");
			}
			else
			{
				animation.Play("open");
			}
		}

		CapsuleCollider capsuleCollider = transform.gameObject.GetComponent<CapsuleCollider>();
		if (capsuleCollider != null)
		{
			capsuleCollider.enabled = false;
		}	

		GameObject.Destroy(gameObject, 1.0f);
	}	
}