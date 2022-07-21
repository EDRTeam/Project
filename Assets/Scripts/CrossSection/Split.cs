using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class Split : MonoBehaviour
{
    public Material matCross;
    public Vector3 phyB;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
             CrossSection();
        }
        
    }

    public void CrossSection()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, phyB, transform.rotation);
        foreach (Collider c in colliders)
        {
            SlicedHull hull = c.gameObject.Slice(transform.localPosition, transform.up);

            if (hull == null)
            {
                Debug.Log("����ʧ��");
            }
            if (hull != null)
            {
                GameObject lower = hull.CreateLowerHull(c.gameObject, matCross);
                lower.transform.position = c.transform.position;
                lower.tag = "CrossSectionPart";      //��ǩ����֮��ɾ������������ģ��
            }
            //c.gameObject.SliceInstantiate(transform.position, transform.up);
        }
        gameObject.SetActive(false);
        if (GameObject.FindGameObjectWithTag("FreeExp"))
        {
            GameObject.FindGameObjectWithTag("FreeExp").SetActive(false);
        }
    }
}
