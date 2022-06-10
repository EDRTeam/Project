using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Shitu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite[] Tuzhi;
    public Image image1;
    public Text zhushi;
    public static Shitu Instance;
    public string[] wenben;
    static int  num=0;

    void Start()
    {
        image1.sprite = Tuzhi[0];
        Instance = this;

    }
    public void OnPointerEnter(PointerEventData enentData)
    {
        num = int.Parse(this.name);     
        image1.sprite = Tuzhi[num];
    }
    public void OnPointerExit(PointerEventData enentData)
    {
        image1.sprite = Tuzhi[0];

    }
    void Update()
    {
        
    }
    public void showzhushi()
    {
       zhushi.text = wenben[num];
        num = 0;

    }
   
}
