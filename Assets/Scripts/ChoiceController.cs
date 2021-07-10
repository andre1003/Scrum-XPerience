﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ChoiceController : MonoBehaviour {

    public string scene;

    public int maxMistakes;

    public List<Text> buttonList;
    public List<Text> descriptionList;

    public Text sceneName;
    public Text functionName;
    public Text scoreText;

    public Text teamSatisfactionText;
    public Text clientSatisfactionText;
    public Text progressText;

    public ErrorManager errorManager;

    public GameObject pauseMenu;

    public NewTimer timeController;

    private MovementController movementController;
    private MouseController mouseController;

    private string function;
    private string mistakeFilePath = Directory.GetCurrentDirectory() + @"\Assets\Data\mistakes.txt";
    private string hitFilePath = Directory.GetCurrentDirectory() + @"\Assets\Data\hits.txt";

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

    private bool isPaused = false;
    private bool isGameStarded = false;

    private int count = 0;

    private void Awake() {
        individualHits = 0;
        individualMistakes = 0;
        groupHits = 0;
        groupMistakes = 0;
        totalChoices = 0;
        passedScenes = new List<string>();
    }

    private void Update() {
        photonView.RPC("CheckGameOver", PhotonTargets.AllBuffered);

        if(Input.GetKeyDown(KeyCode.Space))
            GetStats();
        else if(Input.GetKeyDown(KeyCode.Escape)) {
            isPaused = !isPaused;
            PauseMenu(isPaused);
        }
        else if(Input.GetKeyDown(KeyCode.P))
            EndGame();
    }

    private void PauseMenu(bool isPaused) {
        if(isPaused == true) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        LockOrUnlockPlayer();
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }

    public void SetIsPaused(bool isPaused) {
        this.isPaused = isPaused;
    }

    public bool GetChoices() {
        sceneName.text = scene;
        functionName.text = function;

        System.Random rdn = new System.Random();
        int lenght = buttonList.Count;
        rightChoice = rdn.Next(0, 3); // Alterar para a quantidade de escolhas
        Debug.Log(rightChoice);

        for(int i = 0; i < lenght; i++) {
            if(i == rightChoice) {
                nothingToDo = ChangeTextByFunction(buttonList[i], true, i);
            }
            else {
                nothingToDo = ChangeTextByFunction(buttonList[i], false, i);
            }

            if(nothingToDo)
                break;
        }

        return nothingToDo;
    }

    bool ChangeTextByFunction(Text btnText, bool correct, int index) {
        string[] lines = File.ReadAllLines(Directory.GetCurrentDirectory() + @"\Assets\Data\" + scene + @"\Acerto\" + function + @".txt");
        if(!lines[0].Equals("Nada")) {
            if(correct) {
                string line = lines[round - 1];
                string[] split = line.Split(';');
                btnText.text = split[0];
                descriptionList[index].text = split[1];
            }
            else {
                //btnText.text = File.ReadAllLines(Directory.GetCurrentDirectory() + @"\Assets\Data\" + scene + @"\Erro\" + function + @".txt")[0];
                btnText.text = "Erro";
                descriptionList[index].text = "Erro";
            }
            return false;
        }
        else
            return true;
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

    public void LockOrUnlockPlayer() {
        movementController.enabled = !movementController.enabled;
        mouseController.enabled = !mouseController.enabled;
    }

    public void LockAndHideCursor() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void CheckHit(int buttonId) {
        errorManager.CheckHit(buttonId, rightChoice, buttonList[buttonId].text, descriptionList[buttonId].text, scene, totalChoices);
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
    void IncreaseGroupHits() {
        groupHits++;
    }

    [PunRPC]
    void IncreaseGroupMistakes() {
        groupMistakes++;
    }

    [PunRPC]
    private void CheckGameOver() {
        if(groupMistakes >= maxMistakes)
            timeController.GameOver();
    }

    [PunRPC]
    public void UpdateScore() {
        scoreText.text = "Acertos: " + groupHits + "\nErros: " + groupMistakes;

        //using(StreamWriter writer = new StreamWriter(Directory.GetCurrentDirectory() + @"\Assets\Data\score.txt")) {
        //    string scoreJson = "{\"hits\": " + individualHits + ", \"mistakes\": " + individualMistakes + "}";
        //    writer.Write(scoreJson);
        //}
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
        //Debug.Log(this.round);
    }

    public void SetTurn(int turn) {
        this.turn = turn;
        //Debug.Log(this.round);
    }

    [PunRPC]
    private void AddToGroupMistakes(int number) {
        groupMistakes += number;
    }

    public void EndRound() {
        count++;
        Debug.Log(count);
        int errors = 3 - passedScenes.Count;
        individualMistakes += errors;
        photonView.RPC("AddToGroupMistakes", PhotonTargets.AllBuffered, errors);

        photonView.RPC("UpdateScore", PhotonTargets.AllBuffered);
        
        while(errors > 0) {
            //using(StreamWriter writer = new StreamWriter(mistakeFilePath, true)) {
            //    writer.WriteLine("Tempo Esgotado");
            //}

            //SaveSystem.Save("Tempo Esgotado", "Tempo Esgotado", "Tempo Esgotado", true, totalChoices);
            totalChoices++;
            errors--;
        }

        ClearPassedScenesList();
    }

    public void EndGame() {
        PlayerPrefs.SetInt("player_mistakes", individualMistakes);
        PlayerPrefs.SetInt("player_hits", individualHits);
        PlayerPrefs.SetInt("group_mistakes", groupMistakes);
        PlayerPrefs.SetInt("group_hits", groupHits);
        PlayerPrefs.SetInt("total_choices", totalChoices);

        //PhotonNetwork.LoadLevel(4);
        photonView.RPC("LoadFeedbackScene", PhotonTargets.AllBuffered);
    }

    [PunRPC]
    public void LoadFeedbackScene() {
        PhotonNetwork.LoadLevel(4);
    }

    public void GetStats() {
        string[] lines = File.ReadAllLines(hitFilePath);
        int teamMeeting = 0;
        int clientMeeting = 0;
        int development = 0;

        foreach(string line in lines) {
            if(line.Split(';')[0].Equals("Reuniao Equipe")) {
                teamMeeting++;
            }

            // Client Meeting
            else if(line.Split(';')[0].Equals("Reuniao Cliente")) {
                clientMeeting++;
            }

            // Development Room
            else {
                development++;
            }
        }

        lines = File.ReadAllLines(mistakeFilePath);

        foreach(string line in lines) {
            if(line.Split(';')[0].Equals("Reuniao Equipe")) {
                teamMeeting--;
            }

            // Client Meeting
            else if(line.Split(';')[0].Equals("Reuniao Cliente")) {
                clientMeeting--;
            }

            // Development Room
            else if(line.Split(';')[0].Equals("Desenvolvimento")) {
                development--;
            }

            // Timeout
            else {
                teamMeeting--;
                clientMeeting--;
                development--;
            }
        }

        string teamSatisfaction;
        string clientSatisfaction;

        // Team stats
        if(teamMeeting >= 3) {
            teamSatisfaction = "Feliz";
        }
        else if(teamMeeting <= -3) {
            teamSatisfaction = "Infeliz";
        }
        else {
            teamSatisfaction = "Neutra";
        }

        float clientSatisfactionAverage = Mathf.Ceil(((development * 2) + clientMeeting) / 3);

        // Client stats
        if(clientSatisfactionAverage >= 3f) {
            clientSatisfaction = "Feliz";
        }
        else if(clientSatisfactionAverage <= 3f) {
            clientSatisfaction = "Infeliz";
        }
        else {
            clientSatisfaction = "Neutro";
        }

        double progress = ((((turn - 1f) * 4f) + round) / 20f) * 100f; // In %

        teamSatisfactionText.text = teamSatisfaction;
        clientSatisfactionText.text = clientSatisfaction;

        ChangeColor(teamSatisfaction, teamSatisfactionText);
        ChangeColor(clientSatisfaction, clientSatisfactionText);

        progressText.text = progress + "%";
    }

    void ChangeColor(string status, Text text) {
        if(status.Equals("Feliz"))
            text.color = green;
        else if(status.Equals("Infeliz"))
            text.color = red;
    }
}
