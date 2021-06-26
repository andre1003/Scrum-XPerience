using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class FeedbackManager : MonoBehaviour {
    public Text feedbackText;

    private string mistakeFilePath = Directory.GetCurrentDirectory() + @"\Assets\Data\mistakes.txt";
    private string[] lines;

    private List<string> concepts;

    // Start is called before the first frame update
    void Start() {
        SetAuxiliarFeedback();
    }

    // Update is called once per frame
    void Update() {

    }

    void SetAuxiliarFeedback() {
        List<string> aux = new List<string>();
        lines = File.ReadAllLines(mistakeFilePath);

        foreach(string line in lines) {
            if(line.Equals("Tempo Esgotado"))
                aux.Add("Demorou muito para tomar as decisões! Mais atenção!\n");

            // Team Meeting
            else if(line.Equals("Não Participar Reunião") || line.Equals("") || line.Equals("") || line.Equals(""))
                aux.Add("Sua comunicação com a equipe foi falha! Muito cuidado, pois essa é uma etapa fundamental para o desenvolvimento ágil.\n");

            // Client Meeting
            else if(line.Equals("") || line.Equals("") || line.Equals("") || line.Equals(""))
                aux.Add("A comunicação com o cliente é essencial para os métodos ágeis! Lembre-se sempre disso...\n");

            // Development Room
            else
                aux.Add("Apesar de tudo, o desenvolvimento do software não foi dos melhores... Busque estudar mais sobre o desenvolvimento utilizando metodologias ágeis. Isso vai te ajudar bastante!\n");
        }

        aux = aux.Distinct().ToList();

        foreach(string item in aux) {
            feedbackText.text += item;
        }
    }
}
