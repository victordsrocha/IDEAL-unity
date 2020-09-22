using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Transactions;
using UnityEngine;
using Random = UnityEngine.Random;

public class Decider : MonoBehaviour
{
    private Memory memory { get; set; }
    public Interaction enactedInteraction;
    private Interaction superInteraction;
    private System.Random rnd = new System.Random();

    private void Start()
    {
        memory = GetComponent<Memory>();
        enactedInteraction = null;
        superInteraction = null;
    }

    List<Interaction> GetActivatedInteractions()
    {
        /*
        Uma interação é dita ativada quando sua pré-interação é uma interação em contexto.
        Uma interação é dita em contexto quando ela é a interação realizada (enacted) no
        último passo, incluindo a super-interação criada com base nesta.

        Returns:
            retorna lista de interações ativadas
        */
        var contextInteractions = new List<Interaction>();
        if (enactedInteraction != null)
        {
            contextInteractions.Add(enactedInteraction);
            if (!enactedInteraction.is_primitive())
            {
                contextInteractions.Add(enactedInteraction.PostInteraction);
            }

            if (superInteraction != null)
            {
                contextInteractions.Add(superInteraction);
            }
        }

        var activatedInteractions = new List<Interaction>();
        foreach (var knowInteraction in memory.KnownInteractions.Values)
        {
            if (knowInteraction.is_primitive() == false)
            {
                if (contextInteractions.Contains(knowInteraction.PreInteraction))
                {
                    activatedInteractions.Add(knowInteraction);
                    Debug.Log("activated " + knowInteraction);
                }
            }
        }

        return activatedInteractions;
    }


    public List<Anticipation> Anticipate()
    {
        var anticipations = new List<Anticipation>();
        var activatedInteractions = GetActivatedInteractions();

        // este bloco cria uma lista de anticipations a partir da lista de interações ativadas
        if (enactedInteraction != null)
        {
            foreach (var activatedInteraction in activatedInteractions)
            {
                if (activatedInteraction.PostInteraction.Experiment != null)
                {
                    var newAnticipation = new Anticipation();
                    newAnticipation.experiment = activatedInteraction.PostInteraction.Experiment;
                    newAnticipation.proclivity = activatedInteraction.Weight * activatedInteraction.PostInteraction.valence;
                    foreach (var anticipation in anticipations)
                    {
                        if (newAnticipation.experiment == anticipation.experiment)
                        {
                            //TODO a falta de adaptabilidade com certeza está na forma como é dada a proclivity
                            // -> Problema do ótimo local!
                            anticipation.proclivity += newAnticipation.proclivity;
                            break;
                        }
                        else
                        {
                            anticipations.Add(newAnticipation);
                        }
                    }
                }
            }
        }

        /*
        * este bloco faz uso da lista de enacted interactions armazenadas em experiments
        * se uma dessas interactions é o postInteraction de uma interação ativada:
        * então podemos aumentar a tendência (proclivity) da anticipation de origem
        */
        foreach (var anticipation in anticipations)
        {
            foreach (var experimentEnactedInteraction in anticipation.experiment.EnactedInteractions)
            {
                foreach (var activatedInteraction in activatedInteractions)
                {
                    if (experimentEnactedInteraction == activatedInteraction.PostInteraction)
                    {
                        var proclivity = activatedInteraction.Weight * experimentEnactedInteraction.valence;
                        anticipation.proclivity += proclivity;
                    }
                }
            }
        }

        return anticipations;
    }

    private Experiment GetOtherExperiment(Experiment experiment)
    {
        /*
         * Acessa memória de experimentos conhecidos e retorna um experimento diferente do
         * recebido como argumento.
         * Por enquanto está sendo feito de forma aleatória.
         *
         * TODO refazer esta função usando Set:
         * experiments_set = set(self.memory.known_experiments.values()) - {experiment}
         * return random.choice(list(experiments_set))
         */
        var experiments = memory.KnownExperiments.Values.ToList();
        var index = rnd.Next();
        var other = experiments[index];

        if (other == experiment)
        {
            other = GetOtherExperiment(experiment);
        }

        return other;
    }

    public Experiment SelectExperiment(List<Anticipation> anticipations)
    {
        /*
        The selectExperiment( ) function sorts the list of anticipations by decreasing proclivity of
        their proposed interaction. Then, it takes the fist anticipation (index [0]), which has
        the highest proclivity in the list. If this proclivity is positive, then the agent wants to
        re-enact this proposed interaction, leading to the agent choosing this proposed
        interaction's experiment.

        Por enquanto: se houver alguma anticipation com proclivity > 0 então a anticipation de
        maior proclivity sempre será escolhida, caso contrário sorteia um experimento conhecido
        qualquer para retornar.

        durante fase de treino  tem x% de chance de selecionar um experimento aleatório
        independentemete da maior proclivity encontrada

        Args:
            Anticipation list -> Lista de antecipações criadas por Anticipate( )

        Returns:
            Experiment -> experimento selecionado
        */
        Experiment selectedExperiment;
        if (anticipations.Count > 0)
        {
            anticipations = anticipations.OrderByDescending(x => x.proclivity).ToList();

            foreach (var anticipation in anticipations)
            {
                Debug.Log("propose " + anticipation);
            }

            var selectedAnticipation = anticipations[0];
            if (selectedAnticipation.proclivity > 0)
            {
                selectedExperiment = selectedAnticipation.experiment;
            }
            else
            {
                selectedExperiment = GetOtherExperiment(selectedAnticipation.experiment);
            }
        }
        else
        {
            selectedExperiment = GetOtherExperiment(null);
        }

        return selectedExperiment;
    }

    public void LearnCompositeInteraction(Interaction newEnactedInteraction)
    {
        var previousInteraction = enactedInteraction;
        var lastInteraction = newEnactedInteraction;
        var previousSuperInteraction = superInteraction;
        Interaction lastSuperInteraction = null;

        // learn [previous current] called the super interaction
        if (previousInteraction != null)
        {
            lastSuperInteraction = memory.AddOrGetAndReinforceCompositeInteraction(previousInteraction, lastInteraction);
        }

        // Learn higher-level interactions
        if (previousSuperInteraction != null)
        {
            // learn [penultimate [previous current]]
            memory.AddOrGetAndReinforceCompositeInteraction(previousSuperInteraction.PreInteraction, lastSuperInteraction);

            // learn [[penultimate previous] current]
            memory.AddOrGetAndReinforceCompositeInteraction(previousSuperInteraction, lastInteraction);
        }

        this.superInteraction = lastSuperInteraction;
    }
}