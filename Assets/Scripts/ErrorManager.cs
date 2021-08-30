using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class ErrorManager : MonoBehaviour {
    public ChoiceController choiceController;
    public PhotonView photonView;

    private string path;

    private int timeouts = 0;
    private int teamMeetingMistakes = 0;
    private int clientMeetingMistakes = 0;
    private int developmentMistakes = 0;
    private int teamMeetingHits = 0;
    private int clientMeetingHits = 0;
    private int developmentHits = 0;

    private void Awake() {
        path = Application.persistentDataPath + "/player_data/";
    }

    private void Update() {
        //Debug.Log("Client Mistakes: " + clientMeetingMistakes);
    }

    // Check if the choice is a hit or a mistake and save it in the respective file
    public void CheckHit(int buttonID, int rightChoice, string id, string description, string scene, int index) {
        if(buttonID == rightChoice) {
            choiceController.IncreaseIndividualHits();

            SaveSystem.Save(id, description, scene, "", false, index);
            photonView.RPC("SaveGeneralInfo", PhotonTargets.AllBuffered, scene, false);
        }
        else {
            choiceController.IncreaseIndividualMistakes();

            SaveSystem.Save(id, description, scene, "", true, index);
            photonView.RPC("SaveGeneralInfo", PhotonTargets.AllBuffered, scene, true);

        }
    }

    [PunRPC]
    private void SaveGeneralInfo(string scene, bool isMistake) {
        if(isMistake) {
            if(scene.Equals("Reuniao Equipe")) {
                Increment(0);
            }
            else if(scene.Equals("Reuniao Cliente")) {
                Increment(1);
            }
            else {
                Increment(2);
            }
        }
        else {
            if(scene.Equals("Reuniao Equipe")) {
                Increment(3);
            }
            else if(scene.Equals("Reuniao Cliente")) {
                Increment(4);
            }
            else {
                Increment(5);
            }
        }

        //SaveGeneralInfo();
    }

    public void SaveGeneralInfo() {
        SaveSystem.SaveGeneralInfo(choiceController.GetTimeouts(), teamMeetingMistakes, clientMeetingMistakes, developmentMistakes, teamMeetingHits, clientMeetingHits, developmentHits);
    }

    private void Increment(int option) {
        switch(option) {
            case 0:
                teamMeetingMistakes++;
                break;

            case 1:
                clientMeetingMistakes++;
                break;

            case 2:
                developmentMistakes++;
                break;

            case 3:
                teamMeetingHits++;
                break;

            case 4:
                clientMeetingHits++;
                break;

            case 5:
                developmentHits++;
                break;

            case 6:
                timeouts++;
                break;
        }
    }

    //public void IncreaseMistakes(int option) {
    //    switch(option) {
    //        case 0: // Team Meeting
    //            teamMeetingCount++;
    //            break;

    //        case 1: // Client Meeting
    //            clientMeetingCount++;
    //            break;

    //        case 2: // Development Room
    //            developmentRoomCount++;
    //            break;

    //        default: // Timeout
    //            timeoutCount++;
    //            break;
    //    }
    //}

    //public string GetAllMistakes() {
    //    return "Erros em Reuniao de Equipe: " + teamMeetingCount + "\nErros em Reuniao com Cliente: " + clientMeetingCount + "\nErros de Desenvolvimento: " + developmentRoomCount + "\nTempo Esgotado: " + timeoutCount;
    //}

    public List<int> GetIndividualStats() {
        timeouts = 0;
        teamMeetingMistakes = 0;
        clientMeetingMistakes = 0;
        developmentMistakes = 0;
        teamMeetingHits = 0;
        clientMeetingHits = 0;
        developmentHits = 0;

        string[] files = Directory.GetFiles(path);
        int length = files.Length;

        for(int i = 0; i < length; i++) {
            Decision data = SaveSystem.Load(i);

            if(data.isMistake) {
                if(data.scenery.Equals("Reuniao Equipe")) {
                    Increment(0);
                }
                else if(data.scenery.Equals("Reuniao Cliente")) {
                    Increment(1);
                }
                else if(data.scenery.Equals("Desenvolvimento")) {
                    Increment(2);
                }
                else {
                    Increment(6);
                }
            }
            else {
                if(data.scenery.Equals("Reuniao Equipe")) {
                    Increment(3);
                }
                else if(data.scenery.Equals("Reuniao Cliente")) {
                    Increment(4);
                }
                else {
                    Increment(5);
                }
            }
        }

        List<int> mistakes = new List<int>() {
            timeouts,              // 0
            teamMeetingMistakes,   // 1
            clientMeetingMistakes, // 2
            developmentMistakes,   // 3
            teamMeetingHits,       // 4
            clientMeetingHits,     // 5
            developmentHits        // 6
        };

        return mistakes;
    }

    public List<string> GetMistakes() {
        string[] files = Directory.GetFiles(path);
        int length = files.Length;
        List<string> mistakes = new List<string>();

        Debug.Log(length);

        for(int i = 0; i < length; i++) {
            Decision data = SaveSystem.Load(i);
            if(data.isMistake)
                mistakes.Add(data.decisionId);
        }

        mistakes = mistakes.Distinct().ToList();

        return mistakes;
    }

    public List<string> GetHits() {
        string[] files = Directory.GetFiles(path);
        int length = files.Length;
        List<string> hits = new List<string>();

        Debug.Log(length);

        for(int i = 0; i < length; i++) {
            Decision data = SaveSystem.Load(i);
            if(!data.isMistake)
                hits.Add(data.decisionId);
        }

        hits = hits.Distinct().ToList();

        return hits;
    }

    public List<int> GetGeneralStats() {
        GeneralInfo groupData = SaveSystem.LoadGeneralInfo();
        return new List<int>() {
            groupData.timeouts,
            groupData.teamMeetingMistakes,
            groupData.clientMeetingMistakes,
            groupData.developmentMistakes,
            groupData.teamMeetingHits,
            groupData.clientMeetingHits,
            groupData.developmentHits
        };
    }

    public List<string> GetGeneralMistakes() {
        GeneralInfo groupData = SaveSystem.LoadGeneralInfo();
        List<string> mistakes = new List<string>();

        Debug.Log(groupData.timeouts);

        if(groupData.timeouts != 0)
            mistakes.Add("O grupo demorou para realizar as ações.\n");

        if(groupData.clientMeetingMistakes != 0)
            mistakes.Add("Erros na comunicação com o cliente.\n");

        if(groupData.teamMeetingMistakes != 0)
            mistakes.Add("Erros na comunicação com a equipe.\n");

        if(groupData.developmentMistakes != 0)
            mistakes.Add("Erros no desenvolvimento.\n");

        return mistakes;
    }

    public List<string> GetGeneralHits() {
        GeneralInfo groupData = SaveSystem.LoadGeneralInfo();
        List<string> hits = new List<string>();

        if(groupData.clientMeetingHits != 0)
            hits.Add("Sua comunicação com o cliente foi boa.\n");

        if(groupData.teamMeetingHits != 0)
            hits.Add("Sua comunicação com a equipe foi boa.\n");

        if(groupData.developmentHits != 0)
            hits.Add("O desenvolvimento foi adequado.\n");

        return hits;
    }

    //public void RemoveDuplicates() {
    //    //// Mistakes file ////
    //    List<string> list = new List<string>();
    //    string[] lines = File.ReadAllLines(mistakeFilePath);

    //    foreach(string line in lines) {
    //        list.Add(line);
    //    }

    //    // Removing duplicates
    //    list = list.Distinct().ToList();

    //    // Clearing the file
    //    using(StreamWriter streamWriter = new StreamWriter(mistakeFilePath, false)) {
    //        streamWriter.Write("");
    //    }

    //    // Rewritting the file
    //    foreach(string item in list) {
    //        using(StreamWriter streamWriter = new StreamWriter(mistakeFilePath, true)) {
    //            streamWriter.WriteLine(item);
    //        }
    //    }

    //    //// Hits file ////
    //    lines = File.ReadAllLines(hitsFilePath);

    //    foreach(string line in lines) {
    //        list.Add(line);
    //    }

    //    // Removing duplicates
    //    list = list.Distinct().ToList();

    //    // Clearing the file
    //    using(StreamWriter streamWriter = new StreamWriter(hitsFilePath, false)) {
    //        streamWriter.Write("");
    //    }

    //    // Rewritting the file
    //    foreach(string item in list) {
    //        using(StreamWriter streamWriter = new StreamWriter(hitsFilePath, true)) {
    //            streamWriter.WriteLine(item);
    //        }
    //    }
    //}
}