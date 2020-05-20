using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfiniteGames.InfiniteArena
{
    public class MissileBox : MonoBehaviour
    {
        [Tooltip("The missile Spawner that is responisble for this missile object")]
        public MissileSpawner missileSpawner;

        [Tooltip("The amount of missiles the player will collect for picking up this object")]
        [SerializeField]
        private int missileCount = 3;

        private const string playerTag = "Player";

        // Update is called once per frame
        void Update()
        {
            // Used for motion later
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.tag == playerTag)
            {
                collider.gameObject.GetComponent<PlayerManager>().CollectMissiles(missileCount);
                missileSpawner.MissileTaken();
            }
        }
    }
}

