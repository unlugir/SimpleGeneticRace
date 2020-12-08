using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMover : MonoBehaviour
{
    [SerializeField] public float moveSpeed;
    [SerializeField] public float rotationSpeed;
    [SerializeField] public bool alive;
    [SerializeField] public float lifeTime;

    void Start()
    {
        alive = true;
    }

    
    void Update()
    {
        if (!alive) return;
        lifeTime += Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position,transform.position + transform.right, moveSpeed * Time.deltaTime);
    }

    public void Steer(float amount){
        if (!alive) return;
        transform.Rotate(-transform.forward*amount*rotationSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag != "Wall") return;
        alive = false;
    } 
}
