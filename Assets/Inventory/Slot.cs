using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{

    public void initSlot()
    {
        // 아이템 슬롯이 없다면 널로 초기화
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        // 아이템 슬롯이 있다면 그 값으로 초기화
    }
}
