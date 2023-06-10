using FPS.Gameplay.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimator : MonoBehaviour
{
    public ZombieState zombieState => m_ZombieController.zombieState;

    private Animator m_Animator;
    private ZombieController m_ZombieController;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_ZombieController = GetComponent<ZombieController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_Animator.SetBool("IsIdle", zombieState == ZombieState.Idle);
        m_Animator.SetBool("IsFollowing", zombieState == ZombieState.Following);
    }
}
