using UnityEngine;

public class SpeedUp : MonoBehaviour
{
    [SerializeField] private float speedUp;
    [SerializeField] private float accelerationUp = 40;

    private float defSpeed = 0;
    private float defAcceleration;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerMovement playerMovement))
        {
            if(defSpeed == 0)
            {
                defSpeed = playerMovement.maxSpeed;
                defAcceleration = playerMovement.acceleration;
            }
            playerMovement.acceleration = accelerationUp;
            playerMovement.maxSpeed = speedUp;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerMovement playerMovement))
        {
            playerMovement.maxSpeed = defSpeed;
            playerMovement.acceleration = defAcceleration;
            defSpeed = 0;
        }
    }
}
