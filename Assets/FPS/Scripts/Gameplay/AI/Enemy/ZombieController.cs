using FPS.Game;
using System;
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
        Following,
        Attacking
    }

    [RequireComponent(typeof(Health), typeof(NavMeshAgent), typeof(Animator))]
    public class ZombieController : MonoBehaviour
    {
        [Header("Paramerters")]
        [Tooltip("Type of zombie")]
        public ZombieType zombieType;

        [Tooltip("The starting state of the zombie")]
        public ZombieState startingZombieState;

        [Header("Attack")]
        [Tooltip("Can attack if the distance from the target is less than this value")]
        public float attackRange = 1f;

        [Tooltip("Time required to complete an attack")]
        public float attackDuration = 1.5f;

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
        private WeaponController m_WeaponController;
        private PerceptionManager m_PerceptionManager;

        private void Awake()
        {
            m_Health = GetComponent<Health>();
            m_Animator = GetComponent<Animator>();
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
            m_ZombieMovement = GetComponent<ZombieMovement>();

            m_WeaponController = GetComponentInChildren<WeaponController>();
            m_WeaponController.owner = gameObject;
            m_WeaponController.instigator = gameObject;

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
                //zombieState = ZombieState.Attacking;
                Debug.Log($"{zombieState}");
            }
            Attack();
        }

        private void OnZombieStateChanged(ZombieState oldState, ZombieState newState)
        {
            m_Animator.SetTrigger(newState.ToString());       
        }

        private void OnPerceivedTarget(TargetInfo info)
        {
            Debug.Log($"{gameObject}'s {info.perception} perceived target : {info.target}");
            targetInfo = info;
            zombieState = ZombieState.Following;
        }

        private void MoveTo(Vector3 destination)
        {
            m_ZombieMovement.MoveTo(destination);
        }

        private bool CanAttack()
        {
            return targetInfo.IsVaild() && 
                   zombieState != ZombieState.Attacking &&
                   Vector3.Distance(targetInfo.target.transform.position, transform.position) <= attackRange;
        }

        private void Attack()
        {
            if (!CanAttack()) return;
            
            zombieState = ZombieState.Attacking;
            m_WeaponController.HandleFire1Input(true, false, false);
            this.StartCoroutine(m_WeaponController.GetFire1Interval(), AttackFinish);
        }

        private void AttackFinish()
        {
            if (CanAttack()) return;

            if (targetInfo.IsVaild())
            {
                zombieState = ZombieState.Following;
            }
            else
            {
                zombieState = ZombieState.Idle;
            }
        }
    }
}