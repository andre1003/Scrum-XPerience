using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewTimer : MonoBehaviour {
    public Text timeText;
    public Text choiceTimeText;
    public Text roundText;

    public ChoiceController choiceController;
    public GameObject gameOver;

    public float maxTime;
    public int maxTurn = 5;
    public int maxRound = 5;

    public PhotonView photonView;

    private double timerIncrementValue;
    private double startTime;
    //[SerializeField] double timer = 20;
    private ExitGames.Client.Photon.Hashtable CustomeValue;

    private int round = 1;
    private int turn = 1;

    private bool isRoundEnded = true;

    // Start is called before the first frame update
    void Start() {
        SetTimer();
        roundText.text = "Turno: " + turn.ToString() + "\nRodada: " + round.ToString();
    }

    // Update is called once per frame
    void Update() {
        timerIncrementValue = PhotonNetwork.time - startTime;
        double realTime = maxTime - timerIncrementValue;
        int timeToText = Mathf.RoundToInt((float)realTime);

        timeText.text = "Tempo: " + timeToText.ToString();
        choiceTimeText.text = "Tempo: " + timeToText.ToString();

        if(realTime <= 0) {
            if(isRoundEnded == false) {
                choiceController.EndRound();
                isRoundEnded = true;
            }
            SetTimer();
            if(PhotonNetwork.player.IsMasterClient) {
                photonView.RPC("IncreaseRound", PhotonTargets.AllBuffered);
                //choiceController.EndRound();
            }
            photonView.RPC("EndRound", PhotonTargets.AllBuffered);
        }
        else {
            isRoundEnded = false;
        }

        CheckTurnsEnd();

        choiceController.SetTurn(turn);
        choiceController.SetRound(round);
    }

    void SetTimer() {
        if(PhotonNetwork.player.IsMasterClient) {
            CustomeValue = new ExitGames.Client.Photon.Hashtable();
            startTime = PhotonNetwork.time;
            CustomeValue.Add("StartTime", startTime);
            PhotonNetwork.room.SetCustomProperties(CustomeValue);
        }
        else {
            startTime = double.Parse(PhotonNetwork.room.CustomProperties["StartTime"].ToString());
        }
    }

    void CheckTurnsEnd() {
        if(turn == maxTurn) {
            choiceController.EndGame();
        }
        else if(round == maxRound) {
            round = 0;
            turn++;
            //photonView.RPC("EndRound", PhotonTargets.AllBuffered);
            choiceController.EndRound();
        }
    }

    [PunRPC]
    void IncreaseRound() {
        round++;
    }

    [PunRPC]
    void EndRound() {
        roundText.text = "Turno: " + turn.ToString() + "\nRodada: " + round.ToString();

        //choiceController.EndRound();

        //time = maxTime;
        //timeText.text = "Tempo: " + Mathf.Round(time).ToString();
        //choiceTimeText.text = "Tempo: " + Mathf.Round(time).ToString();

        //timeLeft = true;
    }

    public void GameOver() {
        gameOver.SetActive(true);
    }
}
