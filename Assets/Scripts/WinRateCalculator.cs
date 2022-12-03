using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinRateCalculator : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public double WhenAShootNone(int aPercent, int bPercent, double cPercent)
    {
        double a = aPercent / 100.0;
        double b = bPercent / 100.0;
        double c = cPercent / 100.0;
        double result1 = a * b;
        result1 /= b + c - (b * c);
        result1 /= a + b - (a * b);

        double result2 = a * c * (1 - b);
        result2 /= b + c - (b * c);
        result2 /= a + c - (a * c);

        double result = result1 + result2;
        return result;
    }

    public double WhenAShootC(int aPercent, int bPercent, int cPercent)
    {
        double a = aPercent / 100.0;
        double b = bPercent / 100.0;
        double c = cPercent / 100.0;
        double result1 = a * a * (1 - b);
        result1 /= a + b - (a * b);

        double result2 = (1 - a) * WhenAShootNone(aPercent, bPercent, cPercent);

        double result = result1 + result2;
        return result;
    }

    public double CRate(int aPercent, int bPercent, int cPercent)
    {
        double a = aPercent / 100.0;
        double b = bPercent / 100.0;
        double c = cPercent / 100.0;
        if(WhenAShootNone(aPercent, bPercent, cPercent) > WhenAShootC(aPercent, bPercent, cPercent))
        {
            double result1 = c * c * (1 - a) * (1 - b);
            double result2 = (b + c - b * c) * (a + c - a * c);
            double result = result1 / result2;
            return result;
        } else
        {
            double result1 = c * c * (1 - a) * (1 - a) * (1 - b);
            double result2 = (a + b + c - a * b - b * c - c * a + a * b * c) * (a + c - a * c);
            double result = result1 / result2;
            return result;
        }
    }

    public double BRate(int aPercent, int bPercent, int cPercent)
    {
        if (WhenAShootNone(aPercent, bPercent, cPercent) > WhenAShootC(aPercent, bPercent, cPercent))
        {
            double result = 1 - WhenAShootNone(aPercent, bPercent, cPercent) - CRate(aPercent, bPercent, cPercent);
            return result;
        } else
        {
            double result = 1 - WhenAShootC(aPercent, bPercent, cPercent) - CRate(aPercent, bPercent, cPercent);
            return result;
        }
    }
}
