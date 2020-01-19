using UnityEngine;
using System.Collections;

public static class TypeExtention {

	public static void SetLayerRecursively(this GameObject go, int layer, bool ignoreEffect = false) 
	{
		SetLayerRecursivelyInternal(go, layer, ignoreEffect);
	}
	
	private static void SetLayerRecursivelyInternal(GameObject go, int layer, bool ignoreEffect) 
	{
		if (ignoreEffect && (go.tag == "Effect" || go.GetComponent<ParticleSystem>() != null))
		{
			return;
		}
		go.layer = layer;
		
		
		foreach (Transform child in go.transform) 
		{
			if (ignoreEffect && (child.gameObject.tag == "Effect" || child.gameObject.GetComponent<ParticleSystem>() != null))
			{
				continue;
			}
			child.gameObject.layer = layer;
			SetLayerRecursivelyInternal(child.gameObject, layer, ignoreEffect);
		}
	}
	
	static public Transform FindRecursively(this Transform root, string name)
	{
		if (root.name == name)
		{
			return root;
		}
		
		foreach (Transform child in root)
		{
			Transform t = FindRecursively(child, name);
			
			if (t != null)
			{
				return t;
			}
		}
		
		return null;
	}

	public static T FindBehaviour<T>(this Transform root, string name) where T : MonoBehaviour
	{
		Transform child = FindRecursively(root, name);
		
		if (child == null)
		{
			return null;
		}
		
		T temp = child.GetComponent<T>();
		if (temp == null)
		{
			Debug.LogError(name + " is not has component ");
		}
		
		return temp;
	}
	
	static public void ScaleParticleSystem(this Transform root, Vector3 scale)
	{
		ScaleParticleSystemRecursivelyInternal(root, scale);
	}
	
	static private void ScaleParticleSystemRecursivelyInternal(this Transform root, Vector3 scale)
	{
		ParticleSystem particleSystem = root.GetComponent<ParticleSystem>();
		
		if (particleSystem != null)
		{
			particleSystem.startSize *= scale.x;
			particleSystem.startSpeed *= scale.x;
		}
		
		foreach (Transform child in root)
		{
			ScaleParticleSystemRecursivelyInternal(child, scale);
		}
	}
	
	static public bool TryParseColor(string val, out Color clr)
	{
		clr = Color.black;
		
		val = val.Replace('\"', ' ');
		val = val.Trim();		
		
		string[] splits = val.Split(',');
		
		if (splits.Length != 4)
			return false;
		
		try 
		{
			clr.r = float.Parse(splits[0]);
			clr.g = float.Parse(splits[1]);
			clr.b = float.Parse(splits[2]);
			clr.a = float.Parse(splits[3]);			
		} 
		catch
		{
			return false;
		}
		
		return true;
	}
	
	static public bool TryParseVector2(string val, out Vector2 vec2)
	{
		vec2 = Vector2.zero;
		
		val = val.Replace('\"', ' ');
		val = val.Trim();
		
		string[] splits = val.Split(',');
		
		if (splits.Length != 2)
			return false;
		
		try 
		{
			vec2.x = float.Parse(splits[0]);
			vec2.y = float.Parse(splits[1]);
		} 
		catch
		{
			return false;
		}
		
		return true;
	}
	
	static public bool TryParseVector3(string val, out Vector3 vec3)
	{
		vec3 = Vector3.zero;
		
		val = val.Replace('\"', ' ');
		val = val.Trim();
		
		string[] splits = val.Split(',');
		
		if (splits.Length != 3)
			return false;
		
		try 
		{
			vec3.x = float.Parse(splits[0]);
			vec3.y = float.Parse(splits[1]);
			vec3.z = float.Parse(splits[2]);
		} 
		catch
		{
			return false;
		}
		
		return true;
	}

	static public bool TryParseVector4(string val, out Vector4 vec4)
	{
		vec4 = Vector4.zero;
		
		val = val.Replace('\"', ' ');
		val = val.Trim();
		
		string[] splits = val.Split(',');
		
		if (splits.Length != 4)
			return false;
		
		try 
		{
			vec4.x = float.Parse(splits[0]);
			vec4.y = float.Parse(splits[1]);
			vec4.z = float.Parse(splits[2]);
			vec4.w = float.Parse(splits[3]);			
		} 
		catch
		{
			return false;
		}
		
		return true;
	}
	
	static public bool TryParseQuaternion(string val, out Quaternion quat)
	{
		quat = Quaternion.identity;
		
		val = val.Replace('\"', ' ');
		val = val.Trim();
		
		string[] splits = val.Split(',');
		
		if (splits.Length != 4)
			return false;
		
		try 
		{
			quat.x = float.Parse(splits[0]);
			quat.y = float.Parse(splits[1]);
			quat.z = float.Parse(splits[2]);
			quat.w = float.Parse(splits[3]);			
		} 
		catch
		{
			return false;
		}
		
		return true;
	}	
}
