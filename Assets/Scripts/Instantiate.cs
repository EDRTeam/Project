using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Instantiate : MonoBehaviour
{
    public GameObject[] InstantiatePrefab;
    public Vector3 Pos;
    public Quaternion Rot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Instantiate0()
    {
        GameObject.Instantiate(InstantiatePrefab[0],Pos,Rot);
    }
    public void Instantiate1()
    {
        GameObject.Instantiate(InstantiatePrefab[1], Pos, Rot);
    }
}
