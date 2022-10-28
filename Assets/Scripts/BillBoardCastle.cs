using UnityEngine;
using TMPro;

public class BillBoardCastle : MonoBehaviour
{
    private Transform cam;
    private int count;
    public TextMeshProUGUI counter;
    private CastleController castleController;


    void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        castleController = transform.parent.GetComponent<CastleController>();
    }

    void LateUpdate()
    {
        if (castleController.health > 0)
        {
            counter.enabled = true;
            counter.text = $"{castleController.health}";
        }
        else
        {
            counter.enabled = false;
        }
        transform.LookAt((transform.position + new Vector3(cam.forward.x / 100, cam.forward.y / 100, cam.forward.z / 100)));
    }
}
