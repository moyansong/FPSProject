using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS.Gameplay
{
    public enum WeaponShootType
    {
        Manual,
        Automatic,
    }

    public class RangedWeaponController : WeaponController
    {
        [Header("Weapon Parameters")]
        [Tooltip("Muzzle of the weapon, where the projectiles are shot")]
        public GameObject muzzle;

        [Tooltip("The type of weapon wil affect how it shoots")]
        public WeaponShootType shootType;

        [Tooltip("The interval between two shoot, unit(s)")]
        public float shootInterval = 0.5f;

        [Tooltip("The farthest distance a projectile can fly, unit(m)")]
        public float range = 1000f;

        [Header("Ammo Parameters")]
        [Tooltip("Current ammo")]
        public int ammo = 10;

        [Tooltip("The maximum amount of ammo")]
        public int maxAmmo = 10;

        [Tooltip("Number of ammo cost pre shoot")]
        public int ammoCostPerShoot = 1;

        [Tooltip("Projectile")]
        public GameObject projectile;

        [Tooltip("Shoot real projectile?")]
        public bool useRealProjectile = true;

        [Header("Effect")]
        [Tooltip("Sound effect during weapon firing")]
        public AudioClip shootSound;

        public ProjectileController projectileController
        {
            get
            {
                GameObject projectileInstance = GameObject.Instantiate(projectile);
                ProjectileController controller = projectileInstance.GetComponent<ProjectileController>();
                controller.owner = owner;
                controller.instigator = instigator;
                return controller;
            }
        }

        private GameObject m_MuzzleFlash;
        private float m_LastShootTime;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            m_MuzzleFlash = muzzle.transform.Find("MuzzleFlash").gameObject;
            m_LastShootTime = Time.time;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void Fire1()
        {
            base.Fire1(); 
            Shoot();
        }

        protected bool CanShoot()
        {
            return ammo >= ammoCostPerShoot && Time.time - m_LastShootTime >= shootInterval;
        }

        protected void Shoot()
        {
            if (!CanShoot()) return;

            m_LastShootTime = Time.time;
            ammo -= ammoCostPerShoot;

            ShootEffect();

            RaycastHit hit;
            if (CrosshairRaycast(out hit))
            {
                Debug.Log($"Hit {hit.collider.gameObject.ToString()}, point:{hit.point}, distance:{hit.distance}");
                if (useRealProjectile)
                {
                    projectileController.Shoot(muzzle.transform.position, hit.point);
                }
                else
                {
                    // Spawn effect and damage in hit.point
                }
            }
            else
            {
                Debug.Log("Did not hit");
            }
        }

        private void ShootEffect()
        {
            //m_AudioSource.PlayOneShot(shootSound);
            m_MuzzleFlash.SetActive(true);
            StartCoroutine(DeactiveMuzzleFlash());
        }

        private IEnumerator DeactiveMuzzleFlash()
        {
            yield return new WaitForSeconds(shootInterval / 2);
            m_MuzzleFlash.SetActive(false);
        }
        
        private bool CrosshairRaycast(out RaycastHit hit)
        {
            Vector3 direction = ownerCamera.transform.forward;
            Vector3 origin = ownerCamera.transform.position + direction * 1;
            Debug.DrawLine(origin, origin + direction * range, Color.red, 100f);
            return Physics.Raycast(
                origin,
                direction,
                out hit,
                range
            );
        }

        public override void Reload()
        {
            ammo = maxAmmo;
        }
    }
}