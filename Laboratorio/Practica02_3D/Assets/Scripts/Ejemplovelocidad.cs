using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ejemplovelocidad : MonoBehaviour
{
    public float velocidad = 5.0f;

    void Start()
    {
        Debug.Log("La velocidad es: " + velocidad);
    }
}