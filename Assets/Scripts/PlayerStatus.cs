using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class PlayerStatus : MonoBehaviour
{
    // �÷��̾� �Ӵ�
    [Header("Player Status")]
    public int PlayerMoney = 2000;
    public TextMeshProUGUI MoneyText;

    [Header("Player Health")]
    public HealthBar healthBar;
    public float CurrentHealth;
    public Animator animator;

    [SerializeField] public float MaxHealth;

    [HideInInspector] public CharacterController characterController;
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
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
    }
    public void UpdateMonney()
    {
        MoneyText.text = PlayerMoney.ToString();
    }
}
