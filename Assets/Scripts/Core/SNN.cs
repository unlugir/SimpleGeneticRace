using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

public class SNN
{
    public List<Matrix<double>> weigths;
    public Random rand;
    public List<int> shape;
    public SNN()
    {
        
    }
    public SNN(List<int> layers, Random random)
    {
        rand = random;
        shape = layers;
        weigths = new List<Matrix<double>>();
        for (int k=0;k < layers.Count - 1; k++)
        {
            weigths.Add(Matrix<double>.Build.Dense(layers[k], layers[k + 1],
                (i,j) => 2*rand.NextDouble()-1));
        }
    }
    
    public Matrix<double> Forward(Matrix<double> input)
    {
        var result = input.Clone();
        for(int i=0;i< weigths.Count;i++)
        {
            result = result * weigths[i];
            result = Sigmoid(result);    
        }
        return result;
    }

    public Matrix<double> Sigmoid(Matrix<double> input)
    {
        var temp = input.Clone();
        temp = temp.Map(v => v = SingleSigmoid(v));
        return temp;
    }
    private double SingleSigmoid(double value)
    {
        double k = Math.Exp(value);
        return k / (1.0f + k);
    }
    public SNN DeepClone(){
        SNN newSNN = new SNN();
        newSNN.weigths = new List<Matrix<double>>();
        newSNN.rand = rand;
        newSNN.shape = shape;
        for(int i=0;i< weigths.Count;i++){
            newSNN.weigths.Add(weigths[i].Clone());
        }
        return newSNN;
    }
    public void MutateWeights(Random random){
        for (int k =0;k<weigths.Count;k++){
            var mutation = Matrix<double>.Build.Dense(shape[k], shape[k+1],
                (i,j) => (2*random.NextDouble()-1));
            weigths[k] += mutation;
        }
    }
    public static SNN Combine(SNN snn1,SNN snn2){
        SNN newSNN = new SNN();
        newSNN.rand = new Random();
        newSNN.shape = snn1.shape;
        newSNN.weigths = new List<Matrix<double>>();
        newSNN.weigths.Add(snn1.weigths[0].Clone());
        newSNN.weigths.Add(snn2.weigths[1].Clone());
        return newSNN;
    }
}
