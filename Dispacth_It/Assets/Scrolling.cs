using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<RectTransform>().anchoredPosition += new Vector2(0, 1.5f);
        if (GetComponent<RectTransform>().anchoredPosition.y > 400)
        {
            GetComponent<RectTransform>().anchoredPosition += new Vector2(0, -1700);
        }
    }
}
