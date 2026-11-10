
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Referencia al jugador
    public GameObject player;

    // Distancia entre la cámara y el jugador
    private Vector3 offset;

    void Start()
    {
        if (player != null)
        {
            offset = transform.position - player.transform.position;
        }
    }

    void LateUpdate()
    {
        // Seguir al jugador solamente si existe
        if (player != null)
        {
            transform.position = player.transform.position + offset;
        }
    }
}
