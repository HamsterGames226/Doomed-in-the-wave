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

    private ConrollerWallHP conrollerWallHP;

    private bool openMenu = false;
    void Start()
    {
        conrollerWallHP = GetComponent<ConrollerWallHP>();
    }

    void Update()
    {

    }

    private void OnMouseOver()
    {
        if (!infWin.active && !openMenu)
        {
            infWin.SetActive(true);
            textNumDamba.text = $"Дамба: {numDamba}";
        }
        if (openMenu)
        {
            infWin.SetActive(false);
        }
        else
        {
            if (Input.GetMouseButtonDown(1))
            {
                openMenu = true;
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
