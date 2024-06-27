using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public interface PlusMinusInt_Inteface
{
    public bool CheckBet(int bet);
}

public class PlusMinusInt : MonoBehaviour
{
    [SerializeField] Text valueText;
    //[SerializeField] InputField inputField;
    [SerializeField] TMP_Text betText;
    [SerializeField] public int value;

    public static PlusMinusInt Instance;

    public PlusMinusInt_Inteface callback;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        //inputField.contentType = InputField.ContentType.IntegerNumber;
        betText.text = "1";
        value = 1;
    }

    void UpdateValueText()
    {
        valueText.text = "Value: " + value.ToString();
        betText.text = value.ToString();
    }

    public void IncreaseValue()
    {
        if (!callback.CheckBet(value + 1))
            return;
        
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
        if (!callback.CheckBet(50))
            return;

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
        if (int.TryParse(betText.text, out int inputValue))
        {
            value = inputValue;
            UpdateValueText();
        }
    }

    public void InputValueChange()
    {
       betText.text += value.ToString();
    }
}