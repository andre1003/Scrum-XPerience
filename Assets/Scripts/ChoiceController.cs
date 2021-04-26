using System;
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

    private List<string> passedScenes;

    private void Awake() {
        individualHits = 0;
        individualMistakes = 0;
        passedScenes = new List<string>();
    }

    public void GetChoices() {
        sceneName.text = scene;
        functionName.text = function;

        System.Random rdn = new System.Random();
        int lenght = list.Count;
        rightChoice = rdn.Next(0, 3); // Alterar para a quantidade de escolhas
        Debug.Log(rightChoice);

        for(int i = 0; i < lenght; i++) {
            if(i == rightChoice) {
                ChangeTextByFunction(list[i], true, i);
            }
            else {
                ChangeTextByFunction(list[i], false, i);
            }
        }
    }

    void ChangeTextByFunction(Text btnText, bool right, int index) {
        if(right) {
            string line = File.ReadAllLines(Directory.GetCurrentDirectory() + @"\Assets\Data\" + scene + @"\Acerto\" + function + @".txt")[0];
            string[] split = line.Split(';');
            btnText.text = split[0];
            descriptionList[index].text = split[1];
        }
        else {
            //btnText.text = File.ReadAllLines(Directory.GetCurrentDirectory() + @"\Assets\Data\" + scene + @"\Erro\" + function + @".txt")[0];
            btnText.text = "Erro";
            descriptionList[index].text = "Erro";
        }
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

    public void ClearPassedScenesList() {
        passedScenes.Clear();
    }
}
