using System;
using UnityEngine;

public class GamePlayEntryPoint : MonoBehaviour
{
    [SerializeField] private Transform _slotMachineTransform;
    [SerializeField] private SlotData _slotData;
    [SerializeField] private SlotMachineUI _playerInputHeandler;

    private IBaseSlotMachine _slotMachine;
    private IPlayerStorage _playerStorage;

    private int _currentBet;

    public void Inject(IBaseSlotMachine slotMachine, IPlayerStorage playerStorage, SlotData slotData)
    {
        _slotMachine = slotMachine;
        _slotMachine.InitializeSlotMachine(_slotData, _slotMachineTransform);
        _playerStorage = playerStorage;
        _slotData = slotData;
        _currentBet = slotData.MinSpinCost;

        _slotMachine.OnSlotStop += OnslotMachineStop;

    }

    private void Start()
    {
        var player = new PlayerStorage();
        var slotMachine = new SlotMachine();

        Inject(slotMachine, player, _slotData);

        _playerInputHeandler.UpdateCoinPanel(player.GetBalance());
        _playerInputHeandler.UpdateCurrentBet(_currentBet);

    }

    private void SpinSlotMachine()
    {
        if (_playerStorage.SubtractBalance(_currentBet))
        {
            _slotMachine.SpinSlotMachine();
            _playerInputHeandler.UpdateCoinPanel(_playerStorage.GetBalance());
        }
        else
        {
            Debug.Log("Not enough money!!!");
        }
    }

    private void OnslotMachineStop(bool win, int winCombinationIdx)
    {
        _playerStorage.AddBalance((int)(_currentBet * _slotData.SlotWinCombinations[winCombinationIdx].WinningMultiplier));
        _playerInputHeandler.UpdateCoinPanel(_playerStorage.GetBalance());
        Debug.Log("Slot Machine Stop with result: " + win + ". With Idx " + winCombinationIdx);
    }

    private void OnMaxBet()
    {
        _currentBet = _slotData.MaxSpinCost;
        _playerInputHeandler.UpdateCurrentBet(_currentBet);
    }
    private void OnMinBet()
    {
        _currentBet = _slotData.MinSpinCost;
        _playerInputHeandler.UpdateCurrentBet(_currentBet);
    }

    private void OnIncreaseRate()
    {
        _currentBet += 10;
        _playerInputHeandler.UpdateCurrentBet(_currentBet);
    }

    private void OnDecreaseRate()
    {
        _currentBet -= 10;
        _playerInputHeandler.UpdateCurrentBet(_currentBet);
    }
    private void OnEnable()
    {
        _playerInputHeandler.OnSpin += SpinSlotMachine;
        _playerInputHeandler.OnDecreaseRate += OnDecreaseRate;
        _playerInputHeandler.OnIncreaseRate += OnIncreaseRate;
        _playerInputHeandler.OnMinBet += OnMinBet;
        _playerInputHeandler.OnMaxBet += OnMaxBet;
    }

    private void OnDisable()
    {
        _playerInputHeandler.OnSpin -= SpinSlotMachine;
        _playerInputHeandler.OnDecreaseRate -= OnDecreaseRate;
        _playerInputHeandler.OnIncreaseRate -= OnIncreaseRate;
        _playerInputHeandler.OnMinBet -= OnMinBet;
        _playerInputHeandler.OnMaxBet -= OnMaxBet;
    }

}
