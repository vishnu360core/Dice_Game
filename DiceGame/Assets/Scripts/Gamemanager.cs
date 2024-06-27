using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System;

public class Gamemanager : MonoBehaviour,PlusMinusInt_Inteface,NumberSliderInterface
{
    private int _amt;
    private float payout_amt;
    private float winchance_amt;
    private float winpay_amt;

    [Header("UI Settings:")]
    [SerializeField] Text PayOut_Txt;
    [SerializeField] Text WinChance_Txt;
    [SerializeField] Text walletText;
    [SerializeField] Slider otherslider;

    [SerializeField] Button Bet_Play;
    [SerializeField] Transform parentTransform;
    [SerializeField] GameObject[] prefabToInstantiate;
    [SerializeField] AudioSource audioSource;
    public AudioSourceData audioSourceData;

    [SerializeField] Animation_Number NumberCounter;

    [Header("Managers:")]
    [SerializeField] WalletConnector walletConnector;
    [SerializeField] PlusMinusInt betContainer;
    [SerializeField] NumberSlider numberSlider;

    float currentBalance;
  
    private void Awake()
    {
        Actions.GetWalletBalance += SetWalletBalance;
        Actions.GetDice += GetDiceAction;

        Actions.DeductAction += DeductResult;
        Actions.CreditAction += CreditResult;

        betContainer.callback = this;
        numberSlider.callback = this;
    }

    private void Start()
    {
        
    }

    #region TRANSACTIONS_OUTCOME

    /// <summary>
    /// Credit Result action
    /// </summary>
    /// <param name="enable"></param>
    private void CreditResult(bool enable)
    {
      if(enable) 
      {
         PLayerBet_End();
      }
      else 
      {
         Actions.EnableMessage(true);
         PopMessage.Instance.SetMessage("Credit Failed");

         Invoke("PLayerBet_End", 3.0f);
      }
    }

    /// <summary>
    /// Deduct Result action
    /// </summary>
    /// <param name="enable"></param>
    private void DeductResult(bool enable)
    {
       if(enable)
        {
            Debug.Log("Deduct successfull");

            Invoke("DiceResultAction", 2.0f);
        }
       else
        {
            Bet_Play.interactable = true;
        }
    }
    #endregion

    /// <summary>
    /// Check the current bet with the balance
    /// </summary>
    /// <param name="bet"></param>
    /// <returns></returns>
    public bool CheckBet(int bet)
    {
        if(bet > currentBalance)
        {
            PopMessage.Instance.SetMessage("InSufficient Balance", 3.0f);
        }

        return bet <= currentBalance;
    }

    private void GetDiceAction(int obj)
    {
        _amt = obj;
    }

    private void SetWalletBalance(float obj)
    {
       walletText.text = " $"+ obj.ToString("F2");
       currentBalance = obj;
    }

    void DiceResultAction()
    {
        //ResponseData responseData = JsonUtility.FromJson<ResponseData>(data);
        //_currentHashCode = responseData.hash;
        //_amt = responseData.result;

        Debug.Log("Deduct successfull 1");

        NumberCounter.Value = _amt;
        StartCoroutine(PlaySoundAndStopAfterDuration());

        DOTween.To(() => otherslider.value, x => otherslider.value = x, _amt, 0.5f).OnComplete(() =>
        {
            if (_amt < NumberSlider.instance.slider.value)
            {
                audioSourceData.ApplyTo(audioSource, "Win");

                float f = (PlusMinusInt.Instance.value * winpay_amt)/* - PlusMinusInt.Instance.value*/;
                Debug.Log("Win Amount >>>>>>>>>>>" + f);
               // WalletConnector.Instance.CreditAmount(f)

                GameObject instantiatedPrefab = Instantiate(prefabToInstantiate[0], parentTransform.position, Quaternion.identity, parentTransform);
                DiceStat diceStat = instantiatedPrefab.GetComponent<DiceStat>();
                diceStat.SetNumber(_amt);


                Actions.EnableMessage(true);
                PopMessage.Instance.SetMessage("You Won $" + f.ToString("F2") + "\n Credit in process");

                Network.Instance.Credit(f);
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


                Actions.EnableMessage(true);
                PopMessage.Instance.SetMessage("You lost $" + f );

                Invoke("PLayerBet_End", 3.0f);
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
        Network.Instance.SetDice();

        Actions.EnableMessage(false);
    }

    public void PayOut_Func()
    {
        Debug.LogWarning("Changing PLAYOUT !!!!");

        payout_amt = 100 / NumberSlider.instance.slider.value;
        winpay_amt = payout_amt * 0.99f;
        PayOut_Txt.text = winpay_amt.ToString();
    }

    public void WinChance_Func()
    {
        Debug.LogWarning("Changing WIN !!!!");

        winchance_amt = Mathf.FloorToInt(NumberSlider.instance.slider.value);
        WinChance_Txt.text = winchance_amt.ToString("F" + (Mathf.Approximately(winchance_amt, Mathf.Floor(winchance_amt)) ? "0" : "2"));
    }

    public void Random_Number_Func()
    {
        //API_Calling.Instance.SendGetRequest(OnSuccess, OnError);
        Bet_Play.interactable = false;

        float f = PlusMinusInt.Instance.value;

        Network.Instance.Deduct(f);

        // DiceResultAction();
    }
}