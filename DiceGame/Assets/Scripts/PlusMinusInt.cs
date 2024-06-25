using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlusMinusInt : MonoBehaviour
{
    [SerializeField] Text valueText;
    [SerializeField] InputField inputField;
    [SerializeField] public int value;

    public static PlusMinusInt Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        inputField.contentType = InputField.ContentType.IntegerNumber;
        inputField.text = "1";
    }

    void UpdateValueText()
    {
        valueText.text = "Value: " + value.ToString();
        inputField.text = value.ToString();
    }

    public void IncreaseValue()
    {
        value++;
        UpdateValueText();
    }

    public void DecreaseValue()
    {
        value--;
        if (value < 1)
            value = 1;
        UpdateValueText();
    }

    public void IncreaseAllValue()
    {
        value = 50;
        UpdateValueText();
    }

    public void DecreaseAllValue()
    {
        value = 1;
        if (value < 1)
            value = 1;
        UpdateValueText();
    }

    public void SetValue()
    {
        if (int.TryParse(inputField.text, out int inputValue))
        {
            value = inputValue;
            UpdateValueText();
        }
    }

    public void InputValueChange()
    {
        if (!string.IsNullOrEmpty(value.ToString()))
        {
            int intValue;
            if (int.TryParse(value.ToString(), out intValue))
            {
                if (intValue > 50)
                {
                    inputField.text = "50";
                }
            }
        }
    }
}