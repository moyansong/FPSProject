using FPS.Game;
using System;
using UnityEngine;

namespace FPS.Gameplay
{
    public enum WeaponShootType
    {
        Manual,
        Automatic,
    }

    [System.Serializable]
    public struct Ammo
    {
        [Tooltip("Current ammo")]
        public int ammo;

        [Tooltip("The maximum amount of ammo")]
        public int maxAmmo;

        [Tooltip("Number of ammo cost pre shoot")]
        public int ammoCostPerShoot;

        public Ammo(int a)
        {
            ammo = a;
            maxAmmo = a;
            ammoCostPerShoot = 1;
        }

        public static Ammo operator+(Ammo a, int b)
        {
            a.ammo = Math.Clamp(a.ammo + b, 0, a.maxAmmo);
            return a;
        }

        public static Ammo operator-(Ammo a, int b)
        {
            return a + (-b);
        }

        public static Ammo operator--(Ammo a)
        {
            return a - a.ammoCostPerShoot;
        }

        public static bool operator>(Ammo a, int b)
        {
            return a.ammo > b;
        }

        public static bool operator<(Ammo a, int b)
        {
            return a.ammo < b;
        }

        public static bool operator>=(Ammo a, int b)
        {
            return a.ammo >= b;
        }

        public static bool operator<=(Ammo a, int b)
        {
            return a.ammo <= b;
        }

        public static bool operator==(Ammo a, int b)
        {
            return a.ammo == b;
        }

        public static bool operator!=(Ammo a, int b)
        {
            return a.ammo != b;
        }

        public override bool Equals(object obj)
        {
            return obj is Ammo ammo &&
                   this.ammo == ammo.ammo &&
                   maxAmmo == ammo.maxAmmo &&
                   ammoCostPerShoot == ammo.ammoCostPerShoot;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ammo, maxAmmo, ammoCostPerShoot);
        }
    }

    public class RangedWeaponController : WeaponController
    {
        [Header("RangedWeapon Parameters")]
        [Tooltip("Muzzle of the weapon, where the projectiles are shot")]
        public GameObject muzzle;

        [Tooltip("The type of weapon wil affect how it shoots")]
        public WeaponShootType shootType;

        [Tooltip("The interval between two shoot, unit(s)")]
        public float shootInterval = 0.5f;

        [Tooltip("The farthest distance a projectile can fly, unit(m)")]
        public float range = 1000f;

        [Header("Ammo")]
        //public Ammo ammo;
        [Tooltip("Current ammo")]
        public int ammo = 10;

        [Tooltip("The maximum amount of ammo")]
        public int maxAmmo = 10;

        [Tooltip("Number of ammo cost pre shoot")]
        public int ammoCostPerShoot = 1;

        [Tooltip("The time it takes to reload")]
        public float reloadTime = 1f;

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

        protected override void Start()
        {
            base.Start();
            m_MuzzleFlash = muzzle.transform.Find("MuzzleFlash").gameObject;
            m_LastShootTime = Time.time;
        }

        void Update()
        {
            
        }

        public override bool HandleFire1Input(bool inputDown, bool inputHeld, bool inputUp)
        {
            switch (shootType)
            {
                case WeaponShootType.Manual:
                    if (inputDown)
                    {
                        return Shoot();
                    }
                    return false;
                case WeaponShootType.Automatic:
                    if (inputHeld)
                    {
                        return Shoot();
                    }
                    return false;
            }
            return false; 
        }

        protected bool CanShoot()
        {
            return isIdle && ammo >= ammoCostPerShoot;
        }

        protected bool Shoot()
        {
            if (!CanShoot()) return false;

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

            weaponState = WeaponState.Firing;
            this.StartCoroutine(shootInterval, () => { weaponState = WeaponState.Idle; });
            return true;
        }

        private void ShootEffect()
        {
            //m_AudioSource.PlayOneShot(shootSound);
            m_MuzzleFlash.SetActive(true);
            this.StartCoroutine(shootInterval / 2, () => { m_MuzzleFlash.SetActive(false); });
        }

        private bool CrosshairRaycast(out RaycastHit hit)
        {
            Vector3 direction = ownerCamera.transform.forward;
            Vector3 origin = ownerCamera.transform.position + direction * 1;
            //Debug.DrawLine(origin, origin + direction * range, Color.red, 100f);
            return Physics.Raycast(
                origin,
                direction,
                out hit,
                range
            );
        }

        private bool CanReload()
        {
            return weaponState == WeaponState.Idle;
        }

        public override bool Reload()
        {
            if (!CanReload()) return false;

            weaponState = WeaponState.Reloading;
            this.StartCoroutine(reloadTime, ReloadFinish);

            return true;
        }

        protected void ReloadFinish()
        {
            ammo = maxAmmo;
            weaponState = WeaponState.Idle;
        }
    }
}