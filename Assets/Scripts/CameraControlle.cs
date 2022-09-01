using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlle : MonoBehaviour
{
    [SerializeField] private GameObject character;
    [SerializeField] private float returnSpeed;
    [SerializeField] private float height;
    [SerializeField] private float rearDistance;
    private Vector3 currentVector;

    void Start()
    {
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
