using FPS.Game;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace FPS.Gameplay
{
    public class MeleeWeaponController : WeaponController
    {
        [Header("Strike")]
        [Tooltip("Damage of light strike")]
        public float lightStrikeDamage = 50f;

        [Tooltip("The interval between two light strike, unit(s)")]
        public float lightStrikeInterval = 1f;

        [Tooltip("Damage of heavy strike")]
        public float heavyStrikeDamage = 100f;

        [Tooltip("The interval between two heavy strike, unit(s)")]
        public float heavyStrikeInterval = 1f;

        public float damage { get; set; }

        private ContinuousRaycastHit m_ContinuousRaycastHit;

        private HashSet<Collider> m_IgnoreColliders = new HashSet<Collider>();

        private float m_LastStrikeTime = -10f;

        protected override void Awake()
        {
            m_ContinuousRaycastHit = GetComponent<ContinuousRaycastHit>();
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private bool CanHit(Collider collider)
        {
            return collider.gameObject != owner &&
                   collider.gameObject != instigator && 
                   !m_IgnoreColliders.Contains(collider);
        }

        private void FixedUpdate()
        {
            if (weaponState == WeaponState.Firing)
            {
                RaycastHit[] raycastHits = new RaycastHit[5];
                int n = m_ContinuousRaycastHit.Raycast(raycastHits);
                for (int i = 0; i < Mathf.Min(n, raycastHits.Length); ++i)
                {
                    Collider hitCollider = raycastHits[i].collider;
                    if (CanHit(hitCollider))
                    {
                        Debug.Log($"{owner}'s {gameObject} striked {hitCollider.gameObject}, collider is {hitCollider}");
                        m_IgnoreColliders.Add(hitCollider);
                        ApplyDamage(hitCollider, damage);
                    }
                }
            }
        }

        public override float GetFire1Interval()
        {
            return lightStrikeInterval;
        }

        public override bool HandleFire1Input(bool inputDown, bool inputHeld, bool inputUp)
        {
            if (inputDown)
            {
                return LightStrike();
            }
            return false;
        }

        private bool CanLightStrike()
        {
            return isIdle;
        }

        private bool LightStrike()
        {
            if (!CanLightStrike()) return false;
            
            damage = lightStrikeDamage; 
            m_LastStrikeTime = Time.time;
            weaponState = WeaponState.Firing;
            this.StartCoroutine(lightStrikeInterval, StrikeFinished);

            return true;
        }

        private bool HeavyStrike()
        {
            damage = heavyStrikeDamage;
            m_LastStrikeTime = Time.time;
            weaponState = WeaponState.Firing;
            this.StartCoroutine(heavyStrikeInterval, StrikeFinished);

            return true;
        }

        private void StrikeFinished()
        {
            m_IgnoreColliders.Clear();
            weaponState = WeaponState.Idle;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (m_IgnoreColliders.Contains(collider)) return;

            m_IgnoreColliders.Add(collider);
            ApplyDamage(collider, damage);
        }

        private void ApplyDamage(Collider collider, float damageAmount)
        {
            Health health = collider.GetComponentInChildren<Health>();
            if (health != null)
            {
                DamageEvent damageEvent = new DamageEvent(damageAmount, owner, instigator);
                health.TakeDamage(damageEvent);
            }
        }
    }
}
