using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    public AudioSource AS;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float angle = transform.localEulerAngles.y;
        if (angle < 100)
        {
            transform.localEulerAngles = new Vector3(0, 100, 0);
        }
        if (angle > 140)
        {
            transform.localEulerAngles = new Vector3(0, 140, 0);
        }
        AS.volume = (angle - 100) / 40;
    }
}
