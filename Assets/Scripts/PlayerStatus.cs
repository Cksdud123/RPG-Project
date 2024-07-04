using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    // �÷��̾� �Ӵ�
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
    public GameObject OptionPanel;

    public Animator animator;

    // ��ġ ������Ʈ�� ���� ����
    private bool updatePositionFlag = false;
    private Vector3 newPosition;

    // �÷��̾� ����
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
        if(Input.GetKeyDown(KeyCode.Q)) experienceManager.AddExperience(50);

        if (Input.GetKeyDown(KeyCode.Escape)) ActiveOption();

        // �÷��װ� �����Ǿ� ������ ��ġ�� ������Ʈ
        if (updatePositionFlag)
        {
            transform.position = newPosition;
            updatePositionFlag = false;
        }
    }
    public void UpdateMonney()
    {
        MoneyText.text = PlayerMoney.ToString();
    }
    public void ActiveOption()
    {
        if (!OptionPanel.activeInHierarchy)
        {
            Cursor.lockState = CursorLockMode.None;
            OptionPanel.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            OptionPanel.SetActive(false);
        }
    }
    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }
    public void LoadPlayer()
    {
        if(OptionPanel.activeInHierarchy)
        {
            Cursor.lockState = CursorLockMode.Locked;
            OptionPanel.SetActive(false);
        }

        PlayerData data = SaveSystem.LoadPlayer();

        healthBar.health = data.health;
        experienceManager.currentLevel = data.level;
        PlayerMoney = data.money;

        experienceManager.UpdateLevel();

        // ��ġ �����͸� �����ϰ� �÷��׸� Ȱ��ȭ
        newPosition = new Vector3(data.position[0], data.position[1], data.position[2]);
        updatePositionFlag = true;

        // �κ��丮 ��ũ��Ʈ�� ������ �ִ� ������ŭ 
        for (int i = 0; i < inventories.Length; i++)
        {
            // ������ ���Ե����͸� �����ͼ�
            SlotData[] slotDataArray = data.allInventoryData[i];

            for (int j = 0; j < slotDataArray.Length; j++)
            {
                // �� ������ ������ŭ �Ҵ��� �ڿ�
                SlotData slotData = slotDataArray[j];
                // �ε��� ó��
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

    // ID�� �������� ItemData�� ��ȯ
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
