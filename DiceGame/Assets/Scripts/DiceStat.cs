using UnityEngine;
using UnityEngine.UI;

public class DiceStat : MonoBehaviour
{
    [SerializeField]  Text numberText;

    public void SetNumber(int number)
    {
        numberText.text = number.ToString();
    }
}