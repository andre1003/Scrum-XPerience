using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class ErrorManager : MonoBehaviour {
    public ChoiceController choiceController;

    private string mistakeText;
    private string hitsText;
    private string mistakeFilePath = Directory.GetCurrentDirectory() + @"\Assets\Data\mistakes.txt";
    private string hitsFilePath = Directory.GetCurrentDirectory() + @"\Assets\Data\hits.txt";
    private string[] lines;

    // Check if the choice is a hit or a mistake and save it in the respective file
    public void CheckHit(int buttonID, int rightChoice, string text, string scene) {
        if(buttonID == rightChoice) {
            choiceController.IncreaseIndividualHits();
            using(StreamWriter writer = new StreamWriter(hitsFilePath, true)) {
                writer.WriteLine(scene + ";" + text);
            }
        }
        else {
            choiceController.IncreaseIndividualMistakes();
            using(StreamWriter writer = new StreamWriter(mistakeFilePath, true)) {
                writer.WriteLine(scene + ";" + text);
            }
        }

        choiceController.UpdateScore();
    }
}