using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MinimapScript : MonoBehaviour
{
    [SerializeField]
    public Transform player;

    private void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.z = transform.position.z;
        transform.position = newPosition;
    }
}
