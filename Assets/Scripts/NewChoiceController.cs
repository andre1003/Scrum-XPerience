using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewChoiceController : MonoBehaviour {
    public InputField function;
    public InputField turn;
    public InputField round;

    public void GetChoice() {
        List<Decision> data = SaveSystem.LoadFromDatabase("Reuniao Equipe", function.text, turn.text, round.text);

        Debug.Log(data[0].decisionDescription);
        Debug.Log(data[1].decisionDescription);
        Debug.Log(data[2].decisionDescription);
    }
}
