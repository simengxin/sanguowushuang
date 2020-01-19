using UnityEngine;
using System.Collections;

public class GameMain : MonoBehaviour {
    public bool isLocal = false;
	void Start () {
        AddSomeComponent();
	}

    private void DoSomeSetting() {
        Application.targetFrameRate = 40;

        Application.runInBackground = true;
    }

    private void AddSomeComponent() {
        gameObject.AddComponent<DownloadManager>();
        gameObject.AddComponent<GameStateManager>();
        gameObject.AddComponent<TimerManager>();

    }


	
	void Update () {
        GUIManager.Update();
	}
}
