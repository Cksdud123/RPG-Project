using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    // �÷��̾� �Ӵ�
    public int PlayerMoney = 2000;
    public TextMeshProUGUI MoneyText;

    private void Update()
    {
        UpdateMonney();
    }
    public void UpdateMonney()
    {
        MoneyText.text = PlayerMoney.ToString();
    }
}
