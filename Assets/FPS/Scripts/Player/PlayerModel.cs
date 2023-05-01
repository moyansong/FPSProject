using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace FPS
{
    /// <summary>
    /// 存储玩家的数据
    /// </summary>
    public class PlayerModel : Singleton<PlayerModel>
    {
        // key-ID, val-该物品的数量
        public ConcurrentDictionary<int, int> itemCountDictionary = new ConcurrentDictionary<int, int>();
        
        public void AddItem(int ID)
        {
            if (itemCountDictionary.ContainsKey(ID))
            {
                ++itemCountDictionary[ID];
            }
            else
            {
                itemCountDictionary.TryAdd(ID, 1);
            }
        }

        public void RemoveItem(int ID)
        {
            if (itemCountDictionary.ContainsKey(ID))
            {
                --itemCountDictionary[ID];
                if (itemCountDictionary[ID] == 0)
                {
                    int val = 0;
                    itemCountDictionary.TryRemove(ID, out val);
                }
            }
        }
    }
}
