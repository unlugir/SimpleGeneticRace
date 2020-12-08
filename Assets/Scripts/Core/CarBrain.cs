using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;
public class CarBrain : MonoBehaviour
{
    [SerializeField] float raycastDistance;
    [SerializeField] List<Transform> sensors;
    [SerializeField] LayerMask rayLayer;
    [SerializeField] float brainValue;
    public SNN brain;
    public bool lockMutations;
    CarMover mover;
    public System.Random random; 
    [SerializeField] List<double> sensorDistances;
    private void Start() {
        brain = new SNN(new List<int>{5,5,1},random);
        mover = GetComponent<CarMover>();
        sensorDistances = new List<double>();
    }
    private void Update() {
        if (!mover.alive) return;
        brainValue = GetBrainValue();
        sensorDistances.Clear();
        foreach(Transform point in sensors){
            Vector3 direction = point.position-transform.position;
            RaycastHit2D hit;
            hit = Physics2D.Raycast(transform.position, direction, raycastDistance,rayLayer);
            if (hit.collider != null)
                sensorDistances.Add((double)Vector3.Distance(hit.point, transform.position)/raycastDistance);
            else
                sensorDistances.Add(1);
        }
        
        double brainInput = brain.Forward(Matrix<double>.Build.DenseOfRows(new List<List<double>>(){sensorDistances})).RowSums().Sum();
        brainInput = brainInput*2-1;
        mover.Steer((float)brainInput);
    }
    private void OnDrawGizmos() {
        foreach(Transform point in sensors){
            Gizmos.color = Color.blue;
            Vector3 direction = point.position-transform.position;
            Gizmos.DrawRay(transform.position, direction);
        }
    }

    public void ReplaceBrains(SNN targetBrain){
        if (lockMutations) return;
        brain = targetBrain.DeepClone();
    }
    public void MutateBrains(System.Random random){
        if (lockMutations) return;
        brain.MutateWeights(random);
    }
    public float GetBrainValue(){
        float sum = 0;
        foreach (var layer in brain.weigths){
            sum += (float)layer.ColumnSums().Sum();
        }
        return sum;
    } 
}
