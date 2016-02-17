using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour
{

    public Slider slider;
    public Text text;


    void OnEnable()
    {
        slider.onValueChanged.AddListener(ChangeValue);
        ChangeValue(slider.value);
    }
    void OnDisable()
    {
        slider.onValueChanged.RemoveAllListeners();
    }

    void ChangeValue(float value)
    {
        text.text = value.ToString("n"+0);
    }


}
