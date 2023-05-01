using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


namespace FPS.Gameplay
{

    public class WeaponManager : MonoBehaviour
    {

        private int weaponIndex = 0;// 当前武器在weapons中的下标
        private List<GameObject> weapons = new List<GameObject>();// 所有武器
        private List<WeaponController> weaponControllers = new List<WeaponController>();// 所有武器的控制器

        // 当前的武器
        public GameObject weapon 
        { 
            get
            {
                return weaponIndex < weapons.Count ? weapons[weaponIndex] : null;
            }
        }

        // 当前武器的控制器
        public WeaponController weaponController
        {
            get
            {
                return weaponIndex < weaponControllers.Count ? weaponControllers[weaponIndex] : null;
            }
        }

        // 当前使用武器的ID
        public int weaponID
        {
            get
            {
                return weaponController.weaponID;
            }
        }

        public int weaponCount
        {
            get
            {
                return weapons.Count;
            }
        }

        [Header("Parameters")]
        [Tooltip("The name that will be displayed in the UI for this weapon")]
        public int capacity = 10;

        [Tooltip("Player's own weapon at birth")]
        public List<GameObject> startingWeapons = new List<GameObject>();

        [Header("Weapon Settings")]
        [Tooltip("The weapon holding in the right hand will be placed under this socket")]
        public Transform rightHandSocket;

        [Tooltip("The weapon holding in the left hand will be placed under this socket")]
        public Transform leftHandSocket;

        private Animator animator;
        private AudioSource audioSource;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();

            foreach (var startingWeapon in startingWeapons)
            {
                AddWeapon(startingWeapon);
            }
            EquipWeapon(0); 
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                EquipWeapon(1);
            }
            if (Input.GetMouseButtonDown(0))
            {
                Fire();
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                RemoveWeapon();
            }
        }

        /// <summary>
        /// 切换到下value个武器
        /// </summary>
        /// <param name="value"></param>
        private void EquipWeapon(int value)
        {
            // Fix me：切换武器应该有CD？
            if (weapons.Count == 0)
            {
                Debug.Log("当前没有武器");
                return;
            }

            if (weaponController != null)
            {
                weaponController.Unequip();
            }
            weaponIndex = (weaponIndex + value) % weapons.Count;
            weaponController.Equip();

            animator.SetInteger("WeaponID", weaponID);
            animator.SetBool("Equipping", true);
            StartCoroutine(EquipFinish());
        }

        IEnumerator EquipFinish()
        {
            yield return new WaitForSeconds(0.25f);
            animator.SetBool("Equipping", false);
        }

        public void AddWeapon(int weaponID)
        {

        }

        public void AddWeapon(string weaponName)
        {

        }

        public void AddWeapon(GameObject newWeapon)
        {
            if (weaponCount >= capacity)
            {
                Debug.Log("背包已满, 不能装更多武器了");
                return;
            }

            GameObject newWeaponInstance = Instantiate(newWeapon);
            WeaponController newWeaponController = newWeaponInstance.GetComponent<WeaponController>();

            AttachWeapon(newWeaponInstance, newWeaponController);
            
            newWeapon.SetActive(false);
            newWeaponController.owner = gameObject;
            newWeaponController.SourcePrefab = newWeapon;
            weapons.Add(newWeaponInstance);
            weaponControllers.Add(newWeaponController);

            PlayerModel.Instance.AddItem(newWeaponController.weaponID);
        }

        private void AttachWeapon(GameObject newWeapon, WeaponController newWeaponController)
        {
            if (newWeaponController.isRightHolding)
            {
                newWeapon.transform.SetParent(rightHandSocket, false);
                if (newWeaponController.subWeapon != null)
                {
                    newWeaponController.subWeapon = Instantiate(newWeaponController.subWeapon, leftHandSocket, false);
                }
            }
            else
            {
                newWeapon.transform.SetParent(leftHandSocket, false);
                if (newWeaponController.subWeapon != null)
                {
                    newWeaponController.subWeapon = Instantiate(newWeaponController.subWeapon, rightHandSocket, false);
                }
            }
        }
        // 移除最后一把武器
        public GameObject RemoveWeapon(bool shouldDestroy = true)
        {
            return RemoveWeaponByIndex(weapons.Count - 1, shouldDestroy);
        }

        public GameObject RemoveWeapon(int weaponID, bool shouldDestroy = true)
        {
            for (int i = 0; i < weaponControllers.Count; ++i)
            {
                if (weaponControllers[i].weaponID == weaponID)
                {
                    return RemoveWeaponByIndex(i, shouldDestroy);
                }
            }
            return null;
        }

        public GameObject RemoveWeapon(string weaponName, bool shouldDestroy = true)
        {
            for (int i = 0; i < weaponControllers.Count; ++i)
            {
                if (weaponControllers[i].weaponName == weaponName)
                {
                    return RemoveWeaponByIndex(i, shouldDestroy);
                }
            }
            return null;
        }

        /// <summary>
        /// 移除weapons数组中第index个武器，如果正在使用这把武器，会自动切换到下一把
        /// 内部调用，外部应使用RemoveWeapon
        /// </summary>
        /// <param name="index"></param>
        /// <param name="shouldDestroy"><为true则销毁武器/param>
        /// <returns><不销毁武器的情况下会返回武器，否则返回null/returns>
        private GameObject RemoveWeaponByIndex(int index, bool shouldDestroy = true)
        {
            if (index < 0 || index >= weapons.Count) return null;

            GameObject removedWeapon = weapons[index];
            WeaponController removedWeaponContoller = weaponControllers[index];

            PlayerModel.Instance.RemoveItem(removedWeaponContoller.weaponID);

            weapons.RemoveAt(index);
            weaponControllers.RemoveAt(index);

            if (index == weaponIndex)
            {
                EquipWeapon(0);
            }

            if (shouldDestroy)
            {
                Destroy(removedWeapon);
                return null;
            }
            return removedWeapon;
        }

        /// <summary>
        /// 通常由鼠标左键触发
        /// </summary>
        private void Fire()
        {
            animator.SetTrigger("Fire");
            weaponController.Fire();
        }

        private void Attack()
        {
            
        }
    }
}