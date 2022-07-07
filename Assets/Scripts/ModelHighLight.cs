using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelHighLight : MonoBehaviour
{
    //public GameObject cur_Obj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = CastRay();//创建屏幕发射射线
            if (hit.collider != null)
            {
                //检测到的物体不是需求模型
                if (!hit.collider.CompareTag("HighLight"))
                {   
                    //当前有选中物体 高亮显示关
                    if (cur_Obj != null)
                    {
                        cur_Obj.GetComponent<MeshRenderer>().materials[0].SetFloat("_Emission", 0);
                        cur_Obj = null;
                    }   
                    return;
                }else if (hit.collider.CompareTag("HighLight"))
                {
                    //当前没有选中物体 将检测到的物体高亮显示
                    if (cur_Obj == null)
                    {
                        cur_Obj = hit.collider.gameObject;
                        cur_Obj.GetComponent<MeshRenderer>().materials[0].SetFloat("_Emission", 1);
                    }
                    //有选中物体并且不是新检测到物体
                    if (hit.collider.gameObject != cur_Obj)
                    {
                        cur_Obj.GetComponent<MeshRenderer>().materials[0].SetFloat("_Emission", 0);
                        cur_Obj = hit.collider.gameObject;
                        cur_Obj.GetComponent<MeshRenderer>().materials[0].SetFloat("_Emission", 1);
                    }
                    else return;
                }
            }
            else
            {
                //没有检测到物体 
                if (cur_Obj != null && cur_Obj.CompareTag("HighLight"))
                {
                    cur_Obj.GetComponent<MeshRenderer>().materials[0].SetFloat("_Emission", 0);
                    cur_Obj = null;
                }
                return;
            }
        }*/
    }

    public void HighLightOn()
    {
        gameObject.GetComponent<MeshRenderer>().materials[0].SetFloat("_Emission", 1);
    }

    public void HighLightOff()
    {
        gameObject.GetComponent<MeshRenderer>().materials[0].SetFloat("_Emission", 0);
    }
}
