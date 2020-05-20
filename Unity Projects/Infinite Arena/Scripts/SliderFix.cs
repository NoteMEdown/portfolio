using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InfiniteGames.InfiniteArena
{
    // Simple script to make the fill completely get removed when at 0 value.
    public class SliderFix : MonoBehaviour
    {
        public GameObject sliderFill;
        public Slider slider;

        // Update is called once per frame
        void Update()
        {
            if (sliderFill.activeSelf && slider.value == 0)
                sliderFill.SetActive(false);
            else
                if (!sliderFill.activeSelf && slider.value != 0)
                    sliderFill.SetActive(true);
        }
    }
}

