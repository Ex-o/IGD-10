using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairFinish : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StackedCube"))
        {
            GameManager.Instance.OnGameFinish();
        }
    }
}
