using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DambaInf : MonoBehaviour
{
    [Header("Canvas для позиции окна информации")]
    public Canvas myCanvas;

    [Header("Номер дамбы")]
    public int numDamba;

    [Header("Окно информации")]
    public GameObject infWin;
    public TextMeshProUGUI textNumDamba;
    public Slider SliderHP;

    [Header("Окно прокачки")]
    public GameObject upgradeWin;
    public TextMeshProUGUI upTextNumDamba;
    public Slider upSliderHP;

    private ConrollerWallHP conrollerWallHP;
    public static int сurrentDam = 0;
    void Start()
    {
        conrollerWallHP = GetComponent<ConrollerWallHP>();
    }

    void FixedUpdate()
    {
        if (!upgradeWin.active && сurrentDam != 0)
        {
            сurrentDam = 0;
        }
    }

    private void OnMouseOver()
    {
        if (!infWin.active && !upgradeWin.active)
        {
            infWin.SetActive(true);
            textNumDamba.text = $"Дамба: {numDamba}";
        }
        if (upgradeWin.active)
        {
            infWin.SetActive(false);
        }
        else
        {
            if (Input.GetMouseButtonDown(1))
            {
                upTextNumDamba.text = $"Дамба: {numDamba}";
                сurrentDam = numDamba;
                upSliderHP.value = conrollerWallHP.correctHP / conrollerWallHP.maxHP;
                upgradeWin.SetActive(true);
            }

            SliderHP.value = conrollerWallHP.correctHP / conrollerWallHP.maxHP;
            infWin.transform.position = myCanvas.worldCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, myCanvas.planeDistance));
        }
    }

    void OnMouseExit()
    {
        if (infWin.active)
        {
            infWin.SetActive(false);
        }
    }
}
