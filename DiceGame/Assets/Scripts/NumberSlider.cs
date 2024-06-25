using UnityEngine;
using UnityEngine.UI;

public class NumberSlider : MonoBehaviour
{
    [SerializeField] Text Point_Text;
    [SerializeField] InputField inputField;
    [SerializeField] public Slider slider;
    [SerializeField] Gamemanager _gamemanager;
    public static NumberSlider instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        inputField.onValueChanged.AddListener(UpdateSliderValue);
        slider.onValueChanged.AddListener(UpdateInputFieldValue);

        inputField.text = "50";

        UpdateSliderValue(inputField.text);
    }

    private void UpdateSliderValue(string value)
    {
        if (float.TryParse(value, out float floatValue))
        {
            floatValue = Mathf.Clamp(floatValue, slider.minValue, slider.maxValue);

            slider.value = floatValue;

            Point_Text.text = value.ToString();

            _gamemanager.PayOut_Func();
            _gamemanager.WinChance_Func();
        }
    }

    private void UpdateInputFieldValue(float value)
    {
        // Set the inputField value to the slider value
        inputField.text = value.ToString();
    }

    public void UpdateSliderValue()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            if (!int.TryParse(inputField.text, out int intValue))
            {
                inputField.text = ((int)slider.value).ToString();
            }
            else
            {
                intValue = Mathf.Clamp(intValue, 0, 98);

                inputField.text = intValue.ToString();
            }
        }
    }
}