using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField]
    Transform pointPrefab; //inspector shows the transform for it 


    [SerializeField, Range(10,200)]
    int resolution = 10; // define number of cubes from the inspector

    [SerializeField]
    FunctionLibrary.FunctionName function; //dropdown to select from all the available options

    public enum TransitionMode { Cycle, Random}
    [SerializeField]
    TransitionMode transitionMode;

    [SerializeField]
    FunctionLibrary.FunctionVecName functionVecName;

    [SerializeField, Min(0f)]
    float functionDuration = 1f, transitionDuration = 1f;

    Transform[] points;

    float duration;

    bool transitioning;

    FunctionLibrary.FunctionVecName transitionFunction;
    private void Awake()
    {
        points = new Transform[resolution * resolution]; // making it's size equal to the cubes' resolution
        for(int i = 0, x = 0, z = 0; i < points.Length; i++, x++)  // for printing instances in a loop
        {
            if (x == resolution)
            {
                x = 0;
                z++;
            }
            float step = 2f / resolution; //done to keep scale and position in the domain -1 to 1. which is 2f units. dividing gives the space between each cube
                                          /*The x-position of each point is set by the line:
                                            position.x = (i + 0.5f) * step - 1f;
                                                        i is the current index of the loop.
                                            (i + 0.5f) ensures that the object is centered within its "step" space(since i + 0.5 adjusts it slightly to avoid
                                            positioning exactly on the edges).
                                            Multiplying(i + 0.5f) by step gives the exact x-coordinate for each object based on its index in the loop.
                                            - 1f shifts the entire grid so that it spans from - 1 to 1(rather than 0 to 2). */

            var scale = Vector3.one * step;
            var positionVec = Vector3.zero;
            Transform point = points[i]= Instantiate(pointPrefab); // instantiate the object and give a variable to it's transform
            positionVec.x = ((x + 0.5f) * step -1f); //for scaling it to 0-2 range first then subtracting so it is -1 - 1 range
            positionVec.z = (z + 0.5f) * step - 1f;
        // positionVec.y = positionVec.x; // f(x) = x will ideally make a straight line

        point.localPosition = positionVec;
            // doing this instantiates another instance of the prefab
            point.localScale = scale; // reducing the scale of the cubes
            point.SetParent(transform, false); // to make all the instances children of our empty Graph game object. false added so object doesn't stay at original woorld position
        }
        //point.localPosition = Vector3.right ; // for spawning the cube to the right of the parent game object
    }
    private void Update()
    {
        duration += Time.deltaTime;
        if(transitioning) {
            if (duration >= transitionDuration)
            {
                duration -= transitionDuration;
                transitioning = false;
            }
        }
        else if (duration >= functionDuration)
        {
            duration -= functionDuration;
            transitioning = true;
            transitionFunction = functionVecName;
            //functionVecName = FunctionLibrary.GetNextFunctionName(functionVecName);
            PickNextFunction();
        }
        if (transitioning)
        {
            UpdateFunctionTransition();
        }
        else
        {
            UpdateFunction();
        }
    }
    void PickNextFunction()
    {
        functionVecName = transitionMode == TransitionMode.Cycle ?
            FunctionLibrary.GetNextFunctionName(functionVecName) :
            FunctionLibrary.GetRandomFunctionNameOtherThan(functionVecName);
    }
    void UpdateFunction()
    {
        FunctionLibrary.Function f = FunctionLibrary.GetFunction(function); //calling the function library made in the functionlibrary script
        FunctionLibrary.FunctionVec fv = FunctionLibrary.GetFunctionVec(functionVecName);

        float time = Time.time; //deltaTime not used here because it would only change the y position over a frame. not suitable for continuous time based progression
        float step = 2f / resolution;

        //for (int i = 0; i < points.Length; i++)
        //{

        //    //float lundtime = Time.deltaTime;
        //    Transform point = points[i];
        //    Vector3 positionVec = point.localPosition;
        //    //positionVec.y = positionVec.x * positionVec.x ; //f(x) = x^2 for a parabola
        //    //positionVec.y = Mathf.Sin(Mathf.PI * (positionVec.x + time));
        //    //if (function == 0 )
        //    //{
        //    //    positionVec.y = FunctionLibrary.Wave(positionVec.x, time);
        //    //}           
        //    //if (function == 1)
        //    //{
        //    //    positionVec.y = FunctionLibrary.MultiWave(positionVec.x, time);
        //    //}
        //    //if(function == 2)
        //    //    positionVec.y = FunctionLibrary.Absolute(positionVec.x, time);
        //    //if (function == 3)
        //    //    positionVec.y = FunctionLibrary.Moustache(positionVec.x, time);
        //    //if(function == 4)
        //    //    positionVec.y = FunctionLibrary.Ripple2D(positionVec.x, time);
        //    positionVec.y = f(positionVec.x, positionVec.z, time);
        //    point.localPosition = positionVec;
        //}
        float v = 0.5f * step - 1f;
        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
        {
            if (x == resolution)
            {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }
            float u = (x + 0.5f) * step - 1f;
            points[i].localPosition = fv(u, v, time);
        }
    }

    void UpdateFunctionTransition()
    {
        FunctionLibrary.FunctionVec
            from = FunctionLibrary.GetFunctionVec(transitionFunction), //calling the function library made in the functionlibrary script
            to = FunctionLibrary.GetFunctionVec(functionVecName);
        float progress = duration / transitionDuration;
        float time = Time.time; //deltaTime not used here because it would only change the y position over a frame. not suitable for continuous time based progression
        float step = 2f / resolution;
        float v = 0.5f * step - 1f;
        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
        {
            if (x == resolution)
            {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }
            float u = (x + 0.5f) * step - 1f;
            points[i].localPosition = FunctionLibrary.Morph( u,v , time, from, to, progress);
        }
    }
}
