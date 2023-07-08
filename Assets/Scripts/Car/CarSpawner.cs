using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] GameObject carPrefab;
    [SerializeField] int maxCars = 5;
    [SerializeField] int[] randomTimerBounds;
    int currentCars = 0;
    float spawnTimer = 0;
    int spawnTimerMax;

    void Start()
    {
        InstantiateCar();

    }
    void Update()
    {
        if(currentCars < maxCars) {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnTimerMax){
                InstantiateCar();
            }
        }
    }

    public void InstantiateCar() {
        GameObject car = Instantiate(carPrefab, transform.position, transform.rotation);
        car.GetComponent<Car>().decreaseCount = DecreaseCount;
        currentCars++;
        spawnTimer = 0;
        spawnTimerMax = Random.Range(randomTimerBounds[0], randomTimerBounds[1]);
    }

    public void DecreaseCount() {
        currentCars--;
    }
}
