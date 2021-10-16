using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knob : MonoBehaviour
{
    public GameObject myPointer;
    public Transform minPointerTransform;
    public Transform maxPointerTransform;
    public float minAngle;
    public float maxAngle;
    [SerializeField]
    private bool isRotated = true;
    private float minPointerX;
    private float maxPointerX;
    // Start is called before the first frame update
    void Start()
    {
        minPointerX = minPointerTransform.position.x;
        maxPointerX = maxPointerTransform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotated)
        {
            float angle = transform.localEulerAngles.y;
            //print("angle: " + transform.localEulerAngles.y);
            if (angle < minAngle)
            {
                angle = minAngle;
            }
            else if (angle > maxAngle)
            {
                angle = maxAngle;
            }
            //float x = (angle - minAngle) / (maxAngle - minAngle) * (maxPointerX - minPointerX) + minPointerX;
            float x = maxPointerX - (angle - minAngle) / (maxAngle - minAngle) * (maxPointerX - minPointerX);
            //print(x);
            Vector3 pos = myPointer.transform.position;
            myPointer.transform.position = new Vector3(x, pos.y, pos.z);
        }
    }

    public void Randomize()
    {
        print(transform.localEulerAngles);
        transform.localEulerAngles = new Vector3(0, Random.Range(minAngle, maxAngle), 180);
        print(transform.localEulerAngles);
    }
}
