using Photon.Pun;
using TMPro;
using UnityEngine;

using MySql.Data;
using MySql.Data.MySqlClient;

namespace DIPProject
{
    /// <summary>
    ///     Handles the splash screen interactions
    /// </summary>
    public class SplashHandler : MonoBehaviourPunCallbacks
    {
        #region MonoBehaviourPunCallbacks Callbacks

        /// <summary>
        ///     Was able to connect to the master Server
        /// </summary>
        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to Photon Server.");
        }

        #endregion

        #region Variables

        [Tooltip("Username UI")] public TMP_InputField uiUsername;

        [Tooltip("Enter Button")] public GameObject uiJoinButton;
        public GameObject uiSignupButton;

        [Tooltip("Minimum Length before the Enter Button appears")] [SerializeField]
        private int minimumUsernameLength = 3;

        [Tooltip("The foreground animator, the Logo, Input and Join Button")] [SerializeField]
        private Animator foregroundAnimator;

        [Tooltip("The background animator, the clouds")] [SerializeField]
        private Animator backgroundAnimator;

        #endregion

        public TMP_InputField uiPassword;
        public bool existingUser = false;
        private string getConn = @"server=dipdatabase.cvxt4s3abkq1.us-east-2.rds.amazonaws.com;port=3306;username=admin;password=admin2073";

        #region MonoBehaviour Callbacks

        // Start is called before the first frame update
        private void Start()
        {
            uiJoinButton.SetActive(false);
            uiSignupButton.SetActive(false);
            PhotonNetwork.ConnectUsingSettings();
            
            PhotonNetwork.GameVersion = "0.0";
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return)) JoinLobby();
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Attempts to Join the Lobby
        /// </summary>
        public void JoinLobby()
        {
            if (ReadyToProceed() && AuthenticateUser())
            {
                foregroundAnimator.SetTrigger("Join");
                backgroundAnimator.SetTrigger("Join");
            }
        }

        /// <summary>
        ///     This should only be called by the animation event trigger.
        /// </summary>
        public void JoinLobbyAfterAnimation()
        {
            PhotonNetwork.NickName = uiUsername.text;
            PhotonNetwork.JoinLobby();
            PhotonNetwork.LoadLevel("Landing");
        }

        /// <summary>
        ///     Validates if the player inputs are good and if is connected
        /// </summary>
        /// <returns></returns>
        public bool ReadyToProceed()
        {
            return uiUsername.text.Length >= minimumUsernameLength &&
                   uiPassword.text.Length >= minimumUsernameLength &&
                   PhotonNetwork.IsConnected;
        }

        /// <summary>
        ///     Checks if the username is good, if it's good, then the Join button will appear
        /// </summary>
        public void ButtonUpdate()
        {
            uiJoinButton.SetActive(ReadyToProceed());
            uiSignupButton.SetActive(ReadyToProceed());
        }

        public void CheckUserName()
        {
            try {
            
                using var conn = new MySqlConnection(getConn);
                conn.Open();

                string query = "INSERT INTO users.loginID(appID) VALUES ('" + uiUsername.text + "')";
                using var cmd = new MySqlCommand(query, conn);

                using MySqlDataReader rdr = cmd.ExecuteReader();

                conn.Close();
            
                InsertCredentials();

            } catch {

                Debug.Log("Username exists!");

            }
        }

        private void InsertCredentials()
        {
            try
            {
                using var conn = new MySqlConnection(getConn);
                conn.Open();

                string query = "INSERT INTO users.credentials(appID, appKey) VALUES ('" + uiUsername.text + "', '" + uiPassword.text + "')";
                using var cmd = new MySqlCommand(query, conn);

                using MySqlDataReader rdr = cmd.ExecuteReader();

                conn.Close();

                Debug.Log("Registered!");
                
            } catch {

                Debug.Log("insertCredentials Error...");

            }
        }
        

        public bool AuthenticateUser()
        {
            try
            {
                print("t");
                using var conn = new MySqlConnection(getConn);
                conn.Open();
                print("t");
                string query = "INSERT INTO users.credentials(appID, appKey) VALUES ('" + uiUsername.text + "', '" + uiPassword.text + "')";
                using var cmd = new MySqlCommand(query, conn);
                print("t");
                using MySqlDataReader rdr = cmd.ExecuteReader();

                conn.Close();
                print("t");
                //error message if no catch because pair doesn't exist
                Debug.Log("Wrong Credentials!");

                DeletePair();
                return false;

            } catch {

                //authenticate if catch exception because pair exists
                Debug.Log("Logging In...");

                existingUser = true;
                return true;
            }
        }
        
        private void DeletePair()
        {
            try
            {

                using var conn = new MySqlConnection(getConn);
                conn.Open();

                string query = "DELETE FROM users.credentials WHERE appID = '" + uiUsername.text + "' AND appKey = '" + uiPassword.text + "'";
                using var cmd = new MySqlCommand(query, conn);

                using MySqlDataReader rdr = cmd.ExecuteReader();

                conn.Close();

            } catch {

                Debug.Log("deletePair Error...");

            }
        }

        #endregion
    }
}