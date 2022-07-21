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

    public bool isBianji = false;   //判断是进入编辑模式还是不是  用于后面返回按钮判断

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

    private bool flag_xiti;      //习题部分切换按钮操作
    //private bool isCallOut;      //呼出或隐藏图纸
    private Vector3 originScale_Graph;    //UICanvas下的识图模块的图纸初始大小
    private Vector3 originPos_Graph;      //
    private Vector3 originScale; //2DCamera下canvas的识图模块图纸

    public Button[] buttons;        //存储一些按钮  让其在特定工作完成之后可再点击

    public Transform[] trans_modelCameraOrigin;//存放不同脚本看模型摄像机的起始状态信息
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
        isBianji = false;
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
    public void TuzhiBtn(int Tuzhinum)
    {
        TargetSprit = Tuzhinum;
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
        if (TargetSprit >= 1)
        {
            Graph.GetComponent<RectTransform>().sizeDelta = new Vector2(2400, 1920);
        }
        else
        {
            Graph.GetComponent<RectTransform>().sizeDelta = new Vector2(3800, 1460);
        }
        Graph.GetComponent<Image>().sprite = TuzhiSprit[TargetSprit];
        Tuku.SetActive(false);
        Edittimu.SetActive(false);
        ShituModel.SetActive(true);
    }
    //被改变的物体
    public GameObject Shiyantuzhi;
    //标准实验图纸序号
    public int TargetExp;
    //加载将要用于修改的图片的路径
    public Sprite[] ShiyanSprit;
    public void ExpBtn(int ExpNum)
    {
        TargetExp = ExpNum;
        Shiyantuzhi.GetComponent<Image>().sprite = ShiyanSprit[TargetExp];
    }
    public void FreeExp()
    {
        //自由实验摄像机
        cameras[3].gameObject.SetActive(true);
        FreeModel.SetActive(true);
        ExpModel .SetActive(false);
    }
    public GameObject StandardExpFace;
    public void StandardExpIns()
    {
        StandardExpFace.SetActive(true);
        ExpModel.SetActive(false);
    }
    public void StandardExp()
    {
        //标准实验摄像机--暂时和自由实验相同
        //应当具有题目
        cameras[3].gameObject.SetActive(true);
        FreeModel.SetActive(true);   //后面根据实际实现应删除
        StandardModel.SetActive(true);
        StandardExpFace.SetActive(false);
    }
    public void StandardBackToExp()
    {
        ExpModel.SetActive(true);
        StandardExpFace.SetActive(false);
    }
    public GameObject ExpTuzhi;
    public GameObject ExpTargetTuzhi;
    public void CheckExp()
    {
        ExpTuzhi.SetActive(true);
        ExpTargetTuzhi.GetComponent<Image>().sprite = ShiyanSprit[TargetExp];
    }
    public void CloseCheckExp()
    {
        ExpTuzhi.SetActive(false);
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

        //设置看模型的摄像机的初始位置
        cameras[2].transform.SetPositionAndRotation(trans_modelCameraOrigin[TargetSprit].position, trans_modelCameraOrigin[TargetSprit].rotation);
        cameras[1].rect = new Rect(0.509f, 0.05f, 0.47f, 0.9f);
        cameras[2].rect = new Rect(0.021f, 0.05f, 0.47f, 0.9f);
        cameras[1].gameObject.SetActive(true);
        cameras[2].gameObject.SetActive(true);

        Graph1[TargetSprit].SetActive(true);

        //记录图纸最初大小
        originScale = Graph1[TargetSprit].transform.parent.GetComponent<RectTransform>().localScale;

        viewer.SetActive(true);
        //播放生成模型的动画  0是生成动画  1是习题动画
        TimelineManager.instance.PlayTimeline(TargetSprit, 0, ()=>{
            //播放完生成模型动画让习题按钮可点击
            buttons[0].enabled = true;
        });
    }

    public void Modelzuoti()
    {
        GameObject.Find("UIcontroller").GetComponent<Exercise>().LoadAnswer();
        //第一次进入习题
        if (flag_xiti)
        {
            Vector2 position = new Vector2(0, -430);
            Graph1[TargetSprit].transform.parent.gameObject.GetComponent<RectTransform>().DOScale(Vector3.zero * 0.01f,0.5f);
            Graph1[TargetSprit].transform.parent.gameObject.GetComponent<RectTransform>().DOAnchorPos(position, 0.5f);
            //isCallOut = false;
            Xiti_SetOrigin();

            ShituModel.SetActive(false);
            controller.GetComponent<CameraController>().enabled = false;
            viewer.SetActive(false);
            cameras[1].DORect(new Rect(0, 0, 1, 1), 0.5f);
            cameras[2].DORect(new Rect(0.06f, 0.08f, 0.86f, 0.82f), 1f);
            Modeltimu.SetActive(true);
            Modeltimu.GetComponent<Image>().DOFade(0.8f, 0.6f);

            //回调 动画播放完成显示题目
            TimelineManager.instance.PlayTimeline(TargetSprit, 1, () => {
                CameraController.instance.check = true;
                Xiti_CallOut();
                cameras[2].gameObject.SetActive(false);
                flag_xiti = false;
            });
        }
        else        //开始做题之后开关习题界面
        {
            Xiti_SetOrigin();
            ShituModel.SetActive(false);
            viewer.SetActive(false);
            Modeltimu.SetActive(true);
            Modeltimu.GetComponent<Image>().DOFade(0.8f, 0.6f);
            Xiti_CallOut();
            Vector2 position = new Vector2(0, -430);
            Graph1[TargetSprit].transform.parent.gameObject.GetComponent<RectTransform>().DOScale(Vector3.zero * 0.01f, 0.5f);
            Graph1[TargetSprit].transform.parent.gameObject.GetComponent<RectTransform>().DOAnchorPos(position, 0.5f);

        }

    }
    
    //让习题界面所有UI透明度为0
    private void Xiti_SetOrigin()
    {
        foreach (var image in Modeltimu.gameObject.GetComponentsInChildren<Image>())
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        }
        foreach (var text in Modeltimu.gameObject.GetComponentsInChildren<Text>())
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
        }
    }
    //做习题界面渐出动画
    private void Xiti_CallOut()
    {
        Modeltimu.SetActive(true);
        foreach (var image in Modeltimu.gameObject.GetComponentsInChildren<Image>())
        {
            if (image.gameObject.tag == "InputField")
            {
                continue;
            }
            image.DOFade(0.8f, 1f);
        }
        foreach (var text in Modeltimu.gameObject.GetComponentsInChildren<Text>())
        {
            text.DOFade(1f, 1f);
        }
    }

    public void Xiti_DongHua()
    {
        TimelineManager.instance.PlayTimeline(TargetSprit, 1, () => {
            cameras[2].gameObject.SetActive(false);
            //播放完脚本需求动画之后可再点击
            buttons[1].enabled = true;
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
        Tuku.SetActive(true);
        
        Edittimu.SetActive(true);
        isBianji = true;
        GameObject.Find("UIcontroller").GetComponent<Exercise>().LoadAnswer();
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
        //判断是从编辑模式返回还是从教学模式返回
        if (!isBianji)
        {
            StudyModel.SetActive(true);
        }
        else
        {
            ChooseScene.SetActive(true);
            isBianji = false;
        }
        Tiku.SetActive(false);
        Tuku.SetActive(false);
        ThreeView.SetActive(false);

    }

    //从习题到识图
    public void BackToXiti()
    {
        //出图纸
        Vector2 position = new Vector3(0, 0);
        Graph1[TargetSprit].transform.parent.gameObject.GetComponent<RectTransform>().DOScale(originScale, 0.5f);
        Graph1[TargetSprit].transform.parent.gameObject.GetComponent<RectTransform>().DOAnchorPos(position, 0.5f);

        //设置做题ui透明度为0
        Xiti_SetOrigin();
        Modeltimu.SetActive(false);
    }

    //从识图回到图库列表
    public void ShituToToku()
    {
        Graph1[TargetSprit].SetActive(false);
        GameObject.Find("UIcontroller").GetComponent<Exercise>().Chongzhi();
        GameObject.Find("UIcontroller").GetComponent<Exercise>().LoadAnswer();
        ShituModel.SetActive(false);
        Tuku.SetActive(true);
        InitBMCamera();
        //判断重新选择图纸 进入图纸学习
        flag_xiti = true;
        Graph.transform.parent.transform.localScale = originScale_Graph;
        Graph.transform.parent.localPosition = new Vector3(0, 0,-590);
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

    //到实验模式
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

    //初始化两个分屏摄像机的参数
    private void InitBMCamera()
    {
        cameras[1].rect = new Rect(0.509f, 0.05f, 0.47f, 0.9f);
        cameras[2].rect = new Rect(0.021f, 0.05f, 0.47f, 0.9f);
        cameras[1].gameObject.SetActive(false);
        cameras[2].gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        lastPos = cameras[3].gameObject.transform.position;
        lastRot = cameras[3].gameObject.transform.rotation;

        flag_xiti = true;
        InitBMCamera();
        originScale_Graph = Graph.transform.parent.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
