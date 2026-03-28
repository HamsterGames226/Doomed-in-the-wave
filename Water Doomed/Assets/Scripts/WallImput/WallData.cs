using UnityEngine;
using TMPro;
using System.Collections;

public class WallData : MonoBehaviour
{
    public static float[] HP = { 100f, 100f, 100f };

    public static int beton = 4500; 

    public static float[] numHeightLVL = { 0, 0, 0 }; // 10 уровней, а не слайдр таккак лучше сделать нелененую прогрессию апгрейта. Это просто интерсеней 
    public static float[] numStrengthLVL = { 0, 0, 0 }; // В час ночи кажется гениальным, но дробные уровни помогут плавно апгрейдить слайдр уровня. ЭТО ДОЛЖНО РАБОТАТЬ

    public static int[] priceHeightLVL = { 15, 20, 30, 35, 50, 75, 120, 150, 250, 500}; // Досок в игре будет в 2 раза больше чем камня, поэтому цены будут дороже по отношению к дереву, но +- в 1,5 раза
    public static int[] priceStrengthLVL = { 10, 12, 15, 20, 35, 50, 70, 100, 175, 300}; // Пусть игрок не может защищать того, чего нет. Тогда чтобы прокочать силу надо прокачать высоту. Нужен камень для прокачки!

    public static float[] timeBuildHeightLVL = {2.5f, 4f, 6f, 10f, 12f, 15f, 18f, 22.5f, 25f}; // На возведение высоты будет уделяться больше времени чем на защиту в +- 1,3-1,5 раз
    public static float[] timeBuildStrengthLVL = {1.5f, 2.5f ,4f ,6f ,10f ,12f ,15f ,18f ,20f ,22.5f};

    public static float timeWaitHeight = 0f;
    public static float timeWaitStreng = 0f;
    public static float timeWaitheal = 0f;

    [Header("Звуки")]
    public AudioSource audioSource;
    public AudioClip error;
    public AudioClip comlite;

    [Header("Для восстановления  ХП")]
    public GameObject winDoublePrice;
    public TextMeshProUGUI textRes1;
    public TextMeshProUGUI textRes2;


    void Start()
    {
        //transform.position = myCanvas.worldCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, myCanvas.planeDistance));
    }

    void Update()
    {
        
    }

    public void UpgradeHeight()
    {
        if(numHeightLVL[DambaInf.сurrentDam - 1] <= 9)
        {
            if (beton >= priceHeightLVL[(int)numHeightLVL[DambaInf.сurrentDam - 1]])
            {
                beton -= priceHeightLVL[(int)numHeightLVL[DambaInf.сurrentDam - 1]];
                //timeWaitHeight += 
            }
            else
            {
                Debug.Log("Нет денег :(");
            }
        }
        else
        {
            Debug.Log("У чела максимальный апгрейд высоты на " + DambaInf.сurrentDam.ToString());
        }
        numHeightLVL[DambaInf.сurrentDam - 1] += 1;
    }

    public IEnumerator UpgradeHeightWait(float timeWait)
    {
        yield return new WaitForSeconds(2.5f);
    }
}
