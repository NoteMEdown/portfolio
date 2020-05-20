using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

namespace InfiniteGames.InfiniteArena
{
    public class StartGame : MonoBehaviour
    {
        [Tooltip("Stores all maps that can be voted on")]
        [SerializeField]
        private MapVote[] mapVotes;

        public void Start_Game()
        {
            MapVote mostVotes = null;
            foreach (MapVote map in mapVotes)
            {
                if (mostVotes == null || mostVotes.GetNumVotes() < map.GetNumVotes())
                    mostVotes = map;
            }
            string mapToLoad = mostVotes.GetMapName();

            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel(mapToLoad);
        }
    }
}

