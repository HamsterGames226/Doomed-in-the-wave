using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class DieScript : MonoBehaviour
{
    public GameObject dieMenu;
    public TextMeshProUGUI textScore;

    public static bool life = true;


    void Update()
    {
        if (!life)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(0);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wave"))
        {
            life = false;
            dieMenu.SetActive(true);
            textScore.text = $"{Math.Floor(TimeSurvive.timeSurvive / 60)}:{(TimeSurvive.timeSurvive % 60f):00}\n\n R - перезапуск";
        }
    }
}
