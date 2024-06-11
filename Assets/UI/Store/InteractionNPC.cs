using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionNPC : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txt_item;
    [SerializeField] private GameObject ActivePanel;

    private bool isPanelActive = false;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivePanel.gameObject.SetActive(false);
            txt_item.gameObject.SetActive(true);
        }
    }
}
