using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Decision {
    public string decisionId;
    public string decisionDescription;
    public string scenery;
    public string output;
    public bool isMistake;

    public Decision(string decisionId, string decisionDescription, string scenery, string output, bool isMistake) {
        this.decisionId = decisionId;
        this.decisionDescription = decisionDescription;
        this.scenery = scenery;
        this.output = output;
        this.isMistake = isMistake;
    }

    public bool Equals(Decision obj) {
        if(obj == null)
            return false;
        else if(this.decisionId.Equals(obj.decisionId) && this.decisionDescription.Equals(obj.decisionDescription) && this.scenery.Equals(obj.scenery) && this.output.Equals(obj.output) && this.isMistake == obj.isMistake)
            return true;
        else
            return false;
    }
}
