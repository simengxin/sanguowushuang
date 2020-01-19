using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Blend))]
public class BlendEditor : Editor
{
   // int selected = 4;
    public override void OnInspectorGUI()
    {
        Blend obj = (Blend)target;
		
			GUILayout.BeginHorizontal();
		if(obj.GetComponent<Renderer>().lightmapIndex<LightmapSettings.lightmaps.Length && obj.GetComponent<Renderer>().lightmapIndex>=0)
		{
      if(!LightmapManager.isReadable(LightmapSettings.lightmaps[obj.GetComponent<Renderer>().lightmapIndex].lightmapColor))
		{
		if(GUILayout.Button("Make Readable (Far)"))
			LightmapManager.SetIsReadable(LightmapSettings.lightmaps[obj.GetComponent<Renderer>().lightmapIndex].lightmapColor);
		}
		
		 if(!LightmapManager.isReadable(LightmapSettings.lightmaps[obj.GetComponent<Renderer>().lightmapIndex].lightmapDir))
		{
		if(GUILayout.Button("Make Readable (Near)"))
			LightmapManager.SetIsReadable(LightmapSettings.lightmaps[obj.GetComponent<Renderer>().lightmapIndex].lightmapDir);
		}
		}
       GUILayout.EndHorizontal();
		 GUILayout.Label("Operate on (GPU/CPU)");
        obj.operateOn =(Blend.OperateON) EditorGUILayout.EnumPopup(obj.operateOn);
		
        GUILayout.Label("Blend with");
        obj.BlendTexture =(Texture2D) EditorGUILayout.ObjectField(obj.BlendTexture, typeof(Texture2D),false,GUILayout.Height(75),GUILayout.Width(75));
        GUILayout.Label("Blend Level");
        obj.level = EditorGUILayout.Slider(obj.level, -1, 1);
        GUILayout.Label("Lightmap Options");
        obj.options = (Brightness.AdjustLightmap)EditorGUILayout.EnumPopup(obj.options);
        GUILayout.Label("Execute ON");
        obj.executeTrigger = (Brightness.ExecuteON)EditorGUILayout.EnumPopup(obj.executeTrigger);
		
		obj.useSharedMaterials = EditorGUILayout.Toggle("Use Shared Materials",obj.useSharedMaterials);
		obj.timeScale = EditorGUILayout.FloatField("Time Scale",obj.timeScale);
		obj.useCurve = EditorGUILayout.Toggle("Use Curve",obj.useCurve);
		if(obj.useCurve)
		{
			obj.curve = EditorGUILayout.CurveField("Curve",obj.curve);
			obj.loopCurve = EditorGUILayout.Toggle("Loop Curve",obj.loopCurve);
			obj.startOnFirstKey = EditorGUILayout.Toggle("Start On 1st Key",obj.startOnFirstKey);
			
			if(obj.timeScale<0)
				obj.timeScale = 0;
		}
			else
		{
        GUILayout.Label("Minimum Change");
        obj.Min = EditorGUILayout.Slider(obj.Min, -1, 1);
        GUILayout.Label("Maximum Maximum");
        obj.Max = EditorGUILayout.Slider(obj.Max, -1, 1);
		}
    }
}
