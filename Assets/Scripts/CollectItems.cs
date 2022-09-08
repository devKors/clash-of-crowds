using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CollectItems : MonoBehaviour
{
    public Transform backpack;
    private int numOfItems = 0;
    private int numOfAnimatedItems = 0;

    [SerializeField]
    private float itemHeight = 0.2f;
    [SerializeField]
    private float animationDuration = 0.4f;
    [SerializeField]
    private float animationDurationFactor = 0.02f;
    [SerializeField]
    private float jumpPower = 1.5f;
    [SerializeField]
    private float jumpPowerFactor = 0.02f;


    public void CollectItem(Transform newItem)
    {
        newItem.SetParent(backpack, true);

        float factoredJumpPower = jumpPower - jumpPowerFactor * numOfAnimatedItems;
        float factoredAnimationDuration = animationDuration + animationDurationFactor * numOfAnimatedItems;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(newItem.DOLocalJump(new Vector3(0, itemHeight * numOfAnimatedItems, 0), factoredJumpPower, 1, factoredAnimationDuration)).OnStart(
            () => {
                numOfAnimatedItems++;
            });
        sequence.Join(newItem.DOLocalRotate(Vector3.zero, factoredAnimationDuration).OnComplete(
            () => {

                    newItem.localPosition = new Vector3(0, itemHeight * numOfItems, 0);
                    newItem.localRotation = Quaternion.identity;

                    numOfItems++;
            }
        ));
    }
}