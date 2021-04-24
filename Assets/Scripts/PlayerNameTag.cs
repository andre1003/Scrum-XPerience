using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerNameTag : Photon.MonoBehaviour {
    [SerializeField] private TextMeshProUGUI nameText;

    // Start is called before the first frame update
    void Start() {
        if(photonView.isMine) { return; }

        SetName();
    }

    private void SetName() {
        nameText.text = photonView.owner.NickName;
    }
}
