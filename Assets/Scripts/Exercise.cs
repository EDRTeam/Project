using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Exercise : MonoBehaviour
{
    //读取文档
    string[][] ArrayX;//题目数据
    string[] lineArray;//读取到题目数据
    private int topicMax = 0;//最大题数
    private List<bool> isAnserList = new List<bool>();//存放是否答过题的状态
    //加载题目
    public GameObject tipsbtn;//提示按钮
    public Text tipsText;//提示信息
    public List<Toggle> toggleList;//答题Toggle
    public Text indexText;//当前第几题
    public Text TM_Text;//当前题目
    public List<Text> DA_TextList;//选项
    //按钮功能及提示信息
    public Button BtnBack;//上一题
    public Button BtnNext;//下一题
    public Button BtnTip;//消息提醒
    public GameObject EditBtn;//修改题目按钮
    public Text TextAccuracy;//正确率
    private int anserint = 0;//已经答过几题
    private int isRightNum = 0;//正确题数
    public string tiku;
    private List<int> randomNum = new List<int>();  //存放随机数
    //编辑模式
    private string modeEditor;
    private Dictionary<string , object> dataDict = new Dictionary<string, object>();
    private bool inited = false;
    public static Exercise Instance;
    private string filePath = Application.streamingAssetsPath + "/EditMode.txt";
    public string EditAns;
    //编辑模式所需引用的变量
    public InputField[] Input;
    public InputField Timu;
    //public InputField Tishi;
    //public InputField RightAns;
    public bool IsEdit;
    public string  ModleID = "1001";
    private int topicIndex = 1;//第几题
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Init();
        TextCsv();
        LoadAnswer();
        toggleList[0].onValueChanged.AddListener((isOn) => AnswerRightRrongJudgment(0, isOn, "A"));
        toggleList[1].onValueChanged.AddListener((isOn) => AnswerRightRrongJudgment(1, isOn, "B"));
        toggleList[2].onValueChanged.AddListener((isOn) => AnswerRightRrongJudgment(2, isOn, "C"));
        toggleList[3].onValueChanged.AddListener((isOn) => AnswerRightRrongJudgment(3, isOn, "D"));
        BtnTip.onClick.AddListener(() => Select_Answer(0));
        BtnBack.onClick.AddListener(() => Select_Answer(1));
        BtnNext.onClick.AddListener(() => Select_Answer(2));
    }
    void Update()
    {
        IsEdit = GameObject.Find("UIcontroller").GetComponent<UIControll>().isBianji;
        int target = (GameObject.Find("UIcontroller").GetComponent<UIControll>().TargetSprit) + 1 + 1000;
        ModleID = target.ToString();
        if (IsEdit)
        {
            EditBtn.SetActive(true);
            //RightAns.gameObject.SetActive(true);
        }
        else
        {
            EditBtn.SetActive(false);
            //RightAns.gameObject.SetActive(false);
        }
    }
    void Init()
    {
        if (!inited)
        {
            //初始化文件读取
            inited = true;
            modeEditor = System.IO.File.ReadAllText(filePath);
            dataDict = MiniJSON.Json.Deserialize(modeEditor) as Dictionary<string, object>;
            /*
            SetMessage("1001", "1001", "这是个小测试");
            Debug.LogError(Instance.GetMessage("1001", "1001"));

            var exercise = Instance.GetExercises("1001", 1);
            Debug.LogError(exercise["问题"]);
            Debug.LogError(exercise["答案"]);
            Debug.LogError(exercise["A"]);
            Debug.LogError(exercise["B"]);
            Debug.LogError(exercise["C"]);
            Debug.LogError(exercise["D"]);

            // 这里是设置习题
            Instance.SetExercises("1002", 1, "这个是问题", "B", "A", "B", "C", "D");
            */
        }
    }
    /*****************读取txt数据******************/
    public void TextCsv()
    {
        /*
        //读取csv二进制文件  
        TextAsset binAsset = Resources.Load(tiku, typeof(TextAsset)) as TextAsset;
        //读取每一行的内容  
        lineArray = binAsset.text.Split('\r');
        //创建二维数组  
        ArrayX = new string[lineArray.Length][];
        //把csv中的数据储存在二维数组中  
        for (int i = 0; i < lineArray.Length; i++)
        {
            ArrayX[i] = lineArray[i].Split(':');
        }
        */
        //设置题目状态
        topicMax = Instance.GetDicNum(ModleID);
        for (int x = 0; x < topicMax + 1; x++)
        {
            isAnserList.Add(false);
        }
    }
    public void Chongzhi()
    {
        for (int x = 0; x < topicMax + 1; x++)
        {
            isAnserList[x] = false;
        }
        anserint = 0;
        isRightNum = 0;
        topicIndex = 1;
}
    /*****************加载题目******************/
    public void LoadAnswer()
    {
        if (IsEdit)
        {
            Timu.interactable = true;
            for (int i = 0; i < 4; ++i) Input[i].interactable = true;
        }
        else
        {
            Timu.interactable = false;
            for (int i = 0; i < 4; ++i) Input[i].interactable = false;
        }
        for (int i = 0; i < toggleList.Count; i++)
        {
            toggleList[i].isOn = false;
        }
        for (int i = 0; i < toggleList.Count; i++)
        {
            toggleList[i].interactable = true;
        }
        //GetRandomNum();  //获得随机数
        tipsbtn.SetActive(false);
        tipsText.text = "";
        indexText.text = "第" + topicIndex + "题：";//第几题
        //print(ModleID);
        var exercise = Instance.GetExercises(ModleID, topicIndex);
        EditAns = exercise["答案"].ToString();
        //TM_Text.text = (string)exercise["问题"];//题目
        Timu.text = exercise["问题"].ToString();
        //int idx = ArrayX[topicIndex].Length - 3;//有几个选项
        //DA_TextList[0].text = exercise["A"].ToString();
        //DA_TextList[1].text = exercise["B"].ToString();
        //DA_TextList[2].text = exercise["C"].ToString();
        //DA_TextList[3].text = exercise["D"].ToString();
        Input[0].text = exercise["A"].ToString();
        Input[1].text = exercise["B"].ToString();
        Input[2].text = exercise["C"].ToString();
        Input[3].text = exercise["D"].ToString();
        //RightAns.text = exercise["答案"].ToString();
    }
    /*****************按钮功能******************/
    void Select_Answer(int index)
    {
        switch (index)
        {
            case 0://提示
                var exercise = Instance.GetExercises(ModleID, topicIndex);
                //int idx = ArrayX[topicIndex].Length - 1;
                //int n = int.Parse(ArrayX[topicIndex][idx]);
                string nM = exercise["答案"].ToString();
                /*
                switch (n)

                {

                    case 1:

                        nM = "A";

                        break;

                    case 2:

                        nM = "B";

                        break;

                    case 3:

                        nM = "C";

                        break;

                    case 4:

                        nM = "D";

                        break;

                }
                */
                tipsText.text = "<color=#FFAB08FF>" + "正确答案是：" + nM + "</color>";
                break;
            case 1://上一题
                if (topicIndex > 1)
                {
                    topicIndex--;
                    LoadAnswer();
                }else{
                    tipsText.text = "<color=#27FF02FF>" + "前面已经没有题目了！" + "</color>";
                }
                break;
            case 2://下一题
                int exercisenum = Instance.GetDicNum(ModleID);
                if (topicIndex < exercisenum)
                {
                    topicIndex++;
                    LoadAnswer();
                }else{
                    tipsText.text = "<color=#27FF02FF>" + "哎呀！已经是最后一题了。" + "</color>";
                }
                break;
        }
    }
    /*****************题目对错判断******************/
    void AnswerRightRrongJudgment(int num, bool check, string index)
    {
        if (!IsEdit)
        {
            if (check)
            {
                //判断题目对错
                bool isRight;
                var exercise = Instance.GetExercises(ModleID, topicIndex);
                if (((string)exercise["答案"]) == index)
                {
                    tipsText.text = "<color=#27FF02FF>" + "恭喜你，答对了！" + "</color>";
                    isRight = true;
                    tipsbtn.SetActive(true);
                }
                else
                {
                    tipsText.text = "<color=#FF0020FF>" + "对不起，答错了！" + "</color>";
                    isRight = false;
                    tipsbtn.SetActive(true);
                }
                //正确率计算
                if (isAnserList[topicIndex])
                {
                    tipsText.text = "<color=#FF0020FF>" + "这道题已答过！" + "</color>";
                }
                else
                {
                    anserint++;
                    if (isRight)
                    {
                        isRightNum++;
                    }
                    isAnserList[topicIndex] = true;
                    TextAccuracy.text = "正确率：" + ((float)isRightNum / anserint * 100).ToString("f2") + "%";
                }
                //禁用掉选项
                for (int i = 0; i < toggleList.Count; i++)
                {
                    toggleList[i].interactable = false;
                }
            }
        }
        else
        {
            for (int i = 0; i < toggleList.Count; i++)
            {
                if (i != num)
                {
                    toggleList[i].isOn = false;
                }
            }
            EditAns = index;
        }
    }
    public void Change()
    {
        Instance.SetExercises(ModleID, topicIndex, Timu.text, EditAns, Input[0].text, Input[1].text, Input[2].text, Input[3].text);
    }
    //编辑模式所需函数
    public bool SetMessage(string orbitalId, string messageid, string newMsg)
    {
        Init();
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
                Debug.LogError("请检查默认配置文件是否配置了 " + orbitalId + " 的 message 参数");
            }
        }
        return false;
    }

    public string GetMessage(string orbitalId, string messageid)
    {
        Init();
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
                Debug.LogError("请检查默认配置文件是否配置了 " + orbitalId + " 的 message 参数");
            }
        }
        else
        {
            Debug.LogError("请检查默认配置文件是否配置了轨道 " + orbitalId + " 的参数");
        }
        Debug.LogError("未找到");
        return "";
    }

    public Dictionary<string, object> GetExercises(string orbitalId, int index)
    {
        Init();
        if (dataDict.ContainsKey(orbitalId))
        {
            var orbitaData = dataDict[orbitalId] as Dictionary<string, object>;
            if (orbitaData != null && orbitaData.ContainsKey("exercises"))
            {
                var exercises = orbitaData["exercises"] as Dictionary<string, object>;
                string quesKey = "第" + index.ToString() + "题";
                if (exercises.ContainsKey(quesKey))
                {
                    return exercises[quesKey] as Dictionary<string, object>;
                }
                else
                {
                    Debug.LogError("请检查默认配置文件是否配置了 " + orbitalId + " 的" + quesKey);
                }
                return null;
            }
            else
            {
                Debug.LogError("请检查默认配置文件是否配置了 " + orbitalId + " 的 exercises 参数");
            }
        }
        else
        {
            Debug.LogError("请检查默认配置文件是否配置了轨道 " + orbitalId + " 的参数");
        }
        return null;
    }

    public bool SetExercises(string orbitalId, int index, string question, string answer, string A, string B, string C, string D)
    {
        Init();
        if (dataDict.ContainsKey(orbitalId))
        {
            var orbitaData = dataDict[orbitalId] as Dictionary<string, object>;
            if (orbitaData != null && orbitaData.ContainsKey("exercises"))
            {
                var exercises = orbitaData["exercises"] as Dictionary<string, object>;
                string quesKey = "第" + index.ToString() + "题";
                exercises[quesKey] = new Dictionary<string, object>{
                    {"问题" , question},
                    {"答案" , answer},
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
                Debug.LogError("请检查默认配置文件是否配置了 " + orbitalId + " 的 exercises 参数");
            }
        }
        else
        {
            Debug.LogError("请检查默认配置文件是否配置了轨道 " + orbitalId + " 的参数");
        }
        Debug.LogError("未找到");
        return false;
    }
    public int GetDicNum(string orbitalId)
    {
        if (dataDict.ContainsKey(orbitalId))
        {
            var orbitaData = dataDict[orbitalId] as Dictionary<string, object>;
            if (orbitaData != null && orbitaData.ContainsKey("exercises"))
            {
                var exercises = orbitaData["exercises"] as Dictionary<string, object>;
                return exercises.Count;
            }
        }
        return 0;
    }
}
