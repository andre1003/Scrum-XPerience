using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour {
    public MovementController movementController;
    public MouseController mouseController;

    // Start is called before the first frame update
    void Start() {
        //LockAndHideCursor();
        LockOrUnlockPlayer();
    }

    // Update is called once per frame
    void Update() {

    }

    //public void SetMovementController(MovementController movementController) {
    //    this.movementController = movementController;
    //}

    //public void SetMouseController(MouseController mouseController) {
    //    this.mouseController = mouseController;
    //}

    public void LockOrUnlockPlayer() {
        movementController.enabled = !movementController.enabled;
        mouseController.enabled = !mouseController.enabled;
    }

    public void LockAndHideCursor() {
        if(Cursor.visible) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }

    public void PhaseSwitch(int phase) {
        switch(phase) {
            default:
                Debug.Log("Fase 1 concluída");
                break;
        }
    }
}
