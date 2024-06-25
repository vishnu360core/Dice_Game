using UnityEngine;
using UnityEngine.EventSystems;

public class PointerHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject Message;

    public void OnPointerEnter(PointerEventData eventData)
    {
       Message.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Message.SetActive(false);
    }
}