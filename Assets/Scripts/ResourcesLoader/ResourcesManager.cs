using UnityEngine;
using System.Collections;

public class ResourcesManager {
    private static ResourcesManager _Instance = null;
    public static ResourcesManager Instance {
        get {
            if (_Instance == null) {
                _Instance = new ResourcesManager();
            }
            return _Instance;
        }
    }
    private string uiPrefabPath = "UI/Panel";
    public GameObject GetUIPrefab(string name) {
        return LoadPrefab(name,uiPrefabPath);
    }

    public GameObject LoadPrefab(string name, string path) {
        string loadPath = path + "/" + name;
        GameObject prefab = Resources.Load(loadPath, typeof(GameObject)) as GameObject;
        if (prefab == null) {
            Debug.LogError("prefab is not exist:" + loadPath);
        }

        return prefab;
    }

	private string XMLPath = "Config";
	public TextAsset LoadConfigXML(string name){
		return LoadXMLAsset (name, XMLPath);
	}

	public TextAsset LoadXMLAsset(string name , string path){
		TextAsset textAsset = null;
		string loadPath = path + "/" + name;
		textAsset = Resources.Load (loadPath, typeof(TextAsset)) as TextAsset;

		if (textAsset == null) {
			Debug.LogError(loadPath+" is null");
			return null;
		}
		return textAsset;

	}


	public GameObject LoadEffect(string name)
	{
		return LoadPrefab(name, "Effect");
	}
	
	public GameObject CreateEffect(string name)
	{
		GameObject effect = LoadEffect(name);
		effect = GameObject.Instantiate(effect) as GameObject;
		effect.transform.position = new Vector3(1000,1000,1000);
		
		return effect;
	}
	
	public GameObject CreateEffect(string name, Vector3 pos)
	{
		GameObject effect = LoadEffect(name);
		effect = GameObject.Instantiate(effect, pos, Quaternion.identity) as GameObject;
		
		return effect;
	}


	public Texture2D GetCapital(string name)
	{
		return LoadTexture(name, "UI/Icon/Capital");
	}
	public Texture2D GetItem(string name)
	{
		return LoadTexture(name, "UI/Icon/Item");
	}
	
	public Texture2D LoadTexture(string name, string path)
	{

			string loadPath = path + "/" + name;
			Texture2D tex = Resources.Load(loadPath, typeof(Texture2D)) as Texture2D;
			if (tex == null)
			{
				Debug.LogError("Texture2D " + name + " is not found in Resources");
			}
			
			return tex;

	}
	/// <summary>
	///  加载单位;
	/// </summary>
	/// <param name="name"></param>
	/// <param name="unitType"></param>
	/// <returns></returns>
	public GameObject LoadUnitObject(string name, UNIT_TYPE unitType)
	{
		GameObject unit = null;
		switch(unitType)
		{
		case UNIT_TYPE.WARRIOR:
		case UNIT_TYPE.HERO:
			unit = LoadPrefab(name, COMMDEF.RESPATH_WARRIOR);
			break;
		case UNIT_TYPE.NPC:
		case UNIT_TYPE.ATTACK_NPC:
			unit = LoadPrefab(name, COMMDEF.RESPATH_NPC);
			break;	
		default:
			unit = LoadPrefab(name, COMMDEF.RESPATH_ACT);
			break;
		}
		
		return unit;
	}

	/// <summary>
	/// 获取武将Icon资源
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	public Texture2D GetWarriorIcon(string name)
	{
		return LoadTexture(name, COMMDEF.RESPATH_UI_WARRIOR_ICON);
	}
}
