using UnityEngine;

public class ManagerUI : MonoBehaviour
{
    [SerializeField] private GameObject EnteractibleUI;

    void Update()
    {
        if(CatchObject.cathObj == true || CatchObject.showCatchUI == true || Spawner.showSpawnUI == true)
        {
            EnteractibleUI.SetActive(true);
        }
        else
        {
            EnteractibleUI.SetActive(false);
        }
    }
}
