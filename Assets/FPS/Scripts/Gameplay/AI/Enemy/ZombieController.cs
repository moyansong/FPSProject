using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace FPS.Gameplay.AI
{
    public enum ZombieType
    {
        Shuffle,
        Dizzy,
        Alert
    }

    public enum ZombieState
    {
        Idle,
        Walking,
        Eating,
        Following
    }

    [RequireComponent(typeof(Health), typeof(NavMeshAgent), typeof(Animator))]
    public class ZombieController : MonoBehaviour
    {
        [Header("Paramerters")]
        [Tooltip("Type of zombie")]
        public ZombieType zombieType;

        [Tooltip("The starting state of the zombie")]
        public ZombieState startingZombieState;

        public GameObject testTarget;

        public UnityAction<ZombieState, ZombieState> onZombieStateChanged;

        public ZombieState zombieState
        { 
            get
            {
                return m_ZombieState;
            }
            set
            {
                m_LastZombieState = m_ZombieState;
                m_ZombieState = value;
                onZombieStateChanged?.Invoke(m_LastZombieState, m_ZombieState);
            }
        }

        public TargetInfo targetInfo { get; set; }

        public GameObject target => targetInfo.target;

        public ZombieState lastZombieState => m_ZombieState;

        private ZombieState m_ZombieState;
        private ZombieState m_LastZombieState = ZombieState.Idle;

        private AnimatorStateInfo m_AnimatorStateInfo => m_Animator.GetCurrentAnimatorStateInfo((int)zombieType + 1);

        private Health m_Health;
        private Animator m_Animator;
        private NavMeshAgent m_NavMeshAgent;
        private ZombieMovement m_ZombieMovement;
        private PerceptionManager m_PerceptionManager;

        private void Awake()
        {
            m_Health = GetComponent<Health>();
            m_Animator = GetComponent<Animator>();
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
            m_ZombieMovement = GetComponent<ZombieMovement>();
            m_PerceptionManager = GetComponentInChildren<PerceptionManager>();
            m_PerceptionManager.onPerceivedTarget += OnPerceivedTarget;

            onZombieStateChanged += OnZombieStateChanged;
        }

        // Start is called before the first frame update
        void Start()
        {
            m_Animator.SetLayerWeight(((int)zombieType + 1), 1);
            zombieState = startingZombieState;
            
            //InvokeRepeating("SetRandomZombieState", 10f, 10f);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                zombieState = ZombieState.Following;
            }
        }

        private void OnZombieStateChanged(ZombieState oldState, ZombieState newState)
        {
            m_Animator.SetTrigger(newState.ToString());       
        }

        private void OnPerceivedTarget(TargetInfo info)
        {
            Debug.Log($"{gameObject}'s {info.perception} perceived target : {info.target}");
            targetInfo = info;
        }
    }
}