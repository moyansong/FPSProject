using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FPS.UI
{
    public class CommodityListView : View
    {
        private StoreView storeView;

        private AudioSource audioSource;

        [Header("Parameters")]
        public GameObject storeUI;

        [Tooltip("CommodityList currently displayed on the screen")]
        public GameObject activeList;

        [Header("Effect")]
        [Tooltip("The sound emitted when clicking a normal button")]
        public AudioClip clickSound;


        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            storeView = storeUI.GetComponent<StoreView>();
            audioSource = storeUI.GetComponent<AudioSource>();
            activeList.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void CommodityButtonOnClick(int ID)
        {
            Debug.Log("选择商品，显示商品详细信息");
            audioSource.PlayOneShot(clickSound);
            storeView.SetCommodityDetailVisibility(ID, true);
        }

        public void ShowList(GameObject newActiveList)
        {
            activeList.SetActive(false);
            activeList = newActiveList;
            activeList.SetActive(true);
        }
    }
}