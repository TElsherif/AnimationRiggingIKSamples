using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Android;
using static UnityEngine.ParticleSystem;

[RequireComponent(typeof(ParticleSystem))]
public class ExampleClass : MonoBehaviour
{
    private ParticleSystem ps;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        ps.Stop(); // Cannot set duration whilst Particle System is playing

        MainModule main = ps.main;
        main.duration = 100.0f;

        ps.Play();

        TestClass ts = new TestClass();

        TestStruct s = ts.testStruct;
        s.testValue = 2;

        Debug.Log(ts.testStruct.Equals(s));
    }
}

public struct TestStruct
{
    public float testValue { get; set; }
}

public class TestClass
{
    public TestStruct testStruct { get; } = new TestStruct { testValue = 1 };
}
