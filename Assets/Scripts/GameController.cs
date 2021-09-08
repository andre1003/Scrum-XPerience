using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Methods;

public class GameController : Photon.MonoBehaviour {
    public GameObject playerPrefab;
    public GameObject gameCanvas;
    public GameObject spawnSpot;
    public GameObject postProcessing;

    public GameObject mapCanvas;
    //public GameObject pauseMenu;

    public Toggle postProcessingToggle;

    public ChoiceController choiceController;

    private static string playerFunction;
    private static string gameMethod;

    private List<string> choices;

    private MouseController mouseController;
    private MovementController movementController;

    private void Awake() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        gameCanvas.SetActive(true);

        //if(PlayerPrefs.GetInt("post_processing") == 1) {
        //    postProcessing.SetActive(true);
        //    postProcessingToggle.isOn = true;
        //}
    }

    private void Start() {
        
        gameMethod = PlayerPrefs.GetString("game_method");
        choiceController.SetFunction(PlayerPrefs.GetString("player_function"));
        Debug.Log(playerFunction);
        //choices = GetPlayerChoicesByFunction(playerFunction);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.M)) {
            mapCanvas.SetActive(!mapCanvas.activeSelf);
        }

        if(PlayerPrefs.GetInt("post_processing") == 1) {
            postProcessing.SetActive(true);
        }
        else {
            postProcessing.SetActive(false);
        }
    }

    public void SetPostProcessing(bool isActive) {
        postProcessing.SetActive(isActive);
    }

    //private void Update() {
    //    if(Input.GetKeyDown(KeyCode.Escape))
    //        PauseMenu();
    //}

    public void SpawnPlayer() {
        GameObject spawnedPlayerGO = PhotonNetwork.Instantiate(playerPrefab.name, spawnSpot.transform.position, Quaternion.identity, 0);

        movementController = spawnedPlayerGO.GetComponent<MovementController>();
        mouseController = spawnedPlayerGO.GetComponentInChildren<MouseController>();

        spawnedPlayerGO.GetComponent<MovementController>().enabled = true;
        spawnedPlayerGO.GetComponentInChildren<MouseController>().enabled = true;
        spawnedPlayerGO.GetComponentInChildren<Camera>().enabled = true;

        //choiceController.SetPhotonView(spawnedPlayerGO.GetComponent<PhotonView>());
        //timeController.SetPhotonView(spawnedPlayerGO.GetComponent<PhotonView>());
        choiceController.SetMovementController(spawnedPlayerGO.GetComponent<MovementController>());
        choiceController.SetMouseController(spawnedPlayerGO.GetComponentInChildren<MouseController>());

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gameCanvas.SetActive(false);
    }

    //public void LockOrUnlockPlayer() {
    //    movementController.enabled = !movementController.enabled;
    //    mouseController.enabled = !mouseController.enabled;
    //}

    //private List<string> GetPlayerChoicesByFunction(string function) {
    //    if(gameMethod.Equals("Scrum")) {
    //        return new Scrum().GetChoicesByKey(function);
    //    }
    //    else {
    //        return new XP().GetChoicesByKey(function);
    //    }
    //}

    //private void PauseMenu() {
    //    pauseMenu.SetActive(!pauseMenu.activeSelf);
    //}
}
