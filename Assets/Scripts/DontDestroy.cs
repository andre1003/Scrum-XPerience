using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour {
    private void Awake() {
        // Fixes needed: When going to main scene it does not destroy
        DontDestroyOnLoad(this.gameObject);
    }
}
