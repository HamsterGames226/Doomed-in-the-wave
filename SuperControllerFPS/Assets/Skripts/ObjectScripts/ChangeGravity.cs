using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ChangeGravity : MonoBehaviour
{
    public float gravityIntensity = 9.81f;

    public Volume globalVolume;
    private Vignette vignette;
    private void Start()
    {
        globalVolume.profile.TryGet(out vignette);
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.TryGetComponent(out Rigidbody rb) && other.gameObject.tag == "Player")
        {
            vignette.active = true;
            vignette.color.value = Color.orange;
            rb.useGravity = false;
            rb.AddForce(Vector3.up * gravityIntensity, ForceMode.Acceleration);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Rigidbody rb) && other.gameObject.tag == "Player")
        {
            vignette.active = false;
            rb.useGravity = true    ;
        }
    }
}
