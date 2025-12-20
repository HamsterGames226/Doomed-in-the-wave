using System;
using UnityEngine;

public class ConrollerWallHP : MonoBehaviour
{
    [NonSerialized] public float maxHP = 100f;

    [NonSerialized] public float correctHP = 0f;

    private SpriteRenderer spriteRenderer;
    void Awake()
    {
        correctHP = maxHP;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wave"))
        {
            Destroy(collision.gameObject);

            correctHP -= 10;

            if(correctHP > 0)
            {
                spriteRenderer.color = new Color(1, correctHP / maxHP, correctHP / maxHP);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}
