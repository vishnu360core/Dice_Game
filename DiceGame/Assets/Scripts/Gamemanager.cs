using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System;

public class Gamemanager : MonoBehaviour
{
    private int _amt;
    private float payout_amt;
    private float winchance_amt;
    private float winpay_amt;

    [SerializeField] Text PayOut_Txt;
    [SerializeField] Text WinChance_Txt;
    [SerializeField] Text walletText;
    [SerializeField] Animation_Number NumberCounter;
    [SerializeField] Slider otherslider;

    [SerializeField] Button Bet_Play;
    [SerializeField] Transform parentTransform;
    [SerializeField] GameObject[] prefabToInstantiate;
    [SerializeField] AudioSource audioSource;
    public AudioSourceData audioSourceData;

    [Header("Managers:")]
    [SerializeField] WalletConnector walletConnector;

    [Space]
    public ResponseData response;
    [SerializeField] string _currentHashCode;

    private void Awake()
    {
        Actions.GetWalletBalance += SetWalletBalance;
    }

    private void SetWalletBalance(float obj)
    {
       walletText.text = " $" + obj.ToString("F2");
    }

    void OnSuccess(string data)
    {
        ResponseData responseData = JsonUtility.FromJson<ResponseData>(data);
        _currentHashCode = responseData.hash;
        _amt = responseData.result;
        NumberCounter.Value = _amt;
        StartCoroutine(PlaySoundAndStopAfterDuration());

        DOTween.To(() => otherslider.value, x => otherslider.value = x, _amt, 0.5f).OnComplete(() =>
        {
            if (_amt < NumberSlider.instance.slider.value)
            {
                audioSourceData.ApplyTo(audioSource, "Win");

                float f = (PlusMinusInt.Instance.value * winpay_amt) - PlusMinusInt.Instance.value;
                Debug.Log("Win Amount >>>>>>>>>>>" + f);
               // WalletConnector.Instance.CreditAmount(f)

                GameObject instantiatedPrefab = Instantiate(prefabToInstantiate[0], parentTransform.position, Quaternion.identity, parentTransform);
                DiceStat diceStat = instantiatedPrefab.GetComponent<DiceStat>();
                diceStat.SetNumber(_amt);

                Network.Instance.Credit(_amt);
            }
            else
            {
                audioSourceData.ApplyTo(audioSource, "Lost");

                float f = PlusMinusInt.Instance.value;
                Debug.Log("Loss Amount >>>>>>>>>>>" + f);
                //WalletConnector.Instance.DeductAmount(f);

                GameObject instantiatedPrefab = Instantiate(prefabToInstantiate[1], parentTransform.position, Quaternion.identity, parentTransform);
                DiceStat diceStat = instantiatedPrefab.GetComponent<DiceStat>();
                diceStat.SetNumber(_amt);

                Network.Instance.Deduct(_amt);
            }
        });

        for (int i = 0; i < parentTransform.childCount; i++)
        {
            if (i == 5)
            {
                Destroy(parentTransform.GetChild(i - 5).gameObject);
            }
        }

        //WalletConnector.Instance.ConnectUpdateWallet();
        Invoke("PLayerBet_End", 0.5f);
    }

    void OnError(string error)
    {
        Debug.LogError(error);
    }

    private IEnumerator PlaySoundAndStopAfterDuration()
    {
        audioSourceData.ApplyTo(audioSource, "Dice");
        audioSourceData.loop = false;
        yield return new WaitForSeconds(.5f);
        audioSourceData.loop = true;
        audioSource.Stop();
    }

    private void PLayerBet_End()
    {
        Bet_Play.interactable = true;
    }

    public void PayOut_Func()
    {
        payout_amt = 100 / NumberSlider.instance.slider.value;
        winpay_amt = payout_amt * 0.99f;
        PayOut_Txt.text = winpay_amt.ToString();
    }

    public void WinChance_Func()
    {
        winchance_amt = Mathf.FloorToInt(NumberSlider.instance.slider.value);
        WinChance_Txt.text = winchance_amt.ToString("F" + (Mathf.Approximately(winchance_amt, Mathf.Floor(winchance_amt)) ? "0" : "2"));
    }

    public void Random_Number_Func()
    {
        //API_Calling.Instance.SendGetRequest(OnSuccess, OnError);
        Bet_Play.interactable = false;
    }
}