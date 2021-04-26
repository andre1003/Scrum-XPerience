using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollideController : MonoBehaviour {

    public GameObject choiceCanvas;
    public ChoiceController choiceController;
    //public GameController gameController;
    public string scene;

    private void OnTriggerEnter(Collider other) {
        //gameController.LockOrUnlockPlayer();
        if(!choiceController.GetPassedScenesList().Contains(scene)) {
            choiceController.LockOrUnlockPlayer(); // TEST-ONLY METHOD CALL
            choiceController.scene = scene;
            choiceController.GetChoices();
            choiceCanvas.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
