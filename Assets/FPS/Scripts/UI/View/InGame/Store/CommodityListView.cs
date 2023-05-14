using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FPS.UI
{
    public class CommodityListView : View
    {

        [Header("Parameters")]
        public GameObject storeUI;

        [Tooltip("CommodityList currently displayed on the screen")]
        public GameObject activeList;

        [Header("Effect")]
        [Tooltip("The sound emitted when clicking a normal button")]
        public AudioClip clickSound;

        private StoreView m_StoreView;

        private AudioSource m_AudioSource;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            m_StoreView = storeUI.GetComponent<StoreView>();
            m_AudioSource = storeUI.GetComponent<AudioSource>();
            activeList.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void CommodityButtonOnClick(int ID)
        {
            Debug.Log("选择商品，显示商品详细信息");
            m_AudioSource.PlayOneShot(clickSound);
            m_StoreView.SetCommodityDetailVisibility(ID, true);
        }

        public void ShowList(GameObject newActiveList)
        {
            activeList.SetActive(false);
            activeList = newActiveList;
            activeList.SetActive(true);
        }
    }
}