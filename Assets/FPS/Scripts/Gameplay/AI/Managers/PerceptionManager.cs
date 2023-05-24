using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FPS.Gameplay.AI
{
    // 管理所有感知组件，自动比较目标优先级，获得/丢失目标时会出触发委托
    public class PerceptionManager : MonoBehaviour
    {
        public GameObject target => m_TargetInfo.target;

        public UnityAction<TargetInfo> onPerceivedTarget;
        public UnityAction<TargetInfo> onLostTarget;

        private Perception[] m_Perceptions;

        private TargetInfo m_TargetInfo = new();

        private TargetInfo m_LastTargetInfo = new();

        private HashSet<TargetInfo> m_TargetInfoHash = new();


        private void Awake()
        {
            m_Perceptions = GetComponentsInChildren<Perception>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        void Update()
        {

        }

        public void PerceivedTarget(TargetInfo targetInfo)
        {
            if (!IsVaildTarget(targetInfo)) return;

            m_TargetInfoHash.Add(targetInfo);

            if (targetInfo > m_TargetInfo)
            {
                m_TargetInfo = targetInfo;
                onPerceivedTarget?.Invoke(m_TargetInfo);
            }
        }

        public void LostTarget(TargetInfo targetInfo)
        {
            if (!IsVaildTarget(targetInfo)) return;

            m_TargetInfoHash.Remove(targetInfo);

            if (m_TargetInfo == targetInfo && m_TargetInfoHash.Count > 0)
            {
                foreach (var target in m_TargetInfoHash)
                {
                    if (target > m_TargetInfo)
                    {
                        m_TargetInfo = target;
                    }
                }
                onPerceivedTarget.Invoke(m_TargetInfo);
            }
        }

        private bool IsVaildTarget(TargetInfo targetInfo)
        {
            return targetInfo.target != null && 
                   targetInfo.perception != null;
        }

    }
}