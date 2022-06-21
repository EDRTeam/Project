using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check : MonoBehaviour
{
    public GameObject ans;
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
    public void CheckAns0()
    {
        GameObject.Instantiate(ans, Pos, Rot);
    }

}
