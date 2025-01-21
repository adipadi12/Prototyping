using UnityEngine;
using static UnityEngine.Mathf; //so Mathf doesn't need to bewritten everywhere in the code
public static class FunctionLibrary { //this class is made to define math functions that can be used by our graph to render different things

    public delegate float Function(float x, float z, float t);
    public enum FunctionName { Wave, MultiWave, Absolute, Moustache, Ripple2D, Wave3D, MultiWave3D, Ripple3D }
    static Function[] functions = { Wave, MultiWave, Absolute, Moustache, Ripple2D, Wave3D, MultiWave3D, Ripple3D }; //array here so indices sorted on their own
    public static Function GetFunction(FunctionName name)
    {
        //if (index == 0)
        //{
        //    return Wave;
        //}
        //else if (index == 1)
        //    return MultiWave;
        //else if (index == 2)
        //    return Absolute;
        //else if (index == 3)
        //    return Moustache;
        //else 
        //    return Ripple2D;

        return functions[(int)name]; //no need for if statements now
    }


    public delegate Vector3 FunctionVec(float u, float v, float t);
    public enum FunctionVecName { WaveVec, MultiWaveVec, RippleVec, Sphere, Torus }
    static FunctionVec[] functionVecs = { WaveVec, MultiWaveVec, RippleVec, Sphere, Torus };
    public static FunctionVec GetFunctionVec(FunctionVecName name)
    {
        return functionVecs[(int)name];
    }

    public static FunctionVecName GetNextFunctionName(FunctionVecName name)
    {
        //if ((int)name < functions.Length - 1)
        //{
        //    return name + 1;
        //}
        //else
        //{
        //    return 0;
        //}
        return (int)name < functionVecs.Length - 1 ? name + 1 : 0; // using ternary operator basically means the same thing as what is above
    }
    public static FunctionVecName GetRandomFunctionNameOtherThan(FunctionVecName name)
    {
        var choice = (FunctionVecName)Random.Range(1, functionVecs.Length);
        return choice == name ? 0 : choice;
    }


    public static Vector3 Morph(float u, float v, float t, FunctionVec from, FunctionVec to, float progress)
    {
        return Vector3.LerpUnclamped(from(u, v , t), to(u, v , t), SmoothStep(0f, 1f, progress)); //lerp is linear interpolation. for straight constant sppeed transitions that are constamt. basically for smoother looking transition between functions   
    }

    public static Vector3 WaveVec(float u, float v, float t) //f(x,t) = sin(pi(x + t))
    {
        Vector3 p;
        p.x = u;
        p.y = Sin(PI * (u + v + t));
        p.z = v;
        return p;
    }
    public static Vector3 MultiWaveVec(float u, float v, float t) //f(x,t) = sin(pi(x + t))
    {
        Vector3 p;
        p.x = u;
        p.y = Sin(PI * (u + 0.5f + t));
        p.y += 0.5f * Sin(2f * PI * (v + t)); //defines double the frequency but in the same size as the original
        p.y += Sin(PI * (u + v + 0.25f * t));
        p.y *= 1f / 2.5f;
        p.z = v;
        return p;
    }
    public static Vector3 RippleVec(float u, float v, float t) //f(x,t) = sin(pi(x + t))
    {
        float d = Sqrt(u * u + v * v);
        Vector3 p;
        p.x = u;
        p.y = Sin(PI * (4f * d - t));
        p.y /= 1f + 10f * d;
        p.z = v;
        return p;
    }
    public static Vector3 Sphere(float u, float v, float t)
    {
        float r = 0.9f + 0.1f * Sin(PI * (6f * u + 4f * v + t)); //done to adjust radius of the cylinder
        float s = r * Cos(0.5f * PI * v);
        Vector3 p;
        p.x = s * Sin(PI * u);
        p.y = r * Sin(PI * 0.5f * v); //sin pi / 2 * v along the y axis to make it circular
        p.z = s * Cos(PI * u);
        return p;
    }
    public static Vector3 Torus(float u, float v, float t)
    {
        //float r1 = 0.75f;
        //float r2 = 0.25f; // defining r1 and r2 for major and minor radius that define a torus
        float r1 = 0.9f + 0.1f * Sin(PI * (6f * u + 0.5f * t));
        float r2 = 0.2f + 0.05f * Sin(PI * (8f * u + 4f * v + 2f * t));
        float s = r1 + r2 * Cos(PI * v);
        Vector3 p;
        p.x = s * Sin(PI * u);
        p.y = r2 * Sin(PI * v);
        p.z = s * Cos(PI * u);
        return p;
    }
    public static float Wave(float x, float z, float t) //f(x,t) = sin(pi(x + t))
    {
        return Sin(PI * (x + t)); //x defines the amplitude t defines the frequency
    }

    public static float MultiWave(float x, float z, float t) //f(x,t) = sin(pi(x + t))
    {
        float y = Sin(PI * (x + 0.5f * t));
        y += Sin(2f * PI * (x + t)) / 2f; //defines double the frequency but in the same size as the original
        return y / 1.5f; //done so domain stays in {-1, 1} instead of {-1.5. 1.5}
    }

    public static float Absolute(float x, float z, float t) {
        float a = Abs(x);
        return a / t;
    }

    public static float Moustache(float x, float z, float t)
    {
        float a = Abs(x);
        float b = Sin(4f * PI * a);
        return b / t;
    }

    public static float Ripple2D(float x, float z, float t)
    {
        float a = Abs(x);
        float b = Sin(PI * (4f * a - t));
        return b / (1f + 10f * a);
    }

    public static float Wave3D(float x, float z, float t) //f(x,t) = sin(pi(x + t))
    {
        return Sin(PI * (x + z + t)); //x defines the amplitude t defines the frequency
    }

    public static float MultiWave3D(float x, float z, float t) //f(x,t) = sin(pi(x + t))
    {
        float y = Sin(PI * (x + 0.5f * t));
        y += 0.5f * Sin(2f * PI * (z + t)); //defines double the frequency but in the same size as the original
        y += Sin(PI * (x + z + 0.25f * t));
        return y * (1f / 2.5f); //done so domain stays in {-1, 1} instead of {-1.5. 1.5}
    }

    public static float Ripple3D(float x, float z, float t)
    {
        float a = Sqrt(x * x + z * z);
        float b = Sin(PI * (4f * a - t));
        return b / (1f + 10f * a);
    }
}