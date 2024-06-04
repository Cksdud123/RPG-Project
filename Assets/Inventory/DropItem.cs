using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropItem : MonoBehaviour
{
    [SerializeField] public Slot DropSlot;
    [SerializeField] public Slider DropCountSlider;
    [SerializeField] TextMeshProUGUI txt_ButtonText;
    [SerializeField] TextMeshProUGUI txt_DropItemName;

    [HideInInspector] public int dropAmount = 0;
    private void OnEnable()
    {
        txt_DropItemName.text = DropSlot.ItemInSlot.Name.ToString();
        DropCountSlider.maxValue = DropSlot.AmountInSlot;
    }
    private void Update()
    {
        SetButtonText();
    }
    public void SetButtonText()
    {
        // 드롭할 아이템의 수량을 슬라이더의 값으로 설정
        // 소숫점의 값을 인트로 변경하기 위해 RoundToInt를 사용
        dropAmount = Mathf.RoundToInt(DropCountSlider.value);

        // 버튼 텍스트 업데이트
        txt_ButtonText.text = $"{dropAmount}";
    }
    public void minusButton()
    {
        DropCountSlider.value -= 1;
        // 버튼 텍스트 업데이트
        SetButtonText();
    }
    public void plusButton()
    {
        DropCountSlider.value += 1;
        // 버튼 텍스트 업데이트
        SetButtonText();
    }
    public void DropButon()
    {
        DropSlot.DropItems(DropSlot);
    }
    public void CancelButton()
    {
        DropCountSlider.value = 0;
        txt_ButtonText.text = "";
        gameObject.SetActive(false);
    }
}