using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewTimer : MonoBehaviour {
    public Text timeText;
    public Text choiceTimeText;
    public Text roundText;
    public Text choiceRoundText;
    public Text stageText;

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

    private Color green = new Color(0.1512582f, 1f, 0f);
    private Color yellow = new Color(0.93859f, 1f, 0f);
    private Color orange = new Color(1f, 0.6900075f, 0f);
    private Color red = new Color(1f, 0.1877369f, 0f);

    // Start is called before the first frame update
    void Start() {
        SetTimer();
        roundText.text = "Turno: " + turn.ToString() + "\nRodada: " + round.ToString();
        choiceRoundText.text = "Turno: " + turn.ToString() + "\nRodada: " + round.ToString();
        stageText.text = "Início do Incremento";
        stageText.color = green;
    }

    // Update is called once per frame
    void Update() {
        timerIncrementValue = PhotonNetwork.time - startTime;
        double realTime = maxTime - timerIncrementValue;
        int timeToText = Mathf.RoundToInt((float)realTime);

        //photonView.RPC("CheckRoundEnd", PhotonTargets.AllBuffered);

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
                photonView.RPC("CheckTurnsEnd", PhotonTargets.AllBuffered);
            }
            photonView.RPC("EndRound", PhotonTargets.AllBuffered);
        }
        else {
            isRoundEnded = false;
        }

        

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

    [PunRPC]
    void CheckTurnsEnd() {
        if(round == maxRound) {
            round = 1;
            turn++;
        }

        if(turn == maxTurn) {
            choiceController.EndGame();
        }

        switch(round) {
            case 1:
                stageText.text = "Início do Incremento";
                stageText.color = green;
                break;

            case 2:
                stageText.text = "Primeira Metade do Incremento";
                stageText.color = yellow;
                break;

            case 3:
                stageText.text = "Segunda Metade do Incremento";
                stageText.color = orange;
                break;

            case 4:
                stageText.text = "Final do Incremento";
                stageText.color = red;
                break;
        }
    }

    [PunRPC]
    void IncreaseRound() {
        round++;
    }

    [PunRPC]
    void EndRound() {
        roundText.text = "Turno: " + turn.ToString() + "\nRodada: " + round.ToString();
        choiceRoundText.text = "Turno: " + turn.ToString() + "\nRodada: " + round.ToString();
    }

    public void GameOver() {
        gameOver.SetActive(true);
    }

    //[PunRPC]
    //private void CheckRoundEnd() {
    //    if(PhotonNetwork.player.IsMasterClient) {
    //        int allPassedScenes = choiceController.GetAllPassedScenes();
    //        int players = PhotonNetwork.countOfPlayers;

    //        if(allPassedScenes == (2 * players)) {
    //            if(isRoundEnded == false) {
    //                choiceController.EndRound();
    //                isRoundEnded = true;
    //            }
    //            SetTimer();
    //            photonView.RPC("IncreaseRound", PhotonTargets.AllBuffered);
    //            photonView.RPC("EndRound", PhotonTargets.AllBuffered);
    //        }
    //    }
    //}
}
