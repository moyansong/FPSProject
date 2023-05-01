using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS.Gameplay
{

    public class LighterController : MonoBehaviour
    {
        public GameObject lighter;

        void OnEnable()
        {
            lighter.SetActive(true);
        }

        void OnDisable()
        {
            lighter.SetActive(false);
        }

        void Update()
        {

        }
    }
}