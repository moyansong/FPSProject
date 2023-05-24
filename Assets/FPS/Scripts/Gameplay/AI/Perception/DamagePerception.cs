using FPS.Game;
using FPS.Gameplay.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FPS.Gameplay.AI
{
    public class DamagePerception : Perception
    {
        [Tooltip("Only with health the object can perceive damage")]
        public Health health;

        [Header("Parameters")]
        [Tooltip("Targeting the dead character?")]
        public bool targetingDead = true;

        protected override void Awake()
        {
            base.Awake();
            perceptionType = PerceptionType.Damage;
            health.OnDamaged += OnDamaged;
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override bool IsVaildTarget(GameObject target)
        {
            return base.IsVaildTarget(target) && 
                   (!target.GetComponentInChildren<Health>().isDead || targetingDead);
        }

        private void OnDamaged(DamageEvent evt)
        {
            if (!IsVaildTarget(evt.instigator)) return;
 
            perceptionManager.PerceivedTarget(new(evt.instigator, this));
        }
    }
}