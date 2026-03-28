using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    public PlayerMovement playerMovement;

    private float timer = 0f;

    void Update()
    {
        if(playerMovement.multiplySpeed > 1 && Physics.Raycast(playerMovement.gameObject.transform.position, Vector3.down, 1.1f))
        {
            HandleHeadBob(0.23f, 9f + playerMovement.maxSpeed / 40f);

        }
        else
        {
            Vector3 targetPos = new Vector3(0, 0.66f, 0f);
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * 5f);
            if (Vector3.Distance(transform.localPosition, targetPos) < 0.01f) timer = 0;
        }
    }


    void HandleHeadBob(float intensity, float frequency)
    {
        if (intensity <= 0.01f)
        {
            timer = 0;
            transform.localPosition = Vector3.Lerp(transform.position, Vector3.zero, Time.deltaTime * 5f);
            return;
        }

        timer += Time.deltaTime * frequency;

        float posX = Mathf.Cos(timer * 0.5f) * intensity; // ¬лево-вправо
        float posY = Mathf.Sin(timer) * intensity + 0.66f;        // ¬верх-вниз

        transform.localPosition = new Vector3(posX, posY, 0);
    }
}
