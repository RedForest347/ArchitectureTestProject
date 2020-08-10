using System;
using System.Collections.Generic;
using UnityEngine;
using RangerV;


public struct TestSignal : ISignal
{
    public int test_int;
    public string test_string;

    public TestSignal(int test_int, string test_string)
    {
        this.test_int = test_int;
        this.test_string = test_string;
    }
}



