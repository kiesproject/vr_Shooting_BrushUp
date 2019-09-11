using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirFighter_Baseposs : MonoBehaviour
{
    private GameObject Player;

    private void Start()
    {
        Player = GameManager.instance.Player;
    }

    private void FixedUpdate()
    {
        transform.position = Player.transform.position;
    }
}
