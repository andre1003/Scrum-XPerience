using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AdminSaveController : MonoBehaviour {

    public InputField decisionIdInputField;
    public InputField decisionDescriptionInputField;
    public Dropdown sceneryDropdown;
    public Toggle isMistakeToggle;

    public Text previewText;

    private int index = 0;
    private string path;
    private string scenery;

    private void Awake() {
        path = Application.persistentDataPath + "/data/";
    }

    public void PreviewSave() {
        string text = "ID: " + decisionIdInputField.text + ".\n\nDescriÁ„o: " + decisionDescriptionInputField.text + ".\n\nCen·rio: " + sceneryDropdown.itemText.text;
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

        switch(sceneryDropdown.value) {
            case 0:
                scenery = "Reuniao Equipe";
                break;

            case 1:
                scenery = "Reuniao Cliente";
                break;

            case 2:
                scenery = "Desenvolvimento";
                break;
        }

        SaveSystem.SaveInDatabase(decisionIdInputField.text, decisionDescriptionInputField.text, scenery, isMistakeToggle.isOn, index);
        previewText.text = "Sucesso!";
    }
}
