using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackPasswordHandler : MonoBehaviour {
    private void Awake() {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
