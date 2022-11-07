using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Billboard : MonoBehaviour
{
    private Transform cam;
    private int count;
    public TextMeshProUGUI counter;
    public Image cover;
    private bool isColorSet;


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

                if (!isColorSet)
                {
                    SetCoverColor();
                }
                cover.gameObject.SetActive(true);
            }
            else
            {
                counter.gameObject.SetActive(false);
                cover.gameObject.SetActive(false);
            }

            transform.LookAt(transform.position + new Vector3(cam.forward.x, cam.forward.y, cam.forward.z));
        }
        else
        {
            counter.gameObject.SetActive(false);
            cover.gameObject.SetActive(false);

        }
    }

    private void SetCoverColor()
    {
        GameObject parrent = transform.parent.gameObject;
        Color c = parrent.GetComponentInChildren<Renderer>().material.color;
        cover.color = c;

        isColorSet = true;
    }
}
