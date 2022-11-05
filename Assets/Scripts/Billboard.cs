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
        if (GameManager.Instance.state == GameState.Game)
        {
            count = transform.parent.gameObject.transform.childCount - 1;
            if (count > 0)
            {
                counter.gameObject.SetActive(true);
                counter.text = $"{count}";
            }
            else
            {
                counter.gameObject.SetActive(false);
            }

            transform.LookAt(transform.position + new Vector3(cam.forward.x, cam.forward.y, cam.forward.z));
        }
        else
        {
            counter.gameObject.SetActive(false);
        }
        
    }
}
