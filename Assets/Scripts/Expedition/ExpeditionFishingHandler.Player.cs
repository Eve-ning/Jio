using System.Linq;
using Photon.Pun;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace DIPProject
{
    public partial class ExpeditionFishingHandler 
    {
        #region Variables

        public enum FishingPosition
        {
            Reset = 0,
            FrontLeft = 1,
            FrontRight = 2,
            BackLeft = 3,
            BackRight = 4,
        }
        
        [Tooltip("These are the locations that the players will be teleported to, x, y")] 
        public Vector2[] fishingTeleportLocations;
        
        [Tooltip("These are the fishing positions corresponding to the locations fished.")] 
        public FishingPosition[] fishingPositions;
        
        #endregion

        #region Player Movement Methods

        /// <summary>
        /// Gets the player specific teleport location based on the player list
        /// </summary>
        /// <returns></returns>
        private (FishingPosition, Vector2) GetMyTeleportLocation()
        {
            var players = PhotonNetwork.CurrentRoom.Players;
            int myActorKey = players.First(o => o.Value.NickName == PhotonNetwork.NickName).Key;
            int myIx = players.Keys.OrderBy(o => o).ToList().IndexOf(myActorKey);
            
            return (fishingPositions[myIx], fishingTeleportLocations[myIx]);
        }
        
        /// <summary>
        /// Teleports the player to the fishing locations.
        /// This will call GetTeleportLocation to get the player's destination.
        /// </summary>
        private void TeleportMyPlayer()
        {
            var myPlayer = GetMyPlayer();
            
            // Get my teleport location
            var (position, location) = GetMyTeleportLocation();
            
            Debug.Log("Teleporting myself to: " + location);
            
            myPlayer.transform.SetPositionAndRotation(
                new Vector3(location.x, location.y, transform.position.z),
                Quaternion.identity
            );

            // We need to set for all players so that they all appear to be fishing!
            foreach (var player in GetPlayers())
                player.GetComponent<Animator>().SetInteger("Fishing", (int) position);
        }
        
        /// <summary>
        /// Uses a OR to freeze constraints without loss of info
        /// </summary>
        private void FreezePlayers()
        {
            GetMyPlayer().GetComponent<Rigidbody2D>().constraints |=
                RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        }

        /// <summary>
        /// Uses an XOR to unfreeze constraints without loss of info
        /// </summary>
        public void UnfreezePlayers()
        {
            GetMyPlayer().GetComponent<Rigidbody2D>().constraints ^=
                RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        }

        #endregion
        
    }
}