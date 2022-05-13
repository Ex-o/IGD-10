using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stacker : MonoBehaviour
{
    [SerializeField] private Transform playerVisualTransform, kidModelTransform;
    [SerializeField] private GameObject stackedCubePrefab;

    private float savedTime = 0;
    private float delay = 0.2f;
     
    void OnTriggerStay(Collider other){
     
        if( (Time.time - savedTime) > delay ) {
            savedTime = Time.time;
            
            if (other.CompareTag("Lava"))
            {
                RemoveCube();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CollectableCube"))
        {
            var newCubeNumber = other.transform.childCount + 1;

            AddCube(newCubeNumber);
          
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Diamond"))
        {
            UIManager.Instance.AnimateCoins(other.transform, new Vector3(5, 5, 5));
            UIManager.Instance.UpdateDiamond();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Lava"))
        {
            if( (Time.time - savedTime) > delay )
            {
                savedTime = Time.time;
                RemoveCube();
            }
        }

        if (other.CompareTag("Multiplier"))
        {
            var multi = other.transform.name;
            int factor = Convert.ToInt32(multi.Substring(0, multi.Length - 1));
            var bonusPerLevel = SceneManager.GetActiveScene().buildIndex + 1;
            bonusPerLevel *= 2;
            UIManager.Instance.AddBonus(factor * bonusPerLevel);
        }
    }

    private void RemoveCube()
    {
        var idx = playerVisualTransform.childCount - 1;
        if (idx > 1)
        {
            Destroy(playerVisualTransform.GetChild(idx).gameObject);
        }
    }
    private void AddCube(int cubeNumber)
    {
        kidModelTransform.position = new Vector3(kidModelTransform.position.x, kidModelTransform.position.y + cubeNumber, kidModelTransform.position.z);

        var currentHeight = playerVisualTransform.childCount - 2;

        for (int i = 0; i < cubeNumber; i++)
        { 
            var newCubePosition = new Vector3(transform.position.x, transform.position.y + currentHeight, transform.position.z);
            var newCube = Instantiate(stackedCubePrefab, newCubePosition, Quaternion.identity);
            newCube.transform.SetParent(playerVisualTransform);
            currentHeight++;
        }
    }
}
