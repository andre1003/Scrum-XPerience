using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideControllerForStats : MonoBehaviour {
    public GameObject canvas;
    public ChoiceController choiceController;

    private void OnTriggerEnter(Collider other) {
        if(other.GetComponent<PhotonView>().isMine) {
            choiceController.LockOrUnlockPlayer();
            choiceController.GetStats();
            canvas.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
