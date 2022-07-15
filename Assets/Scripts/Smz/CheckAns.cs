using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAns : MonoBehaviour
{
    public int TargetAns;
    public GameObject[] ModelAns;
    public GameObject fep;
    private GameObject instance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Checkans()
    {
        TargetAns = GameObject.Find("UIcontroller").GetComponent<UIControll>().TargetExp;
        Transform[] ch = fep.GetComponentsInChildren<Transform>();
        Vector3 Pos = Vector3.zero;
        for(int i = 0; i < ch.Length; i++)
        {
            if(ch[i].name != fep.name)
            {
                Pos += ch[i].transform.localPosition;
            }
        }
        Pos = Pos / ch.Length;
        ch = fep.GetComponentsInChildren<Transform>(true);
        Vector3 Scale = ModelAns[TargetAns].transform.localScale;
        for (int i = 0; i < ch.Length; i++)
        {
            if (ch[i].name != fep.name && ch[i].name != ModelAns[TargetAns].name)
            {
                //Scale = Vector3.Cross(Scale, ch[i].transform.localScale);
                Scale.x = Scale.x * ch[i].transform.localScale.x;
                Scale.y = Scale.y * ch[i].transform.localScale.y;
                Scale.z = Scale.z * ch[i].transform.localScale.z;
            }
        }
        float maxScale;
        maxScale = Scale.x;
        if (maxScale < Scale.y)
        {
            maxScale = Scale.y;
        }
        if (maxScale < Scale.z)
        {
            maxScale = Scale.z;
        }
        Scale = new Vector3(maxScale, maxScale, maxScale);
        if (!instance)
        {
            print(fep.transform);
            instance = GameObject.Instantiate(ModelAns[TargetAns], fep.transform);
            instance.transform.localPosition = Pos;
            instance.transform.localScale = Scale;
        }
        else
        {
            Destroy(instance);
        }
        //instance.transform.Translate(Vector3.up * 0.5f * instance.transform.localScale.y, Space.Self);
    }
}
