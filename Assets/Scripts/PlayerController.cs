using UnityEngine;
using DigitalRubyShared;


[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField] 
    private float rotateSpeed;
    [SerializeField] 
    private float gravityForce = 20;
    [SerializeField] 
    private float currentAttractionCharacter = 0;
    private FingersJoystickScript fingersJoystickScript;
    private CharacterController characterController;
    private Animator characterAnimator;
    private GameObject[] characters;
    private int characterIndex;

    private void Awake()
    {

        fingersJoystickScript = GameObject.Find("FingersJoystickPrefab").GetComponent<FingersJoystickScript>();
        fingersJoystickScript.JoystickExecuted = JoystickExecuted;

        characters = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            characters[i] = transform.GetChild(i).gameObject;
        }

        characterIndex = 0;
        SetCharacterSkin(characterIndex);

    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        characterAnimator = characters[characterIndex].GetComponent<Animator>();
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

        if (GameManager.Instance.state == GameState.Lose || GameManager.Instance.state == GameState.Victory)
        {
            return;
        }

        if (amount != Vector2.zero)
        {
            if (GameManager.Instance.state == GameState.Lobby)
            {
                GameManager.Instance.SetGameState(GameState.Game);
            }

            MoveCharacter(new Vector3(amount.x, 0, amount.y));
            RotateCharacter(new Vector3(amount.x, 0, amount.y));
            characterAnimator.SetBool("isMoving", true);
        }
        else
        {
            characterAnimator.SetBool("isMoving", false);
        }
    }

    public void SetCharacterSkin(int index)
    {
        foreach (GameObject go in characters)
        {
            go.SetActive(false);
        }

        if (characters.Length > 0)
        {
            characters[index].SetActive(true);
            characterIndex = index;
        }
    }
    public void SetPlayerMaterial(Material material)
    {
        foreach (GameObject go in characters)
        {
            go.GetComponentInChildren<Renderer>().material = material;
        }
    }
}

