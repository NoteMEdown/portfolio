using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InfiniteGames.InfiniteArena
{
    public class PlayerPanel : MonoBehaviour
    {
        public Text playerNames;

        // Start is called before the first frame update
        void Start()
        {
            PhotonView photonView = PhotonView.Get(this);
            foreach(Photon.Realtime.Player player in PhotonNetwork.PlayerList)
            {
                playerNames.text += player.NickName + "\n";
            }
            photonView.RPC("AddName", RpcTarget.Others, PhotonNetwork.NickName);
        }

        [PunRPC]
        void AddName(string playerName)
        {
            playerNames.text += playerName + "\n";
        }

    }
}

