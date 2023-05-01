using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS.Gameplay
{
    public class WeaponController : MonoBehaviour
    {
        [Header("Parameters")]
        [Tooltip("Each item in the game has a unique ID, and the ID of the weapon usually ranges from 0 to 29999")]
        public int weaponID;

        [Tooltip("The name that will be displayed in the UI for this weapon")]
        public string weaponName;

        [Tooltip("Holding this weapon with the right hand ?")]
        public bool isRightHolding = true;

        [Tooltip("SubWeapon or DualWeapon")]
        public GameObject subWeapon = null;

        [Header("Effect")]
        [Tooltip("Sound effect during weapon firing")]
        public AudioClip fireSound;

        public GameObject owner { get; set; }

        public GameObject SourcePrefab { get; set; }

        private AudioSource audioSource;

        // Start is called before the first frame update
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public virtual void Equip()
        {
            gameObject.SetActive(true);
            if (subWeapon != null)
            {
                subWeapon.SetActive(true);
            }
        }

        public virtual void Unequip()
        {
            gameObject.SetActive(false);
            if (subWeapon != null)
            {
                subWeapon.SetActive(false);
            }
        }

        public virtual void Fire()
        {
            audioSource.PlayOneShot(fireSound);
        }
    }

}