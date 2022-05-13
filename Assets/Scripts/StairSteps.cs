using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairSteps : MonoBehaviour
{
    private Transform stackerTransform, playerVisualTransform;
    
    void Start()
    {
        stackerTransform = GameObject.FindGameObjectWithTag("Stacker").GetComponent<Transform>();
        playerVisualTransform = GameObject.FindGameObjectWithTag("PlayerVisual").GetComponent<Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StackedCube"))
        {
            Debug.Log(transform.name);
            var currentCubeNumber = playerVisualTransform.childCount - 2;

            if(currentCubeNumber >= 2)
            {
                stackerTransform.position += Vector3.up;
                other.transform.parent = null;
            }
            else
            {
                GameManager.Instance.OnGameFinish();
            }
        }
    }
}
