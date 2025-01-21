using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject Chair;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 randomSpawnPos = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-5f, 5f));
            Vector3 randomSpawnRot = Vector3.up * Random.Range(0, 360);

            GameObject newChair = (GameObject)Instantiate(Chair, randomSpawnPos, Quaternion.Euler(randomSpawnRot));
            newChair.transform.parent = transform;
        }
    }
}
