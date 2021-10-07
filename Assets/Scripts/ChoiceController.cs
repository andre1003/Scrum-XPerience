using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class ChoiceController : MonoBehaviour {
    public string scene;

    public int maxMistakes;

    public List<Text> buttonList;
    public List<Text> descriptionList;

    public Text descriptionText;

    public Text sceneName;
    public Text functionName;
    public Text scoreText;

    public Text teamSatisfactionText;
    public Text clientSatisfactionText;
    public Text progressText;
    public Text outputText;

    public ErrorManager errorManager;

    public GameObject outputCanvas;

    public NewTimer timeController;

    private MovementController movementController;
    private MouseController mouseController;

    private string function;

    private int rightChoice;
    private int individualHits;
    private int individualMistakes;
    private int totalChoices;

    private int groupHits;
    private int groupMistakes;

    private int round;
    private int turn;

    private bool nothingToDo = false;

    private List<string> passedScenes;

    private Color green = new Color(6, 142, 0, 255);
    private Color red = new Color(226, 0, 0, 255);

    public PhotonView photonView;

    private bool mistake1 = false;
    private bool passedInClientMeetingRoom = false;

    private int count = 0;
    private int timeouts = 0;

    private List<Decision> data;

    private void Awake() {
        individualHits = 0;
        individualMistakes = 0;
        groupHits = 0;
        groupMistakes = 0;
        totalChoices = 0;
        passedScenes = new List<string>();

        if(PhotonNetwork.player.IsMasterClient) {
            ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();

            properties.Add("GeneralClientMeetingMistakes", 0);
            properties.Add("GeneralTeamMeetingMistakes", 0);
            properties.Add("GeneralDevelopmentMistakes", 0);
            properties.Add("GeneralTimeouts", 0);

            properties.Add("GeneralClientMeetingHits", 0);
            properties.Add("GeneralTeamMeetingHits", 0);
            properties.Add("GeneralDevelopmentHits", 0);

            PhotonNetwork.room.SetCustomProperties(properties);
        }
    }

    private void Update() {
        photonView.RPC("CheckGameOver", PhotonTargets.AllBuffered);

        //if(Input.GetKeyDown(KeyCode.Space))
        //    GetStats();
        //else if(Input.GetKeyDown(KeyCode.P))
        //    EndGame();
    }

    public bool GetChoices() {
        // Fixed needed: functions that don't talk with the client won't be able to play the first round
        mistake1 = false;

        //if(!scene.Equals("Reuniao Cliente") && !passedInClientMeetingRoom && round == 1) {
        //    return true;
        //}
        //else if(scene.Equals("Reuniao Cliente")) {
        //    passedInClientMeetingRoom = true;
        //}

        data = SaveSystem.LoadFromDatabase(scene, function, turn.ToString(), round.ToString());

        if(data != null) {
            nothingToDo = false;

            sceneName.text = scene;
            functionName.text = function;

            System.Random rdn = new System.Random();
            int lenght = buttonList.Count;
            rightChoice = rdn.Next(0, 3);
            Debug.Log(rightChoice);

            for(int i = 0; i < lenght; i++) {
                if(i == rightChoice) {
                    ChangeTextByFunction(buttonList[i], descriptionList[i], data[0]);
                }
                else if(!mistake1) {
                    ChangeTextByFunction(buttonList[i], descriptionList[i], data[1]);
                    mistake1 = true;
                }
                else {
                    ChangeTextByFunction(buttonList[i], descriptionList[i], data[2]);
                }
            }
        }
        else {
            nothingToDo = true;
        }

        return nothingToDo;
    }

    private void ChangeTextByFunction(Text buttonText, Text descriptionText, Decision decision) {
        buttonText.text = decision.decisionId;
        descriptionText.text = decision.decisionDescription;
    }

    public void SetMovementController(MovementController movementController) {
        this.movementController = movementController;
    }

    public void SetMouseController(MouseController mouseController) {
        this.mouseController = mouseController;
    }

    public void SetFunction(string function) {
        this.function = function;
    }

    public void MouseOver(Text choiceText) {
        if(choiceText.text.Equals(data[0].decisionId)) {
            // Hit
            descriptionText.text = data[0].decisionDescription;
        }
        else if(choiceText.text.Equals(data[1].decisionId)) {
            // Mistake 1
            descriptionText.text = data[1].decisionDescription;
        }
        else {
            descriptionText.text = data[2].decisionDescription;
        }
    }

    public void LockOrUnlockPlayer() {
        movementController.enabled = !movementController.enabled;
        mouseController.enabled = !mouseController.enabled;
    }

    public void LockAndHideCursor() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void CheckHit(Text buttonId) {
        errorManager.CheckHit(buttonId, data, totalChoices);
    }

    public void IncreaseIndividualHits() {
        totalChoices++;
        individualHits++;
        // TEST ONLY
        //groupHits++;
        ////////////
        photonView.RPC("IncreaseGroupHits", PhotonTargets.AllBuffered);
        photonView.RPC("UpdateScore", PhotonTargets.AllBuffered);
    }

    public void IncreaseIndividualMistakes() {
        totalChoices++;
        individualMistakes++;
        // TEST ONLY
        //groupMistakes++;
        ////////////
        photonView.RPC("IncreaseGroupMistakes", PhotonTargets.AllBuffered);
        photonView.RPC("UpdateScore", PhotonTargets.AllBuffered);
    }

    [PunRPC]
    private void IncreaseGroupHits() {
        groupHits++;
    }

    [PunRPC]
    private void IncreaseGroupMistakes() {
        groupMistakes++;
    }

    [PunRPC]
    private void CheckGameOver() {
        if(groupMistakes >= maxMistakes) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            movementController.enabled = false;
            mouseController.enabled = false;
            timeController.GameOver();
        }
        else {
            
        }
    }

    [PunRPC]
    public void UpdateScore() {
        scoreText.text = "Acertos: " + groupHits + "\nErros: " + groupMistakes;
    }

    public List<string> GetPassedScenesList() {
        return passedScenes;
    }

    public void AddPassedScene() {
        passedScenes.Add(scene);
    }

    void ClearPassedScenesList() {
        passedScenes.Clear();
    }

    public void SetRound(int round) {
        this.round = round;
    }

    public void SetTurn(int turn) {
        this.turn = turn;
    }

    [PunRPC]
    private void AddToGroupMistakes(int number) {
        groupMistakes += number;
    }

    public void EndRound() {
        count++;
        int errors = 2 - passedScenes.Count;
        individualMistakes += errors;
        photonView.RPC("AddToGroupMistakes", PhotonTargets.AllBuffered, errors);
        photonView.RPC("AddTimeout", PhotonTargets.AllBuffered, errors);
        photonView.RPC("UpdateScore", PhotonTargets.AllBuffered);
        
        while(errors > 0) {
            totalChoices++;
            errors--;
        }

        passedInClientMeetingRoom = false;
        ClearPassedScenesList();
    }

    [PunRPC]
    private void AddTimeout(int errors) {
        timeouts += errors;
        errorManager.SaveGeneralInfo();
    }

    public void EndGame() {
        PlayerPrefs.SetInt("player_mistakes", individualMistakes);
        PlayerPrefs.SetInt("player_hits", individualHits);
        PlayerPrefs.SetInt("group_mistakes", groupMistakes);
        PlayerPrefs.SetInt("group_hits", groupHits);
        PlayerPrefs.SetInt("total_choices", totalChoices);

        photonView.RPC("LoadFeedbackScene", PhotonTargets.AllBuffered);
    }

    [PunRPC]
    public void LoadFeedbackScene() {
        PhotonNetwork.LoadLevel(4);
    }

    public void LoadMainMenuScene() {
        SceneManager.LoadScene(0);
    }

    public void GetStats() {
        string teamSatisfaction;
        string clientSatisfaction;

        // Stats definition
        GeneralInfo generalInfo = SaveSystem.LoadGeneralInfo();

        //int totalHits = generalInfo.clientMeetingHits + generalInfo.developmentHits + generalInfo.teamMeetingHits;
        //int totalMistakes = generalInfo.clientMeetingMistakes + generalInfo.developmentMistakes + generalInfo.teamMeetingMistakes;

        int clientStats = (generalInfo.clientMeetingHits + generalInfo.developmentHits) - (generalInfo.clientMeetingMistakes + generalInfo.developmentMistakes);
        int teamStats = (generalInfo.teamMeetingHits + generalInfo.developmentHits) - (generalInfo.teamMeetingMistakes + generalInfo.developmentMistakes);

        // Client satisfaction
        if(clientStats >= 3) {
            clientSatisfaction = "Feliz";
        }
        else if(clientStats <= -3) {
            clientSatisfaction = "Infeliz";
        }
        else {
            clientSatisfaction = "Neutro";
        }

        // Team satisfaction
        if(teamStats >= 3) {
            teamSatisfaction = "Feliz";
        }
        else if(clientStats <= -3) {
            teamSatisfaction = "Infeliz";
        }
        else {
            teamSatisfaction = "Neutro";
        }

        // Progess in %
        double progress = ((((turn - 1f) * 4f) + round) / 20f) * 100f;

        // Changing stats text
        teamSatisfactionText.text = teamSatisfaction;
        clientSatisfactionText.text = clientSatisfaction;
        progressText.text = progress + "%";

        // Defining text color
        ChangeColor(teamSatisfaction, teamSatisfactionText);
        ChangeColor(clientSatisfaction, clientSatisfactionText);
    }

    private void ChangeColor(string status, Text text) {
        if(status.Equals("Feliz"))
            text.color = green;
        else if(status.Equals("Infeliz"))
            text.color = red;
    }

    public int GetTimeouts() {
        return timeouts;
    }
}
