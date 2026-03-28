using UnityEngine;
using System.Collections;

public class Crouch : MonoBehaviour
{
    [Header("Ссылки")]
    public Rigidbody rb;
    private PlayerMovement playerMovement;

    [Header("Настройки роста")]
    private Vector3 standScale = new Vector3(1f, 1f, 1f);
    private Vector3 crouchScale = new Vector3(1f, 0.5f, 1f);
    public float duration = 0.2f;

    [Header("Настройки подката")]
    public float minSpeedForSlide = 6f;
    public float slideSpeedLimit = 25f;
    public float slideCooldown = 1.0f;
    public float slideDrag = 0.2f;
    public float slopeForce = 15f;

    [Header("Настройки выхода из подката")]
    public float slideExitImpulse = 10f;

    private Coroutine crouchRoutine;
    private bool isSliding = false;
    private bool isCrouching = false;
    private float lastSlideTime;

    private float defaultDrag;
    private float defaultDeceleration;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();

        // Сохраняем начальное значение, даже если это линейное замедление новой версии Unity
        defaultDrag = rb.linearDamping;
        if (playerMovement != null) defaultDeceleration = playerMovement.deceleration;
    }

    void Update()
    {
        bool inputPressed = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C);
        bool inputKeyDown = Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.C);

        // 1. Вход в присед или подкат
        if (inputKeyDown && !isCrouching)
        {
            // Проверка скорости и кулдауна
            bool canSlide = rb.linearVelocity.magnitude > minSpeedForSlide &&
                            IsGrounded() &&
                            Time.time > lastSlideTime + slideCooldown;

            if (canSlide)
            {
                StartCoroutine(SlideProcess());
            }
            else
            {
                StartCrouch();
            }
        }

        // 2. Вставание (если кнопка отпущена)
        // Важно: проверяем !isSliding, чтобы Update не мешал корутине подката, пока она сама не решит завершиться
        if (!inputPressed && isCrouching && !isSliding)
        {
            StopCrouchAndStandUp();
        }
    }

    void FixedUpdate()
    {
        if (isSliding)
        {
            HandleSlidePhysics();
        }
    }

    private void StartCrouch()
    {
        isCrouching = true;
        if (crouchRoutine != null) StopCoroutine(crouchRoutine);
        crouchRoutine = StartCoroutine(LerpCrouch(crouchScale, -0.5f));
    }

    private void StopCrouchAndStandUp()
    {
        // ПРОВЕРКА ПОТОЛКА
        // Если над головой препятствие - мы не можем встать.
        if (Physics.Raycast(transform.position, Vector3.up, 2f))
        {
            // ФИКС: Если мы были в подкате и врезались в "низкий стол",
            // мы должны отключить режим подката, чтобы игрок остался просто в приседе.
            // Иначе isSliding останется true навечно, и игрок застрянет.
            if (isSliding)
            {
                isSliding = false;
                rb.linearDamping = defaultDrag;
                if (playerMovement != null) playerMovement.deceleration = defaultDeceleration;
            }
            return;
        }

        // Если мы были в состоянии скольжения перед тем как встать - даем импульс
        if (isSliding)
        {
            ApplyExitImpulse();
        }

        isCrouching = false;
        isSliding = false;

        // Возвращаем физику
        rb.linearDamping = defaultDrag;
        if (playerMovement != null) playerMovement.deceleration = defaultDeceleration;

        if (crouchRoutine != null) StopCoroutine(crouchRoutine);
        crouchRoutine = StartCoroutine(LerpCrouch(standScale, 0.5f));
    }

    private IEnumerator SlideProcess()
    {
        isSliding = true;
        isCrouching = true;
        lastSlideTime = Time.time;

        rb.linearDamping = slideDrag;

        if (playerMovement != null)
        {
            // Обновляем дефолтное значение на случай, если оно менялось извне
            defaultDeceleration = playerMovement.deceleration;
            playerMovement.deceleration = 0f;
        }

        ApplySlideImpulse();

        if (crouchRoutine != null) StopCoroutine(crouchRoutine);
        crouchRoutine = StartCoroutine(LerpCrouch(crouchScale, -0.5f));

        float timer = 0;
        while (timer < 2.5f)
        {
            timer += Time.deltaTime;

            // Прерывание подката: слишком медленно, отпустили кнопку или нажали прыжок
            if (rb.linearVelocity.magnitude < minSpeedForSlide / 2f && timer > 0.3f) break;
            if ((!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.C)) && timer > 0.3f) break;
            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()) break;

            yield return null;
        }

        StopCrouchAndStandUp();
    }

    private void ApplySlideImpulse()
    {
        Vector3 direction = rb.linearVelocity.normalized;
        if (direction.magnitude < 0.1f) direction = transform.forward;

        float currentSpeed = rb.linearVelocity.magnitude;
        float boost = Mathf.Clamp(currentSpeed + 5f, minSpeedForSlide, slideSpeedLimit);

        // Используем VelocityChange для мгновенного изменения скорости без учета массы
        rb.linearVelocity = direction * boost;
    }

    private void ApplyExitImpulse()
    {
        Debug.Log("Выход из подката: Импульс!");
        Vector3 direction = rb.linearVelocity.normalized;
        direction.y = 0;
        direction.Normalize();

        if (direction.magnitude < 0.1f) direction = transform.forward;

        rb.AddForce(direction * slideExitImpulse, ForceMode.Impulse);
    }

    private void HandleSlidePhysics()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.2f))
        {
            Vector3 slopeDir = Vector3.ProjectOnPlane(Vector3.down, hit.normal).normalized;
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            if (slopeAngle > 5f && slopeAngle < 50f)
            {
                rb.AddForce(slopeDir * slopeForce, ForceMode.Acceleration);
            }
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    IEnumerator LerpCrouch(Vector3 targetS, float offset)
    {
        float elapsed = 0;
        Vector3 startS = transform.localScale;

        // Сохраняем начальную позицию по Y, чтобы точно знать, где пол
        float initialY = transform.position.y;

        while (elapsed < duration)
        {
            float lastPercent = elapsed / duration;
            elapsed += Time.deltaTime;
            float currentPercent = Mathf.Min(elapsed / duration, 1f);

            // 1. Плавный масштаб
            transform.localScale = Vector3.Lerp(startS, targetS, currentPercent);

            // 2. ФИКС ПРИЖАТИЯ К ЗЕМЛЕ:
            // Мы вычисляем, насколько "поднялся" низ модели из-за скейла, 
            // и опускаем объект ровно на это расстояние.
            // Для стандартного капсуля Unity (высота 2) смещение равно (scaleDiff * 1.0)
            float currentHeightChange = (startS.y - transform.localScale.y);

            // Вместо накопления frameOffset, мы просто корректируем позицию относительно земли
            // offset в вашем коде был -0.5f при приседании, что как раз равно половине потери роста
            float targetYOffset = (currentPercent * offset);

            // Используем MovePosition для физической стабильности
            rb.MovePosition(new Vector3(rb.position.x, initialY + targetYOffset, rb.position.z));

            yield return null;
        }

        transform.localScale = targetS;
    }
}

// Старые комментарии сохранены ниже:
//using UnityEngine;
//using System.Collections;

//public class Сrouch : MonoBehaviour
//{
//    public Rigidbody rb;

//    private Vector3 standScale = new Vector3(1f, 1f, 1f);
//    private Vector3 crouchScale = new Vector3(1f, 0.5f, 1f);

//    public float duration = 0.2f; // Длительность анимации приседания
//    private Coroutine crouchRoutine;
//    private bool EndCorutine = true;
//    private void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//    }
//    void Update()
//    {
//        if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.C)) && EndCorutine == true)
//        {
//            if (rb.linearVelocity.magnitude > 10 && Physics.Raycast(transform.position, Vector3.down, 1.1f))
//            {
//                Debug.Log("Скольжение началось");
//                if (rb.linearVelocity.magnitude / 2 < 10)
//                {
//                    rb.AddForce(transform.forward * rb.linearVelocity.magnitude / 1.25f, ForceMode.VelocityChange);
//                }
//                else if (rb.linearVelocity.magnitude / 2 < 25 && rb.linearVelocity.magnitude >10)
//                {
//                    rb.AddForce(transform.forward * rb.linearVelocity.magnitude / 2, ForceMode.VelocityChange);
//                }
//                else
//                {
//                    rb.AddForce(transform.forward * 25, ForceMode.VelocityChange);
//                }

//            }
//            if (crouchRoutine != null) StopCoroutine(crouchRoutine);

//            crouchRoutine = StartCoroutine(DoCrouch(crouchScale, -0.5f));
//        }
//        else if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.C)))
//        {
//            if (crouchRoutine != null) StopCoroutine(crouchRoutine);
//            crouchRoutine = StartCoroutine(DoCrouch(standScale, 0.5f));
//        }
//    }

//    IEnumerator DoCrouch(Vector3 targetScale, float posOffset)
//    {
//        EndCorutine = false;
//        if (targetScale == standScale)
//        {
//            yield return new WaitWhile(() => rb.linearVelocity.magnitude > 10f);
//        }

//        Vector3 startScale = transform.localScale;
//        Vector3 startPos = transform.position;
//        Vector3 targetPos = new Vector3(startPos.x, startPos.y + posOffset, startPos.z);

//        float elapsed = 0;

//        while (elapsed < duration)
//        {
//            elapsed += Time.deltaTime;
//            float percent = elapsed / duration;

//            // Плавно меняем масштаб и позицию одновременно
//            transform.localScale = Vector3.Lerp(startScale, targetScale, percent);
//            transform.position = Vector3.Lerp(startPos, targetPos, percent);

//            yield return null; // Ждем следующего кадра
//        }

//        // Гарантируем точные финальные значения
//        transform.localScale = targetScale;
//        EndCorutine = true;
//    }
//}