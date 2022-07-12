using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditMode : MonoBehaviour
{
    // Start is called before the first frame update
    private string modeEditor;
    private Dictionary<string , object> dataDict = new Dictionary<string, object>();
    private bool inited = false;

    public static EditMode Instance;
    void Awake(){
        Instance = this;
    }

    void Start()
    {
        Init();
    }

    private string filePath = Application.streamingAssetsPath + "/EditMode.txt";

    void Init(){
        if(!inited){
            inited = true;
            modeEditor = System.IO.File.ReadAllText(filePath);
            dataDict = MiniJSON.Json.Deserialize(modeEditor) as Dictionary<string , object>;


            SetMessage("1001" , "1001" , "这是个小测试");
            Debug.LogError(EditMode.Instance.GetMessage("1001" , "1001"));

            var exercise = EditMode.Instance.GetExercises("1001" , 1);
            Debug.LogError(exercise["问题"]);
            Debug.LogError(exercise["答案"]);
            Debug.LogError(exercise["A"]);
            Debug.LogError(exercise["B"]);
            Debug.LogError(exercise["C"]);
            Debug.LogError(exercise["D"]);
            var num = Instance.GetDicNum("1001");
            Debug.LogError(num);
            // 这里是设置习题
            EditMode.Instance.SetExercises("1002" , 1 , "这个是问题" , "B" , "A" , "B" , "C" , "D");
        }
    }

    public bool SetMessage(string orbitalId , string messageid , string newMsg){
        Init();
        if(dataDict.ContainsKey(orbitalId)){
            var orbitaData = dataDict[orbitalId] as Dictionary<string , object>;
            if(orbitaData != null && orbitaData.ContainsKey("message")){
                var message = orbitaData["message"] as Dictionary<string , object>;
                if(message != null){
                    message[messageid] = newMsg;
                    System.IO.File.WriteAllText(filePath , MiniJSON.Json.Serialize(dataDict));
                    return true;
                }
            }else{
                Debug.LogError("请检查默认配置文件是否配置了 " + orbitalId + " 的 message 参数");
            }
        }
        return false;
    }

    public string GetMessage(string orbitalId , string messageid){
        Init();
        if(dataDict.ContainsKey(orbitalId)){
            var orbitaData = dataDict[orbitalId] as Dictionary<string , object>;
            if(orbitaData != null && orbitaData.ContainsKey("message")){
                var message = orbitaData["message"] as Dictionary<string , object>;
                if(message != null && message.ContainsKey(messageid)){
                    return message[messageid] as string;
                }
            }else{
                Debug.LogError("请检查默认配置文件是否配置了 " + orbitalId + " 的 message 参数");
            }
        }else{
            Debug.LogError("请检查默认配置文件是否配置了轨道 " + orbitalId + " 的参数");
        }
        Debug.LogError("未找到");
        return "";
    }

    public Dictionary<string , object> GetExercises(string orbitalId , int index){
        Init();
        if(dataDict.ContainsKey(orbitalId)){
            var orbitaData = dataDict[orbitalId] as Dictionary<string , object>;
            if(orbitaData != null && orbitaData.ContainsKey("exercises")){
                var exercises = orbitaData["exercises"] as Dictionary<string , object>;
                string quesKey = "第" + index.ToString() + "题";
                if(exercises.ContainsKey(quesKey)){
                    return exercises[quesKey] as Dictionary<string , object>;
                }else{
                    Debug.LogError("请检查默认配置文件是否配置了 " + orbitalId + " 的" + quesKey);
                }
                return null;
            }else{
                Debug.LogError("请检查默认配置文件是否配置了 " + orbitalId + " 的 exercises 参数");
            }
        }else{
            Debug.LogError("请检查默认配置文件是否配置了轨道 " + orbitalId + " 的参数");
        }
        return null;
    }

    public bool SetExercises(string orbitalId , int index , string question , string answer , string A , string B , string C , string D){
        Init();
        if(dataDict.ContainsKey(orbitalId)){
            var orbitaData = dataDict[orbitalId] as Dictionary<string , object>;
            if(orbitaData != null && orbitaData.ContainsKey("exercises")){
                var exercises = orbitaData["exercises"] as Dictionary<string , object>;
                string quesKey = "第" + index.ToString() + "题";
                exercises[quesKey] = new Dictionary<string , object>{
                    {"问题" , question},
                    {"答案" , answer},
                    {"A" , A},
                    {"B" , B},
                    {"C" , C},
                    {"D" , D},
                };
                System.IO.File.WriteAllText(filePath , MiniJSON.Json.Serialize(dataDict));
                return true;
            }else{
                Debug.LogError("请检查默认配置文件是否配置了 " + orbitalId + " 的 exercises 参数");
            }
        }else{
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
