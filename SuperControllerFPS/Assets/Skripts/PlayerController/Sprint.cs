using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class Sprint : MonoBehaviour
{
    [Header("Настройки спринта")]
    public float speedUp = 1.5f;
    public float maxStamina = 1f; //Время выносливости в секундах
    public float waitForRegen = 0.25f;
    public float multRegenStamina = 0.75f;

    [Header("Настройки UI")]
    public Slider[] sliders; //correct
    public Image[] fillSliders;

    [Header("Для зума")]
    public Camera camera;

    private float currectStamina = 0f;
    private Rigidbody rb;
    private float waitForRegenCorrect = 0f; // надо чтобы восстановление стамины было дольше
    private PlayerMovement playerMovement;
    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
        currectStamina = maxStamina;            
    }
    void Update()
    {
        if(currectStamina >= maxStamina)
        {
            for(int i = 0; i < sliders.Length; i++)
            {
                sliders[i].gameObject.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < sliders.Length; i++)
            {
                if (!sliders[i].IsActive())
                {
                    sliders[i].gameObject.SetActive(true);
                }
                fillSliders[i].color = Color.Lerp(Color.red, Color.white, currectStamina / maxStamina);
                sliders[i].value = currectStamina / maxStamina;
            }
        }

        if (Input.GetKey(KeyCode.LeftShift) && currectStamina > 0 && rb.linearVelocity.magnitude > 1)
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, 85f, Time.deltaTime * 15f);

            waitForRegenCorrect = 0;
            currectStamina -= Time.deltaTime;
            playerMovement.multiplySpeed = speedUp;

        }
        else
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, 60f, Time.deltaTime * 15f);

            if (currectStamina < maxStamina)
            {
                if(waitForRegenCorrect >= waitForRegen)
                {
                    currectStamina += Time.deltaTime * multRegenStamina;
                }
                else
                {
                    waitForRegenCorrect += Time.deltaTime;
                }
            }
            playerMovement.multiplySpeed = 1f;
        }
    }
}
