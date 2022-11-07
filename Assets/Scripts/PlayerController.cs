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
    [SerializeField]
    private GameObject nicknameCanvas;

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

        if (GameManager.Instance.state == GameState.Victory)
        {
            characterAnimator.SetBool("isMoving", false);
            characterAnimator.SetBool("isDancing", true);
        }
        else if (GameManager.Instance.state == GameState.Lose)
        {
            characterAnimator.SetBool("isMoving", false);
            characterAnimator.SetBool("isDefeated", true);
        }
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
        if (GameManager.Instance.state == GameState.Lose || GameManager.Instance.state == GameState.Victory || GameManager.Instance.state == GameState.Lobby)
        {
            return;
        }

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

    public void SetPlayerName(string name)
    {
        BillboardNickname nicknameController = Instantiate(nicknameCanvas, transform).GetComponent<BillboardNickname>();
        nicknameController.SetText(name);
    }
}

