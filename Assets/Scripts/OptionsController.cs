using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Methods;
using System;

public class OptionsController : MonoBehaviour {
    public GameObject scrumFunctionCanvas;
    public GameObject xpFunctionCanvas;

    public Text votingStatus;

    public GameObject scrumMaster;
    public GameObject productOwner;
    public GameObject devTeam;

    public GameObject softwareManager;
    public GameObject testEngineer;
    public GameObject developer;

    public InfoController infoController;

    public PhotonView photonView;

    public int maxMethodVotes;
    public int maxFunctionVotes;

    private Scrum scrum;
    private XP xp;

    private int scrumVoteCount;
    private int xpVoteCount;

    private int functionsVoted;

    private bool hasVotedForMethod;
    private bool hasVotedForFunction;

    private Dictionary<string, int> functions = new Dictionary<string, int>() {
        { "Scrum Master", 0 },
        { "Product Owner", 1 },
        { "Time de Desenvolvimento", 2 },
        { "Gerente de Projetos", 3 },
        { "Engenheiro de Testes", 4 },
        { "Desenvolvedor", 5 },
    };

    private void Start() {
        scrum = new Scrum();
        xp = new XP();

        scrumVoteCount = 0;
        xpVoteCount = 0;
        functionsVoted = 0;
        hasVotedForMethod = false;
        hasVotedForFunction = false;
    }

    public void ChoseScrum() {
        if(!hasVotedForMethod) {
            hasVotedForMethod = true;
            photonView.RPC("SyncScrumChoice", PhotonTargets.AllBuffered);
        }
    }

    [PunRPC]
    private void SyncScrumChoice() {
        scrumVoteCount++;
        UnityEngine.Debug.Log(scrumVoteCount);

        votingStatus.text = "Votos: " + scrumVoteCount + " Scrum / " + xpVoteCount + " XP.";

        if(scrumVoteCount >= maxMethodVotes) {
            scrumFunctionCanvas.SetActive(true);
            PlayerPrefs.SetString("game_method", scrum.GetMethod());
        }
    }

    public void ChoseXP() {
        if(!hasVotedForMethod) {
            hasVotedForMethod = true;
            photonView.RPC("SyncXPChoice", PhotonTargets.AllBuffered);
        }
    }

    [PunRPC]
    private void SyncXPChoice() {
        xpVoteCount++;
        votingStatus.text = "Votes: " + scrumVoteCount + " Scrum / " + xpVoteCount + " XP.";

        if(xpVoteCount >= maxMethodVotes) {
            xpFunctionCanvas.SetActive(true);
            PlayerPrefs.SetString("game_method", xp.GetMethod());
        }
    }

    public void ScrumFunctionsChoices(string function) {
        if(!hasVotedForFunction) {
            hasVotedForFunction = true;
            photonView.RPC("SyncFunctionVote", PhotonTargets.AllBuffered, functions[function]);
            PlayerPrefs.SetString("player_function", function);
            infoController.SetInfoText("Você será o " + function + ".");
        }
    }

    public void XPFunctionsChoices(string function) {
        if(!hasVotedForFunction) {
            hasVotedForFunction = true;
            photonView.RPC("SyncFunctionVote", PhotonTargets.AllBuffered, functions[function]);
            PlayerPrefs.SetString("player_function", function);
            infoController.SetInfoText("Você será o " + function + ".");
        }
    }

    [PunRPC]
    private void SyncFunctionVote(int op) {
        functionsVoted++;

        if(op == 0)
            scrumMaster.SetActive(false);
        else if(op == 1)
            productOwner.SetActive(false);
        else if(op == 2)
            devTeam.SetActive(false);
        else if(op == 3)
            softwareManager.SetActive(false);
        else if(op == 4)
            testEngineer.SetActive(false);
        else
            developer.SetActive(false);

        if(functionsVoted >= maxFunctionVotes)
            PhotonNetwork.LoadLevel("MainScene");
    }
}
