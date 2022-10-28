using UnityEngine;
using TMPro;

public class Billboard : MonoBehaviour
{
    private Transform cam;
    private int count;
    public TextMeshProUGUI counter;


    void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    void LateUpdate()
    {
        count = transform.parent.gameObject.transform.childCount - 1;
        if (count > 0)
        {
            counter.enabled = true;
            counter.text = $"{count}";
        }
        else
        {
            counter.enabled = false;
        }
        
        transform.LookAt(transform.position + new Vector3(cam.forward.x, cam.forward.y, cam.forward.z));
    }
}
