using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anticipation
{
    public Experiment experiment;
    public int proclivity;

    public Anticipation()
    {
        experiment = null;
        proclivity = 0;
    }

    public override string ToString()
    {
        return experiment.Label + " proclivity " + proclivity;
    }
}