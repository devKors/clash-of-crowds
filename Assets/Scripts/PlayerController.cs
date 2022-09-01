using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRubyShared;


[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float gravityForce = 20;
    [SerializeField] private float currentAttractionCharacter = 0;
    [SerializeField] private FingersJoystickScript fingersJoystickScript;
    private CharacterController characterController;
    private Animator characterAnimator;

    private void Awake()
    {
        fingersJoystickScript.JoystickExecuted = JoystickExecuted;
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        characterAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        GravityHandler();
    }

    private void GravityHandler()
    {
        if (!characterController.isGrounded)
        {
            currentAttractionCharacter -= gravityForce * Time.deltaTime;
        }
        else
        {
            currentAttractionCharacter = 0;
        }
    }

    private void MoveCharacter(Vector3 moveDirection)
    {
        moveDirection = moveDirection * moveSpeed;
        moveDirection.y = currentAttractionCharacter;

        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void RotateCharacter(Vector3 moveDirection)
    {
        if (characterController.isGrounded)
        {
            if (Vector3.Angle(transform.forward, moveDirection) > 0)
            {
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, moveDirection, rotateSpeed, 0);
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
        }
    }

    private void JoystickExecuted(FingersJoystickScript script, Vector2 amount)
    {

        if (amount != Vector2.zero)
        {
            MoveCharacter(new Vector3(amount.x, 0, amount.y));
            RotateCharacter(new Vector3(amount.x, 0, amount.y));
            characterAnimator.SetBool("isMoving", true);
        }
        else
        {
            characterAnimator.SetBool("isMoving", false);
        }
    }
}

