using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideControllerForStats : MonoBehaviour {
    public GameObject canvas;
    public ChoiceController choiceController;

    private void OnTriggerEnter(Collider other) {
        choiceController.LockOrUnlockPlayer(); // TEST-ONLY METHOD CALL
        choiceController.GetStats();
        canvas.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
