using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InfiniteGames.InfiniteArena
{
    public class MapVote : MonoBehaviour, IPunObservable
    {
        #region Serialized GameObjects

        [Tooltip("The text that displayed the number of votes for this particular map")]
        [SerializeField]
        private Text voteText;
        [Tooltip("Name of the Map")]
        [SerializeField]
        private string mapName;

        #endregion

        #region Private Field

        private int numOfVotes;
        private bool hasVoted;

        #endregion

        #region Monobehavior Callbacks

        // Update is called once per frame
        void Update()
        {
            voteText.text = "Votes - " + numOfVotes;
        }

        #endregion

        #region IPunObservable implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(numOfVotes);
            }
            else
            {
                // Network player, receive data
                this.numOfVotes = (int)stream.ReceiveNext();
            }
        }

        #endregion

        #region Custom Field

        public void Vote()
        {
            if (hasVoted)
                return;
            hasVoted = true;
            numOfVotes++;
            gameObject.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
        }

        public int GetNumVotes()
        {
            return numOfVotes;
        }

        public string GetMapName()
        {
            return mapName;
        }

        #endregion
    }

}
