using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Actions
{
    public static Action<string> Credit;
    public static Action<string> Deduct;

    public static Action<float> GetWalletBalance;

    public static Action<int> GetDice;

    public static Action<bool> EnableMessage;

    public static Action<bool> DeductAction;
    public static Action<bool> CreditAction;
}
