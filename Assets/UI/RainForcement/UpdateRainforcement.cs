using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateRainforcement : MonoBehaviour
{
    Slot slot;

    [SerializeField] TextMeshProUGUI ItemNameTxt;
    [SerializeField] TextMeshProUGUI SucRateTxt;
    [SerializeField] TextMeshProUGUI RainforceCost;

    private void Awake()
    {
        slot = GetComponentInChildren<Slot>();
        SetRainforcement();
    }
    private void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (slot != null) SetRainforcement();
    }
    private void SetRainforcement()
    {
        if (slot.ItemInSlot == null)
        {
            ItemNameTxt.text = "아이템 없음";
            SucRateTxt.text = "-";
            RainforceCost.text = "-";
            return;
        }
        else
        {
            ItemNameTxt.text = slot.ItemInSlot.Name;
            SucRateTxt.text = "100";
            RainforceCost.text = "100";
        }
    }
}
