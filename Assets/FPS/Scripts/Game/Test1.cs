using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1 : MonoBehaviour
{
    private Collider m_Collider;
    private Rigidbody m_Rigidbody;

    private void Awake()
    {
        Debug.Log("-----------------Test1.Awake()----------------------------");
    }

    private void OnEnable()
    {
        Debug.Log("-----------------Test1.OnEnable()----------------------------");
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Collider = GetComponent<Collider>();
        m_Rigidbody = GetComponent<Rigidbody>();
        Debug.Log("-----------------Test1.Start()----------------------------");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            
        }
    }
}
