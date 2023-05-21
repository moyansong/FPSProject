using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FPS.Gameplay.AI
{
    public struct TargetInfo
    {
        public GameObject target;

        public Perception perception;
    }

    public enum PerceptionType
    {
        Sight,
        Hearing,
        Collision,
        Damage,
        Smell
    }

    public class Perception : MonoBehaviour
    {
        [Header("Perception")]


        [Header("Ignore")]
        public List<GameObject> ignoreGameObjects = new();

        [Header("Target")]


        [Header("Parameters")]
        [Tooltip("Objects beyond this distance will not be perceived")]
        public float maxRange;

        [Tooltip("Priority of perception components on the same game object")]
        public int priority = 0;

        public UnityAction<TargetInfo> onDetectedTarget;
        public UnityAction<TargetInfo> onLostTarget;

        public PerceptionType perceptionType { get; protected set; }

        // Start is called before the first frame update
        protected virtual void Start()
        {

        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }
        
        protected virtual bool IsVaildTarget(GameObject target)
        {
            return false;
        }

        public virtual GameObject GetHighestPriorityTarget(GameObject target1, GameObject target2)
        {
            return null;
        }
    }
}