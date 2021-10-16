using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class EffectApplier : MonoBehaviour
{
    public AudioMixer AM;
    public GameObject TargetFrequency;
    public List<GameObject> Frequency;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(TargetFrequency.transform.position, Frequency[0].transform.position) + Vector3.Distance(TargetFrequency.transform.position, Frequency[1].transform.position);
        AM.SetFloat("Distortion", dist*8);
        AM.SetFloat("CutOffFreq", Mathf.Max(1000, 6000 - 6000 * dist * 10));
    }
}
