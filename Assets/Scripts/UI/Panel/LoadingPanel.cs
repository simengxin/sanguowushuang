using UnityEngine;
using System.Collections;

public class LoadingPanel : IView {

    protected override void OnStart()
    {
        Debug.LogError("LoadingPanel onstart");
        DownloadManager.Instance.UpdateProgressEvent += UpdateProgressEvent;
    }

    private void UpdateProgressEvent(float progress, string fileName)
    { 
    
    }
    protected override void OnShow()
    {
        
    }

    protected override void OnHide()
    {
        
    }
    public override void Update() {
        //float x = DownloadManager.Instance.DownloadProgress;
        //Debug.LogError("x" + x);
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
       
    }
}
