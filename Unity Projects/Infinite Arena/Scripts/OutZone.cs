using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfiniteGames.InfiniteArena
{
    public class OutZone : MonoBehaviour
    {
        private const string playerTag = "Player";

        private void OnTriggerEnter(Collider collider)
        {
            Debug.Log("Player fell out");
            if(collider.gameObject.tag == playerTag)
            {
                collider.gameObject.GetComponent<PlayerManager>().Die();
            }
        }
    }
}

