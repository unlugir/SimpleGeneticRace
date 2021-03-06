﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Car : MonoBehaviour, IComparable
{
    public CarMover carMover;
    public CarBrain carBrain;
    private void Start() {
        carBrain = GetComponent<CarBrain>();
        carMover = GetComponent<CarMover>();
        GetComponent<Renderer>().material.color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }
    public float GetFitness(){
        return carMover.lifeTime;
    }
    public int CompareTo(object o){
        Car c = (Car)o;
        return this.carMover.lifeTime.CompareTo(c.carMover.lifeTime);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag != "FinishLine") return;
        GameManager.Instance.carsController.RaceBest(this);
    }
        
        
}
