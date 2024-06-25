using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class PopMessage : MonoBehaviour
{
    static PopMessage instance;
    public static PopMessage Instance { get { return instance; } }


    [Header("UI Settings:")]
    [SerializeField] GameObject _panel;
    [SerializeField] TMP_Text messageText;


    private void Awake()
    {
        if (instance == null)
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Actions.EnableMessage += EnablePanel ;
    }

    private void EnablePanel(bool enable)
    {
       _panel.SetActive(enable);
    }


    /// <summary>
    /// Action implemented on message action
    /// </summary>
    /// <param name="message"></param>
    public void SetMessage(string message,float delay  = -1)
    {
        if(delay < 0)
        messageText.text = message;
        else
        {
           StartCoroutine(SetMessageAction(message, delay));
        }
    }


    IEnumerator SetMessageAction(string message,float delay)
    {
        messageText.text = message;
        yield return new WaitForSeconds(delay);
        EnablePanel(false);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
