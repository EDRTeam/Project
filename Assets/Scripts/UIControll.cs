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

    public bool isBianji = false;   //�ж��ǽ���༭ģʽ���ǲ���  ���ں��淵�ذ�ť�ж�

    public GameObject viewer;     //����Ԥ��ͼ
    public GameObject controller; //����������Ŀ�����
    public Camera[] cameras;

    public Transform thPos;      //��������ͼ�ĸ���ģ�͵�λ��
    public Transform thMpos;     //�ۿ�����ͼʱ��ģ�͵��������
    private Vector3 lastPos;
    private Quaternion lastRot;
    public GameObject thM;       //Ҫ��������ͼ��ģ�ͣ�Ҳ����ѧ������ģ��
    private GameObject tempThM;  //�����������������������ͼ��ģ��

    public GameObject CrossPlane;
    public GameObject CrossPanel;

    private bool flag_xiti;      //ϰ�ⲿ���л���ť����
    //private bool isCallOut;      //����������ͼֽ
    private Vector3 originScale_Graph;    //UICanvas�µ�ʶͼģ���ͼֽ��ʼ��С
    private Vector3 originPos_Graph;      //
    private Vector3 originScale; //2DCamera��canvas��ʶͼģ��ͼֽ

    public Button[] buttons;        //�洢һЩ��ť  �������ض��������֮����ٵ��

    public Transform[] trans_modelCameraOrigin;//��Ų�ͬ�ű���ģ�����������ʼ״̬��Ϣ
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
    //���ı������
    public GameObject Yulantuzhi;
    //��Ҫ�ı��ͼƬ
    public Sprite YulantuzhiSprit;
    //���ؽ�Ҫ�����޸ĵ�ͼƬ��·��
    public Sprite[] TuzhiSprit;
    //֮����÷����õı���
    public int TargetSprit;
    //ѡ��ͼֽ
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
    //��ҳ����Ҫ�޸ĵ�sprit
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
    //���ı������
    public GameObject Shiyantuzhi;
    //��׼ʵ��ͼֽ���
    public int TargetExp;
    //���ؽ�Ҫ�����޸ĵ�ͼƬ��·��
    public Sprite[] ShiyanSprit;
    public void ExpBtn(int ExpNum)
    {
        TargetExp = ExpNum;
        Shiyantuzhi.GetComponent<Image>().sprite = ShiyanSprit[TargetExp];
    }
    public void FreeExp()
    {
        //����ʵ�������
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
        //��׼ʵ�������--��ʱ������ʵ����ͬ
        //Ӧ��������Ŀ
        cameras[3].gameObject.SetActive(true);
        FreeModel.SetActive(true);   //�������ʵ��ʵ��Ӧɾ��
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
    //������ʵ�鷵��
    public void BacktoExp()
    {
        //����ʵ�������false
        cameras[3].gameObject.SetActive(false);
        ExpModel.SetActive(true);
        StandardModel.SetActive(false); 
        FreeModel.SetActive(false);

        //�������ں˶Դ𰸵�ģ��
        GameObject[] temp =  GameObject.FindGameObjectsWithTag("Anwser");
        foreach (var go in temp)
        {
            Destroy(go);
        }
        Destroy(tempThM);      //���������������������������ͼ��ģ��
        //����ѧ�����Ļ�������
        for (int i = 0; i < thM.transform.childCount; i++)
        {
            Destroy(thM.transform.GetChild(i).gameObject);
        }
    }

    //��Ҫ�޸ĵ�����
    public GameObject[] Graph1;
    //����ģ�Ͱ�ť
    public void BuildModel()
    {

        ShituModel.SetActive(false);
        //ShengchengModel.SetActive(true);

        //���ÿ�ģ�͵�������ĳ�ʼλ��
        cameras[2].transform.SetPositionAndRotation(trans_modelCameraOrigin[TargetSprit].position, trans_modelCameraOrigin[TargetSprit].rotation);
        cameras[1].rect = new Rect(0.509f, 0.05f, 0.47f, 0.9f);
        cameras[2].rect = new Rect(0.021f, 0.05f, 0.47f, 0.9f);
        cameras[1].gameObject.SetActive(true);
        cameras[2].gameObject.SetActive(true);

        Graph1[TargetSprit].SetActive(true);

        //��¼ͼֽ�����С
        originScale = Graph1[TargetSprit].transform.parent.GetComponent<RectTransform>().localScale;

        viewer.SetActive(true);
        //��������ģ�͵Ķ���  0�����ɶ���  1��ϰ�⶯��
        TimelineManager.instance.PlayTimeline(TargetSprit, 0, ()=>{
            //����������ģ�Ͷ�����ϰ�ⰴť�ɵ��
            buttons[0].enabled = true;
        });
    }

    public void Modelzuoti()
    {
        GameObject.Find("UIcontroller").GetComponent<Exercise>().LoadAnswer();
        //��һ�ν���ϰ��
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

            //�ص� �������������ʾ��Ŀ
            TimelineManager.instance.PlayTimeline(TargetSprit, 1, () => {
                CameraController.instance.check = true;
                Xiti_CallOut();
                cameras[2].gameObject.SetActive(false);
                flag_xiti = false;
            });
        }
        else        //��ʼ����֮�󿪹�ϰ�����
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
    
    //��ϰ���������UI͸����Ϊ0
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
    //��ϰ����潥������
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
            //������ű����󶯻�֮����ٵ��
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
        Debug.Log("�˳�");
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
        //�ж��Ǵӱ༭ģʽ���ػ��Ǵӽ�ѧģʽ����
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

    //��ϰ�⵽ʶͼ
    public void BackToXiti()
    {
        //��ͼֽ
        Vector2 position = new Vector3(0, 0);
        Graph1[TargetSprit].transform.parent.gameObject.GetComponent<RectTransform>().DOScale(originScale, 0.5f);
        Graph1[TargetSprit].transform.parent.gameObject.GetComponent<RectTransform>().DOAnchorPos(position, 0.5f);

        //��������ui͸����Ϊ0
        Xiti_SetOrigin();
        Modeltimu.SetActive(false);
    }

    //��ʶͼ�ص�ͼ���б�
    public void ShituToToku()
    {
        Graph1[TargetSprit].SetActive(false);
        GameObject.Find("UIcontroller").GetComponent<Exercise>().Chongzhi();
        GameObject.Find("UIcontroller").GetComponent<Exercise>().LoadAnswer();
        ShituModel.SetActive(false);
        Tuku.SetActive(true);
        InitBMCamera();
        //�ж�����ѡ��ͼֽ ����ͼֽѧϰ
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

    //��ʵ��ģʽ
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

    //�˳�����ͼ�ۿ�
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

    //��ʼ����
    public void ToCrossSection()
    {
        CrossPlane.SetActive(true);
        CrossPanel.SetActive(true);
    }

    //�˳�����
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
        //ɾ�����в�����ģ��
        GameObject[] temps = GameObject.FindGameObjectsWithTag("CrossSectionPart");
        foreach(var go in temps)
        {
            Destroy(go);
        }
        thPos.parent.gameObject.SetActive(true);
        CrossPlane.SetActive(true);
        thM.SetActive(true);
    }

    //��ʼ����������������Ĳ���
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
