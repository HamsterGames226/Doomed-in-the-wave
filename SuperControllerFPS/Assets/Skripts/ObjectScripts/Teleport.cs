using System.Linq;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public string namePortal;

    private void OnCollisionEnter(Collision collision)
    {
        Teleport allTeleportScripts = FindObjectsByType<Teleport>(FindObjectsSortMode.None).FirstOrDefault(s => s.namePortal == namePortal && s != this);

        if (allTeleportScripts != null)
        {

            collision.transform.position = allTeleportScripts.transform.position + allTeleportScripts.transform.forward * 2f;
        }
    }
}
