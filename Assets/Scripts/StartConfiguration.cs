using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;
using UnityEngine.UI;

public class StartConfiguration : MonoBehaviour {
    public InputField passwordInputField;
    public GameObject passwordCanvas;
    public GameObject playButton;
    public Text infoText;

    // Start is called before the first frame update
    void Start() {
        HttpClient client = new HttpClient();
        string serverStatus = SaveSystem.Get(SaveSystem.homeUrl, client);
        if(serverStatus.Equals("Connection Failed")) {
            playButton.SetActive(false);
            infoText.text = "Falha na conexão. Provavelmente nossos servidores estão fora do ar. Tente novamente mais tarde...";
        }
        else {
            CheckSentDecisions();
        }
    }

    // Update is called once per frame
    void Update() {

    }

    private void CheckSentDecisions() {
        InfoFile info = SaveSystem.LoadInfoFile();

        if(info != null && !info.status.Equals("done")) {
            passwordCanvas.SetActive(true);
        }
        else {
            // Delete all
            SaveSystem.DeleteAll();
        }
    }

    public void Login() {
        //////
        /// TEST NEEDED
        //////
        HttpClient client = new HttpClient();
        string password = passwordInputField.text;
        InfoFile info = SaveSystem.LoadInfoFile();

        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("username", info.username);
        data.Add("password", password);

        SaveSystem.Post(SaveSystem.loginUrl, data, client);

        if(info.status.Equals("starting")) {
            // Create match and send decisions

            // Save match
            data.Add("role", info.role);
            data.Add("hits", info.hits);
            data.Add("mistakes", info.mistakes);
            data.Add("individual_feedback", info.feedback);
            data.Add("group", info.group);
            string matchId = SaveSystem.Post(SaveSystem.matchRegisterUrl, data, client);

            if(!matchId.Equals("Connection Failed")) {
                SaveSystem.InfoFile("match created", info.username, info.role, info.hits, info.mistakes, info.feedback, info.group, matchId);
                data.Clear();

                List<Decision> decisions = SaveSystem.LoadAll();
                // Save decisions
                foreach(Decision decision in decisions) {
                    data.Add("decision", decision.decisionId);
                    data.Add("scenery", decision.scenery);
                    data.Add("is_mistake", decision.isMistake.ToString());
                    SaveSystem.Post(SaveSystem.decisionRegisterUrl + matchId + "/", data, client);
                    data.Clear();
                }
            }
        }
        else if(info.status.Equals("match created")) {
            // Just send decisions
            List<Decision> decisions = SaveSystem.LoadAll();

            // Save decisions
            foreach(Decision decision in decisions) {
                data.Add("decision", decision.decisionId);
                data.Add("scenery", decision.scenery);
                data.Add("is_mistake", decision.isMistake.ToString());
                SaveSystem.Post(SaveSystem.decisionRegisterUrl + info.matchId + "/", data, client);
                data.Clear();
            }
        }

        // Delete all decisions
        passwordCanvas.SetActive(false);
        SaveSystem.DeleteAll();
    }
}
