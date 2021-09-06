using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewChoiceController : MonoBehaviour {
    public InputField function;
    public InputField turn;
    public InputField round;

    public GameObject choiceCanvas;
    public GameObject outputCanvas;

    public List<Text> buttonList;
    public List<Text> descriptionList;
    public Text outputText;
    
    private List<Decision> data;
    private bool mistake1 = false;
    private int rightChoice;
    private int[] indexes = new int[3];

    //public void GetChoice() {
    //    data = SaveSystem.LoadFromDatabase("Reuniao Equipe", function.text, turn.text, round.text);

    //    Debug.Log(data[0].decisionDescription); // Escolha certa
    //    Debug.Log(data[1].decisionDescription);
    //    Debug.Log(data[2].decisionDescription);
    //}

    public void GetChoices() {
        data = SaveSystem.LoadFromDatabase("Reuniao Equipe", function.text, turn.text, round.text);

        System.Random rdn = new System.Random();
        int lenght = buttonList.Count;
        rightChoice = rdn.Next(0, 3); // Alterar para a quantidade de escolhas
        Debug.Log(rightChoice);

        for(int i = 0; i < lenght; i++) {
            if(i == rightChoice) {
                ChangeTextByFunction(buttonList[i], descriptionList[i], data[0]);
                indexes[0] = i;
            }
            else if(!mistake1) {
                ChangeTextByFunction(buttonList[i], descriptionList[i], data[1]);
                mistake1 = true;
                indexes[1] = i;
            }
            else {
                ChangeTextByFunction(buttonList[i], descriptionList[i], data[2]);
                indexes[2] = i;
            }
        }

        choiceCanvas.SetActive(true);
    }

    private void ChangeTextByFunction(Text buttonText, Text descriptionText, Decision decision) {
        buttonText.text = decision.decisionId;
        descriptionText.text = decision.decisionDescription;
    }

    public void CheckHit(int index) {
        if(index == indexes[0]) { // Escolha certa
            outputText.text = data[0].output;
        }
        else if(index == indexes[1]) { // Mistake1
            outputText.text = data[1].output;
        }
        else { // Mistake2
            outputText.text = data[2].output;
        }

        outputCanvas.SetActive(true);
        choiceCanvas.SetActive(false);
    }
}
