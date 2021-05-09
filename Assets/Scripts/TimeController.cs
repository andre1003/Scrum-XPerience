using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour {
    public Text timeText;
    public Text choiceTimeText;
    public Text roundText;

    public ChoiceController choiceController;
    public GameObject gameOver;

    public float maxTime;
    private float time;
    private bool timeLeft = true;
    private int round = 1;

    // Start is called before the first frame update
    void Start() {
        timeText.text = time.ToString();
        roundText.text = "Rodada: " + round.ToString();
        time = maxTime;
    }

    // Update is called once per frame
    void Update() {
        if(timeLeft) {
            if(Mathf.Round(time) > 0) {
                time -= Time.deltaTime;
                timeText.text = "Tempo: " + Mathf.Round(time).ToString();
                choiceTimeText.text = "Tempo: " + Mathf.Round(time).ToString();
            }
            else {
                timeLeft = false;
            }
        }

        if(choiceController.GetPassedScenesList().Count == 4 || !timeLeft) { // New round
            round++;
            roundText.text = "Rodada: " + round.ToString();

            choiceController.EndRound();

            time = maxTime;
            timeText.text = "Tempo: " + Mathf.Round(time).ToString();
            choiceTimeText.text = "Tempo: " + Mathf.Round(time).ToString();
            
            timeLeft = true;
        }
    }
}
