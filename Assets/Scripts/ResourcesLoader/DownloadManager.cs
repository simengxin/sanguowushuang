using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
public class DownloadManager : MonoBehaviour {
    private static DownloadManager _Instance = null;
    public static DownloadManager Instance {
        get {
            if (_Instance == null) {
                _Instance = FindObjectOfType(typeof(DownloadManager)) as DownloadManager;
            }

            return _Instance;
        }
    }
    private float m_DownProgress = 0.0f;

    /// <summary>
    /// 加载进度
    /// </summary>
    public float DownloadProgress
    {
        get
        {
            return (int)(m_DownProgress * 100) / 100.0f;
        }
    }

    //进度条委托
    public delegate void DelegateProgress(float pregress, string fileName);
    public event DelegateProgress UpdateProgressEvent;

    public delegate void LoadCallBack(params object[] args);
    public void LoadScene(string name, LoadCallBack loadHandler, params object[] args) {
        StartCoroutine(LoadSceneBundle(name, loadHandler, args));
    }
    AsyncOperation async = null;
    private IEnumerator LoadSceneBundle(string name, LoadCallBack loadHandle, params object[] args) {
        async = SceneManager.LoadSceneAsync(name);
        yield return async;
        Resources.UnloadUnusedAssets();
        GC.Collect();
        Debug.Log(name+"  Scene is loaded");
        if (loadHandle != null) {
            loadHandle(args);
        }
        async = null;
    }

	void Start () {
	
	}
	

	void Update () {
        if (async != null)
        {
            m_DownProgress = (async.progress);

            if (UpdateProgressEvent != null)
            {
                UpdateProgressEvent(m_DownProgress,"");
                //Debug.LogError("m_DownProgress:" + m_DownProgress);
            }
        }
	}
}
