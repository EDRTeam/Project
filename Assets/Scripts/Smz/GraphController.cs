using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphController : MonoBehaviour
{
    public Sprite[] GraphImage;
    public GameObject[] graph;
    int now;

    public GameObject Graph;
    // Start is called before the first frame update
    void Start()
    {
        now = 0;
    }

    public void ChangeImage()
    {
        if(now + 1 < GraphImage.Length)
        {
            now++;
            Graph.GetComponent<Image>().sprite = GraphImage[now];
        }
        else
        {
            now = 0;
            Graph.GetComponent<Image>().sprite = GraphImage[now];
        }
        foreach (var g in graph)
        {
            g.SetActive(false);
        }
        graph[now].SetActive(true);
    }
}
