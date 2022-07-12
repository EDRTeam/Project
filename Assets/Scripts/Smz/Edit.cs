using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Edit : MonoBehaviour
{
    public GameObject TDZhushi; 
    public TextMeshPro TDZishi;
    public string ModelID;
    public string ZhushiID;
    public InputField UIZhushi;
    public bool isEdit;
    private string modeEditor;
    private Dictionary<string, object> dataDict = new Dictionary<string, object>();
    private bool inited = false;
    public static Edit Instance;
    private string filePath = Application.streamingAssetsPath + "/EditMode.txt";
    void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }
    void Init()
    {
        if (!inited)
        {
            inited = true;
            modeEditor = System.IO.File.ReadAllText(filePath);
            dataDict = MiniJSON.Json.Deserialize(modeEditor) as Dictionary<string, object>;

            /*
            SetMessage("1001", "1001", "���Ǹ�С����");
            Debug.LogError(Instance.GetMessage("1001", "1001"));

            var exercise = Instance.GetExercises("1001", 1);
            Debug.LogError(exercise["����"]);
            Debug.LogError(exercise["��"]);
            Debug.LogError(exercise["A"]);
            Debug.LogError(exercise["B"]);
            Debug.LogError(exercise["C"]);
            Debug.LogError(exercise["D"]);

            // ����������ϰ��
            Instance.SetExercises("1002", 1, "���������", "B", "A", "B", "C", "D");
            */
        }
    }
    // Update is called once per frame
    void Update()
    {
        TDZishi.text = UIZhushi.text;
    }
    public void OpenZhushi()
    {
        UIZhushi.gameObject.SetActive(true);
        TDZhushi.SetActive(true);
        isEdit = GameObject.Find("UIcontroller").GetComponent<UIControll>().isBianji;
        if (!isEdit)
        {
            UIZhushi.interactable = false;
        }
        UIZhushi.text = Instance.GetMessage(ModelID,ZhushiID);
        //Debug.LogError(Instance.GetMessage("1001", "1001"));
    }
    public void CloseZhushi()
    {
        isEdit = GameObject.Find("UIcontroller").GetComponent<UIControll>().isBianji;
        if (isEdit)
        {
            Instance.SetMessage(ModelID, ZhushiID, UIZhushi.text);
        }
        UIZhushi.interactable = true;
        UIZhushi.gameObject.SetActive(false);
        TDZhushi.SetActive(false);
    }
    public bool SetMessage(string orbitalId, string messageid, string newMsg)
    {
        if (dataDict.ContainsKey(orbitalId))
        {
            var orbitaData = dataDict[orbitalId] as Dictionary<string, object>;
            if (orbitaData != null && orbitaData.ContainsKey("message"))
            {
                var message = orbitaData["message"] as Dictionary<string, object>;
                if (message != null)
                {
                    message[messageid] = newMsg;
                    System.IO.File.WriteAllText(filePath, MiniJSON.Json.Serialize(dataDict));
                    return true;
                }
            }
            else
            {
                Debug.LogError("����Ĭ�������ļ��Ƿ������� " + orbitalId + " �� message ����");
            }
        }
        return false;
    }

    public string GetMessage(string orbitalId, string messageid)
    {
        if (dataDict.ContainsKey(orbitalId))
        {
            var orbitaData = dataDict[orbitalId] as Dictionary<string, object>;
            if (orbitaData != null && orbitaData.ContainsKey("message"))
            {
                var message = orbitaData["message"] as Dictionary<string, object>;
                if (message != null && message.ContainsKey(messageid))
                {
                    return message[messageid] as string;
                }
            }
            else
            {
                Debug.LogError("����Ĭ�������ļ��Ƿ������� " + orbitalId + " �� message ����");
            }
        }
        else
        {
            Debug.LogError("����Ĭ�������ļ��Ƿ������˹�� " + orbitalId + " �Ĳ���");
        }
        Debug.LogError("δ�ҵ�");
        return "";
    }

    public Dictionary<string, object> GetExercises(string orbitalId, int index)
    {
        if (dataDict.ContainsKey(orbitalId))
        {
            var orbitaData = dataDict[orbitalId] as Dictionary<string, object>;
            if (orbitaData != null && orbitaData.ContainsKey("exercises"))
            {
                var exercises = orbitaData["exercises"] as Dictionary<string, object>;
                string quesKey = "��" + index.ToString() + "��";
                if (exercises.ContainsKey(quesKey))
                {
                    return exercises[quesKey] as Dictionary<string, object>;
                }
                else
                {
                    Debug.LogError("����Ĭ�������ļ��Ƿ������� " + orbitalId + " ��" + quesKey);
                }
                return null;
            }
            else
            {
                Debug.LogError("����Ĭ�������ļ��Ƿ������� " + orbitalId + " �� exercises ����");
            }
        }
        else
        {
            Debug.LogError("����Ĭ�������ļ��Ƿ������˹�� " + orbitalId + " �Ĳ���");
        }
        return null;
    }

    public bool SetExercises(string orbitalId, int index, string question, string answer, string A, string B, string C, string D)
    {
        if (dataDict.ContainsKey(orbitalId))
        {
            var orbitaData = dataDict[orbitalId] as Dictionary<string, object>;
            if (orbitaData != null && orbitaData.ContainsKey("exercises"))
            {
                var exercises = orbitaData["exercises"] as Dictionary<string, object>;
                string quesKey = "��" + index.ToString() + "��";
                exercises[quesKey] = new Dictionary<string, object>{
                    {"����" , question},
                    {"��" , answer},
                    {"A" , A},
                    {"B" , B},
                    {"C" , C},
                    {"D" , D},
                };
                System.IO.File.WriteAllText(filePath, MiniJSON.Json.Serialize(dataDict));
                return true;
            }
            else
            {
                Debug.LogError("����Ĭ�������ļ��Ƿ������� " + orbitalId + " �� exercises ����");
            }
        }
        else
        {
            Debug.LogError("����Ĭ�������ļ��Ƿ������˹�� " + orbitalId + " �Ĳ���");
        }
        Debug.LogError("δ�ҵ�");
        return false;
    }
}
