using UnityEngine;
using UnityEngine.UI;

namespace DIPProject
{
    /// <summary>
    ///     The handler for the top-right mail icon
    ///     Currently, we're doing a single sprite swap, ideally, we have a separate alert icon so that we can handle
    ///     the 2 states, (Open, Close) & (Read, Unread) separately.
    ///     For this, we'll rewrite this a little.
    /// </summary>
    public class MailIconHandler : MonoBehaviour
    {
        #region MonoBehavior Callbacks

        // Start is called before the first frame update
        private void Start()
        {
            UpdateSprite();
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///     Here, we update the sprite on every important call
        ///     We can determine the state via the current status.
        /// </summary>
        private void UpdateSprite()
        {
            switch (status)
            {
                case Status.CLOSED:
                    targetImage.sprite = closed;
                    break;
                case Status.CLOSED_ALERT:
                    targetImage.sprite = closedAlert;
                    break;
                case Status.OPENED:
                    targetImage.sprite = opened;
                    break;
            }
        }

        #endregion

        #region Variables

        public enum Status : ushort
        {
            CLOSED,
            CLOSED_ALERT,
            OPENED
        }

        [Tooltip("The Sprite for a closed mail icon")]
        public Sprite closed;

        [Tooltip("The Sprite for a closed mail icon with an Alert")]
        public Sprite closedAlert;

        [Tooltip("The Sprite for a opened mail icon")]
        public Sprite opened;

        [Tooltip("The target image to swap sprites with.")]
        public Image targetImage;

        [Tooltip("Whether the user has read it")]
        private bool read;

        public Status status = Status.CLOSED_ALERT;

        #endregion

        #region Mouse Callbacks

        public void OnMouseDown()
        {
            read = true;
            status = Status.OPENED;
            UpdateSprite();
        }

        public void OnMouseExit()
        {
            status = read ? Status.CLOSED : Status.CLOSED_ALERT;
            UpdateSprite();
        }

        public void OnMouseOver()
        {
            status = Status.OPENED;
            UpdateSprite();
        }

        #endregion
    }
}