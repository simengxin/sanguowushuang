using UnityEngine;
using UnityEditor;
using System.Collections;

public class SceneInfo : EditorWindow 
{
	private Bounds bounds;
	private bool initialized = false;
	
	[MenuItem("GameEditor/SceneInformation")]
	static void Init() 
	{
		EditorWindow.GetWindow(typeof(SceneInfo));
	}
	
	void OnGUI() 
	{
		if (GUILayout.Button("Calculate")) 
		{
			GetSceneInfo();
		}
		
		if (initialized)
		{
			Vector3 size = (bounds.max - bounds.min);
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("Origin:\t");
			EditorGUILayout.SelectableLabel(bounds.min.ToString());
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("Size:\t");
			EditorGUILayout.SelectableLabel(size.ToString());
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("X_Origin:\t");
			EditorGUILayout.SelectableLabel(bounds.min.x.ToString());
			GUILayout.Label("Y_Origin:\t");
			EditorGUILayout.SelectableLabel(bounds.min.z.ToString());
			GUILayout.EndHorizontal();
		
			GUILayout.BeginHorizontal();
			GUILayout.Label("X_Center:\t");
			EditorGUILayout.SelectableLabel(bounds.center.x.ToString());
			GUILayout.Label("Y_Center:\t");
			EditorGUILayout.SelectableLabel(bounds.center.z.ToString());
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("Width:\t");
			EditorGUILayout.SelectableLabel(size.x.ToString());
			GUILayout.Label("Height:\t");
			EditorGUILayout.SelectableLabel(size.z.ToString());
			GUILayout.EndHorizontal();
		}
	}
	
	void GetSceneInfo()
	{
		Renderer[] renderers = Object.FindObjectsOfType(typeof(Renderer)) as Renderer[];
		bounds = new Bounds();
		
		foreach (Renderer renderer in renderers)
		{
			bounds.Encapsulate(renderer.bounds);
		}
		
		initialized = true;
	}
	
}
