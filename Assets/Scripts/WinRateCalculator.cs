using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinRateCalculator : MonoBehaviour
{

    public double AWinRate = 0.3;
    public double BWinRate = 0.6;
    public double CWinRate = 1;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(WhenAShootNone());
        Debug.Log(WhenAShootC());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    double WhenAShootNone()
    {
        double result1 = AWinRate * BWinRate;
        result1 /= BWinRate + CWinRate - (BWinRate * CWinRate);
        result1 /= AWinRate + BWinRate - (AWinRate * BWinRate);

        double result2 = AWinRate * CWinRate * (1 - BWinRate);
        result2 /= BWinRate + CWinRate - (BWinRate * CWinRate);
        result2 /= AWinRate + CWinRate - (AWinRate * CWinRate);

        double result = result1 + result2;
        return result;
    }

    double WhenAShootC()
    {
        double result1 = AWinRate * AWinRate * (1 - BWinRate);
        result1 /= AWinRate + BWinRate - (AWinRate * BWinRate);

        double result2 = (1 - AWinRate) * WhenAShootNone();

        double result = result1 + result2;
        return result;
    }
}
