using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;

using Photon.Pun;
using UnityEngine.UI;

namespace InfiniteGames.InfiniteArena
{
    /// <summary>
    /// Player manager.
    /// </summary>
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region Private Fields
        [Tooltip("The Player's Camera")]
        [SerializeField]
        private Camera playerCamera;
        [Tooltip("The transform of the gun object")]
        [SerializeField]
        private Transform gunTransform;
        [Tooltip("The current Health of our player")]
        [SerializeField]
        private int hitPoints;
        private int shotCount;
        [Tooltip("How many shots the player can shoot at any given time")]
        [SerializeField]
        private const int maxShotCount = 3;
        private const string basicShotTag = "BasicShot";
        private string playerName;
        [Tooltip("DO NOT EDIT (Shown for testing)")]
        [SerializeField]
        private int playerNum;
        private Vector3 respawnPoint;
        private Text playerHpText;
        private Text playerNameText;
        private int missileCount;
        [Tooltip("How many charges of Jump the player can store")]
        [SerializeField]
        private const int maxJumpCount = 2;
        [Tooltip("How long it takes for a jump to charge")]
        [SerializeField]
        private float jumpChargeTime = 3.0f;
        private int jumpCount;
        private float jumpTime; // Current time between last jump gained
        #endregion

        #region Public Fields
        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;
        //[Tooltip("The Player's UI GameObject Prefab")]
        //[SerializeField]
        //public GameObject PlayerUiPrefab;
        public bool lockCursor = true;
        public HudControl hud;
        #endregion

        #region MonoBehaviour CallBacks

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake()
        {
            // #Important
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
            if (photonView.IsMine)
            {
                PlayerManager.LocalPlayerInstance = this.gameObject;
            }
            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);
        }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start()
        {
            if (photonView.IsMine)
            {
                shotCount = maxShotCount;
                jumpCount = maxJumpCount;
                //CreateUI();
                MatchUI();

                hud = GameObject.FindObjectOfType<HudControl>();
                if (!hud)
                    Debug.LogError("Can not find the HUD!");
                hud.UpdateMissileCount(missileCount);
            }
            else
            {
                playerCamera.enabled = false;
                playerCamera.gameObject.GetComponent<AudioListener>().enabled = false;
            }

            #if UNITY_5_4_OR_NEWER
            // Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, loadingMode) =>
            {
                this.CalledOnLevelWasLoaded(scene.buildIndex);
            };
            #endif
        }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity on every frame.
        /// </summary>
        void Update()
        {
            if (photonView.IsMine) // Only process inputs if correct player
            {
                //if (playerCamera.depth == 0)
                //{
                //    FixCameras();
                //}
                UpdateHealthUI();
                UpdateJumpTime();
                hud.UpdateJumpSliders(jumpCount, maxJumpCount, jumpTime, jumpChargeTime);
            }
        }
 

        #if !UNITY_5_4_OR_NEWER
        /// <summary>See CalledOnLevelWasLoaded. Outdated in Unity 5.4.</summary>
        void OnLevelWasLoaded(int level)
        {
            this.CalledOnLevelWasLoaded(level);
        }
        #endif


        void CalledOnLevelWasLoaded(int level)
        {
            if (!photonView.IsMine)
            {
                return;
            }
            // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }
            //CreateUI();
            MatchUI();
        }

#endregion

#region IPunObservable implementation


        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(hitPoints);
                stream.SendNext(playerName);
                stream.SendNext(playerNum);
                stream.SendNext(respawnPoint);
            }
            else
            {
                // Network player, receive data
                this.hitPoints = (int)stream.ReceiveNext();
                this.playerName = (string)stream.ReceiveNext();
                this.playerNum = (int)stream.ReceiveNext();
                this.respawnPoint = (Vector3)stream.ReceiveNext();
            }
        }


#endregion

#region Custom

        /// <summary>
        /// Processes the inputs. Maintain a flag representing when the user is pressing Fire.
        /// </summary>

        //private void CreateUI()
        //{
        //    if (PlayerUiPrefab != null)
        //    {
        //        GameObject _uiGo = Instantiate(this.PlayerUiPrefab);
        //        _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        //    }
        //    else
        //    {
        //        Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
        //    }
        //}

        //private void FixCameras()
        //{
        //    Camera[] allCams = GameObject.FindObjectsOfType<Camera>();
        //    for (int i = 0; i < allCams.Length; i++)
        //    {
        //        allCams[i].depth = 0;
        //    }
        //    playerCamera.depth = 1;
        //}

        public void ShotRemoved()
        {
            if (!photonView.IsMine)
                return;
            if (shotCount < maxShotCount)
            {
                shotCount++;
                if(hud)
                    hud.UpdateShots(true);
            }
                
        }

        public void ShotObject(GameObject other)
        {
            if (!photonView.IsMine)
                return;
            Debug.Log(gameObject.tag + " shot " + other.tag);
            // Do certain things depending on what gameobject the player hits
        }
        
        public bool CanShoot()
        {
            return shotCount > 0;
        }

        public void ShotFired()
        {
            if(shotCount > 0)
            {
                shotCount--;
                hud.UpdateShots(false);
            }
        }

        private void MatchUI()
        {
            GameObject playerHealthUI = GameObject.Find("Player"+playerNum+"UI");
    //        Debug.Log("This players number is: " + playerNum);
    //        Debug.Log("playerHealthUI was found: " + (playerHealthUI != null));
            if (playerHealthUI)
            {
                playerHealthUI.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
                //           playerHealthUI.GetComponent<PlayerInfo>().SetPlayer(this);
                playerHpText = playerHealthUI.GetComponent<PlayerInfo>().playerHp;
                playerNameText = playerHealthUI.GetComponent<PlayerInfo>().playerName ;
            }
        }

        private void UpdateHealthUI()
        {
            if (playerHpText)
            {
                playerHpText.text = hitPoints + "%";
            }
            if (playerNameText)
            {
                playerNameText.text = playerName;
            }
        }

        private void UpdateJumpTime()
        {
            if (jumpCount >= maxJumpCount)
                jumpTime = 0f;
            else
            {
                jumpTime += Time.deltaTime;
                if (jumpTime >= jumpChargeTime)
                {
                    jumpTime = 0f;
                    jumpCount++;
                }
            }
                
        }

        public int GetHitPoints()
        {
            return hitPoints;
        }

        public string GetPlayerName()
        {
            return playerName;
        }
        
        public Vector3 GetRespawnPoint()
        {
            return respawnPoint;
        }

        public void SetRespawnPoint(Vector3 _respawnPoint)
        {
            respawnPoint = _respawnPoint;
        }

        public void SetPlayerName(string _playerName)
        {
            playerName = _playerName;
        }

        public void SetPlayerNum(int _playerNum)
        {
            playerNum = _playerNum;
        }

        public int GetPlayerNum()
        {
            return playerNum;
        }

        public void Respawn()
        {
            hitPoints = 0;
        }

        public void Die()
        {
            GetComponent<PlayerController>().StopMoving();
            gameObject.SetActive(false);
        }

        public void CollectMissiles(int missilePickup)
        {
            if (!photonView.IsMine)
                return;
            missileCount += missilePickup;
            hud.UpdateMissileCount(missileCount);
        }

        public bool TryJump()
        {
            if(jumpCount >= 1)
            {
                jumpCount--;
                return true;
            }
            if(jumpCount < 0)
            {
                jumpCount = 0; // Fixes any negative jump count problems
                Debug.LogError("We have a negative jump count!");
            }
            return false;
        }

        public bool TryMissileShot()
        {
            if (missileCount >= 1)
            {
                missileCount--;
                hud.UpdateMissileCount(missileCount);
                return true;
            }
            return false;
        }

        public void Damage(float damage)
        {
            Debug.Log("Damage Done: " + damage);
            if (photonView.IsMine == false)
                return;
            hitPoints += (int)damage;
        }


#endregion
    }
}