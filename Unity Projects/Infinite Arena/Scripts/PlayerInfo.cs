using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InfiniteGames.InfiniteArena
{
    public class PlayerInfo : MonoBehaviour, IPunObservable
    {
        [Tooltip("Displays the health of a player")]
        [SerializeField]
        public Text playerHp;

        [Tooltip("Displays the name of a player")]
        [SerializeField]
        public Text playerName;

        [Tooltip("This number should match it's player's playerNum")]
        [SerializeField]
        private int numUI;

        //private PlayerManager player;

        private void Start()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount < numUI)
            {
                gameObject.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {
            //if (playerHp && player)
            //{
            //    playerHp.text = player.GetHitPoints() + "%";
            //}
            //if(playerName && player)
            //{
            //    playerName.text = player.GetPlayerName();
            //}
        }

        //public void SetPlayer(PlayerManager _player)
        //{
        //    player = _player;
        //}

        //#region IPunObservable implementation


        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(playerHp.text.ToString());
                stream.SendNext(playerName.text.ToString());
            }
            else
            {
                this.playerHp.text = (string)stream.ReceiveNext();
                this.playerName.text = (string)stream.ReceiveNext();
            }
        }


        //#endregion
    }
}

