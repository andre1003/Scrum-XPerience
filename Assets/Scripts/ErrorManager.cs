using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class ErrorManager : MonoBehaviour {
    public Text mistakeText;
    public Text hitsText;

    private string mistakeFilePath = Directory.GetCurrentDirectory() + @"\Assets\Data\mistakes.txt";
    private string hitsFilePath = Directory.GetCurrentDirectory() + @"\Assets\Data\hits.txt";
    private string[] lines;

    private List<string> concepts;

    // Start is called before the first frame update
    void Start() {
        //lines = File.ReadAllLines(mistakeFilePath);
        //concepts = SetAuxiliarFeedback();

        //foreach(var i in concepts) {
        //    testText.text += i + "\n";
        //}
        ShowMistakesAndHits();
    }

    void ShowMistakesAndHits() {
        List<string> aux = new List<string>();
        lines = File.ReadAllLines(mistakeFilePath);

        foreach(string line in lines) {
            aux.Add(line + "\n");
        }

        aux = aux.Distinct().ToList();

        foreach(string item in aux) {
            mistakeText.text += item;
        }

        lines = File.ReadAllLines(hitsFilePath);
        aux.Clear();

        foreach(string line in lines) {
            aux.Add(line + "\n");
        }

        aux = aux.Distinct().ToList();
        
        foreach(string item in aux) {
            hitsText.text += item;
        }
    }

    // Update is called once per frame
    void Update() {

    }
}