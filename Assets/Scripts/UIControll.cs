using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIControll : MonoBehaviour
{
    public GameObject StartScene;
    public GameObject LoginScene;
    public GameObject Zhuce;
    public GameObject ChooseScene;
    public GameObject StudyModel;
    public GameObject ExpModel;
    public GameObject WebModel;
    public GameObject ShituModel;
    public GameObject Tuku;
    public GameObject Tuzhi;
    public GameObject Edittimu;
    public GameObject Tiku;
    public GameObject ShengchengModel;
    public GameObject Modeltimu;
    public GameObject Modeltimuanswer;
    public GameObject FreeModel;
    public GameObject StandardModel;
    public GameObject ThreeView;
    //public GameObject Modeltimu;

    public GameObject viewer;     //缩略预览图
    public GameObject controller; //摄像机操作的控制器
    public Camera[] cameras;

    public Transform thPos;      //生成三视图的副本模型的位置
    public Transform thMpos;     //观看三视图时看模型的相机坐标
    private Vector3 lastPos;
    private Quaternion lastRot;
    public GameObject thM;       //要生成三视图的模型，也就是学生做的模型
    private GameObject tempThM;  //在三个相机下用于生成三视图的模型

    public GameObject CrossPlane;
    public GameObject CrossPanel;  

    public void Getin()
    {
        StartScene.SetActive(false);
        Zhuce.SetActive(false);
        LoginScene.SetActive(true);
    }
    public void Login()
    {
        LoginScene.SetActive(false);
        ChooseScene.SetActive(true);
    }
    public void Tozhuce()
    {
        LoginScene.SetActive(false);
        Zhuce.SetActive(true);
    }
    public void ChooseStudy()
    {
        ChooseScene.SetActive(false);
        StudyModel.SetActive(true);
    }
    public void ToTuku()
    {
        //SceneManager.LoadScene("shitu");
        StudyModel.SetActive(false);
        Tuku.SetActive(true);
    }
    //被改变的物体
    public GameObject Yulantuzhi;
    //需要改变的图片
    public Sprite YulantuzhiSprit;
    //加载将要用于修改的图片的路径
    public Sprite[] TuzhiSprit;
    //之后调用方便用的变量
    public int TargetSprit;
    //选择图纸
    public void TuzhiBtn0()
    {
        TargetSprit = 0;
        Yulantuzhi.GetComponent<Image>().sprite = TuzhiSprit[TargetSprit];
    }
    public void TuzhiBtn1()
    {
        TargetSprit = 1;
        Yulantuzhi.GetComponent<Image>().sprite = TuzhiSprit[TargetSprit];
    }
    public void Backtoshitu()
    {
        ShituModel.SetActive(true);
        Modeltimu.SetActive(false);
        
    }
    public void BacktoModeltimu()
    {
        Modeltimu.SetActive(true);
        Modeltimuanswer.SetActive(false);

    }
    //该页面需要修改的sprit
    public GameObject Graph;
    public void ChooseShitu()
    {
        Graph.GetComponent<Image>().sprite = TuzhiSprit[TargetSprit];
        Tuku.SetActive(false);
        Edittimu.SetActive(false);
        ShituModel.SetActive(true);
    }
    public void FreeExp()
    {
        //自由实验摄像机
        cameras[3].gameObject.SetActive(true);
        FreeModel.SetActive(true);
        ExpModel .SetActive(false);
    }
    public void StandardExp()
    {
        //标准实验摄像机--暂时和自由实验相同
        //应当具有题目
        cameras[3].gameObject.SetActive(true);
        FreeModel.SetActive(true);   //后面根据实际实现应删除
        StandardModel.SetActive(true);
        ExpModel.SetActive(false);
    }

    //从自由实验返回
    public void BacktoExp()
    {
        //自由实验摄像机false
        cameras[3].gameObject.SetActive(false);
        ExpModel.SetActive(true);
        StandardModel.SetActive(false); 
        FreeModel.SetActive(false);

        //销毁用于核对答案的模型
        GameObject[] temp =  GameObject.FindGameObjectsWithTag("Anwser");
        foreach (var go in temp)
        {
            Destroy(go);
        }
        Destroy(tempThM);      //销毁在三个相机下用于生成三视图的模型
        //销毁学生做的基本构件
        for (int i = 0; i < thM.transform.childCount; i++)
        {
            Destroy(thM.transform.GetChild(i).gameObject);
        }
    }
    //需要修改的物体
    public GameObject[] Graph1;
    //生成模型按钮
    public void BuildModel()
    {
        ShituModel.SetActive(false);
        //ShengchengModel.SetActive(true);
        cameras[1].gameObject.SetActive(true);
        cameras[2].gameObject.SetActive(true);
        Graph1[TargetSprit].SetActive(true);
        cameras[1].rect = new Rect(0.509f, 0.05f, 0.47f, 0.9f);
        cameras[2].rect = new Rect(0.021f, 0.05f, 0.47f, 0.9f);
    }
    public void Modelzuoti()
    {
        Graph1[TargetSprit].SetActive(false);
        foreach(var image in Modeltimu.gameObject.GetComponentsInChildren<Image>())
        {
            image.color = new Color(image.color.r,image.color.g,image.color.b,0f);
        }
        ShituModel.SetActive(false);
        controller.GetComponent<CameraController>().enabled = false;
        viewer.SetActive(false);
        cameras[1].gameObject.SetActive(false);
        cameras[2].DORect(new Rect(0, 0, 1, 1), 1f);
        
        //回调 动画播放完成显示题目
        TimelineManager.instance.PlayTimeline(0,() => {
            CameraController.instance.check = true;
            Modeltimu.SetActive(true);
            foreach(var image in Modeltimu.gameObject.GetComponentsInChildren<Image>())
            {
                image.DOFade(0.8f, 1f);
            }
        });

    }
    public void ChooseTiku()
    {
        Tiku.SetActive(true);
        StudyModel.SetActive(false);
    }
    public void ChooseExp()
    {
        ChooseScene.SetActive(false);
        ExpModel.SetActive(true);
    }
    public void ChooseWeb()
    {
        ChooseScene.SetActive(false);
        WebModel.SetActive(true);
    }
    public void ChooseEdit()
    {
        ChooseScene.SetActive(false);
        ShituModel.SetActive(true);
        
        Edittimu.SetActive(true);

    }
    public void Exit()
    {
        Debug.Log("退出");
        Application.Quit();
    }
    public void BackToLogin()
    {

        LoginScene.SetActive(true);
        ChooseScene.SetActive(false);
    }
    public void BackToChosse()
    {
        ChooseScene.SetActive(true);
        WebModel.SetActive(false);
        StudyModel.SetActive(false);
        ExpModel.SetActive(false);
    }
    public void BackToStudy()
    {

        StudyModel.SetActive(true);
        Tiku.SetActive(false);
        Tuku.SetActive(false);
        ThreeView.SetActive(false);
    }

    //从自主学习习题返回
    public void BackToTuku()
    {
        ShituModel.SetActive(false);
        Tuku.SetActive(true);
        cameras[2].gameObject.SetActive(false);

        //设置做题ui透明度为0
        foreach (var image in Modeltimu.gameObject.GetComponentsInChildren<Image>())
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        }
        Modeltimu.SetActive(false);
    }
    public void Checktuzhi()
    {
       
        Tuzhi.SetActive(true);
    }
    public void CloseChecktuzhi()
    {

        Tuzhi.SetActive(false);
    }
    public void Finishimodeltimu()
    {

        Modeltimu.SetActive(false);
        Modeltimuanswer.SetActive(true);
    }
    public void CloseModelanswer()
    {
        Modeltimu.SetActive(true);
        Modeltimuanswer.SetActive(false);
    }
    public void Backtoshitumodel()
    {
        ShituModel .SetActive(true);
        ShengchengModel.SetActive(false);
    }
    public void ToXiti()
    {
        cameras[0].gameObject.SetActive(false);
        cameras[3].gameObject.SetActive(true);
    }
    public void LookSanshi()
    {
        lastPos = cameras[3].gameObject.transform.position;
        lastRot = cameras[3].gameObject.transform.rotation;
        tempThM = GameObject.Instantiate(thM, thPos.position, thPos.rotation);
        for(int i = 0; i < FreeModel.transform.childCount; i++)
        {
            FreeModel.transform.GetChild(i).gameObject.SetActive(false);
        }
        //cameras[0].gameObject.SetActive(true);
        ThreeView.SetActive(true);
        cameras[3].gameObject.transform.DOMove(thMpos.position,0.5f);
        cameras[3].gameObject.transform.DORotateQuaternion(thMpos.rotation, 0.5f);
        //cameras[3].gameObject.SetActive(false);
    }

    //退出三视图观看
    public void ThreeVtoFreeE()
    {
        for (int i = 0; i < FreeModel.transform.childCount; i++)
        {
            FreeModel.transform.GetChild(i).gameObject.SetActive(true);
        }
        ThreeView.SetActive(false);
        CrossPanel.gameObject.SetActive(false);
        cameras[3].gameObject.transform.DOMove(lastPos,0.5f);
        cameras[3].gameObject.transform.DORotateQuaternion(lastRot, 0.5f);

        Destroy(tempThM);
    }

    //开始剖切
    public void ToCrossSection()
    {
        CrossPlane.SetActive(true);
        CrossPanel.SetActive(true);
    }

    //退出剖切
    public void ExitCrossSection()
    {
        CrossPlane.SetActive(false);
        CrossPanel.SetActive(false);
        GameObject[] temps = GameObject.FindGameObjectsWithTag("CrossSectionPart");
        foreach (var go in temps)
        {
            Destroy(go);
        }
        thM.SetActive(true);
    }

    public void RedoCorssSection()
    {
        //删除剖切产生的模型
        GameObject[] temps = GameObject.FindGameObjectsWithTag("CrossSectionPart");
        foreach(var go in temps)
        {
            Destroy(go);
        }
        thPos.parent.gameObject.SetActive(true);
        CrossPlane.SetActive(true);
        thM.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        lastPos = cameras[3].gameObject.transform.position;
        lastRot = cameras[3].gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
