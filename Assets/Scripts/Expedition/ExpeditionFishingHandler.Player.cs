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

        [Tooltip("These are the locations that the players will be teleported to, x, y")] 
        public Vector2[] fishingTeleportLocations;
        // These are the locations that the players were before teleporting. 
        private Vector2 fishingPreviousLocation = Vector2.zero;

        #endregion

    
        #region Player Movement Methods

        /// <summary>
        /// Gets the player specific teleport location based on the player list
        /// </summary>
        /// <returns></returns>
        private Vector2 GetMyTeleportLocation()
        {
            var players = PhotonNetwork.CurrentRoom.Players;
            int myActorKey = players.First(o => o.Value.NickName == PhotonNetwork.NickName).Key;
            int myIx = players.Keys.OrderBy(o => o).ToList().IndexOf(myActorKey);
            
            return fishingTeleportLocations[myIx];
        }
        
        /// <summary>
        /// Teleports the player to the fishing locations.
        /// This will call GetTeleportLocation to get the player's destination.
        /// </summary>
        private void TeleportMyPlayerTo()
        {
            var player = GetMyPlayer();
            
            // Remember where I was            
            var position = player.transform.position;
            fishingPreviousLocation = new Vector2(position.x, position.y);
            
            // Get my teleport location
            var myTeleportLocation = GetMyTeleportLocation();
            Debug.Log("Teleporting myself to: " + myTeleportLocation);
            player.transform.SetPositionAndRotation(
                new Vector3(myTeleportLocation.x, myTeleportLocation.y, transform.position.z),
                Quaternion.identity
            );
            
        }

        /// <summary>
        /// Teleports the player back to where they were
        /// </summary>
        private void TeleportMyPlayerBack()
        {
            var player = GetMyPlayer();
            Debug.Log("Teleporting myself back: " + fishingPreviousLocation);
            player.transform.SetPositionAndRotation(fishingPreviousLocation, Quaternion.identity);
            fishingPreviousLocation = Vector2.zero;
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