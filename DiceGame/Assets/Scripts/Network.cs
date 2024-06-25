using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Network : MonoBehaviour
{
    static Network instance;
    public static Network Instance { get { return instance; } }

    [DllImport("__Internal")]
    private static extern void WebSocketInit(string url);

    [DllImport("__Internal")]
    private static extern void Send(int index,string message);

    private void Awake()
    {
       if (instance == null)
            instance = this;
    }


    private void Start()
    {
        WebSocketInit("ws://localhost:5050");
        WebSocketInit("ws://localhost:5040");
        WebSocketInit("ws://localhost:5030");
        WebSocketInit("ws://localhost:5020");
    }

    void ConnectCallBack(int index)
    {
        switch (index) 
        {
            case 0:  Debug.Log("Data connection Open"); SetDice(); break;
            case 1: Debug.Log("Wallet connection Open");break;
            case 2: Debug.Log("Credit connection Open");break;
            case 3: Debug.Log("Deduct connection Open"); break;
        }
    }

    void ReceiveMessage(string data)
    {
        string[] parts;
        string message;
        string index;

        parts = data.Split('@');
        message = parts[0];
        index = parts[1];

        if (message == "Ping")
            return;

        switch(int.Parse(index))
        {
            case 0: 
                ResponseData response = JsonUtility.FromJson<ResponseData>(message);
                Actions.GetDice(response.result);

                break;
            case 1:
                float _balance = float.Parse(message);
                Actions.GetWalletBalance(_balance);

                break;
            case 2:
                Actions.Credit(message);
           
                break;
            case 3:
                Actions.Deduct(message);    

                break;
        }
    }

    public void SetDice()
    {
        Send(0, "Set");
    }

    #region WALLET

    public void BalanceOf(string address)
    {
        Send(1,address);
    }

    #endregion

    #region TRANSACTIONS

    public void Credit(float amount)
    {
        Send(2,amount.ToString());  
    }

    public void Deduct(float amount)
    {
        Send(3, amount.ToString());
    }

    #endregion
}
