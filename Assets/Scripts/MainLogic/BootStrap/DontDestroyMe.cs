using UnityEngine;
using System.Collections;

public class DontDestroyMe : MonoBehaviour {
    void Awake() {
        DontDestroyOnLoad(transform.gameObject);
    }
}
