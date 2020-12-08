using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class GameManager: MonoBehaviour
{
    [SerializeField] public CarsController carsController;
    [SerializeField] public CanvasController canvasController;

    public static GameManager Instance { get; private set; }
    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

}