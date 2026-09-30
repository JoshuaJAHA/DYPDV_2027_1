using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperacionesMatematicas : MonoBehaviour
{
    private float num1 = 3.0f;

    public float num2 = 5.0f;

    void Start()
    {
        float num3 = num1 + num2;

        Debug.Log("La suma es: " + num3);
    }
}