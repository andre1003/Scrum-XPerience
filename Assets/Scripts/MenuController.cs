using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
    [SerializeField] private string version = "0.1";
    
    public InputField usernameInputField;
    public InputField passwordInputField;

    /*public InputField joinGameInputField;
    public InputField createGameInputField;*/

    public InputField roomNameInputField;

    public GameObject startButton;
    public GameObject usernameInputCanvas;
    public GameObject roomOptionsCanvas;

    public DatabaseConnection dbConnection;

    private bool isUserAuthenticated = false;

    public void Awake() {
        PhotonNetwork.ConnectUsingSettings(version);
    }

    // Start is called before the first frame update
    void Start() {
        usernameInputField.Select();
    }

    private void Update() {
        if(File.Exists(Directory.GetCurrentDirectory() + @"\Data\data.txt") && isUserAuthenticated == false) {
            dbConnection.TokenConnection();
            isUserAuthenticated = true;
        }
    }

    void OnConnectedToMaster() {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public void ChangeTextInput() {
        if(usernameInputField.text.Length >= 3 && passwordInputField.text.Length >= 3) {
            startButton.SetActive(true);
        }
        else {
            startButton.SetActive(false);
        }
    }

    /* TEST ONLY METHOD */
    public void SetUsername() {
        roomOptionsCanvas.SetActive(true);
        roomNameInputField.Select();
        usernameInputCanvas.SetActive(false);
        PhotonNetwork.playerName = usernameInputField.text;
    }
    /* TEST ONLY METHOD */

    public void AuthUser() {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        usernameInputField.text = "";
        passwordInputField.text = "";

        Process.Start(Directory.GetCurrentDirectory() + @"\Scripts\first_login.exe", username + " " + password);        
    }

    public void SetUsernameFromDB(string username) {
        roomOptionsCanvas.SetActive(true);
        roomNameInputField.Select();
        usernameInputCanvas.SetActive(false);
        PhotonNetwork.playerName = username;
    }

    public void CreateGame() {
        PhotonNetwork.CreateRoom(roomNameInputField.text, new RoomOptions() { MaxPlayers = 4 }, null);
    }

    public void JoinGame() {
        PhotonNetwork.JoinOrCreateRoom(roomNameInputField.text, new RoomOptions() { MaxPlayers = 4 }, TypedLobby.Default);
    }

    private void OnJoinedRoom() {
        PhotonNetwork.LoadLevel("MatchConfiguration");
    }
}
