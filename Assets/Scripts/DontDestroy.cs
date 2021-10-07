using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour {
    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }
}
