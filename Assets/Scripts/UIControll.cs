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
    //public GameObject Modeltimu;

    public GameObject viewer;     //缩略预览图

    public Camera[] cameras;
    

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
    public void ChooseShitu()
    {
        Tuku.SetActive(false);
        Edittimu.SetActive(false);
        ShituModel.SetActive(true);
    }
    public void FreeExp()
    {
        FreeModel.SetActive(true);
        ExpModel .SetActive(false);
    }
    public void StandardExp()
    {
        StandardModel.SetActive(true);
        ExpModel.SetActive(false);
    }
    public void BacktoExp()
    {
        ExpModel.SetActive(true);
        StandardModel.SetActive(false); 
        FreeModel.SetActive(false);
    }
    //生成模型按钮
    public void BuildModel()
    {
        ShituModel.SetActive(false);
        //ShengchengModel.SetActive(true);
        cameras[1].gameObject.SetActive(true);
        cameras[2].gameObject.SetActive(true);
        cameras[1].rect = new Rect(0.509f, 0.05f, 0.47f, 0.9f);
        cameras[2].rect = new Rect(0.021f, 0.05f, 0.47f, 0.9f);
    }
    public void Modelzuoti()
    {
        foreach(var image in Modeltimu.gameObject.GetComponentsInChildren<Image>())
        {
            image.color = new Color(image.color.r,image.color.g,image.color.b,0f);
        }
        ShituModel.SetActive(false);
        
        viewer.SetActive(false);
        cameras[1].gameObject.SetActive(false);
        cameras[2].DORect(new Rect(0, 0, 1, 1), 1f);
        
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
    }
    public void BackToTuku()
    {
        ShituModel.SetActive(false);
        Tuku.SetActive(true);
        cameras[2].gameObject.SetActive(false);
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
