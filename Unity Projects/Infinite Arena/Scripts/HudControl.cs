using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InfiniteGames.InfiniteArena
{
    public class HudControl : MonoBehaviour
    {
        public GameObject[] shots;

        public Slider jumpSlider1;
        public Slider jumpSlider2;

        public Text missileText;

        public void UpdateShots(bool increase)
        {
            if (increase)
            {
                foreach (GameObject shot in shots)
                {
                    if (!shot.activeSelf)
                    {
                        shot.SetActive(true);
                        return;
                    }
                }
            }
            else
            {
                for (int i = shots.Length - 1; i >= 0; i--)
                {
                    if (shots[i].activeSelf)
                    {
                        shots[i].SetActive(false);
                        return;
                    }
                }
            }
        }

        public void UpdateJumpSliders(int jumpCount, int maxJumpCount, float jumpTime, float jumpChargeTime)
        {
            float ratio = jumpTime / jumpChargeTime; // returns 0 to 1 depending on how charged the next jump is
            if (jumpCount == maxJumpCount)
            {
                jumpSlider1.value = 1;
                jumpSlider2.value = 1;
                return;
            }
            if (jumpCount > 0)
            {
                jumpSlider1.value = 1;
                jumpSlider2.value = ratio;
                return;
            }
            jumpSlider1.value = ratio;
            jumpSlider2.value = 0;
        }

        public void UpdateMissileCount(int missileCount)
        {
            missileText.text = missileCount.ToString();
        }
    }

}
