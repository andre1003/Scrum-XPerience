using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GeneralInfo {
    public int timeouts;
    public int teamMeetingMistakes;
    public int clientMeetingMistakes;
    public int developmentMistakes;

    public int teamMeetingHits;
    public int clientMeetingHits;
    public int developmentHits;

    public GeneralInfo(int timeouts, int teamMeetingMistakes, int clientMeetingMistakes, int developmentMistakes, int teamMeetingHits, int clientMeetingHits, int developmentHits) {
        this.timeouts = timeouts;
        this.teamMeetingMistakes = teamMeetingMistakes;
        this.clientMeetingMistakes = clientMeetingMistakes;
        this.developmentMistakes = developmentMistakes;

        this.teamMeetingHits = teamMeetingHits;
        this.clientMeetingHits = clientMeetingHits;
        this.developmentHits = developmentHits;
    }
}
