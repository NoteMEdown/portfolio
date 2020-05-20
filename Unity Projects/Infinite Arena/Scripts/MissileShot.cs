using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfiniteGames.InfiniteArena
{
    public class MissileShot : MonoBehaviour
    {
        [Tooltip("How long the shot will last before getting destroyed")]
        [SerializeField]
        private float despawnTime = 2.0f;
        [Tooltip("How fast the object moves per second")]
        [SerializeField]
        private float shotSpeed = 10.0f;
        [Tooltip("Rigidbody for Missile Shot")]
        [SerializeField]
        private Rigidbody rb;
        [Tooltip("How far back the explosion is from the center of the shot that got removed")]
        [SerializeField]
        private float explosionOffset = 1f;

        private float timeAlive;
        private const string playerTag = "Player";
        private int playerNum = -1;

        [Tooltip("The explosion made when the shot hits something")]
        public GameObject explosion;

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
            try
            {
                if (collider.gameObject.GetComponent<PlayerManager>().GetPlayerNum() == playerNum)
                    return;
            }
            catch (System.Exception) {}
            Instantiate(explosion, transform.position - (transform.forward * explosionOffset), transform.rotation, null);
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
            Destroy(gameObject);
        }

        #endregion

        #region public methods

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

