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
        character = GameObject.FindGameObjectWithTag("Player");
        transform.position = new Vector3(character.transform.position.x, character.transform.position.y + height, character.transform.position.z - rearDistance);
        transform.rotation = Quaternion.LookRotation(character.transform.position - transform.position);
    }

    void Update()
    {
        CameraMove();
    }

    private void CameraMove()
    {
        currentVector = new Vector3(character.transform.position.x, character.transform.position.y + height, character.transform.position.z - rearDistance);
        transform.position = Vector3.Lerp(transform.position, currentVector, returnSpeed * Time.deltaTime);
    }
}
