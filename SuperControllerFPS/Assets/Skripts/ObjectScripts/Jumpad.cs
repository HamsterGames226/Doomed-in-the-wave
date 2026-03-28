using UnityEngine;

public class Jumpad : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerMovement playerMovement))
        {
            playerMovement.Jump(transform.up, jumpForce);
        }
    }
}
