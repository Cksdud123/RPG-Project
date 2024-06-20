using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropItem : MonoBehaviour
{
    [SerializeField] public Slot DropSlot;
    [SerializeField] TextMeshProUGUI txt_ButtonText;
    [SerializeField] TextMeshProUGUI txt_DropItemName;
    [SerializeField] public Slider DropCountSlider;

    [HideInInspector] public int dropAmount = 0;
    [HideInInspector] public PlayerStatus playerStatus;
    private void Awake()
    {
        playerStatus = FindObjectOfType<PlayerStatus>();
    }
    private void OnEnable()
    {
        if (DropSlot != null && DropSlot.ItemInSlot != null)
        {
            txt_DropItemName.text = DropSlot.ItemInSlot.Name;
            DropCountSlider.maxValue = DropSlot.AmountInSlot;  // maxValue를 여기서 업데이트
            DropCountSlider.value = 0;  // 슬라이더 값을 초기화
            SetButtonText();  // 버튼 텍스트 업데이트
        }
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
        DropSlot = null;
        gameObject.SetActive(false);
    }

    public void SellItemButton()
    {
        // 현재 버릴 총량을 구함
        int MoneyAmount = dropAmount * DropSlot.ItemInSlot.Price;

        // PlayerStatus에 저장
        playerStatus.PlayerMoney += MoneyAmount;
        playerStatus.UpdateMonney();

        if (DropSlot.AmountInSlot == 0) return;

        // 아이템 삭제 로직 추가
        if (dropAmount > 0)
        {
            if (dropAmount >= DropSlot.AmountInSlot)
            {
                DropSlot.ItemInSlot = null;
                DropSlot.AmountInSlot = 0;

                DropSlot.SellAmount.text = DropSlot.AmountInSlot.ToString();
                DropSlot.SellAmount.gameObject.SetActive(false);
                DropSlot.SellIcon.texture = null;
                DropSlot.SellIcon.gameObject.SetActive(false);
            }
            else
            {
                DropSlot.AmountInSlot -= dropAmount;
                DropSlot.SellAmount.text = DropSlot.AmountInSlot.ToString();
            }

            DropSlot.UpdateSlot();
        }

        // DropItem의 Slider 최대 갯수를 줄임
        DropCountSlider.maxValue = DropSlot.AmountInSlot;
    }

}