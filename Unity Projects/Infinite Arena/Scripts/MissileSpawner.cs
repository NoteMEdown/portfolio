using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfiniteGames.InfiniteArena
{
    public class MissileSpawner : MonoBehaviour
    {
        [Tooltip("Object that represents the Missile box in game")]
        public GameObject Missiles;

        [Tooltip("How long after the missiles are picked up will a new set of missiles spawn")]
        [SerializeField]
        private float missileRespawnTime = 10f;

        [Tooltip("If the missiles spawn automatically after a set amount of time")]
        [SerializeField]
        private bool timedRespawn = true;

        private float deadTime;

        // Update is called once per frame
        void Update()
        {
            if(!Missiles.activeSelf)
            {
                deadTime += Time.deltaTime;
                if(timedRespawn && deadTime >= missileRespawnTime)
                {
                    Missiles.SetActive(true);
                }
            }
        }

        public void MissileTaken()
        {
            Missiles.SetActive(false);
            deadTime = 0f;
        }
    }
}

