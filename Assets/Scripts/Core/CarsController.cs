using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CarsController : MonoBehaviour
{
    [SerializeField] GameObject carPrefab;
        [SerializeField] public int carsAmount;
    [SerializeField] List<GameObject> roads;
    private int currentRoadIndex = 0;
    public int iteration = 0;
    public bool useBestOfOne = true;
    List<Car> generatedCars;
    System.Random mutationRandomizer;
    
    private void Start() {
        mutationRandomizer = new System.Random();
        generatedCars = new List<Car>();
        for(int i=0;i<carsAmount;i++){
            GameObject carObj = Instantiate(carPrefab,this.transform);
            carObj.GetComponent<CarBrain>().random = new System.Random(i);
            generatedCars.Add(carObj.GetComponent<Car>());
        }
    }
    public void SwitchRoad()
    {
        ResetCars();
        roads[currentRoadIndex].SetActive(false);
        currentRoadIndex++;
        if (currentRoadIndex >= roads.Count) currentRoadIndex = 0;
        roads[currentRoadIndex].SetActive(true);
    }
    private void BestOfOne(){
        Car bestCar = generatedCars.First();
        foreach(var car in generatedCars){
            if (car == bestCar) continue;
            car.carBrain.ReplaceBrains(bestCar.carBrain.brain);
            car.carBrain.MutateBrains(new System.Random(mutationRandomizer.Next()));
        }
    }
    private void BestOfTwo(){
        if(generatedCars.Count < 2){ 
            Debug.LogWarning("You need at least 2 cars to use BO2 method");
            return;
        }
        List<Car> bestCars = generatedCars.Take(2).ToList();
        SNN combinedBrains = SNN.Combine(bestCars[0].carBrain.brain,bestCars[1].carBrain.brain);
        foreach(var car in generatedCars){
            if (bestCars.Contains(car)) continue;
            car.carBrain.ReplaceBrains(combinedBrains);
            car.carBrain.MutateBrains(new System.Random(mutationRandomizer.Next()));            
        }

    }
    public void SortCars(){
        generatedCars.Sort();
        generatedCars.Reverse();
    }
    private void Update() {
        if(!generatedCars.Any(car=>car.carMover.alive)){
            iteration++;
            SortCars();
            if (useBestOfOne)
                BestOfOne();
            else
                BestOfTwo();
            ResetCars();
        }
    }
     private void ResetCars(){
        foreach(var car in generatedCars){
            car.transform.position = transform.position;
            car.transform.rotation = carPrefab.transform.rotation;
            car.carMover.alive = true;
            car.carMover.lifeTime = 0;
        }
    }
}
