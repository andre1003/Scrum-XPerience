using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class FeedbackManager : MonoBehaviour {
    public Text feedbackText;
    public ErrorManager errorManager;
    public GameObject continueButton;

    private string mistakeFilePath = Directory.GetCurrentDirectory() + @"\Assets\Data\mistakes.txt";
    private string individualFeedbackFilePath = Directory.GetCurrentDirectory() + @"\Assets\Data\individual_feedback.txt";
    private string[] lines;

    private string timeoutText = "Você demorou muito para tomar as decisões! Mais atenção!";
    private string teamMeetingText = "Sua comunicação com a equipe foi falha! Muito cuidado, pois essa é uma etapa fundamental para o desenvolvimento ágil.";
    private string clientMeetingText = "A comunicação com o cliente é essencial para os métodos ágeis! Lembre-se sempre disso...";
    private string developmentRoomText = "Apesar de tudo, o desenvolvimento do software não foi dos melhores... Busque estudar mais sobre o desenvolvimento utilizando metodologias ágeis. Isso vai te ajudar bastante!";

    private List<string> feedback = new List<string>();

    private bool canContinue = false;

    private short count = 0;

    // Start is called before the first frame update
    void Start() {
        ShowStats();
    }

    private void Update() {
        if(canContinue) {
            if(Input.GetKeyDown(KeyCode.Return)) {
                if(count == 1)
                    ShowFeedback();
                else {
                    canContinue = false;
                    continueButton.SetActive(canContinue);
                    feedbackText.text = "";
                }
            }
        }
    }

    private void ShowStats() {
        int totalChoices = PlayerPrefs.GetInt("total_choices");
        Debug.Log(totalChoices);
        List<Decision> datas = new List<Decision>();

        for(int i = 0; i < totalChoices; i++) {
            Decision data = SaveSystem.Load(i);
            if(data.isMistake)
                datas.Add(data);

            //Debug.Log(data.decisionId + " " + data.scenary + " " + data.isMistake);
        }

        List<string> aux = new List<string>();

        for(int i = 0; i < datas.Count(); i++) {
            Debug.Log(datas[i].scenery);

            // Timeout
            if(datas[i].scenery.Equals("Tempo Esgotado")) {
                errorManager.IncreaseMistakes(4);
                aux.Add(timeoutText);
            }

            // Team Meeting
            else if(datas[i].scenery.Equals("Reuniao Equipe")) {
                errorManager.IncreaseMistakes(0);
                aux.Add(teamMeetingText);
            }

            // Client Meeting
            else if(datas[i].scenery.Equals("Reuniao Cliente")) {
                errorManager.IncreaseMistakes(1);
                aux.Add(clientMeetingText);
            }

            // Development Room
            else {
                errorManager.IncreaseMistakes(2);
                aux.Add(developmentRoomText);
            }
        }

        feedback = aux.Distinct().ToList();

        StartCoroutine(TypewriterEffect(errorManager.GetAllMistakes(), feedbackText));
    }

    //void ShowStats() {
    //    List<string> aux = new List<string>();
    //    lines = File.ReadAllLines(mistakeFilePath);

    //    foreach(string line in lines) {
    //        // Timeout
    //        if(line.Equals("Tempo Esgotado")) {
    //            errorManager.IncreaseMistakes(4);
    //            aux.Add(timeoutText);
    //        }

    //        // Team Meeting
    //        else if(line.Split(';')[0].Equals("Reuniao Equipe")) {
    //            errorManager.IncreaseMistakes(0);
    //            aux.Add(teamMeetingText);
    //        }

    //        // Client Meeting
    //        else if(line.Split(';')[0].Equals("Reuniao Cliente")) {
    //            errorManager.IncreaseMistakes(1);
    //            aux.Add(clientMeetingText);
    //        }

    //        // Development Room
    //        else {
    //            errorManager.IncreaseMistakes(2);
    //            aux.Add(developmentRoomText);
    //        }
    //    }

    //    feedback = aux.Distinct().ToList();

    //    StartCoroutine(TypewriterEffect(errorManager.GetAllMistakes(), feedbackText));
    //}

    void ShowFeedback() {
        feedbackText.text = "";
        canContinue = false;
        continueButton.SetActive(canContinue);
        string aux = "";
        int index = 0;
        int lenght = feedback.Count();

        foreach(string item in feedback) {
            if(index + 1 != lenght)
                aux += item + "\n\n";
            else
                aux += item;
            index++;
        }

        StartCoroutine(TypewriterEffect(aux, feedbackText));

        using(StreamWriter streamWriter = new StreamWriter(individualFeedbackFilePath)) {
            streamWriter.WriteLine(aux);
        }
    }

    IEnumerator TypewriterEffect(string text, Text uiText) {
        foreach(char character in text) {
            uiText.text += character;
            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(2f);

        canContinue = true;
        continueButton.SetActive(canContinue);
        count++;
    }

    IEnumerator TypewriterEffect2(string text, Text uiText) {
        foreach(char character in text) {
            uiText.text += character;
            yield return new WaitForSeconds(0.02f);
        }
    }
}
