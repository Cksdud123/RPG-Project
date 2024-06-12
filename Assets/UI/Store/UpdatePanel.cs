using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePanel : MonoBehaviour
{
    public Transform Store;
    public void SetUpdateButton()
    {
        Store.transform.SetSiblingIndex(0);
        Store.gameObject.SetActive(true);

        for (int i = 0; i < Store.parent.childCount; i++)
        {
            Transform sibling = Store.parent.GetChild(i);
            if (sibling != Store)
            {
                sibling.gameObject.SetActive(false);
            }
        }
    }
}
