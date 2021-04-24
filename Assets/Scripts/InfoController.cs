using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Methods;

public class InfoController : MonoBehaviour {
    public Text infoText;

    public void OverMethod(string method) {
        if(method.Equals("Scrum")) {
            infoText.text = new Scrum().GetMethodDescription();
        }
        else {
            infoText.text = new XP().GetMethodDescription();
        }
    }

    public void OverFunction(string function) {
        if(function.Equals("Scrum Master") || function.Equals("Product Owner") || function.Equals("Development Team")) {
            infoText.text = new Scrum().GetFunctionDescription(function);
        }
        else {
            infoText.text = new XP().GetFunctionDescription(function);
        }
    }

    public void Exit() {
        infoText.text = "";
    }
}
