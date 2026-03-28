using UnityEngine;

public class Slippery : MonoBehaviour
{
    [SerializeField] private float decelerationForce = 50f;

    private float defDecelerationForce = 0;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerMovement playerMovement))
        {
            if (defDecelerationForce == 0)
            {
                defDecelerationForce = playerMovement.deceleration;
            }
            playerMovement.deceleration = defDecelerationForce;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerMovement playerMovement))
        {
            playerMovement.deceleration = defDecelerationForce;
            defDecelerationForce = 0;
        }
    }
}
