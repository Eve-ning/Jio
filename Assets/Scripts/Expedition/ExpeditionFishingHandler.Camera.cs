using System;
using System.Collections;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DIPProject
{
    public partial class ExpeditionFishingHandler
    {
        #region Variables

        [Tooltip("This is the position the camera will pan to when fishing.")]
        public Camera fishingCamera;

        private Camera _localCamera;
        private Vector3 _savedPos;
        private float _savedSize = 5.0f;
        
        private int _cameraAnimationFrames = 50;
        private float _cameraAnimationDelay = 0.01f;
        #endregion
        
        /// <summary>
        /// Moves the camera to another position based on the fishingCameraPosition.
        /// This will use an asynchronous loop to move the camera.
        /// We have this as a coded animation as Unity doesn't like relative animation positions 
        /// </summary>
        /// <returns></returns>
        private IEnumerator MoveCameraTo(Vector3 pos, float size)
        {
            _localCamera = GetMyPlayer().GetComponentInChildren<Camera>();
            _savedPos = _localCamera.transform.position;
            var tgtPos = pos;
            var posDelta = tgtPos - _savedPos;

            _savedSize = _localCamera.orthographicSize;
            var tgtCameraSize = size;
            var sizeDelta = tgtCameraSize - _savedSize;
            
            float animationProgress = 0f;
            for (var i = 0; i < _cameraAnimationFrames; i++)
            {
                // ^ 0.5 Squared smoothing easing
                animationProgress = (float)Math.Sqrt((float) i / _cameraAnimationFrames) ;
                _localCamera.transform.position = _savedPos + posDelta * animationProgress;
                _localCamera.orthographicSize = _savedSize + sizeDelta * animationProgress;
                yield return new WaitForSeconds(_cameraAnimationDelay);
            }
        }

        public IEnumerator MoveCameraToFishing()
        {
            yield return MoveCameraTo(fishingCamera.transform.position,
                fishingCamera.orthographicSize);
        }
        public IEnumerator MoveCameraFromFishing()
        {
            yield return MoveCameraTo(_savedPos, _savedSize);
        }

    }
}