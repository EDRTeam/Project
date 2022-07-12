using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetBottuonName : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach(var text in gameObject.transform.GetComponentsInChildren<Text>())
        {
            text.text = text.transform.parent.gameObject.name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
