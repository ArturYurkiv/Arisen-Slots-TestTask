using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewSlotData", menuName = "Slot Machine/Slot Data")]
public class SlotData : ScriptableObject
{
    public Sprite[] SlotItems;
    public Sprite ExtraRaw;

    public int SlotColumnCount;
    public int MinStartShiftCount;
    public int MaxStartShiftCount;

    public int MinSpinCost;
    public int MaxSpinCost;


    public float WinChance;
    public float TimeInterval;

    public SlotWinCombination[] SlotWinCombinations;


}
