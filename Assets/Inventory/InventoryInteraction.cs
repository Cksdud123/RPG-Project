using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInteraction : MonoBehaviour
{
    public GameObject PanelInventory;
    public Slot[] slots = new Slot[25];
    private void Awake()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].initSlot();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (PanelInventory.activeSelf)
            {
                // �κ��丮�� Ȱ��ȭ ������ ��, ��Ȱ��ȭ�մϴ�.
                Cursor.lockState = CursorLockMode.Locked;
                PanelInventory.SetActive(false);
                Time.timeScale = 1f;
            }
            else
            {
                // �κ��丮�� ��Ȱ��ȭ ������ ��, Ȱ��ȭ�մϴ�.
                Cursor.lockState = CursorLockMode.None;
                PanelInventory.transform.position = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
                PanelInventory.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }
}
