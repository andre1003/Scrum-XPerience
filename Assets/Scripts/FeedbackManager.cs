using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using System.Net.Http;

public class FeedbackManager : MonoBehaviour {
    public Text feedbackText;
    public ErrorManager errorManager;
    public GameObject continueButton;
    public GameObject continueButton2;

    public Text titleText;
    public Text individualMistakes;
    public Text individualHits;

    public GameObject feedbackCanvas;
    public GameObject mistakesAndHitsCanvas;
    public GameObject endCanvas;

    public InputField passwordInputField;
    public GameObject passwordCanvas;
    public Text infoText;

    private string mistakeFilePath = Directory.GetCurrentDirectory() + @"\Assets\Data\mistakes.txt";
    private string individualFeedbackFilePath = Directory.GetCurrentDirectory() + @"\Assets\Data\individual_feedback.txt";
    private string[] lines;

    private string timeoutText = "Você demorou muito para tomar as decisões! Mais atenção!";
    private string teamMeetingText = "Sua comunicação com a equipe foi falha! Muito cuidado, pois essa é uma etapa fundamental para o desenvolvimento ágil.";
    private string clientMeetingText = "A comunicação com o cliente é essencial para os métodos ágeis! Lembre-se sempre disso...";
    private string developmentRoomText = "Apesar de tudo, o desenvolvimento do software não foi dos melhores... Busque estudar mais sobre o desenvolvimento utilizando metodologias ágeis. Isso vai te ajudar bastante!";

    private string individualFeedback = "";
    private string generalFeedback = "";

    private string saveMatchExecutablePath = Directory.GetCurrentDirectory() + @"\Assets\Scripts\send_to_cloud.exe";
    private string decisionsPath;

    private string password;

    private List<string> feedback = new List<string>();

    private bool canContinue = false;

    private short count = 0;

    // Start is called before the first frame update
    void Start() {
        decisionsPath = Application.persistentDataPath + "/player_data/";
        passwordCanvas.SetActive(true);
        //ShowIndividualStats();
    }

    private void Update() {
        if(canContinue) {
            if(Input.GetKeyDown(KeyCode.Return)) {
                if(count == 1)
                    ShowIndividualHitsAndMistakes();
                else if(count == 3)
                    ShowIndividualFeedback();
                else if(count == 4)
                    ShowGeneralStats();
                else if(count == 5)
                    ShowGeneralHitsAndMistakes();
                else if(count == 7)
                    End();
            }
        }
    }

    public void Login() {
        password = passwordInputField.text;
        ShowIndividualStats();
    }

    private void SendDataToDatabase() {
        //////
        /// TEST NEEDED
        //////

        //string username = PlayerPrefs.GetString("username");
        //string password = PlayerPrefs.GetString("password");
        //string role = PlayerPrefs.GetString("player_function");
        //int hits = errorManager.GetAllHits();
        //int mistakes = errorManager.GetAllMistakes();
        //string group = PlayerPrefs.GetString("room");

        string username = "andre.aragao";
        string role = "Product Owner";

        

        List<Decision> decisions = SaveSystem.LoadAll();
        int hits = 0;
        int mistakes = 0;
        foreach(Decision decision in decisions) {
            if(decision.isMistake == true)
                mistakes++;
            else
                hits++;
        }

        string group = "Tigers";

        SaveSystem.InfoFile("starting", username, role, hits.ToString(), mistakes.ToString(), individualFeedback, group, "");

        HttpClient client = new HttpClient();
        Dictionary<string, string> data = new Dictionary<string, string>();
        
        // Login
        data.Add("username", username);
        data.Add("password", password);
        string response = SaveSystem.Post(SaveSystem.loginUrl, data, client);
        data.Clear();

        if(response.Equals("fail")) {
            infoText.color = Color.red;
            infoText.text = "Senha incorreta! Tente novamente.";
        }
        else if(!response.Equals("Connection Failed")) {
            passwordCanvas.SetActive(false);

            // Save match
            data.Add("role", role);
            data.Add("hits", hits.ToString());
            data.Add("mistakes", mistakes.ToString());
            data.Add("individual_feedback", individualFeedback);
            data.Add("group", group);
            string matchId = SaveSystem.Post(SaveSystem.matchRegisterUrl, data, client);

            if(!matchId.Equals("Connection Failed")) {
                SaveSystem.InfoFile("match created", username, role, hits.ToString(), mistakes.ToString(), individualFeedback, group, matchId);
                data.Clear();

                // Save decisions
                foreach(Decision decision in decisions) {
                    data.Add("decision", decision.decisionId);
                    data.Add("scenery", decision.scenery);
                    data.Add("is_mistake", decision.isMistake.ToString());
                    SaveSystem.Post(SaveSystem.decisionRegisterUrl + matchId + "/", data, client);
                    data.Clear();
                }

                SaveSystem.InfoFile("done", username, role, hits.ToString(), mistakes.ToString(), individualFeedback, group, matchId);
            }
        }
        else {
            UnityEngine.Debug.Log(response);
        }
    }

    public void ShowIndividualStats() {
        passwordCanvas.SetActive(true);

        titleText.text = "Estatísticas Individuais";
        canContinue = false;
        List<int> mistakes = errorManager.GetIndividualStats();
        string stats = "Erros em Reuniao de Equipe: " + mistakes[1] + "\nErros em Reuniao com Cliente: " + mistakes[2] + "\nErros de Desenvolvimento: " + mistakes[3] + "\nTempo Esgotado: " + mistakes[0];
        StartCoroutine(TypewriterEffect(stats, feedbackText));

        if(mistakes[0] != 0)
            individualFeedback += timeoutText + "\n";

        if(mistakes[1] != 0)
            individualFeedback += teamMeetingText + "\n";

        if(mistakes[2] != 0)
            individualFeedback += clientMeetingText + "\n";

        if(mistakes[3] != 0)
            individualFeedback += developmentRoomText + "\n";

        SendDataToDatabase();
    }

    private void ShowIndividualHitsAndMistakes() {
        canContinue = false;
        mistakesAndHitsCanvas.SetActive(true);
        continueButton.SetActive(false);
        continueButton2.SetActive(false);
        ShowIndividualMistakes();
        ShowIndividualHits();
    }

    public void ShowIndividualMistakes() {
        List<string> mistakes = errorManager.GetMistakes();
        string aux = "";
        foreach(string mistake in mistakes) {
            aux += mistake + "\n";
        }

        StartCoroutine(TypewriterEffect(aux, individualMistakes));
    }

    public void ShowIndividualHits() {
        List<string> hits = errorManager.GetHits();
        string aux = "";
        foreach(string hit in hits) {
            aux += hit + "\n";
        }

        StartCoroutine(TypewriterEffect(aux, individualHits));
    }

    private void ShowIndividualFeedback() {
        titleText.text = "Feedback Individual";
        canContinue = false;
        continueButton.SetActive(false);
        continueButton2.SetActive(false);
        feedbackText.text = "";
        mistakesAndHitsCanvas.SetActive(false);

        StartCoroutine(TypewriterEffect(individualFeedback, feedbackText));
    }

    private void ShowGeneralStats() {
        titleText.text = "Estatísticas do Grupo";
        canContinue = false;
        continueButton.SetActive(false);
        continueButton2.SetActive(false);
        feedbackText.text = "";
        mistakesAndHitsCanvas.SetActive(false);
        List<int> mistakes = errorManager.GetGeneralStats();
        string stats = "Erros em Reuniao de Equipe: " + mistakes[1] + "\nErros em Reuniao com Cliente: " + mistakes[2] + "\nErros de Desenvolvimento: " + mistakes[3] + "\nTempo Esgotado: " + mistakes[0];
        StartCoroutine(TypewriterEffect(stats, feedbackText));


    }

    private void ShowGeneralHitsAndMistakes() {
        canContinue = false;
        mistakesAndHitsCanvas.SetActive(true);
        continueButton.SetActive(false);
        continueButton2.SetActive(false);
        individualMistakes.text = "";
        individualHits.text = "";
        ShowGeneralMistakes();
        ShowGeneralHits();
    }

    public void ShowGeneralMistakes() {
        List<string> mistakes = errorManager.GetGeneralMistakes();
        string aux = "";
        foreach(string mistake in mistakes) {
            aux += mistake + "\n";
        }

        StartCoroutine(TypewriterEffect(aux, individualMistakes));
    }

    public void ShowGeneralHits() {
        List<string> hits = errorManager.GetGeneralHits();
        string aux = "";
        foreach(string hit in hits) {
            aux += hit + "\n";
        }

        StartCoroutine(TypewriterEffect(aux, individualHits));
    }

    private void End() {
        canContinue = false;
        mistakesAndHitsCanvas.SetActive(false);
        feedbackCanvas.SetActive(false);
        endCanvas.SetActive(true);
        PlayerPrefs.DeleteAll();
    }

    public void EndGame() {
        Application.Quit();
    }

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
        continueButton2.SetActive(canContinue);
        count++;
    }

    IEnumerator TypewriterEffect2(string text, Text uiText) {
        foreach(char character in text) {
            uiText.text += character;
            yield return new WaitForSeconds(0.02f);
        }
    }
}
