using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarSprite;
    [SerializeField] private float reduceSpeed = 10f;

    private float target = 1;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }
    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
        // ü���� ���̴� ���� �ε巴�� ������
        healthBarSprite.fillAmount = Mathf.MoveTowards(healthBarSprite.fillAmount, target, reduceSpeed * Time.deltaTime);
    }
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        target = currentHealth / maxHealth;
    }
}
