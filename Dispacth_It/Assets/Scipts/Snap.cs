using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snap : MonoBehaviour
{
    public List<Port> portScripts;

    [SerializeField]
    string BaseTag;
    [SerializeField]
    Transform DefaultTransform, TargetTransform;
    [SerializeField]
    bool Held;
    [SerializeField]
    AudioSource AS;

    public GameObject Plug;

    void Start()
    {
        
    }

    void Update()
    {
        if (!Held && TargetTransform)
        {
            if (TargetTransform.GetComponent<SnapBase>().indicator)
            {
                TargetTransform.GetComponent<SnapBase>().indicator.SetActive(false);
            }
            transform.position += (TargetTransform.position - transform.position)/20;
            transform.localRotation = TargetTransform.localRotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == BaseTag)
        {
            other.GetComponent<SnapBase>().indicator.SetActive(true);
            TargetTransform = other.GetComponent<SnapBase>().TargetTransform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == BaseTag)
        {
            other.GetComponent<SnapBase>().indicator.SetActive(false);
            TargetTransform = DefaultTransform;
        }
    }

    public void OnHold()
    {
        Held = true;
    }

    public void OnRelease()
    {
        Held = false;
        TargetTransform.gameObject.GetComponent<SnapBase>().Connect.Invoke();
        //GetComponent<Rigidbody>().isKinematic = false;
    }

    public void ResetToDefault()
    {
        TargetTransform = DefaultTransform;
        for (int i = 0; i < portScripts.Count; i++)
        {
            portScripts[i].StopGlow();
        }
    }
}
