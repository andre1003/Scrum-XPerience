using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data {
    public string decisionId;
    public string decisionDescription;
    public string scenary;
    public bool isMistake;

    public Data(string decisionId, string decisionDescription, string scenary, bool isMistake) {
        this.decisionId = decisionId;
        this.decisionDescription = decisionDescription;
        this.scenary = scenary;
        this.isMistake = isMistake;
    }
}
