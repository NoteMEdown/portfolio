using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfiniteGames.InfiniteArena
{
    public class BasicShot : MonoBehaviour
    {
        [Tooltip("How long the shot will last before getting destroyed")]
        [SerializeField]
        private float despawnTime = 2.0f;
        [Tooltip("How fast the object moves per second")]
        [SerializeField]
        private float shotSpeed = 10.0f;
        [Tooltip("Rigidbody for Basic Shot")]
        [SerializeField]
        private Rigidbody rb;
        [Tooltip("Initial Force being added to the player that is hit")]
        [SerializeField]
        private float shotForce = 10f;
        [Tooltip("Damage being added to the player that is hit")]
        [SerializeField]
        private int shotDamage = 10;
        [Tooltip("How far back the explosion is from the center of the shot that got removed")]
        [SerializeField]
        private float explosionOffset = 1f;

        private float timeAlive;
        private PlayerManager player;
        private const string playerTag = "Player";
        private int playerNum = -1;

        //[Tooltip("The explosion made when the shot hits something")]
        //public GameObject explosion;

        #region MonoBehaviour Callbacks
        
        private void Start()
        {
            SetVelocity(transform.forward * shotSpeed);
        }

        void Update()
        {
            UpdateTime();
        }

        private void OnTriggerEnter(Collider collider)
        {
            Debug.Log("Shot object: " + collider.gameObject.name);
            Debug.Log("With Tag: " + collider.gameObject.tag);

            try
            {
                if (collider.gameObject.GetComponent<PlayerManager>().GetPlayerNum() == playerNum)
                    return;
                collider.gameObject.GetComponent<PlayerController>().AddForce(transform.forward * shotForce);
                collider.gameObject.GetComponent<PlayerManager>().Damage(shotDamage);
            }
            catch (System.Exception){ Debug.Log("No Controller/PlayerManager found on shot object"); }
            player.ShotObject(collider.gameObject);
            //    CheckPlayer(collider.gameObject);
            //     Instantiate(explosion, transform.position - (transform.forward * explosionOffset), transform.rotation, null);
            DestroyThis(); 
        }

        #endregion

        #region private methods

        private void UpdateTime()
        {
            timeAlive += Time.deltaTime;
            if (timeAlive >= despawnTime)
            {
                DestroyThis();
            }
        }

        private void DestroyThis()
        {
            player.ShotRemoved();
            Destroy(gameObject);
        }

        private bool CheckPlayer(GameObject other)
        {
            if (other.tag == playerTag) // If object hit is a player
            {
                if (other.GetComponent<PhotonView>().Owner == PhotonNetwork.LocalPlayer) // And if the person got hit is the local player ( Makes it only execute once )
                {
                    other.GetComponent<PlayerController>().AddForce(transform.forward * shotForce);
                    other.GetComponent<PlayerManager>().Damage(shotDamage);

                    return true;
                }
            }
            return false;
        }

        #endregion

        #region public methods

        public void SetPlayer(PlayerManager _player)
        {
            player = _player;
        }

        public void SetVelocity(Vector3 v)
        {
            rb.velocity = v;
        }

        public void SetPlayerNum(int _playerNum)
        {
            playerNum = _playerNum;
        }
        #endregion
    }
}

