using UnityEngine;
using System.Collections;

public class LoginPanel : IView
{

    protected override void OnStart()
    {
        Debug.LogError("LoginPanel onstart");
        
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
        Debug.LogError("1111");
		if (sender.name.Equals("BtnEnter"))
        {
            Debug.LogError("OnClick");
            GameStateManager.LoadScene(2);
        }
    }
}
