using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS.Gameplay.AI
{
    public class PerceptionManager : MonoBehaviour
    {
        private Perception[] m_Perceptions;


        private void Awake()
        {
            m_Perceptions = GetComponentsInChildren<Perception>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}