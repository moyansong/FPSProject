using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FPS.Gameplay.AI
{
    [RequireComponent(typeof(ZombieController))]
    public class ZombieMovement : MonoBehaviour
    {

        public ZombieType zombieType
        {
            get
            {
                return m_ZombieController.zombieType;
            }
        }

        public ZombieState zombieState
        {
            get
            {
                return m_ZombieController.zombieState;
            }
        }

        private Animator m_Animator;
        private NavMeshAgent m_NavMeshAgent;
        private ZombieController m_ZombieController;

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
            m_ZombieController = GetComponent<ZombieController>();
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