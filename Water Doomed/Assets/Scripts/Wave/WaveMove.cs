using UnityEngine;
using UnityEngine.EventSystems;

public class WaveMove : MonoBehaviour
{
    public float speedWave = 2f;
    public Vector2 moveDirection = Vector2.left;

    public void FixedUpdate()
    {
        if (DieScript.life)
        {
            transform.Translate(moveDirection * speedWave * Time.deltaTime);
        }
    }
}
