using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAI : MonoBehaviour
{
    [SerializeField] private Memory memory;
    [SerializeField] private Decider decider;
    [SerializeField] private Enacter enacter;
    [SerializeField] private string mood;

    private void Start()
    {
        mood = null;
    }

    private Interaction GetIntendedInteraction()
    {
        var anticipations = decider.Anticipate();
        var experiment = decider.SelectExperiment(anticipations);
        var intendedInteraction = experiment.IntendedInteraction;
        return intendedInteraction;
    }

    void TryEnactIntendedInteractionAndLearn(Interaction intendedInteraction)
    {
        var enactedInteraction = enacter.Enact(intendedInteraction);
        if (enactedInteraction != intendedInteraction)
        {
            // logica -> nao lembro se intendedInteraction.experiment é sempre igual ao experiment.intendedInteraction
            // estou assumindo que seja
            intendedInteraction.Experiment.EnactedInteractions.Add(enactedInteraction);
            Debug.Log("enacted_interaction != intended_interaction");
        }

        Debug.Log("Enacted " + enactedInteraction);
        decider.LearnCompositeInteraction(enactedInteraction);
        decider.enactedInteraction = enactedInteraction;
    }

    private void SetMood()
    {
        mood = decider.enactedInteraction.valence >= 0 ? "PLEASED" : "PAINED";
    }

    public void Step()
    {
        var intendedInteraction = GetIntendedInteraction();
        TryEnactIntendedInteractionAndLearn(intendedInteraction);
        SetMood();
    }
}