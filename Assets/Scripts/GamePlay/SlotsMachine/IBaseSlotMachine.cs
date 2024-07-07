using System;
using UnityEngine;

public interface IBaseSlotMachine 
{
    public event Action<bool, int> OnSlotStop;

    public void InitializeSlotMachine(SlotData slotData, Transform slotMachineTransforml);

    public void SpinSlotMachine();

}
