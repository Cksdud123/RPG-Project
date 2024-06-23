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
        Inventory inventory = FindObjectOfType<Inventory>();
        SaveSystem.SavePlayer(this, inventory);
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

        Inventory inventory = FindObjectOfType<Inventory>();

        for (int i = 0; i < data.SaveSlotData.Length; i++)
        {
            if (data.SaveSlotData[i].itemDataID == -1)
            {
                inventory.slots[i].ResetSlot();
            }
            else
            {
                ItemData itemData = GetItemDataByID(data.SaveSlotData[i].itemDataID);
                if (itemData != null)
                {
                    inventory.slots[data.SaveSlotData[i].slotIndex].ItemInSlot = itemData;
                    inventory.slots[data.SaveSlotData[i].slotIndex].AmountInSlot = data.SaveSlotData[i].amount;
                    inventory.slots[data.SaveSlotData[i].slotIndex].SetSlot();
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
