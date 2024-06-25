using System.Collections;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class API_Calling : MonoBehaviour
{
    private static API_Calling instance;
    public static API_Calling Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SendGetRequest(Action<string> onSuccess, Action<string> OnError)
    {
        StartCoroutine(SendGR(onSuccess, OnError));
    }

    IEnumerator SendGR(Action<string> onSucess, Action<string> OnError)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://thecrypto360.com/dice.php"))
        {

            webRequest.SetRequestHeader("Access-Control-Allow-Credentials", "true");
           // webRequest.SetRequestHeader("Access-Control-Allow-Headers", "Accept, X-Access-Token, X-Application-Name, X-Request-Sent-Time");
            webRequest.SetRequestHeader("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            webRequest.SetRequestHeader("Access-Control-Allow-Origin", "*");
            webRequest.SetRequestHeader("Access-Control-Allow-Origin", "*");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
                onSucess?.Invoke(webRequest.downloadHandler.text);
            else
                OnError?.Invoke(webRequest.downloadHandler.error);
        }
    }
}

[System.Serializable]
public class ResponseData
{
    public int result;
    public string hash;
    public int blockNumber;
}