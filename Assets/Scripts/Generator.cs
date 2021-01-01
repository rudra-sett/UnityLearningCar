using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static NeuralNet;

public class Generator : MonoBehaviour
{
    [SerializeField]
    public GameObject carobject;
    static public GameObject carmodel;
    [SerializeField]
    Text highscore;
    [SerializeField]
    Text generation;
    [SerializeField]
    public bool resetweights;

    public void rweights()
    {
        resetweights = true;
    }
    // Start is called before the first frame update
    //Car carinstance = new Car();

    List<Car> cars = new List<Car>();
    int deadcars;
    int numcars = 20;
    int gen = 1;
    float hs;
    
    void Start()
    {
        //proper reference to the car model/prefab
        carmodel = carobject;

        //set up list of cars
        setupcars(null,null);

    }
    
    public void setupcars(float[,] weights1,float[,]weights2)
    {
        cars = new List<Car>();
        for (int i = 0; i < numcars; i++)
        {
            cars.Add(new Car());
        }
        foreach (Car item in cars)
        {
            item.makecar();
            item.setupnetwork(weights1,weights2);
        }
    }
    public class Car
    {
        public GameObject car;
        public bool dead = false;
        public float distance;
        public Vector3 previouspos;
        public float timealive;
        public void makecar() {
            car = Instantiate(carmodel, position: new Vector3(6, 1, Random.Range(-2, 2)), rotation: new Quaternion(0, 0, 0, 0));
            previouspos = car.transform.position;
        }
        public void goforward()
        {
            car.GetComponent<Drive>().goforward();
        }
        public void gobackward()
        {
            car.GetComponent<Drive>().gobackward();
        }
        public void goleft()
        {
            car.GetComponent<Drive>().steerleft();
        }
        public void goright()
        {
            car.GetComponent<Drive>().steerright();
        }
        public List<float> getdistances()
        {
            //forward
            Physics.Raycast(car.transform.position, car.transform.forward, out RaycastHit hitinfo,100);
            //left
            Physics.Raycast(car.transform.position, -car.transform.right, out RaycastHit lefthitinfo, 100);
            //right
            Physics.Raycast(car.transform.position, car.transform.right, out RaycastHit righthitinfo, 100);
            //diagright
            Physics.Raycast(car.transform.position, (-car.transform.right+car.transform.forward), out RaycastHit diaglefthitinfo, 100);
            //diagleft
            Physics.Raycast(car.transform.position, (car.transform.right+car.transform.forward), out RaycastHit diagrighthitinfo, 100);
            
            //distances
            List<float> data = new List<float>{ hitinfo.distance, lefthitinfo.distance, righthitinfo.distance,diagrighthitinfo.distance,diaglefthitinfo.distance};
            for (int i = 0; i<data.Count;i++)
            {
                data[i] = (data[i] - data.Average()) / data.Average();
            }
            return data;
        }

        //Neural Network
        public NeuralNet.Layer layer1 = new NeuralNet.Layer();
        public NeuralNet.Layer layer2 = new NeuralNet.Layer();
        public void setupnetwork(float[,] weights1,float[,] weights2)
        {
            layer1.numinputs = 5;
            layer1.numoutputs = 5;
            layer2.numinputs = 5;
            layer2.numoutputs = 3;

            layer1.initweights();
            if (weights1 != null)
            {
                //print("got saved weights!");
                layer1.weights = weights1;
            }
            layer1.randomizeweights();
            
            layer2.initweights();
            if (weights2 != null)
            {
                print("got saved weights");
                layer2.weights = weights2;
            }
            layer2.randomizeweights();
            
        }
    }
    
    public void runcars()
    {
        foreach (Car item in cars)
        {
            //check aliveness before doing anything
            if (item.car.GetComponent<Drive>().dead & item.dead == false)
            {
                item.dead = true;
                deadcars += 1;
            }
            if (item.car.GetComponent<Rigidbody>().velocity.magnitude < 0.01 & item.timealive > 4f & item.dead == false)
            {
                item.dead = true;
                deadcars += 1;
            }            
            //handles movement
            if (item.dead == false)
            {
                //for scoring purposes, calculate distance traveled by car
                Vector3 distanceVector = item.car.transform.position - item.previouspos;
                float distanceThisFrame = distanceVector.magnitude;
                item.distance += distanceThisFrame;
                item.previouspos = item.car.transform.position;

                item.distance = (item.car.transform.position - new Vector3(3, -8, 60)).magnitude;

                //timealive
                item.timealive += Time.deltaTime;

                //run the network
                item.layer1.inputs = item.getdistances();
                item.layer1.propogate();
                item.layer2.inputs = item.layer1.outputs;
                item.layer2.propogate();
                List<float> result = item.layer2.outputs;

                //drive the car
                /*float finaloutput = Mathf.Max(result.ToArray());
                finaloutput = result.IndexOf(finaloutput);
                if (finaloutput == 0)
                {
                    item.goforward();
                    //print("car is going forward");
                }
                if (finaloutput == 1)
                {
                    item.gobackward();
                    //print("car is going backward");
                }
                if (finaloutput == 2)
                {
                    item.goleft();
                    //print("car is going left");
                }
                if (finaloutput == 3)
                {
                    item.goright();
                    //print("car is going right");
                }*/
                float[] finalmatrix = new float[3];
                //round all outputs to 1 or 0 to map to a direction
                finalmatrix[0] = Mathf.Round(result[0]);
                finalmatrix[1] = Mathf.Round(result[1]);
                finalmatrix[2] = Mathf.Round(result[2]);
                //finalmatrix[3] = Mathf.Round(result[3]);
                if (finalmatrix[0] == 1)
                {
                    item.goforward();
                    //print("car is going forward");
                }
                //if (finalmatrix[1] == 1)
                //{
                  //  item.gobackward();
                    //print("car is going backward");
                //}
                if (finalmatrix[1] == 1)
                {
                    item.goleft();
                    //print("car is going left");
                }
                if (finalmatrix[2] == 1)
                {
                    item.goright();
                    //print("car is going right");
                }


            }
            
        }
    }

    public void resetcars()
    {
        foreach (Car item in cars)
        {
            item.dead = true;
            deadcars += 1;
        }
    }
    // Update is called once per frame
    void Update()
    {

        //all cars have failed
        if (deadcars >= numcars)
        {
            print("all cars died!");
            List<float> scores = new List<float>();
            foreach (Car item in cars)
            {
                scores.Add(item.distance);
                Destroy(item.car);
            }
            
            int id = scores.IndexOf(Mathf.Max(scores.ToArray()));
            //print("best score was" + Mathf.Max(scores.ToArray()));
            float[,] goodlayer1 = cars[id].layer1.weights;
            float[,] goodlayer2 = cars[id].layer2.weights;
            if (resetweights)
            {
                goodlayer1 = null;
                goodlayer2 = null;
                resetweights = false;
                gen = 0;
            }
            cars.Clear();
            print("setting up new cars");
            setupcars(goodlayer1,goodlayer2);
            deadcars = 0;
            gen += 1;
            generation.text = "Generation: " + gen;
            if (Mathf.Max(scores.ToArray()) > hs)
            {
                hs = Mathf.Max(scores.ToArray());
                highscore.text = "High Score: " + hs;
            }
        }
        //calculates movement for each car
        if (deadcars < numcars) 
        { 
            runcars();
        }

    }
    

}
