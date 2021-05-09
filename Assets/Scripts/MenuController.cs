using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class MenuController : MonoBehaviour {
    [SerializeField] private string version = "0.1";
    
    public InputField usernameInputField;
    public InputField passwordInputField;

    public Text alertText;

    public InputField roomNameInputField;

    public GameObject startButton;
    public GameObject usernameInputCanvas;
    public GameObject roomOptionsCanvas;

    public DatabaseConnection dbConnection;

    private static string scorePath = Directory.GetCurrentDirectory() + @"\Assets\Data\score.txt";
    private static string loginExecutablePath = Directory.GetCurrentDirectory() + @"\Assets\Scripts\login.exe";

    private EventSystem system;
    private Selectable firstElement;

    public void Awake() {
        PhotonNetwork.ConnectUsingSettings(version);

        if(File.Exists(scorePath))
            File.Delete(scorePath);
    }

    // Start is called before the first frame update
    void Start() {
        usernameInputField.Select();
        system = EventSystem.current;
        firstElement = usernameInputField;
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Tab))
            TabBetweenElements();
    }

    void TabBetweenElements() {
        Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
        UnityEngine.Debug.Log(next);

        if(next != null) {
            InputField inputfield = next.GetComponent<InputField>();
            if(inputfield != null)
                inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret

            system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
        }
        else {
            firstElement.Select();
        }
    }

    void CheckLogin() {
        string fileContent = File.ReadAllText(scorePath);

        if(fileContent.Equals("Credenciais incorretas")) { // If auth data was not recognized by server (no user data returned)
            usernameInputField.text = ""; // Clear username text
            passwordInputField.text = ""; // Clear password text
            alertText.text = "Credenciais incorretas! Tente novamente."; // Display a message for user

            usernameInputField.Select();

            File.Delete(scorePath);
        }
        else {
            SetUsernameFromDB(usernameInputField.text);
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

        passwordInputField.text = "";
        alertText.text = "";

        Process.Start(loginExecutablePath, username + " " + password).WaitForExit(); // Wait for the end of login process

        CheckLogin();
    }

    public void SetUsernameFromDB(string username) {
        roomOptionsCanvas.SetActive(true);
        roomNameInputField.Select();
        firstElement = roomNameInputField;
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
