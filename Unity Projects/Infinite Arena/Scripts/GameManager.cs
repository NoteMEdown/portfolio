using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

namespace InfiniteGames.InfiniteArena
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region Public Fields

        public static GameManager Instance;
        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;
        [Tooltip("Stores all spawnPoint locations")]
        public Transform[] spawnPoints;
        [Tooltip("Stores all player health UIs")]
        public GameObject[] playerUIs;
        [Tooltip("The button to exit the game")]
        public GameObject leaveButton;
        [Tooltip("The UI elements that will be shown to every player")]
        public SharedUI sharedUI;

        #endregion

        #region Private Fields

        [Tooltip("Time it takes a user to respawn")]
        [SerializeField]
        private float MaxRespawnTime = 5.0f;
        [Tooltip("Initial time wait until match starts")]
        [SerializeField]
        private float MaxStartTime = 10.0f;

        private float respawnTime;
        private float startTime;
        private bool matchGoing;

        private PlayerManager[] players;

        #endregion

        #region MonoBehaviour CallBacks

        void Start()
        {
            Instance = this;
            GetComponent<PhotonView>().ViewID = 999;
            //Debug.Log("Set up health: " + Instance.setUpHealth);
            //Instance.setUpHealth = false;
            //Debug.Log("Current players in list: " + players.Count);
            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                if (PlayerManager.LocalPlayerInstance == null)
                {
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                    int playerNum = Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer);
                    Vector3 spawnPoint = spawnPoints[playerNum + 1].position;
                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    PhotonNetwork.Instantiate(this.playerPrefab.name, spawnPoint, Quaternion.identity, 0);
                    PlayerManager currentPlayer = PlayerManager.LocalPlayerInstance.GetComponent<PlayerManager>();
                    currentPlayer.SetPlayerNum(playerNum + 1); // changes from 0-3 to 1-4 for player numbers
                    currentPlayer.SetPlayerName(PhotonNetwork.LocalPlayer.NickName);
                    currentPlayer.SetRespawnPoint(spawnPoint);
                   // PlayerManager.LocalPlayerInstance.SetActive(false);
                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }
        }

        private void Update()
        {
            if (!matchGoing)
            {
                UpdateStartTime();
                return;
            }
            if (!PlayerManager.LocalPlayerInstance.activeInHierarchy)
            {
                UpdateRespawnTime();
            }
        }

        #endregion

        #region Photon Callbacks


        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }


        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


                LoadArena();
            }
        }


        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


              //  LoadArena();
            }
        }


        #endregion


        #region Private Methods


        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
        }

        private void UpdateStartTime()
        {
            startTime += Time.deltaTime;
            sharedUI.StartTimeText.gameObject.SetActive(true);
            sharedUI.StartTimeText.text = "Match Starts in\n" + (MaxStartTime - startTime).ToString("0.##");
            if (startTime >= MaxStartTime)
            {
                startTime = 0f;
                sharedUI.StartTimeText.gameObject.SetActive(false);
                matchGoing = true;
                StartMatch();
            }
        }

        private void UpdateRespawnTime()
        {
            respawnTime += Time.deltaTime;
            sharedUI.RespawnTimeText.gameObject.SetActive(true);
            sharedUI.RespawnTimeText.text = "Respawn in...\n" + (MaxRespawnTime - respawnTime).ToString("0.##");
            if (respawnTime >= MaxRespawnTime)
            {
                respawnTime = 0f;
                sharedUI.RespawnTimeText.gameObject.SetActive(false);
                PlayerManager.LocalPlayerInstance.transform.position = PlayerManager.LocalPlayerInstance.GetComponent<PlayerManager>().GetRespawnPoint();
                PhotonView photonView = PhotonView.Get(this);
                photonView.RPC("Respawn", RpcTarget.Others, PlayerManager.LocalPlayerInstance.GetComponent<PlayerManager>().GetPlayerNum());
                PlayerManager.LocalPlayerInstance.SetActive(true);
                PlayerManager.LocalPlayerInstance.GetComponent<PlayerManager>().Respawn();
            }
        }

        private void StartMatch()
        {
            PlayerManager[] tempPlayers = GameObject.FindObjectsOfType<PlayerManager>();
            players = new PlayerManager[tempPlayers.Length];
            Debug.Log("Number of Players: " + tempPlayers.Length);
            foreach (PlayerManager player in tempPlayers)
            {
                Debug.Log("Player Number: " + player.GetPlayerNum());
                players[player.GetPlayerNum() - 1] = player;
            }
         //   respawnTime = MaxRespawnTime; // Skips the first respawn time
            PlayerManager.LocalPlayerInstance.gameObject.GetComponent<PlayerInput>().SetGameRunning(true);
        }

        [PunRPC]
        void Respawn(int playerNum)
        {
            Debug.Log("Attempting to respawn player " + playerNum);
            players[playerNum - 1].gameObject.SetActive(true);
            players[playerNum - 1].transform.position = players[playerNum - 1].GetComponent<PlayerManager>().GetRespawnPoint();
        }


        #endregion


        #region Public Methods


        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void ExitGame()
        {
            Application.Quit();
        }


        #endregion
    }
}