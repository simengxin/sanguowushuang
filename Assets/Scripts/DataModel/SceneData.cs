using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class SceneData {
	public int ID;
	public string Name;
	public string LevelName;
	public string GameState;
}
public class SceneDataManager{
	private Dictionary<int,SceneData> m_SceneDataDic = null;

	public SceneData GetData(int key){
		if (m_SceneDataDic == null) {
			LoadSceneData();
		}
		return m_SceneDataDic.ContainsKey (key) ? m_SceneDataDic [key] : null;
	}

	public void LoadSceneData(){
		m_SceneDataDic = new Dictionary<int, SceneData> ();
		string textAsset = ResourcesManager.Instance.LoadConfigXML ("SceneData").text;
		
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml (textAsset);
		XmlNode equipXN = xmlDoc.SelectSingleNode("SceneDatas");

		XmlNodeList list = equipXN.ChildNodes;
		if (list != null && list.Count > 0) {
			foreach (XmlNode node in list) {
				XmlElement element = node as XmlElement;
				if(element.Name.Equals("SceneData")){
					SceneData info = new SceneData();

					info.ID = CommonHelper.Str2Int(element.GetAttribute("ID"));
					info.Name = element.GetAttribute("Name");
					info.LevelName = element.GetAttribute("LevelName");
					info.GameState = element.GetAttribute("GameState");

					if(!m_SceneDataDic.ContainsKey(info.ID)){
						m_SceneDataDic.Add(info.ID,info);
					}
				}
			}
		}
	}

}