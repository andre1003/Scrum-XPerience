using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InfoFile{
    public string status;
    public string username;
    public string role;
    public string hits;
    public string mistakes;
    public string feedback;
    public string group;
    public string matchId;

    public InfoFile(string status, string username, string role, string hits, string mistakes, string feedback, string group, string matchId) {
        this.status = status;
        this.username = username;
        this.role = role;
        this.hits = hits;
        this.mistakes = mistakes;
        this.feedback = feedback;
        this.group = group;
        this.matchId = matchId;
    }
}
