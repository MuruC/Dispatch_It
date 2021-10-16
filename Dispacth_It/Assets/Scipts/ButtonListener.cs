using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MessageTree;
public class ButtonListener : MonoBehaviour
{
    public phonePlugInEnum PhoneType;
    GameObject PhoneMessageTree;
    // Start is called before the first frame update
    void Start()
    {
        PhoneMessageTree = GameObject.Find("Sphere");
        gameObject.GetComponent<Button>().onClick.AddListener(() => PhoneMessageTree.GetComponent<PhoneMessage>().ChooseOption(PhoneType));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
