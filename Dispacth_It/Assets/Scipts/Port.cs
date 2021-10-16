using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessageTree;
public class Port : MonoBehaviour
{
    public phonePlugInEnum PhoneType;
    private PhoneMessage PhoneMessage;
    public MeshRenderer MR;
    public Material GlowMat;
    public Material NormalMat;

    // Start is called before the first frame update
    void Start()
    {
        PhoneMessage = GameObject.Find("PhoneMessage").GetComponent<PhoneMessage>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Plug")
        {
            //PhoneMessage.ChooseOption(PhoneType);
        }
    }

    public void Choose()
    {
        Glow();
        PhoneMessage.ChooseOption(PhoneType);
    }

    public void Glow()
    {
        Material[] Mats = new Material[1];
        Mats[0] = GlowMat;
        MR.materials = Mats;
    }

    public void StopGlow()
    {
        Material[] Mats = new Material[1];
        Mats[0] = NormalMat;
        MR.materials = Mats;
    }
}
