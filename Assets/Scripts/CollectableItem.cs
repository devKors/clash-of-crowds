using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    private bool isCollected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isCollected)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            CollectItems collectItems;

            if (other.TryGetComponent(out collectItems))
            {
                collectItems.CollectItem(this.transform);
                isCollected = true;
            }
        }
    }
}
