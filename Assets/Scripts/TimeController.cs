using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour {
    public Text timeText;
    public Text choiceTimeText;
    public ChoiceController choiceController;
    public GameObject gameOver;
    private float time = 20f;
    private bool timeLeft = true;

    // Start is called before the first frame update
    void Start() {
        timeText.text = time.ToString();
    }

    // Update is called once per frame
    void Update() {
        if(timeLeft) {
            if(Mathf.Round(time) > 0) {
                time -= Time.deltaTime;
                timeText.text = "Tempo: " + Mathf.Round(time).ToString();
                choiceTimeText.text = "Tempo: " + Mathf.Round(time).ToString();
            }
            else {
                gameOver.SetActive(true);
                choiceController.LockOrUnlockPlayer();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                timeLeft = false;
            }
        }
    }
}
