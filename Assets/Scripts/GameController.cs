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

    public GameObject choiceCanvas;
    public GameObject statsCanvas;

    public Toggle postProcessingToggle;

    public ChoiceController choiceController;

    private bool isPaused = false;

    private void Awake() {
        GameObject[] musicObjects = GameObject.FindGameObjectsWithTag("Music");
        foreach(GameObject musicObject in musicObjects)
            Destroy(musicObject);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        gameCanvas.SetActive(true);
        choiceController.SetFunction(PlayerPrefs.GetString("player_function"));

        if(PlayerPrefs.GetInt("post_processing") == 1)
            postProcessing.SetActive(true);
        
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
        System.Random random = new System.Random();
        float z = random.Next(-3, 3);
        float x = random.Next(-3, 3);
        Vector3 spawnPosition = new Vector3(spawnSpot.transform.position.x + x, spawnSpot.transform.position.y, spawnSpot.transform.position.z + z);

        GameObject spawnedPlayerGO = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity, 0);

        spawnedPlayerGO.GetComponent<MovementController>().enabled = true;
        spawnedPlayerGO.GetComponentInChildren<MouseController>().enabled = true;
        spawnedPlayerGO.GetComponentInChildren<Camera>().enabled = true;

        choiceController.SetMovementController(spawnedPlayerGO.GetComponent<MovementController>());
        choiceController.SetMouseController(spawnedPlayerGO.GetComponentInChildren<MouseController>());

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gameCanvas.SetActive(false);
    }

    public void PauseMenu(bool isPaused) {
        this.isPaused = isPaused;

        if(choiceCanvas.activeSelf == false && statsCanvas.activeSelf == false) {
            if(isPaused == true) {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            }
            else {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            choiceController.LockOrUnlockPlayer();
        }        
        
        pauseMenu.SetActive(!pauseMenu.activeSelf);
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
