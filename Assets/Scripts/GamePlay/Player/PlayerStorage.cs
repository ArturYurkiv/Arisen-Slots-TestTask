using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStorage : IPlayerStorage
{
    private const string BalanceKey = "PlayerBalance";

    private int _startBalance = 200;

    public int GetBalance() => PlayerPrefs.GetInt(BalanceKey, _startBalance);

    public void AddBalance(int amount)
    {
        var currentBalance = GetBalance();
        currentBalance += amount;
        PlayerPrefs.SetInt(BalanceKey, currentBalance);
        PlayerPrefs.Save();
    }

    public bool SubtractBalance(int amount)
    {
        var currentBalance = GetBalance();
        if (currentBalance >= amount)
        {
            currentBalance -= amount;
            PlayerPrefs.SetInt(BalanceKey, currentBalance);
            PlayerPrefs.Save();

            return true;
        }
        return false;
    }
    public void ResetBalance()
    {
        PlayerPrefs.SetInt(BalanceKey, _startBalance);
        PlayerPrefs.Save();
    }

}
