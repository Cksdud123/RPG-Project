using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{

    public void initSlot()
    {
        // ������ ������ ���ٸ� �η� �ʱ�ȭ
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        // ������ ������ �ִٸ� �� ������ �ʱ�ȭ
    }
}
