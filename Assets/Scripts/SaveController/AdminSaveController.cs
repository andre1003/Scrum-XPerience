using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AdminSaveController : MonoBehaviour {
    // Save choice fields
    public GameObject adminCanvas;
    public InputField decisionIdInputField;
    public InputField decisionDescriptionInputField;
    public InputField outputInputField;
    public InputField turnInputField;
    public InputField roundInputField;
    public Dropdown sceneryDropdown;
    public Dropdown roleDropdown;
    public Toggle isMistakeToggle;
    public Text previewText;

    // Search fields
    public GameObject searchCanvas;
    public InputField searchTurn;
    public InputField searchRound;
    public Dropdown searchScenery;
    public Dropdown searchRole;

    private int index = 0;
    private string path;
    private string scenery;

    private List<Decision> decisions;

    private void Awake() {
        path = Application.persistentDataPath + "/data/";
        decisions = new List<Decision>();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Keypad0)) {
            searchCanvas.SetActive(!searchCanvas.activeSelf);
            adminCanvas.SetActive(!adminCanvas.activeSelf);
        }
            
    }

    public void PreviewSave() {
        string text = "ID: " + decisionIdInputField.text + ".\n\nDescrição: " + decisionDescriptionInputField.text + ".\n\nCenário: " + sceneryDropdown.itemText.text;
        if(isMistakeToggle.isOn)
            text += "\n\nÉ um erro: Sim.";
        else
            text += "\n\nÉ um erro: Não.";

        previewText.text = text;
    }

    public void Save() {
        string role = "";
        
        if(Directory.Exists(path)) {
            string[] files = Directory.GetFiles(path);
            index = files.Length;
        }

        switch(roleDropdown.value) {
            // Scrum
            case 0:
                role = "Scrum Master";
                break;

            case 1:
                role = "Product Owner";
                break;

            case 2:
                role = "Time de Desenvolvimento";
                break;

            // XP
            case 3:
                role = "Gerente de Projetos";
                break;

            case 4:
                role = "Engenheiro de Testes";
                break;

            case 5:
                role = "Desenvolvedor";
                break;
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

        if(decisions.Count == 2) {
            // Mistake1
            SaveSystem.DeleteFromDB(scenery, role, turnInputField.text, roundInputField.text);
        }

        SaveSystem.SaveInDatabase(decisionIdInputField.text, decisionDescriptionInputField.text, scenery, outputInputField.text, isMistakeToggle.isOn, role, turnInputField.text, roundInputField.text);
        previewText.text = decisionIdInputField.text + " salvo com sucesso!";

        if(decisions.Count > 0) {
            SetFields(decisions[0].decisionId, decisions[0].decisionDescription, decisions[0].scenery, searchRole.value, decisions[0].output, searchTurn.text, searchRound.text, decisions[0].isMistake);
            decisions.RemoveAt(0);
        }

    }

    public void SearchChoice() {
        string role = "";
        string scenery = "";

        switch(searchRole.value) {
            // Scrum
            case 0:
                role = "Scrum Master";
                break;

            case 1:
                role = "Product Owner";
                break;

            case 2:
                role = "Time de Desenvolvimento";
                break;

            // XP
            case 3:
                role = "Gerente de Projetos";
                break;

            case 4:
                role = "Engenheiro de Testes";
                break;

            case 5:
                role = "Desenvolvedor";
                break;
        }

        switch(searchScenery.value) {
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

        decisions = SaveSystem.LoadFromDatabase(scenery, role, searchTurn.text, searchRound.text);
        SetFields(decisions[0].decisionId, decisions[0].decisionDescription, decisions[0].scenery, searchRole.value, decisions[0].output, searchTurn.text, searchRound.text, decisions[0].isMistake);
        decisions.RemoveAt(0);
    }

    private void SetFields(string id, string description, string scenery, int role, string output, string turn, string round, bool isMistake) {
        decisionIdInputField.text = id;
        decisionDescriptionInputField.text = description;

        roleDropdown.value = role;

        if(scenery.Equals("Reuniao Equipe"))
            sceneryDropdown.value = 0;
        else if(scenery.Equals("Reuniao Cliente"))
            sceneryDropdown.value = 1;
        else if(scenery.Equals("Desenvolvimento"))
            sceneryDropdown.value = 2;

        outputInputField.text = output;
        turnInputField.text = turn;
        roundInputField.text = round;
        isMistakeToggle.isOn = isMistake;
    }
}
