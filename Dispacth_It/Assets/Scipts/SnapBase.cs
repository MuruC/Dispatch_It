using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SnapBase : MonoBehaviour
{
    public Transform TargetTransform;
    public GameObject indicator;
    public UnityEvent Connect;


    void Start()
    {
        indicator.SetActive(false);
    }

    void Update()
    {
        
    }
}
