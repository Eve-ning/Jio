using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashHandler : MonoBehaviourPunCallbacks
{
    public InputField uiUsername;
    public GameObject uiEnterButton;
    public int minimumUsernameLength = 3;

    // Start is called before the first frame update
    void Start()
    {
        uiEnterButton.SetActive(false);
        Connect();
    }

	private void Awake()
	{
        PhotonNetwork.AutomaticallySyncScene = true;
	}

	public void Connect()
	{
        if (PhotonNetwork.IsConnected)
		{
            PhotonNetwork.JoinLobby();
		}
        else
		{
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = "0.0";
		}
	}

    //Was able to connect to the master Server
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("Connected to Photon Server.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void validateUsername()
	{
        uiEnterButton.SetActive(uiUsername.text.Length >= minimumUsernameLength && PhotonNetwork.IsConnectedAndReady);
	}

    public void joinLobby()
	{
        PhotonNetwork.NickName = uiUsername.text;
        PhotonNetwork.LoadLevel("Landing");
    }
}
