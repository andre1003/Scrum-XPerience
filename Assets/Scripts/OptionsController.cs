using System.Collections;
using System.Collections.Generic;
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

    public Text connectedUsers;

    public PhotonView photonView;

    public int maxVotes;

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
        { "Development Team", 2 },
        { "Software Manager", 3 },
        { "Test Engineer", 4 },
        { "Developer", 5 },
    };

    private void Start() {
        scrum = new Scrum();
        xp = new XP();

        scrumVoteCount = 0;
        xpVoteCount = 0;
        functionsVoted = 0;
        hasVotedForMethod = false;
        hasVotedForFunction = false;

        photonView.RPC("SetPlayerData", PhotonTargets.AllBuffered);
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
        Debug.Log(scrumVoteCount);

        votingStatus.text = "Votes: " + scrumVoteCount + " Scrum / " + xpVoteCount + " XP.";

        if(scrumVoteCount >= maxVotes) {
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

        if(xpVoteCount >= maxVotes) {
            xpFunctionCanvas.SetActive(true);
            PlayerPrefs.SetString("game_method", xp.GetMethod());
        }
    }

    public void ScrumFunctionsChoices(string function) {
        if(!hasVotedForFunction) {
            hasVotedForFunction = true;
            photonView.RPC("SyncFunctionVote", PhotonTargets.AllBuffered, functions[function]);
            PlayerPrefs.SetString("player_function", function);
        }
    }

    public void XPFunctionsChoices(string function) {
        if(!hasVotedForFunction) {
            hasVotedForFunction = true;
            photonView.RPC("SyncFunctionVote", PhotonTargets.AllBuffered, functions[function]);
            PlayerPrefs.SetString("player_function", function);
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

        if(functionsVoted >= 1)
            PhotonNetwork.LoadLevel("MainScene");
    }

    [PunRPC]
    private void SetPlayerData() {
        string usernames = "Connected Players:\n\n";
        int i = 0;

        for(int index = 0; index < PhotonNetwork.playerList.Length; index++) {
            try {
                usernames += PhotonPlayer.Find(i + 1).NickName + "\n";
            }
            finally {
                i++;
            }
        }

        connectedUsers.text = usernames;
    }
}
