using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject SpawnObject;

    public static bool showSpawnUI = false;

    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.tag == "Spawner" && CatchObject.cathObj == false)
            {
                showSpawnUI = true;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    SpawnObj(hit);
                }
            }
            else
            {
                showSpawnUI = false;
            }
        }
        else
        {
            showSpawnUI = false;
        }
    }

    void SpawnObj(RaycastHit hit)
    {
        for (int i = 0; i < 10; i++)
        {
            Instantiate(SpawnObject, hit.collider.gameObject.transform.position + new Vector3(0, 5, 0), Quaternion.identity);

        }
    }
}
