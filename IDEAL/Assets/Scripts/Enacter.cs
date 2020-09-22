using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class Enacter : MonoBehaviour
{
    public Memory memory { get; set; }
    public EnvInterface EnvInterface { get; set; }

    private void Start()
    {
        memory = GetComponent<Memory>();
        EnvInterface = GetComponent<EnvInterface>();
    }

    public Interaction Enact(Interaction intendedInteraction)
    {
        if (intendedInteraction.is_primitive())
        {
            return EnvInterface.Enact(intendedInteraction);
        }
        else
        {
            // Enact the pre-interaction
            var enactedPreInteraction = Enact(intendedInteraction.PreInteraction);
            if (enactedPreInteraction != intendedInteraction.PreInteraction)
            {
                // if the preInteraction failed then the enaction of the intendedInteraction is interrupted here
                return enactedPreInteraction;
            }
            else
            {
                // enact the post-interaction
                var enactedPostInteraction = Enact(intendedInteraction.PostInteraction);
                return memory.AddOrGetCompositeInteraction(enactedPreInteraction, enactedPostInteraction);
            }
        }
    }
}