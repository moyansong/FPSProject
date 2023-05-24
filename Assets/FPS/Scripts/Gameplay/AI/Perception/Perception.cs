using FPS.Game;
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

        public TargetInfo(GameObject target, Perception perception)
        {
            this.target = target;
            this.perception = perception;
        }

        public override bool Equals(object obj)
        {
            return obj is TargetInfo info &&
                   EqualityComparer<GameObject>.Default.Equals(target, info.target) &&
                   EqualityComparer<Perception>.Default.Equals(perception, info.perception);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(target, perception);
        }

        public static bool operator ==(TargetInfo a, TargetInfo b)
        {
            return a.target == b.target && a.perception == b.perception;
        }

        public static bool operator !=(TargetInfo a, TargetInfo b)
        {
            return !(a == b);
        }

        public static bool operator >(TargetInfo a, TargetInfo b)
        {
            if (a.target == null || a.perception == null) return false;
            if (b.target == null || b.perception == null)
            {
                throw new ArgumentException("Both targetInfo are not vaild");
            }
            return a.perception.priority > b.perception.priority;
        }

        public static bool operator <(TargetInfo a, TargetInfo b)
        {
            return b > a;
        }
    }

    public enum PerceptionType
    {
        Sight,
        Hearing,
        Collision,
        Damage,
        Smell
    }

    // 探测目标，获得/丢失目标时会通知PerceptionManager
    public class Perception : MonoBehaviour
    {
        [Header("Ignore")]
        [Tooltip("Ignore these game objects")]
        public GameObject[] ignoreGameObjects;

        [Header("Target")]
        [Tooltip("Characters that do not belong to this type will not perceive")]
        public CharacterType[] targetingCharacterTypes;

        [Header("Perception")]
        [Tooltip("Objects beyond this distance will not be perceived")]
        public float maxDistance;

        [Tooltip("Priority of perception components on the same game object")]
        public int priority = 0;

        [Header("References")]
        public PerceptionManager perceptionManager;

        public PerceptionType perceptionType { get; protected set; }

        private HashSet<GameObject> m_IgnoreObjectHash = new();

        private HashSet<CharacterType> m_TargetingTypeHash = new();

        protected HashSet<GameObject> m_TargetHash = new();

        protected virtual void Awake()
        {
            foreach (var obj in ignoreGameObjects)
            {
                m_IgnoreObjectHash.Add(obj);
            }
            foreach (var type in targetingCharacterTypes)
            {
                m_TargetingTypeHash.Add(type);
            }
        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {

        }
        
        protected void AddIgnore(GameObject ignoreObject)
        {
            m_IgnoreObjectHash.Add(ignoreObject);
        }

        protected void AddTargeting(CharacterType type)
        {
            m_TargetingTypeHash.Add(type);
        }

        protected virtual bool IsVaildTarget(GameObject target)
        {
            CharacterType targetType = target.GetComponentInChildren<Identity>().characterType;
            return !m_TargetHash.Contains(target) &&
                   !m_IgnoreObjectHash.Contains(target) && 
                   m_TargetingTypeHash.Contains(targetType);
        }

        public virtual GameObject GetHighestPriorityTarget(GameObject target1, GameObject target2)
        {
            return null;
        }
    }
}