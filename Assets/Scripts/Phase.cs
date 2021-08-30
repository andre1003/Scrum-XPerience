using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase : MonoBehaviour {
    public int phase;
    public TutorialController tutorialController;

    private void OnTriggerEnter(Collider other) {
        tutorialController.PhaseSwitch(phase);
    }
}
