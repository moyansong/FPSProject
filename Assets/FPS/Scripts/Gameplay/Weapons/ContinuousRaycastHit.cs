using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS.Gameplay
{
    public class ContinuousRaycastHit : MonoBehaviour
    {
        [Header("Parameters")]
        [Tooltip("Use the lines connecting these points for collision detection")]
        public Transform[] tracePoints;

        private Vector3[] m_LastTracePosition;

        private float m_LastRaycastTime = 0f;

        RaycastHit[] m_RaycastHits = new RaycastHit[50];

        // Start is called before the first frame update
        void Start()
        {
            m_LastTracePosition = new Vector3[tracePoints.Length];
        }

        // Update is called once per frame
        void Update()
        {

        }

        void UpdatePosition()
        {
            for (int i = 0; i < tracePoints.Length; ++i)
            {
                m_LastTracePosition[i] = tracePoints[i].position;
            }
        }

        public int Raycast(RaycastHit[] results)
        {
            if (Time.time - m_LastRaycastTime > Time.fixedDeltaTime * 1.2f)
            {
                UpdatePosition();
            }
            m_LastRaycastTime = Time.time;

            int index = 0;
            for (int i = 0; i < tracePoints.Length; ++i)
            {
                Vector3 lastTracePosition = m_LastTracePosition[i];
                Vector3 newTracePosition = tracePoints[i].transform.position;
                m_LastTracePosition[i] = tracePoints[i].position;
                Debug.DrawRay(lastTracePosition, newTracePosition - lastTracePosition, Color.red, 3f);

                Ray ray = new Ray(lastTracePosition, newTracePosition - lastTracePosition);          
                int n = Physics.RaycastNonAlloc(ray, m_RaycastHits, Vector3.Distance(lastTracePosition, newTracePosition));
                
                for (int j = 0; j < Mathf.Min(n, m_RaycastHits.Length); ++j)
                {
                    if (index < results.Length)
                    {
                        results[index++] = m_RaycastHits[j];
                    }
                    else
                    {
                        return results.Length;
                    }
                }
            }
            return index;
        }
    }
}