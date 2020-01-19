using UnityEngine;
using System.Collections;

public class CamerSet : MonoBehaviour {
	public float Width = 1280.0f;
	public float Height = 720.0f;
	
	// Update is called once per frame
	void Update () {
		if (Screen.width / Screen.height >= Width / Height) {
			GetComponent<Camera>().orthographicSize = Height / Screen.height;
		} else {
			GetComponent<Camera>().orthographicSize = Width/Screen.width;
		}
	}
}
