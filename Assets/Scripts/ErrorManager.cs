using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class ErrorManager : MonoBehaviour {
    public ChoiceController choiceController;
    public PhotonView photonView;

    private string mistakeFilePath = Directory.GetCurrentDirectory() + @"\Assets\Data\mistakes.txt";
    private string hitsFilePath = Directory.GetCurrentDirectory() + @"\Assets\Data\hits.txt";

    private int timeoutCount = 0;
    private int teamMeetingCount = 0;
    private int clientMeetingCount = 0;
    private int developmentRoomCount = 0;

    // Check if the choice is a hit or a mistake and save it in the respective file
    public void CheckHit(int buttonID, int rightChoice, string id, string description, string scene, int index) {
        if(buttonID == rightChoice) {
            choiceController.IncreaseIndividualHits();

            //SaveSystem.Save(id, description, scene, false, index);
        }
        else {
            choiceController.IncreaseIndividualMistakes();

            //SaveSystem.Save(id, description, scene, true, index);
        }
    }

    public void IncreaseMistakes(int option) {
        switch(option) {
            case 0: // Team Meeting
                teamMeetingCount++;
                break;

            case 1: // Client Meeting
                clientMeetingCount++;
                break;

            case 2: // Development Room
                developmentRoomCount++;
                break;

            default: // Timeout
                timeoutCount++;
                break;
        }
    }

    public string GetAllMistakes() {
        return "Erros em Reuniao de Equipe: " + teamMeetingCount + "\nErros em Reuniao com Cliente: " + clientMeetingCount + "\nErros de Desenvolvimento: " + developmentRoomCount + "\nTempo Esgotado: " + timeoutCount;
    }

    public void RemoveDuplicates() {
        //// Mistakes file ////
        List<string> list = new List<string>();
        string[] lines = File.ReadAllLines(mistakeFilePath);

        foreach(string line in lines) {
            list.Add(line);
        }

        // Removing duplicates
        list = list.Distinct().ToList();

        // Clearing the file
        using(StreamWriter streamWriter = new StreamWriter(mistakeFilePath, false)) {
            streamWriter.Write("");
        }

        // Rewritting the file
        foreach(string item in list) {
            using(StreamWriter streamWriter = new StreamWriter(mistakeFilePath, true)) {
                streamWriter.WriteLine(item);
            }
        }

        //// Hits file ////
        lines = File.ReadAllLines(hitsFilePath);

        foreach(string line in lines) {
            list.Add(line);
        }

        // Removing duplicates
        list = list.Distinct().ToList();

        // Clearing the file
        using(StreamWriter streamWriter = new StreamWriter(hitsFilePath, false)) {
            streamWriter.Write("");
        }

        // Rewritting the file
        foreach(string item in list) {
            using(StreamWriter streamWriter = new StreamWriter(hitsFilePath, true)) {
                streamWriter.WriteLine(item);
            }
        }
    }
}