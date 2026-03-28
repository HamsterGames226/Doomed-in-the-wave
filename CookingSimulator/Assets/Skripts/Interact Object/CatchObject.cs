using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CatchObject : MonoBehaviour
{
    //[SerializeField] private GameObject EnteractibleUI;

    public float pickupRange = 3f;  // Дистанция взятия
    public Transform holdPos;       // Точка удержания предмета
    private GameObject heldObj;     // Ссылка на текущий предмет
    private Rigidbody heldRb;       // Rigidbody предмета

    public static bool cathObj = false;
    public static bool showCatchUI = false;


    private float rotationX;
    private float rotationY;

    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out RaycastHit hit) && heldObj == null)
        {
            if(hit.collider.gameObject.tag == "Cath" && Vector3.Distance(this.gameObject.transform.position, hit.collider.gameObject.transform.position) <= pickupRange)
            {
                showCatchUI = true;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    TryPickUp(hit);
                }
            }
            else
            {
                showCatchUI = false;
            }
        }
        else if(heldObj != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                DropObject();
            }
            MoveObject();
        }
        else
        {
            showCatchUI = false;
        }
    }

    void TryPickUp(RaycastHit hit)
    {
        rotationX = hit.normal.x;
        rotationY = hit.normal.y;

        cathObj = true;
        heldObj = hit.collider.gameObject;
        heldRb = heldObj.GetComponent<Rigidbody>();

        heldRb.useGravity = false;      // Отключаем гравитацию
        heldRb.linearDamping = 10;               // Увеличиваем сопротивление, чтобы не дергался
        heldRb.constraints = RigidbodyConstraints.FreezeRotation; // Запрет вращения

        heldObj.transform.parent = holdPos; // Привязываем к руке
    }

    void DropObject()
    {
        cathObj = false;
        heldRb.useGravity = true;
        heldRb.linearDamping = 1;
        heldRb.constraints = RigidbodyConstraints.None;

        heldObj.transform.parent = null; // Отвязываем от камеры
        heldObj = null;
    }
    void MoveObject()
    {
        if (Input.GetMouseButton(0))
        {
            rotationX -= Input.GetAxis("Mouse X");
            rotationY -= Input.GetAxis("Mouse Y");
            heldRb.gameObject.transform.rotation = Quaternion.Euler(0, rotationX, rotationY);
        }

        if (Vector3.Distance(heldObj.transform.position, holdPos.position) > 0.1f)
        {
            Vector3 moveDir = (holdPos.position - heldObj.transform.position);
            heldRb.AddForce(moveDir * 150f);
        }
    }
}
