using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;
using UnityEngine.UI;
using Methods;

public class GameController : Photon.MonoBehaviour {
    public GameObject playerPrefab;
    public GameObject gameCanvas;
    public GameObject spawnSpot;
    public GameObject postProcessing;

    public GameObject mapCanvas;
    public GameObject pauseMenu;

    public Toggle postProcessingToggle;

    public ChoiceController choiceController;

    private bool isPaused = false;

    private void Awake() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        gameCanvas.SetActive(true);
        choiceController.SetFunction(PlayerPrefs.GetString("player_function"));
        SaveGroup();
    }

    private void Update() {
        CheckInput();
    }

    private void CheckInput() {
        if(Input.GetKeyDown(KeyCode.M)) {
            mapCanvas.SetActive(!mapCanvas.activeSelf);
        }
        else if(Input.GetKeyDown(KeyCode.Escape)) { // Remove for build -> || Input.GetKeyDown(KeyCode.E)
            isPaused = !isPaused;
            PauseMenu(isPaused);
        }
    }

    public void SetPostProcessing(bool isActive) {
        postProcessing.SetActive(isActive);
    }

    public void SpawnPlayer() {
        GameObject spawnedPlayerGO = PhotonNetwork.Instantiate(playerPrefab.name, spawnSpot.transform.position, Quaternion.identity, 0);

        spawnedPlayerGO.GetComponent<MovementController>().enabled = true;
        spawnedPlayerGO.GetComponentInChildren<MouseController>().enabled = true;
        spawnedPlayerGO.GetComponentInChildren<Camera>().enabled = true;

        choiceController.SetMovementController(spawnedPlayerGO.GetComponent<MovementController>());
        choiceController.SetMouseController(spawnedPlayerGO.GetComponentInChildren<MouseController>());

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gameCanvas.SetActive(false);
    }

    private void PauseMenu(bool isPaused) {
        if(isPaused == true) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        choiceController.LockOrUnlockPlayer();
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }

    public void SetIsPaused(bool isPaused) {
        this.isPaused = isPaused;
    }

    private void SaveGroup() {
        if(PhotonNetwork.player.IsMasterClient) {
            Debug.Log(PlayerPrefs.GetString("group"));
            HttpClient client = new HttpClient();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("name", PlayerPrefs.GetString("group"));
            data.Add("score", "0");
            string response = SaveSystem.Post(SaveSystem.groupRegisterUrl, data, client);
            Debug.Log(response);
        }
    }
}
