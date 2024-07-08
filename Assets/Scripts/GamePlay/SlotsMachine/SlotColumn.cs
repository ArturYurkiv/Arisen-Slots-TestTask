using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SlotColumn : MonoBehaviour
{
    [SerializeField] private Transform _slotsItemTransform;
    [SerializeField] private GameObject _slotItemPrefab;
    [SerializeField] private GridLayoutGroup _columnLayoutGroup;

    private int _currentSlot;
    private bool _isSpining = false;


    public void InitializeColumn(SlotData slotData)
    {
        InstantiateSlotItem(slotData.ExtraRaw);
        for (int i = 0; i < slotData.SlotItems.Length; i++)
        {
            InstantiateSlotItem(slotData.SlotItems[i]);
        }
        InstantiateSlotItem(slotData.ExtraRaw);

    }

    private void InstantiateSlotItem(Sprite itemSprite)
    {
        var slotItem = Instantiate(_slotItemPrefab, _slotsItemTransform);
        slotItem.GetComponent<Image>().sprite = itemSprite;
    }

    public (bool, int) GetColumnStatus() => (_isSpining, _currentSlot);

    public void SpinColumn(bool win, int idxReward, SlotData slotData, int shiftMultiplier)
    {
        StartCoroutine(StartSpenColumn(win, idxReward, slotData, shiftMultiplier));
    }

    private IEnumerator StartSpenColumn(bool win, int idxReward, SlotData slotData, int shiftMultiplier)
    {
        _isSpining = true;

        var randomShiftCount = Random.Range(slotData.MinStartShiftCount * shiftMultiplier, slotData.MaxStartShiftCount * shiftMultiplier);

        int randomValue = win
            ? (randomShiftCount - (randomShiftCount % slotData.SlotWinCombinations.Length)) + (slotData.SlotWinCombinations.Length + (idxReward - _currentSlot))
            : randomShiftCount;

        for (int i = 0; i < randomValue; i++)
        {
            float slotIndex = Mathf.Abs(_slotsItemTransform.transform.localPosition.y / _columnLayoutGroup.cellSize.y);

            if (slotIndex == slotData.SlotItems.Length - 1)
            {
                float yPosition = 0 - (slotIndex * _columnLayoutGroup.cellSize.y);
                _slotsItemTransform.transform.localPosition = new Vector2(_slotsItemTransform.transform.localPosition.x, _slotsItemTransform.transform.localPosition.y - yPosition);
            }
            else
            {
                _slotsItemTransform.transform.localPosition = new Vector2(_slotsItemTransform.transform.localPosition.x, _slotsItemTransform.transform.localPosition.y - _columnLayoutGroup.cellSize.y);
            }

            yield return new WaitForSeconds(slotData.TimeInterval);
        }

        _currentSlot = -Mathf.RoundToInt(_slotsItemTransform.transform.localPosition.y / _columnLayoutGroup.cellSize.y);
        _isSpining = false;
    }

}
