using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BillboardNickname : MonoBehaviour
{
    private Transform cam;
    public TextMeshProUGUI nickname;


    void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    public void SetText(string name)
    {
        nickname.text = name;
    }

    void LateUpdate()
    {
        transform.LookAt((transform.position + new Vector3(cam.forward.x / 100, cam.forward.y / 100, cam.forward.z / 100)));
    }
}
