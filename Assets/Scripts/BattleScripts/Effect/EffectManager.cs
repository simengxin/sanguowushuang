using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EffectTypes{
	Normal = 1,
	WeaponTrail = 2,
	Bullet = 3,
	Missile = 4,
}

public class EffectManager  {
	private static EffectManager s_EffectManager = null;
	public static EffectManager Instance{
		get{
			if(s_EffectManager == null){
				s_EffectManager = new EffectManager();
			}
			return s_EffectManager;
		}
	}

	private Dictionary<string,List<GameObject>> m_EffectPoolList = new Dictionary<string, List<GameObject>> ();

	private GameObject m_EffectParent = null;
	public EffectManager(){
		m_EffectParent = new GameObject("Effect");
		GameObject.DontDestroyOnLoad (m_EffectParent);
	}

	public GameObject CreateEffect(int id , Transform tran){
		EffectData info = DataManager.s_EffectDataManager.GetData(id);
		if (info == null)
		{
			return null;
		}
		
		if ((EffectTypes)info.EffectType == EffectTypes.Normal)
		{		
			GameObject effect = CreateEffect(info.ArtRes, info.Save);
			if (effect == null)
			{
				Debug.Log("Effect is not exist... " + info.ArtRes);
				return null;
			}
			
			BindOnDot(effect.transform, info, tran);
			effect.SetActive(true);
			SetEffectLiveTime(effect, info);
			return effect;
		}
		else if ((EffectTypes)info.EffectType == EffectTypes.Bullet ||
		         (EffectTypes)info.EffectType == EffectTypes.Missile)
		{
			GameObject effect = CreateEffect(info.ArtRes, info.Save);
			if (effect == null)
			{
				Debug.Log("Effect is not exist... " + info.ArtRes);
				return null;
			}
			BindOnDot(effect.transform, info, tran);
			return effect;
		}
		
		return null;
	
	}


	public GameObject CreateEffect(int id ,Vector3 pos){
		EffectData info = DataManager.s_EffectDataManager.GetData (id);

		if (info == null) {
			return null;
		}

		GameObject effect = CreateEffect (info.ArtRes, info.Save);
		if (effect == null) {
			
			Debug.LogError ("Effect is not exit..." + info.ArtRes);
			return null;
		}

		effect.transform.position = pos;
		effect.SetActive (true);
		SetEffectLiveTime (effect,info);
		return effect;
	}

	public GameObject CreateEffect(int id){
		EffectData info = DataManager.s_EffectDataManager.GetData (id);
		if (info == null) {
			return null;
		}

		GameObject effect = CreateEffect (info.ArtRes, info.Save);
		if (effect == null) {

			Debug.LogError ("Effect is not exit..." + info.ArtRes);
			return null;
		}

		effect.SetActive (true);
		return effect;
	}


	public GameObject CreateEffect(string name,bool isSave){
		GameObject effect = null;
		if(isSave){
			if(!m_EffectPoolList.ContainsKey(name)){
				m_EffectPoolList.Add(name,new List<GameObject>());
			}else if(m_EffectPoolList[name].Count>0){
				effect = m_EffectPoolList[name][m_EffectPoolList[name].Count - 1];
				m_EffectPoolList[name].RemoveAt(m_EffectPoolList[name].Count - 1);
				return effect;
			}
			//Debug.Log
		}

		GameObject prefab = ResourcesManager.Instance.LoadEffect (name);
		if (prefab != null) {
			effect = GameObject.Instantiate(prefab, Vector3.one * 1000, prefab.transform.localRotation) as GameObject;
			effect.transform.parent = m_EffectParent.transform;
			effect.name = name;
		}
		return effect;
	}

	/// <summary>
	/// 绑定特效点;
	/// </summary>
	/// <param name="effectTran"></param>
	/// <param name="info"></param>
	/// <param name="tran"></param>
	void BindOnDot(Transform effectTran,EffectData info,Transform tran){
		if (effectTran == null || tran == null || info == null) {
			return;
		}
		if (info.Dot == null || info.Dot.Length == 0) {
			effectTran.parent = tran;
			effectTran.localPosition = Vector3.zero;
		} else {
			EffectDot effectDot = tran.GetComponent<EffectDot>();
			if(effectDot == null){
				effectTran.parent = tran;
			}else{
				if(info.Bind){
					Transform dot = effectDot.GetEffectDot(info.Dot[0]);
					if(dot== null){
						Debug.LogError(info.Dot[0]+" is null");
						return;
					}
					effectTran.parent = dot;
					effectTran.localPosition = Vector3.zero;
					if(info.BindRotate){
						effectTran.rotation = tran.rotation;
					}
				}else{
					Transform dot = effectDot.GetEffectDot(info.Dot[0]);
					if(dot == null){
						Debug.LogError(info.Dot[0]+" is null");
						return;
					}

					effectTran.position = dot.position;
					if(info.BindRotate){
						effectTran.rotation = tran.rotation;
					}
				}
			}

		}
	}

	private void SetEffectLiveTime(GameObject effect,EffectData info){
		if (info.Save) {
			if(info.LiveTime != 0){
				TimerManager.Instance.AddTimer("Battle"+effect.GetInstanceID().ToString(),info.LiveTime,ResaveEffect,effect);
			}
		}

	}

	private void ResaveEffect(params object[] args){
		if (args == null) {
			return;
		}

		GameObject effect = args [0] as GameObject;
		if (effect != null) {
			effect.SetActive(false);
			effect.transform.parent = m_EffectParent.transform;
			m_EffectPoolList[effect.name].Add(effect);
		}
	}










}
