﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ChoiceController : MonoBehaviour {

    public string function;
    public string scene;

    public List<Text> list;
    public List<Text> descriptionList;

    public Text sceneName;
    public Text functionName;
    public Text scoreText;

    public MovementController movementController;
    public MouseController mouseController;

    private int rightChoice;
    private int individualHits;
    private int individualMistakes;

    private int round;

    private bool nothingToDo = false;

    private List<string> passedScenes;

    private void Awake() {
        individualHits = 0;
        individualMistakes = 0;
        passedScenes = new List<string>();
    }

    public bool GetChoices() {
        sceneName.text = scene;
        functionName.text = function;

        System.Random rdn = new System.Random();
        int lenght = list.Count;
        rightChoice = rdn.Next(0, 3); // Alterar para a quantidade de escolhas
        Debug.Log(rightChoice);

        for(int i = 0; i < lenght; i++) {
            if(i == rightChoice) {
                nothingToDo = ChangeTextByFunction(list[i], true, i);
            }
            else {
                nothingToDo = ChangeTextByFunction(list[i], false, i);
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

    /* TEST-ONLY METHODS */
    public void LockOrUnlockPlayer() {
        movementController.enabled = !movementController.enabled;
        mouseController.enabled = !mouseController.enabled;
    }

    public void LockAndHideCursor() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    /* TEST-ONLY METHODS */

    public void CheckHit(int buttonID) {
        if(buttonID == rightChoice) {
            individualHits++;
            using(StreamWriter writer = new StreamWriter(Directory.GetCurrentDirectory() + @"\Assets\Data\hits.txt", true)) {
                writer.WriteLine(list[buttonID].text);
            }
        }
        else {
            individualMistakes++;
            using(StreamWriter writer = new StreamWriter(Directory.GetCurrentDirectory() + @"\Assets\Data\mistakes.txt", true)) {
                writer.WriteLine(list[buttonID].text);
            }
        }

        UpdateScore();
    }

    void UpdateScore() {
        scoreText.text = "Acertos: " + individualHits + "\nErros: " + individualMistakes;

        using(StreamWriter writer = new StreamWriter(Directory.GetCurrentDirectory() + @"\Assets\Data\score.txt")) {
            string scoreJson = "{\"hits\": " + individualHits + ", \"mistakes\": " + individualMistakes + "}";
            writer.Write(scoreJson);
        }
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
        Debug.Log(this.round);
    }

    public void EndRound() {
        int errors = 4 - passedScenes.Count;
        individualMistakes += errors;

        UpdateScore();

        while(errors > 0) {
            using(StreamWriter writer = new StreamWriter(Directory.GetCurrentDirectory() + @"\Assets\Data\mistakes.txt", true)) {
                writer.WriteLine("Tempo Esgotado");
            }

            errors--;
        }

        ClearPassedScenesList();
    }
}
