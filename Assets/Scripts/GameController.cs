using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Methods;

public class GameController : Photon.MonoBehaviour {
    public GameObject playerPrefab;
    public GameObject gameCanvas;
    public GameObject spawnSpot;

    private static string playerFunction;
    private static string gameMethod;

    private List<string> choices;

    private MouseController mouseController;
    private MovementController movementController;

    private void Awake() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        gameCanvas.SetActive(true);
    }

    private void Start() {
        
        gameMethod = PlayerPrefs.GetString("game_method");
        playerFunction = PlayerPrefs.GetString("player_function");
        Debug.Log(playerFunction);
        choices = GetPlayerChoicesByFunction(playerFunction);
    }

    public void SpawnPlayer() {
        float randomValue = Random.Range(-1f, 1f);

        GameObject spawnedPlayerGO = PhotonNetwork.Instantiate(playerPrefab.name, spawnSpot.transform.position, Quaternion.identity, 0);

        movementController = spawnedPlayerGO.GetComponent<MovementController>();
        mouseController = spawnedPlayerGO.GetComponentInChildren<MouseController>();

        spawnedPlayerGO.GetComponent<MovementController>().enabled = true;
        spawnedPlayerGO.GetComponentInChildren<MouseController>().enabled = true;
        spawnedPlayerGO.GetComponentInChildren<Camera>().enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gameCanvas.SetActive(false);
    }

    public void LockOrUnlockPlayer() {
        movementController.enabled = !movementController.enabled;
        mouseController.enabled = !mouseController.enabled;
    }

    private List<string> GetPlayerChoicesByFunction(string function) {
        if(gameMethod.Equals("Scrum")) {
            return new Scrum().GetChoicesByKey(function);
        }
        else {
            return new XP().GetChoicesByKey(function);
        }
    }
}
