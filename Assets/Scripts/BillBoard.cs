using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    [SerializeField]
    private Camera m_Camera = null;

    public void Update()
    {
        transform.LookAt(transform.position - m_Camera.transform.rotation * Vector3.back, m_Camera.transform.rotation * Vector3.up);
    }
}
