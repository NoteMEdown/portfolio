using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory {

    private int creditGain;

    public Factory() // default factory for testing
    {
        creditGain = Random.Range(2500,15000);
    }

	public Factory(int c)
    {
        creditGain = c;
    }

    public int GetCreditGain()
    {
        return creditGain;
    }
}
