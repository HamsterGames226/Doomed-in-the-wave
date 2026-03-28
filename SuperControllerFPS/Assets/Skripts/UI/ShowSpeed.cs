using UnityEngine;
using TMPro;
using System;

public class ShowSpeed : MonoBehaviour
{
    private Rigidbody rb;

    public TextMeshProUGUI textSpeed;

    public int maxSpeed = 0;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if(Math.Round(rb.linearVelocity.magnitude * 3.6f, 0) > maxSpeed)
        {
            maxSpeed = (int)Math.Round(rb.linearVelocity.magnitude * 3.6f, 0);
        }
        textSpeed.text = $"{Math.Round(rb.linearVelocity.magnitude * 3.6f, 0)} κμ/χ ({Math.Round(rb.linearVelocity.magnitude, 0)} μ/c) \n"  + $"Μΰκρ: {maxSpeed} κμ/χ ({Math.Round(maxSpeed /3.6f,0)} μ/ρ)";

        if (Input.GetKeyDown(KeyCode.Q))
        {
            maxSpeed = 0;
        }
    }
}
