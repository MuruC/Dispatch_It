using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftLight : MonoBehaviour
{
    public AudioSource AS;
    public Material M1;
    public Material M2;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (AS.isPlaying)
        {
            GetComponent<MeshRenderer>().material = M2;
        } else
        {
            GetComponent<MeshRenderer>().material = M1;
        }
    }
}
