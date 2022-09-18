using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StackItems : MonoBehaviour
{
    public Transform backpack;
    public Stack<Transform> items;
    public int numOfItems = 0;
    private int numOfAnimatedItems = 0;

    [SerializeField]
    private float itemHeight = 0.2f;
    [SerializeField]
    private float animationDuration = 0.4f;
    [SerializeField]
    private float animationDurationFactor = 0.02f;
    [SerializeField]
    private float jumpPower = 1.2f;
    [SerializeField]
    private float jumpPowerFactor = 0.02f;

    void Start()
    {
        items = new Stack<Transform>();
    }

    public void PushItem(Transform newItem)
    {
        newItem.SetParent(backpack, true);

        float factoredJumpPower = jumpPower - jumpPowerFactor * numOfAnimatedItems;
        float factoredAnimationDuration = animationDuration + animationDurationFactor * numOfAnimatedItems;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(newItem.DOLocalJump(new Vector3(0, itemHeight * numOfAnimatedItems, 0), factoredJumpPower, 1, factoredAnimationDuration)).OnStart(
            () => {
                numOfAnimatedItems++;
            });
        // TODO: USE QUATERNION;
        sequence.Join(newItem.DOLocalRotate(Vector3.zero, factoredAnimationDuration).OnComplete(
            () => {

                    newItem.localPosition = new Vector3(0, itemHeight * numOfItems, 0);
                    newItem.localRotation = Quaternion.identity;

                    numOfItems++;
                    items.Push(newItem);

            }
        ));
    }

    public Transform PopItem()
    {
        if (items.Count > 0)
        {
            numOfAnimatedItems--;
            numOfItems--;
            return items.Pop();
        }

        return null;
    }
}