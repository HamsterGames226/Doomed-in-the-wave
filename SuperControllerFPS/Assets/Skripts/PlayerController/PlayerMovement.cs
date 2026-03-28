using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.SceneView;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float sensitivityX = 1;

    [Header("Параметры, связанные с скоростью")]
    [SerializeField] public float maxSpeed = 10f;       // Скорость
    [SerializeField] public float acceleration = 40f;   // Разгон
    [SerializeField] public float deceleration = 50f;   // Тормоз

    [Header("Параметры, связанные с пржком")]
    [SerializeField] private float jumpForce = 5f;

    [Header("Настройки Банихопа")]
    [SerializeField] public bool enableBunnyHop = true; // Включить банихоп
    [SerializeField] public float airControl = 0.8f;    // Контроль в воздухе (множитель ускорения)

    private GameObject bodyMove;

    [HideInInspector] public float multiplySpeed = 1;

    private Vector3 moveDirection;
    private Rigidbody rb;
    private Vector3 inputDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        bodyMove = this.gameObject;
    }

    void Update()
    {
        bodyMove.transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
        inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetKeyDown(KeyCode.Space) && Physics.Raycast(transform.position, Vector3.down, 1.1f) && this.gameObject.transform.localScale.y >= 1)
        {
            Jump(Vector3.up, jumpForce);
        }
    }

    void FixedUpdate()
    {
        // Проверка земли для банихопа
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        if (this.gameObject.transform.localScale.y >= 1 || rb.linearVelocity.magnitude < 10)
        {
            moveDirection = (transform.forward * inputDirection.z + transform.right * inputDirection.x) * maxSpeed * multiplySpeed * this.gameObject.transform.localScale.y;

            // БАНИХОП: Если мы в воздухе и летим быстрее макс скорости - сохраняем текущую скорость как целевую
            if (enableBunnyHop && !isGrounded && rb.linearVelocity.magnitude > moveDirection.magnitude && moveDirection.magnitude > 0)
            {
                moveDirection = moveDirection.normalized * rb.linearVelocity.magnitude;
            }
        }
        else
        {
            moveDirection = new Vector3(0, 0, 0);
        }

        float currentStep;

        if (enableBunnyHop && !isGrounded)
        {
            // БАНИХОП: В воздухе используем airControl, если есть ввод. Если ввода нет — трение 0.
            if (inputDirection.magnitude > 0)
            {
                // Применяем ускорение для стрейфов
                currentStep = acceleration * multiplySpeed * airControl;
            }
            else
            {
                // Убираем замедление, чтобы сохранить инерцию
                currentStep = 0f;
            }
        }
        else
        {
            // Стандартная логика (земля или банихоп выключен)
            // Если угол между тем, куда мы едем, и тем, куда жмем, больше 90 градусов — включаем торможение
            currentStep = Vector3.Dot(rb.linearVelocity.normalized, (moveDirection / (maxSpeed * multiplySpeed * this.gameObject.transform.localScale.y)).normalized) > 0.5f ? acceleration * multiplySpeed : deceleration;
        }

        rb.linearVelocity = Vector3.MoveTowards(
            rb.linearVelocity, //Текущее значение
            new Vector3(moveDirection.x, rb.linearVelocity.y, moveDirection.z), // Значение цели
            currentStep * Time.fixedDeltaTime // Максимальная скорость
        );
    }

    public void Jump(Vector3 direction, float force)
    {
        Debug.Log("Jump: " + direction.ToString());
        rb.AddForce(direction.normalized * force, ForceMode.Impulse);
    }
}