using FPS.Game;
using System.Collections.Generic;
using UnityEngine;


namespace FPS.Gameplay
{

    public class WeaponManager : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("Usually the character")]
        public GameObject owner;

        [Tooltip("Usually the player")]
        public GameObject instigator;

        [Tooltip("Usually the main camera")]
        public Camera ownerCamera;

        public PlayerInputHandler inputHandler;

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

        private int m_WeaponIndex = -1;// 当前武器在weapons中的下标
        private List<GameObject> m_Weapons = new List<GameObject>();// 所有武器
        private List<WeaponController> m_WeaponControllers = new List<WeaponController>();// 所有武器的控制器

        // 当前的武器
        public GameObject weapon 
        { 
            get
            {
                return m_WeaponIndex >= 0 && m_WeaponIndex < m_Weapons.Count ? m_Weapons[m_WeaponIndex] : null;
            }
        }

        // 当前武器的控制器
        public WeaponController weaponController
        {
            get
            {
                return m_WeaponIndex >= 0 && m_WeaponIndex < m_WeaponControllers.Count ? m_WeaponControllers[m_WeaponIndex] : null;
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
                return m_Weapons.Count;
            }
        }

        private Animator m_fpsAnimator;
        private AudioSource m_audioSource;

        // Start is called before the first frame update
        void Start()
        {
            m_fpsAnimator = GetComponentInChildren<Animator>();
            m_audioSource = GetComponent<AudioSource>();

            foreach (var startingWeapon in startingWeapons)
            {
                AddWeapon(startingWeapon);
            }
            ChangeWeapon(1); 
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                ChangeWeapon(-1);
            }
            if (inputHandler.GetReloadInputDown())
            {
                Reload();
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                RemoveWeapon();
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                Test();
            }

            HandleFire1Input();
        }

        private void Test()
        {
            
        }

        /// <summary>
        /// 切换到下value个武器
        /// </summary>
        /// <param name="value"></param>
        private void ChangeWeapon(int value = 1)
        {
            // Fix me：切换武器应该有CD？
            if (m_Weapons.Count == 0)
            {
                Debug.Log("当前没有武器");
                return;
            }

            WeaponController oldWeaponController = null;
            if (weaponController != null)
            {
                weaponController.Unequip();
                oldWeaponController = weaponController;
            }
            m_WeaponIndex = (m_WeaponIndex + value) % m_Weapons.Count;
            if (m_WeaponIndex < 0) m_WeaponIndex = m_Weapons.Count - 1;
            weaponController.Equip();

            m_fpsAnimator.SetLayerWeight(oldWeaponController != null ? oldWeaponController.animatorLayer : 0, 0f);
            m_fpsAnimator.SetLayerWeight(weaponController.animatorLayer, 1f);

            EventManager.Broadcast(new WeaponChangedEvent(oldWeaponController, weaponController));
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
            if (newWeaponController == null)
            {
                Debug.Log($"获取{newWeaponInstance.ToString()}的WeaponController失败");
                Destroy(newWeaponInstance);
                return;
            }

            AttachWeapon(newWeaponInstance, newWeaponController);
            
            newWeapon.SetActive(false);
            newWeaponController.owner = owner != null ? owner : gameObject;
            newWeaponController.instigator = instigator != null ? instigator : owner;
            newWeaponController.ownerCamera = ownerCamera;
            newWeaponController.sourcePrefab = newWeapon;
            newWeaponController.animator = m_fpsAnimator;

            m_Weapons.Add(newWeaponInstance);
            m_WeaponControllers.Add(newWeaponController);

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
            return RemoveWeaponByIndex(m_Weapons.Count - 1, shouldDestroy);
        }

        public GameObject RemoveWeapon(int weaponID, bool shouldDestroy = true)
        {
            for (int i = 0; i < m_WeaponControllers.Count; ++i)
            {
                if (m_WeaponControllers[i].weaponID == weaponID)
                {
                    return RemoveWeaponByIndex(i, shouldDestroy);
                }
            }
            return null;
        }

        public GameObject RemoveWeapon(string weaponName, bool shouldDestroy = true)
        {
            for (int i = 0; i < m_WeaponControllers.Count; ++i)
            {
                if (m_WeaponControllers[i].weaponName == weaponName)
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
            if (index < 0 || index >= m_Weapons.Count) return null;

            GameObject removedWeapon = m_Weapons[index];
            WeaponController removedWeaponContoller = m_WeaponControllers[index];

            PlayerModel.Instance.RemoveItem(removedWeaponContoller.weaponID);

            m_Weapons.RemoveAt(index);
            m_WeaponControllers.RemoveAt(index);

            if (index == m_WeaponIndex)
            {
                ChangeWeapon(0);
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
        private void HandleFire1Input()
        {            
            if (weaponController.HandleFire1Input(inputHandler.GetFire1InputDown(), inputHandler.GetFire1InputHeld(), inputHandler.GetFire1InputUp()))
            {
                m_fpsAnimator.SetTrigger("Fire1");
            }
        }

        private void Reload()
        {     
            if (weaponController.Reload())
            {
                m_fpsAnimator.SetTrigger("Reload");
            }
        }

    }
}