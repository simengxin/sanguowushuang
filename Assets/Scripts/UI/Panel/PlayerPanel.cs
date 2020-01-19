using UnityEngine;
using System.Collections;

public class PlayerPanel : IView
{

    protected override void OnStart()
    {
        Debug.LogError("PlayerPanel onstart");
        
    }

    protected override void OnShow()
    {
        
    }

    protected override void OnHide()
    {
        
    }

    protected override void OnDestory()
    {
        
    }
    protected override void OnDrag(GameObject sender, object param)
    {
        
    }

    protected override void OnPress(GameObject sender, object param)
    {
        
    }

    protected override void OnClick(GameObject sender, object param)
    {
		if (sender.name.Equals("FightBtn"))
		{
			OnClickFightBtn();
		}

    }

	private void OnClickFightBtn()
	{
		//DungeonLogic.LoadDungeon(1);
		GUIManager.ShowView("MapPanel");
	}
}
