using UnityEngine;
using UnityEngine.UI;

using TMPro;

public interface NumberSliderInterface
{
    public void PayOut_Func();
    public void WinChance_Func();
}

public class NumberSlider : MonoBehaviour
{

    [Header("UI Settings:")]
    [SerializeField] Text Point_Text;
    //[SerializeField] rollOverText rollOverText;
    [SerializeField] TMP_Text rollOverText;
    [SerializeField] public Slider slider;


   //SerializeField] Gamemanager _gamemanager;
    public static NumberSlider instance;

    public NumberSliderInterface callback;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        //rollOverText.onValueChanged.AddListener(UpdateSliderValue);
        slider.onValueChanged.AddListener(UpdaterollOverTextValue);

        rollOverText.text = "50";

        UpdateSliderValue(rollOverText.text);
    }

    private void UpdateSliderValue(string value)
    {
        if (float.TryParse(value, out float floatValue))
        {
            floatValue = Mathf.Clamp(floatValue, slider.minValue, slider.maxValue);

            slider.value = floatValue;

            Point_Text.text = value.ToString();

            callback.PayOut_Func();
            callback.WinChance_Func();
        }
    }

    private void UpdaterollOverTextValue(float value)
    {
        // Set the rollOverText value to the slider value
        rollOverText.text = value.ToString();
        UpdateSliderValue();

        UpdateSliderValue(rollOverText.text);
    }

    public void UpdateSliderValue()
    {
        if (!string.IsNullOrEmpty(rollOverText.text))
        {
            if (!int.TryParse(rollOverText.text, out int intValue))
            {
                rollOverText.text = ((int)slider.value).ToString();
            }
            else
            {
                intValue = Mathf.Clamp(intValue, 0, 98);

                rollOverText.text = intValue.ToString();
            }
        }
    }
}