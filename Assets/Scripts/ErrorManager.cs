using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class ErrorManager : MonoBehaviour {
    public ChoiceController choiceController;
    public PhotonView photonView;

    public GameObject outputCanvas;
    public Text outputText;

    private string path;

    private int timeouts = 0;
    private int teamMeetingMistakes = 0;
    private int clientMeetingMistakes = 0;
    private int developmentMistakes = 0;
    private int teamMeetingHits = 0;
    private int clientMeetingHits = 0;
    private int developmentHits = 0;

    private void Awake() {
        path = Application.persistentDataPath + "/player_data/";
    }

    private void Update() {
        //Debug.Log("Client Mistakes: " + clientMeetingMistakes);
    }

    // Check if the choice is a hit or a mistake and save it in the respective file
    public void CheckHit(Text buttonId, List<Decision> data, int index) {
        Decision decision;

        if(buttonId.text.Equals(data[0].decisionId)) {
            choiceController.IncreaseIndividualHits();
            decision = data[0];
        }
        else {
            choiceController.IncreaseIndividualMistakes();

            if(buttonId.text.Equals(data[1].decisionId)) {
                decision = data[1];
            }
            else {
                decision = data[2];
            }
        }

        outputText.text = decision.output;

        SaveSystem.Save(decision.decisionId, decision.decisionDescription, decision.scenery, "", decision.isMistake, index);
        photonView.RPC("SaveGeneralInfo", PhotonTargets.AllBuffered, decision.scenery, decision.isMistake);
        
        outputCanvas.SetActive(true);
    }

    [PunRPC]
    private void SaveGeneralInfo(string scene, bool isMistake) {
        if(isMistake) {
            if(scene.Equals("Reuniao Equipe")) {
                Increment(0);
            }
            else if(scene.Equals("Reuniao Cliente")) {
                Increment(1);
            }
            else {
                Increment(2);
            }
        }
        else {
            if(scene.Equals("Reuniao Equipe")) {
                Increment(3);
            }
            else if(scene.Equals("Reuniao Cliente")) {
                Increment(4);
            }
            else {
                Increment(5);
            }
        }

        //SaveGeneralInfo();
    }
    private void Increment(int option) {
        switch(option) {
            case 0:
                teamMeetingMistakes++;
                break;

            case 1:
                clientMeetingMistakes++;
                break;

            case 2:
                developmentMistakes++;
                break;

            case 3:
                teamMeetingHits++;
                break;

            case 4:
                clientMeetingHits++;
                break;

            case 5:
                developmentHits++;
                break;

            case 6:
                timeouts++;
                break;
        }
    }

    public void SaveGeneralInfo() {
        SaveSystem.SaveGeneralInfo(choiceController.GetTimeouts(), teamMeetingMistakes, clientMeetingMistakes, developmentMistakes, teamMeetingHits, clientMeetingHits, developmentHits);
    }
}