using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollideController : Photon.MonoBehaviour {

    public GameObject choiceCanvas;
    public ChoiceController choiceController;
    //public GameController gameController;
    public string scene;

    private bool nothingToDo;

    private void OnTriggerEnter(Collider other) {
        if(other.GetComponent<PhotonView>().isMine) {
            if(!choiceController.GetPassedScenesList().Contains(scene)) {
                choiceController.scene = scene;
                nothingToDo = choiceController.GetChoices();

                if(!nothingToDo) {
                    choiceController.LockOrUnlockPlayer();
                    choiceCanvas.SetActive(true);
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
                }

            }
        }
    }
}
