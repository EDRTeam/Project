using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SliderValueToText : MonoBehaviour {
    Text text;
    InputField inputField;
    void Awake()
    {
        text = GetComponent<Text>();
        inputField = GetComponent<InputField>();
    }

	public void SetText(Slider slider)
    {
        text.text = System.Math.Round(slider.value, 2).ToString();
    }

    public void SetInputFieldText(Slider slider)
    {
        inputField.text = System.Math.Round(slider.value, 2).ToString();
    }
}
