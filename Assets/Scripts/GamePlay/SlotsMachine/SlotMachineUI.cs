using System;
using TMPro;
using UnityEngine;

public class SlotMachineUI : MonoBehaviour
{
    public event Action OnSpin;
    public event Action OnDecreaseRate;
    public event Action OnIncreaseRate;
    public event Action OnMaxBet;
    public event Action OnMinBet;

    [SerializeField] private TextMeshProUGUI _playerCoin;
    [SerializeField] private TextMeshProUGUI _currentBet;

    public void Spin() => OnSpin?.Invoke();

    public void DecreaseRate() => OnDecreaseRate?.Invoke();

    public void IncreaseRate() => OnIncreaseRate?.Invoke();

    public void MaxBet() => OnMaxBet?.Invoke();

    public void MinBet() => OnMinBet?.Invoke();

    public void UpdateCoinPanel(int coin) => _playerCoin.text = coin.ToString();

    public void UpdateCurrentBet(int bet) => _currentBet.text = bet.ToString();
}
