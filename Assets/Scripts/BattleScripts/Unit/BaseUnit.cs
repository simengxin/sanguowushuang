using UnityEngine;
using System.Collections;

public class BaseUnit  {
	public GameObject m_UnitObject;
	/// <summary>
	/// 名称;
	/// </summary>
	/// <returns></returns>
	protected string m_Name = "";
	public virtual string Name{
		get{
			return m_Name;
		}

		set{
			m_Name = value;
		}
	}
	/// <summary>
	/// 等级;
	/// </summary>
	/// <returns></returns>
	protected int m_Level;
	public virtual int Level
	{
		get {return m_Level;}
		set { m_Level = value;}
	}
	
	/// <summary>
	/// 高度;
	/// </summary>
	/// <returns></returns>
	protected float m_Height;
	public virtual float Height
	{
		get {return m_Height;}
		set { m_Height = value;}
	}
	
	/// <summary>
	/// 半径;
	/// </summary>
	/// <returns></returns>
	protected float m_Radius;
	public virtual float Radius
	{
		get {return m_Radius;}
		set { m_Radius = value;}
	}
	
	/// <summary>
	/// 移动速度;
	/// </summary>
	/// <returns></returns>
	protected float m_Speed;
	public virtual float Speed
	{
		get {return m_Speed;}
		set { m_Speed = value;}
	}
	
	public  void Init()
	{
	}
	
	public  void PropertyCallback(string strPropName)
	{
		
	}
	
	//public  void RecordCallback(string strRecName, FlexNet.E_Record_Ctrl_Type eType, int iParam1, int iParam2)
	//{
		
	//}
	
	public  void Destroy()
	{
		
	}

}
