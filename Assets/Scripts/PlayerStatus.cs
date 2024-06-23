using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    // 플레이어 머니
    [Header("Player Status")]
    public int PlayerMoney = 2000;
    public TextMeshProUGUI MoneyText;

    [Header("Player Health")]
    public HealthBar healthBar;
    public float CurrentHealth;
    [SerializeField] public float MaxHealth;

    [Header("Player Level")]
    public ExperienceManager experienceManager;

    [Header("Save & Load")]
    public ItemData[] allItems;
    public Inventory[] inventories;

    public Animator animator;
    // 플레이어 상태
    private void Start()
    {
        healthBar.maxHealth = MaxHealth;
        CurrentHealth = MaxHealth;
    }
    public void PlayerDamage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        healthBar.takeDamage(damageAmount);

        if (CurrentHealth <= 0f)
        {
            //Die();
        }
    }
    public void Die()
    {
        CurrentHealth = 0;

        animator.SetTrigger("Die");
    }
    private void Update()
    {
        UpdateMonney();

        if (Input.GetKeyDown(KeyCode.O)) SavePlayer();

        if (Input.GetKeyDown(KeyCode.P)) LoadPlayer();
    }
    public void UpdateMonney()
    {
        MoneyText.text = PlayerMoney.ToString();
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }
    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        healthBar.health = data.health;
        experienceManager.currentLevel = data.level;
        PlayerMoney = data.money;

        experienceManager.UpdateLevel();

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        transform.position = position;

        // 인벤토리 스크립트를 가지고 있는 갯수만큼 
        for (int i = 0; i < inventories.Length; i++)
        {
            // 저장한 슬롯데이터를 가져와서
            SlotData[] slotDataArray = data.allInventoryData[i];

            for (int j = 0; j < slotDataArray.Length; j++)
            {
                // 각 슬롯의 갯수만큼 할당한 뒤에
                SlotData slotData = slotDataArray[j];
                // 인덱스 처리
                if (slotData.itemDataID == -1)
                {
                    inventories[i].slots[j].ResetSlot();
                }
                else
                {
                    ItemData itemData = GetItemDataByID(slotData.itemDataID);
                    if (itemData != null)
                    {
                        inventories[i].slots[j].ItemInSlot = itemData;
                        inventories[i].slots[j].AmountInSlot = slotData.amount;
                        inventories[i].slots[j].SetSlot();
                    }
                }
            }
        }
    }

    // ID를 기준으로 ItemData를 반환
    public ItemData GetItemDataByID(int id)
    {
        foreach (ItemData item in allItems)
        {
            if (item.ID == id)
                return item;
        }
        return null;
    }
}
