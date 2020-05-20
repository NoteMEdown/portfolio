using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InfiniteGames.InfiniteArena
{
    public class SharedUI : MonoBehaviour
    {
        [Tooltip("Text Object for displaying time until game starts")]
        public Text StartTimeText;
        [Tooltip("Text Object for displaying time until player respawns")]
        public Text RespawnTimeText;
    }
}

