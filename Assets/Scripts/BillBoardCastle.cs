using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BillBoardCastle : MonoBehaviour
{
    private Transform cam;
    private int count;
    public TextMeshProUGUI counter;
    private CastleController castleController;
    [SerializeField]
    private Image cover;


    void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        castleController = transform.parent.GetComponent<CastleController>();
    }

    public void SetCoverColor(Color color)
    {
        cover.color = color;
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
