using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WithdrawItems : MonoBehaviour
{
    [SerializeField]
    private float animationDuration = 0.2f;
    [SerializeField]
    private float waitBetweenAnimations = 0.1f;
    [SerializeField]
    private float jumpPower = 0f;
    [SerializeField]
    private GameObject character;
    private IEnumerator withdrawCoroutine;
    private CrowdSpawner crowdSpawner;
    [SerializeField]
    private GameObject crowdSpawnerGO;
    public bool isSpawning = true;
    private Material material;

    void Awake()
    {
        crowdSpawner = crowdSpawnerGO.GetComponent<CrowdSpawner>();
        isSpawning = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(character.tag))
        {
            StackItems stackItems;

            if (other.TryGetComponent(out stackItems))
            {
                    if (stackItems.items.Count > 0)
                    {
                        isSpawning = true;
                        crowdSpawner.InstantiateCrowd();
                        withdrawCoroutine = WithdrawItemsCoroutine(stackItems);
                        StartCoroutine(withdrawCoroutine);
                    }

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(character.tag))
        {
            if (withdrawCoroutine != null)
            {
                StopCoroutine(withdrawCoroutine);
            }
            isSpawning = false;
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
            else
            {
                isSpawning =  false;
            }

            yield return new WaitForSeconds(waitBetweenAnimations);
        }
    }

    private void AnimateItemMovement(Transform item)
    {
        item.SetParent(transform);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(item.DOLocalJump(new Vector3(2.5f, 0.5f, 0), jumpPower, 1, animationDuration));
        // TODO: USE QUATERNION;
        sequence.Join(item.DOLocalRotate(Vector3.zero, animationDuration)).OnComplete(
            () => {

                    // item.localPosition = new Vector3(0, 0.5f, 0);
                    // item.localRotation = Quaternion.identity;
                    Destroy(item.gameObject);

                    crowdSpawner.InstantiateUnitToCrowd(material);
            }
        );
    }


    public void SetMaterial(Material m)
    {
        this.material = m;
    }
}
