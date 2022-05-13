using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Turn : MonoBehaviour
{
    [SerializeField] private Transform player;
    private void OnTriggerEnter(Collider other)
    {      
        player.transform.DORotateQuaternion(Quaternion.Euler(transform.rotation.x, 90, transform.rotation.z), 1f);
    }
}