using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUGraph : MonoBehaviour
{
    [SerializeField]
    ComputeShader computeShader;

    [SerializeField]
    Material material;

    [SerializeField]
    Mesh mesh;

    [SerializeField, Range(10, 200)]
    int resolution = 10; // define number of cubes from the inspector

    static readonly int
        positionsID = Shader.PropertyToID("_Position"),
        resolutionID = Shader.PropertyToID("_Resolution"),
        stepID = Shader.PropertyToID("_Step"),
        timeID = Shader.PropertyToID("_Time");
    //storing the identfiers for all these in readonly format 

    [SerializeField]
    FunctionLibrary.FunctionName function; //dropdown to select from all the available options

    public enum TransitionMode { Cycle, Random }
    [SerializeField]
    TransitionMode transitionMode;

    [SerializeField]
    FunctionLibrary.FunctionVecName functionVecName;

    [SerializeField, Min(0f)]
    float functionDuration = 1f, transitionDuration = 1f;

    float duration;

    bool transitioning;

    FunctionLibrary.FunctionVecName transitionFunction;
    
    ComputeBuffer positionBuffer; // to enable the GPU instancing 

    private void OnEnable()
    {
        positionBuffer = new ComputeBuffer(resolution * resolution, 3 * 4); // store 3D position vectors 3 times 4 bytes require 0.48 MB
    }

    private void OnDisable()
    {
        positionBuffer.Release(); // buffer released, indicates GPU memory claimed by the buffer can be used immediately
        positionBuffer = null;
    }

    private void Update()
    {
       // duration += Time.deltaTime;
        UpdateFunctionoOnGPU();
    }

    void UpdateFunctionoOnGPU()
    {
        float step = 2f / resolution;
        computeShader.SetInt(resolutionID, resolution);
        computeShader.SetFloat(stepID, step);
        computeShader.SetFloat(timeID, Time.time); // calculates step size and resolution which is done by invoking the SetInt and SetFloat methods
        computeShader.SetBuffer(0, positionsID,positionBuffer);
        int groups = Mathf.CeilToInt(resolution / 8f);
        computeShader.Dispatch(0, groups, groups, 1);

        material.SetBuffer(positionsID, positionBuffer);
        material.SetFloat(stepID, step);
        var bounds = new Bounds(Vector3.zero, Vector3.one * (2f + 2f / resolution));
        Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, positionBuffer.count); // used here coz ew know how many instances are gonna be there
    }
}
