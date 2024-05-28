using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Slot[] slots = new Slot[25];
    private void Awake()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].initSlot();
        }
    }
}
