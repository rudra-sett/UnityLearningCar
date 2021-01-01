using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNet : MonoBehaviour
{

    public class Layer
    {
        public int numinputs;
        public int numoutputs;
        public float[,] weights;
        public void initweights()
        {
            weights = new float[numinputs, numoutputs];
        }
        public void randomizeweights()
        {
            float[] lrates = new float[] {0.001f,0.005f, 0.01f, 0.05f, 0.1f, 0.5f, 1f ,5f,10};
            float lrate = lrates[Random.Range(0, lrates.Length)];
            //print("Layer of " + numinputs + " inputs and " + numoutputs + " outputs");
            for (int i = 0; i < numinputs; i++)
            {
                for (int o = 0; o < numoutputs; o++)
                {
                    weights[i, o] += Random.Range(-lrates[Random.Range(0, lrates.Length)], lrates[Random.Range(0, lrates.Length)]);
                }
            }
        }
        public List<float> inputs = new List<float>();
        public List<float> outputs = new List<float>();
        float activate(float value)
        {
            //tanh function
            //return 2 / ((1 + Mathf.Pow(2.71828f, -2.0f * value)) - 1);
            //sigmoid
            return 1.0f / (1.0f + (float)Mathf.Exp(-value));
        }


        public void propogate()
        {
            outputs = new List<float>();
            for (int output = 0 ; output < numoutputs; output++)
            {
                float outputsum = 0;
                for (int input = 0; input < numinputs; input++) {
                    outputsum += inputs[input]*weights[input,output];
                }
                outputsum = activate(outputsum+1);
                outputs.Add(outputsum);
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
       /* Layer layer1 = new Layer();
        layer1.numinputs = 3;
        layer1.numoutputs = 5;
        layer1.initweights();

        //inputs of a layer must be equal to outputs of previous layer
        Layer layer2 = new Layer();
        layer2.numinputs = 5;
        layer2.numoutputs = 4;
        layer2.initweights();*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
}
