using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using NativeWebSocket;
using System.Text;
using Thirdweb;
using Thirdweb.Examples;

public class WalletConnector : MonoBehaviour
{
    [Header("WalletConnect:")]
    [SerializeField] Prefab_ConnectWallet connectWallet;

    string _currentAddress;

    #region CONTRACT
    string abi = @" [
	{
		""inputs"": [],
		""name"": ""placebid"",
		""outputs"": [],
		""stateMutability"": ""payable"",
		""type"": ""function""
	},
	{
		""inputs"": [
			{
				""internalType"": ""address payable"",
				""name"": ""_to"",
				""type"": ""address""
			},
			{
				""internalType"": ""uint256"",
				""name"": ""_amount"",
				""type"": ""uint256""
			}
		],
		""name"": ""rewardFunc"",
		""outputs"": [],
		""stateMutability"": ""nonpayable"",
		""type"": ""function""
	},
	{
		""inputs"": [
			{
				""internalType"": ""address payable"",
				""name"": ""_to"",
				""type"": ""address""
			},
			{
				""internalType"": ""uint256"",
				""name"": ""_amount"",
				""type"": ""uint256""
			}
		],
		""name"": ""withdrawFunc"",
		""outputs"": [],
		""stateMutability"": ""nonpayable"",
		""type"": ""function""
	},
	{
		""inputs"": [],
		""name"": ""checkBalance"",
		""outputs"": [
			{
				""internalType"": ""uint256"",
				""name"": """",
				""type"": ""uint256""
			}
		],
		""stateMutability"": ""view"",
		""type"": ""function""
	}
]";

    Contract contract;
    #endregion

    private void Start()
    {
        Actions.Credit += CreditAction;
        Actions.Deduct += DeductAction;

        connectWallet.onConnected.AddListener(GetWalletAddressAndBalance);

        var sdk = ThirdwebManager.Instance.SDK;
        contract = sdk.GetContract("0xd7059957411ad31a0453bba8de7371D0b9f096d5", abi);
    }

    public void GetWalletAddressAndBalance(string address)
    {
        _currentAddress = address;

		RequestBalance();
    }

    public void RequestBalance()
    {
        Network.Instance.BalanceOf(_currentAddress);
    }

    private async void CreditAction(string wei)
    {
		TransactionResult result = null;


        try
		{
			 result  = await contract.Write("rewardFunc",_currentAddress,wei);

			Debug.LogWarning("Credit >>" + result.ToString());

			Actions.EnableMessage(false);
			Actions.CreditAction(true);

			RequestBalance();	
		}
		catch(Exception ex) 
		{
            Debug.LogWarning("Credit >>" + ex.Message);

			if (ex.Message == "repriced")
			{
                Actions.EnableMessage(false);
                Actions.CreditAction(true);
                RequestBalance();

				return;
            }

            Actions.EnableMessage(false);
			Actions.CreditAction(false);

            RequestBalance();
        } 
    }

    private async void DeductAction(string wei)
    {
		Actions.EnableMessage(true);
		PopMessage.Instance.SetMessage("Deduct in process");

		TransactionResult result = null;


        try
        {
		    result = await contract.Write("placebid",new TransactionRequest() { value = wei,gasLimit = "100000" });

            Debug.LogWarning("Deduct >>" + result.ToString());

            Actions.EnableMessage(false);
            Actions.DeductAction(true);
            RequestBalance();
        }
        catch(Exception ex) 
        {

            Debug.LogWarning("Deduct >>" + ex.Message);

			if(ex.Message == "repriced")
			{
                Actions.EnableMessage(false);
                Actions.DeductAction(true);
                RequestBalance();

				return;
            }

            Actions.EnableMessage(false);
			Actions.DeductAction(false);
            RequestBalance();
        }
    }
}