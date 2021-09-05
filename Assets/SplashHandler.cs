using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashHandler : MonoBehaviour
{
    public InputField uiUsername;
    public GameObject uiEnterButton;
    public int minimumUsernameLength = 3;

    // Start is called before the first frame update
    void Start()
    {
        uiEnterButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void validateUsername()
	{
        uiEnterButton.SetActive(uiUsername.text.Length >= minimumUsernameLength);
	}

    public void joinLobby()
	{
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        PhotonNetwork.NickName = uiUsername.text;
        PhotonNetwork.LoadLevel("Landing");
    }
}
