using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonChatManager : MonoBehaviour, IChatClientListener
{
    ChatClient chatClient;
	[Tooltip("The InputField for sending a message")]
	[SerializeField] InputField chatInput;
	[Tooltip("The TextWindow")]
	[SerializeField] Text chatWindow;
	[SerializeField] Button chatBtn;

	#region IChatClientListener Callbacks

	public void DebugReturn(DebugLevel level, string message)
	{
		//throw new System.NotImplementedException();
	}

	public void OnChatStateChange(ChatState state)
	{
	}

	public void OnConnected()
	{
		MsgRoom(PhotonNetwork.NickName + " has connected to the channel!");
		chatClient.Subscribe(PhotonNetwork.CurrentRoom.Name);
		ToggleChat(true);
	}

	public void OnDisconnected()
	{
		MsgRoom(PhotonNetwork.NickName + " has disconnected from the channel!");
		ToggleChat(false);
	}

	public void OnGetMessages(string channelName, string[] senders, object[] messages)
	{
		string msg = "";
		for (int i = 0; i < senders.Length; i++)
		{
			msg = string.Format("{0}{1}={2}, ", msg, senders[i], messages[i]);
		}
		Debug.LogFormat("OnGetMessages: {0} ({1}) > {2}", channelName, senders.Length, msg);
		AppendChat(senders, messages);
	}

	public void OnPrivateMessage(string sender, object message, string channelName)
	{
		//throw new System.NotImplementedException();
	}

	public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
	{
		//throw new System.NotImplementedException();
	}

	public void OnSubscribed(string[] channels, bool[] results)
	{
		Debug.Log(PhotonNetwork.NickName + " has subscribed to " + PhotonNetwork.CurrentRoom.Name);
	}

	public void OnUnsubscribed(string[] channels)
	{
		//throw new System.NotImplementedException();
	}

	public void OnUserSubscribed(string channel, string user)
	{
		Debug.Log(user + " has subscribed to " + channel);
	}

	public void OnUserUnsubscribed(string channel, string user)
	{
		Debug.Log(user + " has unsubscribed from " + channel);
	}

	#endregion

	#region MonoBehavior Callbacks

	// Start is called before the first frame update
	void Start()
    {
		ToggleChat(false);
    }


    // Update is called once per frame
    void Update()
    {
		if (PhotonNetwork.InRoom)
		{
			if (chatClient == null) Connect();
			chatClient.Service();
		}
    }

	#endregion

	#region Public Methods
	public void Connect()
	{
		Debug.Log("Photon Chat attempting to Connect");
		chatClient = new ChatClient(this);
		chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(PhotonNetwork.NickName));

	}

	void MsgRoom(string msg)
	{
		bool result = chatClient.PublishMessage(PhotonNetwork.CurrentRoom.Name, msg);

		Debug.Log("Attempting to send message " + msg + " to " + PhotonNetwork.CurrentRoom.Name);
		if (!result) Debug.Log("Failed to send message");
	}

	public void SendChatMsg()
	{
		MsgRoom(chatInput.text);
		chatInput.text = "";
	}

	void ToggleChat(bool status)
	{
		chatBtn.enabled = status;
		chatInput.enabled = status;
		chatWindow.enabled = status;
	}

	public void AppendChat(string[] senders, object[] messages)
	{
		for (int i = 0; i < senders.Length; i++)
		{
			chatWindow.text += "\n" + senders[i] + ": " + messages[i];
		}

		Debug.Log(chatWindow.text.Split('\n'));
	}


	#endregion
}
