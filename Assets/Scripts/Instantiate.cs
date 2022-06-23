using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Instantiate : MonoBehaviour
{
    public GameObject[] InstantiatePrefab;
    public Transform generatePos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ModelInstantiate(GameObject prefab)
    {
        GameObject fe = GameObject.Instantiate(prefab, generatePos.position, generatePos.rotation);
        fe.transform.SetParent(generatePos);
    }
}
