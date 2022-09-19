using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackableItem : MonoBehaviour
{
    [SerializeField]
    private GameObject character;
    public bool isStacked = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isStacked) { return; }

        if (other.CompareTag(character.tag))
        {
            StackItems stackItems;

            if (other.TryGetComponent(out stackItems))
            {
                stackItems.PushItem(this.transform);
                isStacked = true;
            }
        }
    }
}
