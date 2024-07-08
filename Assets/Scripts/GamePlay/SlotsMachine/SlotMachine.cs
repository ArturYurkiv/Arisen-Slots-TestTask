using Slots.Game.Heallpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMachine : IBaseSlotMachine
{
    public event System.Action<bool, int> OnSlotStop;

    private List<SlotColumn> _slotsColumns;

    private SlotData _slotData;

    private Coroutines _coroutines;

    public void InitializeSlotMachine(SlotData slotData, Transform slotMachineTransform)
    {
        _coroutines = new GameObject("[COROUTINES]").AddComponent<Coroutines>();
        _slotsColumns = new List<SlotColumn>();
        _slotData = slotData;
        InitSlotColumn(slotData, slotMachineTransform);
    }

    public void SpinSlotMachine()
    {
        var spinResult = GetSpinResult();

        for (int i = 0; i < _slotsColumns.Count; i++)
        {
            _slotsColumns[i].SpinColumn(spinResult.Item1, spinResult.Item2, _slotData, i+1);
        }

        _coroutines.StartCoroutine(CheckSlotsStatus(result =>
        {
            OnSlotStop?.Invoke(result, spinResult.Item2);

        }));
    }

    private void InitSlotColumn(SlotData slotData, Transform slotMachineTransform)
    {
        var columnPrefab = Resources.Load<SlotColumn>("SlotColumnPrefab");

        for (int i = 0; i < slotData.SlotColumnCount; i++)
        {
           var column = Object.Instantiate(columnPrefab, slotMachineTransform);
            column.InitializeColumn(slotData);
            _slotsColumns.Add(column);
        }
    }

    private (bool, int) GetSpinResult()
    {
        var randomWinChance = Random.Range(0, 100);

        if (randomWinChance < _slotData.WinChance)
        {
            var randomWinCombination = Random.Range(0, 100);

            for (int i = 0; i < _slotData.SlotWinCombinations.Length; i++)
            {
                if (randomWinCombination <= _slotData.SlotWinCombinations[i].RewardChance)
                {
                    return (true, i);
                }
            }
            return (false, 0);

        }
        else
        {
            return (false, 0);
        }
    }

    private IEnumerator CheckSlotsStatus(System.Action<bool> callback)
    {
        bool allStopped;

        do
        {
            allStopped = true;

            foreach (var slot in _slotsColumns)
            {
                if (slot.GetColumnStatus().Item1)
                {
                    allStopped = false;
                    break;
                }
            }

            yield return null; 
        } while (!allStopped);

        bool allIndicesEqual = true;
        int firstIndex = _slotsColumns[0].GetColumnStatus().Item2;

        foreach (var slot in _slotsColumns)
        {
            if (slot.GetColumnStatus().Item2 != firstIndex)
            {
                allIndicesEqual = false;
                break;
            }
        }

        callback(allIndicesEqual);
    }

}
