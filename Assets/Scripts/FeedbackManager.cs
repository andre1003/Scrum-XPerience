using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class FeedbackManager : MonoBehaviour {
    public Text feedbackText;
    public ErrorManager errorManager;

    private string mistakeFilePath = Directory.GetCurrentDirectory() + @"\Assets\Data\mistakes.txt";
    private string[] lines;

    private List<string> concepts;

    private int timeoutCount;
    private int teamMeetingCount;
    private int clientMeetingCount;
    private int developmentRoomCount;

    private string timeoutText = "Demorou muito para tomar as decisões! Mais atenção!\n";
    private string teamMeetingText = "Sua comunicação com a equipe foi falha! Muito cuidado, pois essa é uma etapa fundamental para o desenvolvimento ágil.\n";
    private string clientMeetingText = "A comunicação com o cliente é essencial para os métodos ágeis! Lembre-se sempre disso...\n";
    private string developmentRoomText = "Apesar de tudo, o desenvolvimento do software não foi dos melhores... Busque estudar mais sobre o desenvolvimento utilizando metodologias ágeis. Isso vai te ajudar bastante!\n";

    // Start is called before the first frame update
    void Start() {
        SetAuxiliarFeedback();

        timeoutCount = 0;
        teamMeetingCount = 0;
        clientMeetingCount = 0;
        developmentRoomCount = 0;
    }

    void SetAuxiliarFeedback() {
        List<string> aux = new List<string>();
        lines = File.ReadAllLines(mistakeFilePath);

        foreach(string line in lines) {
            // Timeout
            if(line.Equals("Tempo Esgotado")) {
                timeoutCount++;
                aux.Add(timeoutText);
            }

            // Team Meeting
            else if(line.Split(';')[0].Equals("Reuniao Equipe")) {
                teamMeetingCount++;
                aux.Add(teamMeetingText);
            }

            // Client Meeting
            else if(line.Split(';')[0].Equals("Reuniao Cliente")) {
                clientMeetingCount++;
                aux.Add(clientMeetingText);
            }

            // Development Room
            else {
                developmentRoomCount++;
                aux.Add(developmentRoomText);
            }
        }

        aux = aux.Distinct().ToList();

        feedbackText.text = "Erros em Reunião de Equipe: " + teamMeetingCount + "\nErros em Reunião com Cliente: " + clientMeetingCount + "\nErros de Desenvolvimento: " + developmentRoomCount + "\n";

        foreach(string item in aux) {
            feedbackText.text += item;
        }
    }
}
