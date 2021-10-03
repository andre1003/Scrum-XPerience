using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Net.Http;

public class MenuController : MonoBehaviour {
    [SerializeField] private string version = "0.1";
    
    public InputField usernameInputField;
    public InputField passwordInputField;

    public Text alertText;

    public InputField roomNameInputField;

    public GameObject startButton;
    public GameObject usernameInputCanvas;
    public GameObject roomOptionsCanvas;
    public GameObject connectionFailedCanvas;

    private EventSystem system;
    private Selectable firstElement;

    private Color red = new Color(226, 0, 0, 255);
    private Color black = new Color(0, 0, 0, 255);

    public void Awake() {
        bool connectionStatus = PhotonNetwork.ConnectUsingSettings(version); // False for connection failed

        if(connectionStatus == false) {
            // Connection to Photon server failed
            connectionFailedCanvas.SetActive(true);
        }
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

    private void TabBetweenElements() {
        Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
        UnityEngine.Debug.Log(next);

        if(next != null) {
            InputField inputfield = next.GetComponent<InputField>();
            if(inputfield != null)
                inputfield.OnPointerClick(new PointerEventData(system));  // If it's an input field, also set the text caret

            system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
        }
        else {
            firstElement.Select();
        }
    }

    private void CheckLogin(string response) {
        if(response.Equals("fail")) { // If auth data was not recognized by server (no user data returned)
            usernameInputField.text = ""; // Clear username text
            passwordInputField.text = ""; // Clear password text
            alertText.color = red; // Set text color to red
            alertText.text = "Credenciais incorretas! Tente novamente."; // Display a message for user

            usernameInputField.Select();
        }
        else {
            SetUsernameFromDB(usernameInputField.text);
        }
    }

    private void OnConnectedToMaster() {
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

        alertText.color = black;
        alertText.text = "Conectando...";

        HttpClient client = new HttpClient();
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("username", username);
        data.Add("password", password);
        string response = SaveSystem.Post(SaveSystem.loginUrl, data, client);

        CheckLogin(response);
    }

    public void SetUsernameFromDB(string username) {
        roomOptionsCanvas.SetActive(true);
        roomNameInputField.Select();
        firstElement = roomNameInputField;
        usernameInputCanvas.SetActive(false);
        PhotonNetwork.playerName = username;
        PlayerPrefs.SetString("username", username);
    }

    public void CreateGame() {
        PhotonNetwork.CreateRoom(roomNameInputField.text, new RoomOptions() { MaxPlayers = 4 }, null);
        PlayerPrefs.SetString("group", roomNameInputField.text);
    }

    public void JoinGame() {
        PhotonNetwork.JoinOrCreateRoom(roomNameInputField.text, new RoomOptions() { MaxPlayers = 4 }, TypedLobby.Default);
        PlayerPrefs.SetString("group", roomNameInputField.text);
    }

    private void OnJoinedRoom() {
        PhotonNetwork.LoadLevel("MatchConfiguration");
    }

    public void BackToMainMenu() {
        SceneManager.LoadScene(0);
    }
}
