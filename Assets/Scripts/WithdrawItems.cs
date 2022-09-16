using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WithdrawItems : MonoBehaviour
{
    private IEnumerator withdrawCoroutine;
    [SerializeField]
    private float animationDuration = 0.2f;
    [SerializeField]
    private float waitBetweenAnimations = 0.1f;
    [SerializeField]
    private float jumpPower = 0f;
    [SerializeField]
    private GameObject unitSpawner;
    [SerializeField]
    private GameObject character;
    private UnitSpawnerController unitSpawnerController;


    void Start()
    {
        unitSpawnerController = (UnitSpawnerController)unitSpawner.GetComponent(typeof(UnitSpawnerController));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(character.tag))
        {
            StackItems stackItems;

            if (other.TryGetComponent(out stackItems))
            {
                withdrawCoroutine = WithdrawItemsCoroutine(stackItems);
                StartCoroutine(withdrawCoroutine);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(character.tag))
        {
            StopCoroutine(withdrawCoroutine);
        }
    }

    IEnumerator WithdrawItemsCoroutine(StackItems stackItems)
    {
        while (true)
        {
            Transform item = stackItems.PopItem();

            if (item != null)
            {
                AnimateItemMovement(item);
            }

            yield return new WaitForSeconds(waitBetweenAnimations);
        }
    }

    private void AnimateItemMovement(Transform item)
    {
        item.SetParent(transform);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(item.DOLocalJump(new Vector3(0, 0.5f, 0), jumpPower, 1, animationDuration));
        // TODO: USE QUATERNION;
        sequence.Join(item.DOLocalRotate(Vector3.zero, animationDuration)).OnComplete(
            () => {

                    item.localPosition = new Vector3(0, 0.5f, 0);
                    item.localRotation = Quaternion.identity;

                    unitSpawnerController.SpawnUnit();
            }
        );
    }
}
