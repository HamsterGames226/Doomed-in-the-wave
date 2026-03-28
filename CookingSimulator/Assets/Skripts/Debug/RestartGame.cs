using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public Rigidbody playerRB;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) || this.gameObject.transform.position.y < -10)
        {
            playerRB.linearVelocity = Vector3.zero;
            playerRB.gameObject.transform.position = new Vector3(0, 2, 0);
        }
    }
}
