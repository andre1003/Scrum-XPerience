using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ChoiceController : MonoBehaviour {

    public string function;
    public string scene;
    public List<Text> list;

    private void Start() {
        System.Random rdn = new System.Random();
        int lenght = list.Count;
        int rightChoice = rdn.Next(0, 3); // Alterar para a quantidade de escolhas
        Debug.Log(rightChoice);

        for(int i = 0; i < lenght; i++) {
            if(i == rightChoice) {
                ChangeTextByFunction(list[i], true);
            }
            else {
                ChangeTextByFunction(list[i], false);
            }
        }
    }

    void ChangeTextByFunction(Text btnText, bool right) {
        if(scene.Equals("Reuniao Equipe")) {
            if(right) {
                btnText.text = File.ReadAllLines(Directory.GetCurrentDirectory() + @"\Assets\Data\" + scene + @"\" + function + @".txt")[0];
            }
            else {
                if(function.Equals("Scrum Master")) {
                    btnText.text = "Scrum Master Wrong";
                }
                else if(function.Equals("Product Owner")) {
                    btnText.text = "Product Owner Wrong";
                }
            }
        }
        else {
            if(right) {
                if(function.Equals("Scrum Master")) {
                    btnText.text = "Scrum Master Right";
                }
                else if(function.Equals("Product Owner")) {
                    btnText.text = "Product Owner Right";
                }
            }
            else {
                if(function.Equals("Scrum Master")) {
                    btnText.text = "Wrong";
                }
                else if(function.Equals("Product Owner")) {
                    btnText.text = "Wrong";
                }
            }
        }
    }

    public void ShowText(Text btnText) {
        Debug.Log(btnText.text);
        btnText.text = "Acessado!";
    }
}
