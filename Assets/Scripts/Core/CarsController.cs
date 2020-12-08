using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CarsController : MonoBehaviour
{
    [SerializeField] GameObject carPrefab;
    [SerializeField] public int carsAmount;
    [SerializeField] List<Road> roads;
    private int currentRoadIndex = 0;
    public int iteration = 0;
    private bool raceModeEnabled;
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
        roads[currentRoadIndex].gameObject.SetActive(false);
        currentRoadIndex++;
        if (currentRoadIndex >= roads.Count) currentRoadIndex = 0;
        roads[currentRoadIndex].gameObject.SetActive(true);
    }
    private void BestOfOne(Car fatherCar = null){
        Car bestCar;
        if (fatherCar ==null) bestCar = generatedCars.First();
        else bestCar = fatherCar;

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
    public void EnableRaceMode(){
        raceModeEnabled = true;
        roads[currentRoadIndex].EnableFinishLine();
    }
    public void DisableRaceMode(){
        raceModeEnabled = false;
        roads[currentRoadIndex].DisableFinishLine();
    }
    public bool SwitchRaceMode(){
        if (raceModeEnabled){
            DisableRaceMode();
            return false;
        }
        else{
            EnableRaceMode();
            return true;
        }
    }
    public void RaceBest(Car triggerCar){
        if (!raceModeEnabled) return;
        if (triggerCar.GetFitness() == 0) return;
        iteration++;
        GameManager.Instance.canvasController
            .SetBestTime(triggerCar.GetFitness(), false);
        GameManager.Instance.canvasController
            .SetBestColor(triggerCar.GetComponent<Renderer>().material.color);
        BestOfOne(triggerCar);
        ResetCars();
    }
    public void SortCars(){
        generatedCars.Sort();
        generatedCars.Reverse();
    }
    private void Update() {
        int aliveCarsAmout = generatedCars.Where(car=>car.carMover.alive).ToList().Count;
        GameManager.Instance.canvasController.SetAlive(aliveCarsAmout);
        GameManager.Instance.canvasController.SetIteration(iteration);
        if(aliveCarsAmout==0){
            iteration++;
            SortCars();
            GameManager.Instance.canvasController
                .SetBestColor(generatedCars.First().GetComponent<Renderer>().material.color);
            GameManager.Instance.canvasController
                .SetBestTime(generatedCars.First().GetFitness(),true);
            if (useBestOfOne)
                BestOfOne();
            else
                BestOfTwo();
            ResetCars();

        }
    }
    public void ResetCars(){
        foreach(var car in generatedCars){
            car.transform.position = transform.position;
            car.transform.rotation = carPrefab.transform.rotation;
            car.carMover.alive = true;
            car.carMover.lifeTime = 0;
        }
    }
}
