using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviourPunCallbacks
{
	[SerializeField] private string VersionName = "0.0.1";
	[SerializeField] private GameObject UsernameMenu;
	[SerializeField] private GameObject ConnectPanel;
	[SerializeField] private GameObject StartButton;

	[SerializeField] private InputField UsernameInput;
	[SerializeField] private InputField CreateInput;
	[SerializeField] private InputField JoinInput;


	private void Awake()
	{
		PhotonNetwork.ConnectUsingSettings();
	}


	private void Start()
	{
		UsernameMenu.SetActive(true);
	}

	public override void OnConnectedToMaster()
	{
		PhotonNetwork.JoinLobby(TypedLobby.Default);
		Debug.Log("Connected to Photon Server.");
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public void ChangeUserNameInput()
	{
		StartButton.SetActive(UsernameInput.text.Length >= 3) ;
	}

	public void SetUserName()
	{
		UsernameMenu.SetActive(false);
		PhotonNetwork.NickName = UsernameInput.text;
		PhotonNetwork.JoinLobby();
	}

	public void CreateGame()
	{
		PhotonNetwork.CreateRoom(CreateInput.text, new RoomOptions() { MaxPlayers = 5 }, null);
		Debug.Log("Created Game " + CreateInput.text);
	}

	public void JoinGame()
	{
		PhotonNetwork.JoinOrCreateRoom(JoinInput.text, new RoomOptions() { MaxPlayers = 5 }, null);
		Debug.Log("Joined Or Created Game " + JoinInput.text);
	}

	public override void OnJoinedRoom() 
	{
		Debug.Log("Successfully Joined Room.");
		Debug.Log("Moving To MainGame Level");
		PhotonNetwork.LoadLevel("MainGame");
	}
}

