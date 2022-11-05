using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject character;
    [SerializeField]
    private float returnSpeed;
    [SerializeField]
    private float height;
    [SerializeField]
    private float rearDistance;
    private Vector3 currentVector;
    [SerializeField]
    private Quaternion rotationQuaternion;
    [SerializeField]
    private Vector3 positionVector;
    private Transform characterTransform;

    void Awake()
    {
        SetCharacter();
    }

    void Update()
    {
        SetCharacter();

        if (GameManager.Instance.state == GameState.Lose || GameManager.Instance.state == GameState.Victory)
        {
            CameraMoveAroundCharacter();
        }
        else
        {
            CameraMove();
        }
        
    }

    private void CameraMove()
    {
        if (character != null)
        {
            currentVector = new Vector3(character.transform.position.x - positionVector.x, character.transform.position.y + height, character.transform.position.z - rearDistance);
            transform.position = Vector3.Lerp(transform.position, currentVector, returnSpeed * Time.deltaTime);
            transform.rotation = rotationQuaternion;
        }
    }

    private void CameraMoveAroundCharacter()
    {
        if (character != null)
        {
            transform.LookAt(character.transform);
            transform.Translate(Vector3.right * Time.deltaTime);
        }
    }

    private void SetCharacter()
    {
        if (character == null)
        {
            character = GameObject.FindGameObjectWithTag("Player");

            if (character != null)
            {
                transform.position = new Vector3(character.transform.position.x + positionVector.x, character.transform.position.y + height, character.transform.position.z - rearDistance);
                transform.rotation = Quaternion.LookRotation(character.transform.position - transform.position);
            }
        }
    }
}
