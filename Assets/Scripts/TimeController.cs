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
    private int turn = 1;
    private bool isGameOver = false;

    // Start is called before the first frame update
    void Start() {
        timeText.text = time.ToString();
        roundText.text = "Turno: " + turn.ToString() + "\nRodada: " + round.ToString();
        time = maxTime;
    }

    // Update is called once per frame
    void Update() {
        if(!isGameOver) {
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
                EndRound();
            }

            choiceController.SetRound(round);
            

            CheckTurnsEnd();
        }

        choiceController.SetTurn(turn);
    }

    void CheckTurnsEnd() {
        if(turn == 5) {
            gameOver.SetActive(true);
            isGameOver = true;
        }
        else if(round == 5) {
            round = 1;
            turn++;
            EndRound();
        }
    }

    void EndRound() {
        roundText.text = "Turno: " + turn.ToString() + "\nRodada: " + round.ToString();

        choiceController.EndRound();

        time = maxTime;
        timeText.text = "Tempo: " + Mathf.Round(time).ToString();
        choiceTimeText.text = "Tempo: " + Mathf.Round(time).ToString();

        timeLeft = true;
    }
}
