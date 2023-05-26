using FPS.Game;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace FPS.Gameplay
{
    [RequireComponent(typeof(Collider))]
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

        [Header("MeleeWeapon Parameters")]
        [Tooltip("Will not collide with these colliders")]
        public HashSet<Collider> ignoreColliders = new HashSet<Collider>();

        public float damage { get; set; }

        private Collider m_Collider;

        private float m_LastStrikeTime = -10f;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            m_Collider = GetComponent<Collider>();
            m_Collider.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (m_Collider.enabled)
            {
                //Debug.Log("Enabled");
            }
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
            m_Collider.enabled = true;
            weaponState = WeaponState.Firing;
            this.StartCoroutine(lightStrikeInterval, StrikeFinished);

            return true;
        }

        private bool HeavyStrike()
        {
            damage = heavyStrikeDamage;
            m_LastStrikeTime = Time.time;
            m_Collider.enabled = true;
            weaponState = WeaponState.Firing;
            this.StartCoroutine(heavyStrikeInterval, StrikeFinished);

            return true;
        }

        private void StrikeFinished()
        {
            m_Collider.enabled = false;
            ignoreColliders.Clear();
            weaponState = WeaponState.Idle;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (ignoreColliders.Contains(collider)) return;

            ignoreColliders.Add(collider);
            ApplyDamage(collider, damage);
        }

        private void ApplyDamage(Collider collider, float damageAmount)
        {
            Health health = collider.GetComponent<Health>();
            if (health != null)
            {
                DamageEvent damageEvent = new DamageEvent(damageAmount, owner, instigator);
                health.TakeDamage(damageEvent);
            }
        }
    }
}
