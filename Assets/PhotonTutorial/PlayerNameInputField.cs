using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;


namespace Com.PhotonPunTut.DIPProject
{
    [RequireComponent(typeof(InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {
        #region Private Constants

        const string playerNamePrefKey = "Player Name";

		#endregion

		#region MonoBehavior CallBacks

		// Start is called before the first frame update
		void Start()
        {
            string defaultName = string.Empty;
            InputField _inputField = GetComponent<InputField>();
            if (_inputField != null)
			{
                if (PlayerPrefs.HasKey(playerNamePrefKey))
				{
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                    _inputField.text = defaultName;
				}
			}

            PhotonNetwork.NickName = defaultName;
        }

		#endregion

		#region Public Methods

        public void SetPlayerName(string name)
		{
            if (string.IsNullOrEmpty(name))
			{
                Debug.LogError("Setting empty player name.");
                return;
            }
            PhotonNetwork.NickName = name;

            PlayerPrefs.SetString(playerNamePrefKey, name);
		}

		#endregion


		// Update is called once per frame
		void Update()
        {

        }
    }
}