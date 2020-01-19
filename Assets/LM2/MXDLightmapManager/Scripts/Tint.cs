using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class Tint : MonoBehaviour
{

    public delegate void ObjectTint();
    public event ObjectTint TintUp;
    public enum AdjustTint { Far, Near, Both, off };
    public enum ExecuteON { Start, Awake, Trigger };
    public enum ExecuteONLite { Start, Awake, Trigger };
    public Color TintColor;
    public AdjustTint options;
    public ExecuteON executeTrigger;
   // private bool IsExecuting = false;

    public void TintLightmap()
    {
        if (TintUp != null)
            TintUp();
    }
    // Use this for initialization
    void Start()
    {

        if (executeTrigger == ExecuteON.Start)
        {
            AdjustLightmapTint(TintColor, options);
        }
        if (executeTrigger == ExecuteON.Trigger)
        {
            TintUp += new ObjectTint(Lightmap_Tint);
        }
    }

    void Lightmap_Tint()
    {
        AdjustLightmapTint(TintColor, options);
    }

    void Awake()
    {
        if (executeTrigger == ExecuteON.Awake)
        {
            AdjustLightmapTint(TintColor, options);
        }

    }
  

    public void AdjustLightmapTint(Color val)
    { 
            if (this.gameObject.isStatic && this.GetComponent<Renderer>() != null)
            {
                SetTint(LightmapSettings.lightmaps[this.GetComponent<Renderer>().lightmapIndex].lightmapColor, val);
                SetTint(LightmapSettings.lightmaps[this.GetComponent<Renderer>().lightmapIndex].lightmapDir, val);
            }
           
    }
    public void AdjustLightmapTint(Color val, AdjustTint options)
    {
          if (this.gameObject.isStatic && this.GetComponent<Renderer>() != null)
            {
                if (options == AdjustTint.Both)
                {
                    SetTint(LightmapSettings.lightmaps[this.GetComponent<Renderer>().lightmapIndex].lightmapColor, val);
                    SetTint(LightmapSettings.lightmaps[this.GetComponent<Renderer>().lightmapIndex].lightmapDir, val);
                }
                else if (options == AdjustTint.Far)
                {
                    SetTint(LightmapSettings.lightmaps[this.GetComponent<Renderer>().lightmapIndex].lightmapColor, val);

                }
                else if (options == AdjustTint.Near)
                {
                    SetTint(LightmapSettings.lightmaps[this.GetComponent<Renderer>().lightmapIndex].lightmapDir, val);

                }
            }
           
     
    }
    void SetTint(Texture2D tex, Color val)
    {

        if (tex != null)
        {
            var original = tex.GetPixels();
            Color[] colors = new Color[original.Length];
            for (int i = 0; i < original.Length; i++)
            {


                UnityEngine.Color c = original[i] * val;              

              
                colors[i] = new UnityEngine.Color(c.r, c.g, c.b, c.a);
                //        tex.SetPixel(x, y, new UnityEngine.Color(r, g, c.b, c.a));


            }
            tex.SetPixels(colors);
            tex.Apply();
        }
    }

}
