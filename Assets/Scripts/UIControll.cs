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
    //���ı������
    public GameObject Yulantuzhi;
    //��Ҫ�ı��ͼƬ
    public Sprite YulantuzhiSprit;
    //���ؽ�Ҫ�����޸ĵ�ͼƬ��·��
    public Sprite[] TuzhiSprit;
    //֮����÷����õı���
    public int TargetSprit;
    //ѡ��ͼֽ
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
    //��ҳ����Ҫ�޸ĵ�sprit
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
        //����ʵ�������
        cameras[3].gameObject.SetActive(true);
        FreeModel.SetActive(true);
        ExpModel .SetActive(false);
    }
    public void StandardExp()
    {
        //��׼ʵ�������--��ʱ������ʵ����ͬ
        //Ӧ��������Ŀ
        cameras[3].gameObject.SetActive(true);
        FreeModel.SetActive(true);   //�������ʵ��ʵ��Ӧɾ��
        StandardModel.SetActive(true);
        ExpModel.SetActive(false);
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
        
        //�ص� �������������ʾ��Ŀ
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

        StudyModel.SetActive(true);
        Tiku.SetActive(false);
        Tuku.SetActive(false);
        ThreeView.SetActive(false);
    }

    //������ѧϰϰ�ⷵ��
    public void BackToTuku()
    {
        ShituModel.SetActive(false);
        Tuku.SetActive(true);
        cameras[2].gameObject.SetActive(false);

        //��������ui͸����Ϊ0
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
