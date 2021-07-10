using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AdminSaveController : MonoBehaviour {

    public InputField decisionIdInputField;
    public InputField decisionDescriptionInputField;
    public InputField sceneryInputField;
    public Toggle isMistakeToggle;

    public Text previewText;

    private int index = 0;
    private string path = Directory.GetCurrentDirectory() + "/data/";

    public void PreviewSave() {
        string text = "ID: " + decisionIdInputField.text + ".\n\nDescriÁ„o: " + decisionDescriptionInputField.text + ".\n\nCen·rio: " + sceneryInputField.text;
        if(isMistakeToggle.isOn)
            text += "\n\n… um erro: Sim.";
        else
            text += "\n\n… um erro: N„o.";

        previewText.text = text;
    }

    public void Save() {
        if(Directory.Exists(path)) {
            string[] files = Directory.GetFiles(path);
            index = files.Length;
        }
        
        SaveSystem.SaveInDatabase(decisionIdInputField.text, decisionDescriptionInputField.text, sceneryInputField.text, isMistakeToggle.isOn, index);
    }
}
