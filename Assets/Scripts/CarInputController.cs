using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInputController : MonoBehaviour
{
    CarMover mover;
    void Start()
    {
        mover = GetComponent<CarMover>();
    }

    // Update is called once per frame
    void Update()
    {
        mover.Steer(Input.GetAxis("Horizontal"));
    }
}
