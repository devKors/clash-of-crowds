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

    void Start()
    {
        SetCharacter();
    }

    void Update()
    {
        SetCharacter();
        
        CameraMove();
    }

    private void CameraMove()
    {
        if (character != null)
        {
            currentVector = new Vector3(character.transform.position.x, character.transform.position.y + height, character.transform.position.z - rearDistance);
            transform.position = Vector3.Lerp(transform.position, currentVector, returnSpeed * Time.deltaTime);
        }
    }

    private void SetCharacter()
    {
        if (character == null)
        {
            character = GameObject.FindGameObjectWithTag("Player");
            transform.position = new Vector3(character.transform.position.x, character.transform.position.y + height, character.transform.position.z - rearDistance);
            transform.rotation = Quaternion.LookRotation(character.transform.position - transform.position);
        }
        
    }
}
