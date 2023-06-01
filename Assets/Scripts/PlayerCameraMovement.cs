using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraMovement : MonoBehaviour
{
    public Transform Player;
    public bool IsFreezed = false;

    private float smoothing = 0.1f;
    private Vector3 offset = new Vector3(0, 0, -6);

    private void FixedUpdate()
    {
        if (IsFreezed) return;

        var playerPos = Player.position;
        var currentPos = Vector3.Lerp(transform.position, playerPos + offset, smoothing);
        transform.position = currentPos;
    }
}
