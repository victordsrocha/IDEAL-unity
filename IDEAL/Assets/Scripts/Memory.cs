using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Memory : MonoBehaviour
{
    public Dictionary<string, Interaction> KnownInteractions;
    public Dictionary<string, Experiment> KnownExperiments;

    private void Start()
    {
        KnownInteractions = new Dictionary<string, Interaction>();
        KnownExperiments = new Dictionary<string, Experiment>();
        //initSimpleTarget();
    }

    public Interaction AddOrGetCompositeInteraction(Interaction preInteraction, Interaction postInteraction)
    {
        string label = "(" + preInteraction.Label + postInteraction.Label + ")";
        Interaction interaction;
        if (!KnownInteractions.ContainsKey(label))
        {
            interaction = AddOrGetInteraction(label);
            interaction.PreInteraction = preInteraction;
            interaction.PostInteraction = postInteraction;
            interaction.valence = preInteraction.valence + postInteraction.valence;
            AddOrGetAbstractExperiment(interaction);
        }
        else
        {
            interaction = KnownInteractions[label];
        }

        return interaction;
    }

    public Interaction AddOrGetAndReinforceCompositeInteraction(Interaction preInteraction, Interaction postInteraction)
    {
        var compositeInteraction = AddOrGetCompositeInteraction(preInteraction, postInteraction);
        compositeInteraction.Weight++;

        if (compositeInteraction.Weight == 1)
        {
            Debug.Log("Learn " + compositeInteraction);
        }
        else
        {
            Debug.Log("reinforce " + compositeInteraction);
        }

        return compositeInteraction;
    }

    private Interaction AddOrGetInteraction(string label)
    {
        if (!KnownInteractions.ContainsKey(label))
        {
            Interaction interaction = new Interaction(label);
            KnownInteractions.Add(label, interaction);
        }

        return KnownInteractions[label];
    }

    private Experiment AddOrGetAbstractExperiment(Interaction interaction)
    {
        string label = interaction.Label.Replace('e', 'E').Replace('r', 'R').Replace(')', '|');
        if (!KnownExperiments.ContainsKey(label))
        {
            var abstractExperiment = new Experiment(label);
            abstractExperiment.IntendedInteraction = interaction;
            interaction.Experiment = abstractExperiment;
            KnownExperiments.Add(label, abstractExperiment);
        }

        return KnownExperiments[label];
    }

    private Interaction AddPrimitiveInteraction(string label)
    {
        Interaction interaction;
        if (!KnownInteractions.ContainsKey(label))
        {
            interaction = AddOrGetInteraction(label);
            interaction.valence = CalcValence(label);
        }
        else
        {
            interaction = KnownInteractions[label];
        }

        return interaction;
    }

    public Interaction GetPrimitiveInteraction(string label)
    {
        return KnownInteractions[label];
    }

    private int CalcValence(string label)
    {
        /*sum_valence = 0
        sum_valence += label.count('^') * (-5)
        sum_valence += label.count('>') * (-5)
        sum_valence += label.count('v') * (-5)
        sum_valence += label.count('.') * (-1)
        sum_valence += label.count('-') * (+0)
        sum_valence += label.count('*') * (+10)
        sum_valence += label.count('+') * (+30)
        sum_valence += label.count('x') * (+50)
        sum_valence += label.count('o') * (-30)

        if label[0] == '>':
        sum_valence += label.count('w') * (-200)
        else:
        sum_valence += label.count('w') * (-200)

        return sum_valence*/
        return 0;
    }

    private void InitSimpleTarget()
    {
        /*l1 = self.add_primitive_interaction("^w**")
        l2 = self.add_primitive_interaction("^w+*")
        l3 = self.add_primitive_interaction("^wx*")
        l4 = self.add_primitive_interaction("^wo*")
        l5 = self.add_primitive_interaction("^w-*")
        l6 = self.add_primitive_interaction("^w*+")
        l7 = self.add_primitive_interaction("^w++")
        l8 = self.add_primitive_interaction("^wx+")
        l9 = self.add_primitive_interaction("^wo+")
        l10 = self.add_primitive_interaction("^w-+")
        l11 = self.add_primitive_interaction("^w*x")
        l12 = self.add_primitive_interaction("^w+x")
        l13 = self.add_primitive_interaction("^wxx")
        l14 = self.add_primitive_interaction("^wox")
        l15 = self.add_primitive_interaction("^w-x")
        l16 = self.add_primitive_interaction("^w*o")
        l17 = self.add_primitive_interaction("^w+o")
        l18 = self.add_primitive_interaction("^wxo")
        l19 = self.add_primitive_interaction("^woo")
        l20 = self.add_primitive_interaction("^w-o")
        l21 = self.add_primitive_interaction("^w*-")
        l22 = self.add_primitive_interaction("^w+-")
        l23 = self.add_primitive_interaction("^wx-")
        l24 = self.add_primitive_interaction("^wo-")
        l25 = self.add_primitive_interaction("^w--")
        l26 = self.add_primitive_interaction("^.**")
        l27 = self.add_primitive_interaction("^.+*")
        l28 = self.add_primitive_interaction("^.x*")
        l29 = self.add_primitive_interaction("^.o*")
        l30 = self.add_primitive_interaction("^.-*")
        l31 = self.add_primitive_interaction("^.*+")
        l32 = self.add_primitive_interaction("^.++")
        l33 = self.add_primitive_interaction("^.x+")
        l34 = self.add_primitive_interaction("^.o+")
        l35 = self.add_primitive_interaction("^.-+")
        l36 = self.add_primitive_interaction("^.*x")
        l37 = self.add_primitive_interaction("^.+x")
        l38 = self.add_primitive_interaction("^.xx")
        l39 = self.add_primitive_interaction("^.ox")
        l40 = self.add_primitive_interaction("^.-x")
        l41 = self.add_primitive_interaction("^.*o")
        l42 = self.add_primitive_interaction("^.+o")
        l43 = self.add_primitive_interaction("^.xo")
        l44 = self.add_primitive_interaction("^.oo")
        l45 = self.add_primitive_interaction("^.-o")
        l46 = self.add_primitive_interaction("^.*-")
        l47 = self.add_primitive_interaction("^.+-")
        l48 = self.add_primitive_interaction("^.x-")
        l49 = self.add_primitive_interaction("^.o-")
        l50 = self.add_primitive_interaction("^.--")

        m1 = self.add_primitive_interaction(">w**")
        m2 = self.add_primitive_interaction(">w+*")
        m3 = self.add_primitive_interaction(">wx*")
        m4 = self.add_primitive_interaction(">wo*")
        m5 = self.add_primitive_interaction(">w-*")
        m6 = self.add_primitive_interaction(">w*+")
        m7 = self.add_primitive_interaction(">w++")
        m8 = self.add_primitive_interaction(">wx+")
        m9 = self.add_primitive_interaction(">wo+")
        m10 = self.add_primitive_interaction(">w-+")
        m11 = self.add_primitive_interaction(">w*x")
        m12 = self.add_primitive_interaction(">w+x")
        m13 = self.add_primitive_interaction(">wxx")
        m14 = self.add_primitive_interaction(">wox")
        m15 = self.add_primitive_interaction(">w-x")
        m16 = self.add_primitive_interaction(">w*o")
        m17 = self.add_primitive_interaction(">w+o")
        m18 = self.add_primitive_interaction(">wxo")
        m19 = self.add_primitive_interaction(">woo")
        m20 = self.add_primitive_interaction(">w-o")
        m21 = self.add_primitive_interaction(">w*-")
        m22 = self.add_primitive_interaction(">w+-")
        m23 = self.add_primitive_interaction(">wx-")
        m24 = self.add_primitive_interaction(">wo-")
        m25 = self.add_primitive_interaction(">w--")
        m26 = self.add_primitive_interaction(">.**")
        m27 = self.add_primitive_interaction(">.+*")
        m28 = self.add_primitive_interaction(">.x*")
        m29 = self.add_primitive_interaction(">.o*")
        m30 = self.add_primitive_interaction(">.-*")
        m31 = self.add_primitive_interaction(">.*+")
        m32 = self.add_primitive_interaction(">.++")
        m33 = self.add_primitive_interaction(">.x+")
        m34 = self.add_primitive_interaction(">.o+")
        m35 = self.add_primitive_interaction(">.-+")
        m36 = self.add_primitive_interaction(">.*x")
        m37 = self.add_primitive_interaction(">.+x")
        m38 = self.add_primitive_interaction(">.xx")
        m39 = self.add_primitive_interaction(">.ox")
        m40 = self.add_primitive_interaction(">.-x")
        m41 = self.add_primitive_interaction(">.*o")
        m42 = self.add_primitive_interaction(">.+o")
        m43 = self.add_primitive_interaction(">.xo")
        m44 = self.add_primitive_interaction(">.oo")
        m45 = self.add_primitive_interaction(">.-o")
        m46 = self.add_primitive_interaction(">.*-")
        m47 = self.add_primitive_interaction(">.+-")
        m48 = self.add_primitive_interaction(">.x-")
        m49 = self.add_primitive_interaction(">.o-")
        m50 = self.add_primitive_interaction(">.--")

        r1 = self.add_primitive_interaction("vw**")
        r2 = self.add_primitive_interaction("vw+*")
        r3 = self.add_primitive_interaction("vwx*")
        r4 = self.add_primitive_interaction("vwo*")
        r5 = self.add_primitive_interaction("vw-*")
        r6 = self.add_primitive_interaction("vw*+")
        r7 = self.add_primitive_interaction("vw++")
        r8 = self.add_primitive_interaction("vwx+")
        r9 = self.add_primitive_interaction("vwo+")
        r10 = self.add_primitive_interaction("vw-+")
        r11 = self.add_primitive_interaction("vw*x")
        r12 = self.add_primitive_interaction("vw+x")
        r13 = self.add_primitive_interaction("vwxx")
        r14 = self.add_primitive_interaction("vwox")
        r15 = self.add_primitive_interaction("vw-x")
        r16 = self.add_primitive_interaction("vw*o")
        r17 = self.add_primitive_interaction("vw+o")
        r18 = self.add_primitive_interaction("vwxo")
        r19 = self.add_primitive_interaction("vwoo")
        r20 = self.add_primitive_interaction("vw-o")
        r21 = self.add_primitive_interaction("vw*-")
        r22 = self.add_primitive_interaction("vw+-")
        r23 = self.add_primitive_interaction("vwx-")
        r24 = self.add_primitive_interaction("vwo-")
        r25 = self.add_primitive_interaction("vw--")
        r26 = self.add_primitive_interaction("v.**")
        r27 = self.add_primitive_interaction("v.+*")
        r28 = self.add_primitive_interaction("v.x*")
        r29 = self.add_primitive_interaction("v.o*")
        r30 = self.add_primitive_interaction("v.-*")
        r31 = self.add_primitive_interaction("v.*+")
        r32 = self.add_primitive_interaction("v.++")
        r33 = self.add_primitive_interaction("v.x+")
        r34 = self.add_primitive_interaction("v.o+")
        r35 = self.add_primitive_interaction("v.-+")
        r36 = self.add_primitive_interaction("v.*x")
        r37 = self.add_primitive_interaction("v.+x")
        r38 = self.add_primitive_interaction("v.xx")
        r39 = self.add_primitive_interaction("v.ox")
        r40 = self.add_primitive_interaction("v.-x")
        r41 = self.add_primitive_interaction("v.*o")
        r42 = self.add_primitive_interaction("v.+o")
        r43 = self.add_primitive_interaction("v.xo")
        r44 = self.add_primitive_interaction("v.oo")
        r45 = self.add_primitive_interaction("v.-o")
        r46 = self.add_primitive_interaction("v.*-")
        r47 = self.add_primitive_interaction("v.+-")
        r48 = self.add_primitive_interaction("v.x-")
        r49 = self.add_primitive_interaction("v.o-")
        r50 = self.add_primitive_interaction("v.--")

        # alguns experimentos podem ser descobertos pelo próprio agente
        # depois tentar retirar alguns casos, especialmente as "falhas"

        # self.add_or_get_abstract_experiment(l1)
        # self.add_or_get_abstract_experiment(l2)
        # self.add_or_get_abstract_experiment(l3)
        # self.add_or_get_abstract_experiment(l4)
        # self.add_or_get_abstract_experiment(l5)
        # self.add_or_get_abstract_experiment(l6)
        # self.add_or_get_abstract_experiment(l7)
        # self.add_or_get_abstract_experiment(l8)
        # self.add_or_get_abstract_experiment(l9)
        # self.add_or_get_abstract_experiment(l10)
        # self.add_or_get_abstract_experiment(l11)
        # self.add_or_get_abstract_experiment(l12)
        # self.add_or_get_abstract_experiment(l13)
        # self.add_or_get_abstract_experiment(l14)
        # self.add_or_get_abstract_experiment(l15)
        # self.add_or_get_abstract_experiment(l16)
        # self.add_or_get_abstract_experiment(l17)
        # self.add_or_get_abstract_experiment(l18)
        # self.add_or_get_abstract_experiment(l19)
        # self.add_or_get_abstract_experiment(l20)
        # self.add_or_get_abstract_experiment(l21)
        # self.add_or_get_abstract_experiment(l22)
        # self.add_or_get_abstract_experiment(l23)
        # self.add_or_get_abstract_experiment(l24)
        # self.add_or_get_abstract_experiment(l25)
        # self.add_or_get_abstract_experiment(l26)
        # self.add_or_get_abstract_experiment(l27)
        # self.add_or_get_abstract_experiment(l28)
        # self.add_or_get_abstract_experiment(l29)
        # self.add_or_get_abstract_experiment(l30)
        # self.add_or_get_abstract_experiment(l31)
        # self.add_or_get_abstract_experiment(l32)
        # self.add_or_get_abstract_experiment(l33)
        # self.add_or_get_abstract_experiment(l34)
        # self.add_or_get_abstract_experiment(l35)
        # self.add_or_get_abstract_experiment(l36)
        # self.add_or_get_abstract_experiment(l37)
        # self.add_or_get_abstract_experiment(l38)
        # self.add_or_get_abstract_experiment(l39)
        # self.add_or_get_abstract_experiment(l40)
        # self.add_or_get_abstract_experiment(l41)
        # self.add_or_get_abstract_experiment(l42)
        # self.add_or_get_abstract_experiment(l43)
        # self.add_or_get_abstract_experiment(l44)
        # self.add_or_get_abstract_experiment(l45)
        # self.add_or_get_abstract_experiment(l46)
        # self.add_or_get_abstract_experiment(l47)
        # self.add_or_get_abstract_experiment(l48)
        # self.add_or_get_abstract_experiment(l49)
        self.add_or_get_abstract_experiment(l50)

        # self.add_or_get_abstract_experiment(m1)
        # self.add_or_get_abstract_experiment(m2)
        # self.add_or_get_abstract_experiment(m3)
        # self.add_or_get_abstract_experiment(m4)
        # self.add_or_get_abstract_experiment(m5)
        # self.add_or_get_abstract_experiment(m6)
        # self.add_or_get_abstract_experiment(m7)
        # self.add_or_get_abstract_experiment(m8)
        # self.add_or_get_abstract_experiment(m9)
        # self.add_or_get_abstract_experiment(m10)
        # self.add_or_get_abstract_experiment(m11)
        # self.add_or_get_abstract_experiment(m12)
        # self.add_or_get_abstract_experiment(m13)
        # self.add_or_get_abstract_experiment(m14)
        # self.add_or_get_abstract_experiment(m15)
        # self.add_or_get_abstract_experiment(m16)
        # self.add_or_get_abstract_experiment(m17)
        # self.add_or_get_abstract_experiment(m18)
        # self.add_or_get_abstract_experiment(m19)
        # self.add_or_get_abstract_experiment(m20)
        # self.add_or_get_abstract_experiment(m21)
        # self.add_or_get_abstract_experiment(m22)
        # self.add_or_get_abstract_experiment(m23)
        # self.add_or_get_abstract_experiment(m24)
        # self.add_or_get_abstract_experiment(m25)
        # self.add_or_get_abstract_experiment(m26)
        # self.add_or_get_abstract_experiment(m27)
        # self.add_or_get_abstract_experiment(m28)
        # self.add_or_get_abstract_experiment(m29)
        # self.add_or_get_abstract_experiment(m30)
        # self.add_or_get_abstract_experiment(m31)
        # self.add_or_get_abstract_experiment(m32)
        # self.add_or_get_abstract_experiment(m33)
        # self.add_or_get_abstract_experiment(m34)
        # self.add_or_get_abstract_experiment(m35)
        # self.add_or_get_abstract_experiment(m36)
        # self.add_or_get_abstract_experiment(m37)
        # self.add_or_get_abstract_experiment(m38)
        # self.add_or_get_abstract_experiment(m39)
        # self.add_or_get_abstract_experiment(m40)
        # self.add_or_get_abstract_experiment(m41)
        # self.add_or_get_abstract_experiment(m42)
        # self.add_or_get_abstract_experiment(m43)
        # self.add_or_get_abstract_experiment(m44)
        # self.add_or_get_abstract_experiment(m45)
        # self.add_or_get_abstract_experiment(m46)
        # self.add_or_get_abstract_experiment(m47)
        # self.add_or_get_abstract_experiment(m48)
        # self.add_or_get_abstract_experiment(m49)
        self.add_or_get_abstract_experiment(m50)

        # self.add_or_get_abstract_experiment(r1)
        # self.add_or_get_abstract_experiment(r2)
        # self.add_or_get_abstract_experiment(r3)
        # self.add_or_get_abstract_experiment(r4)
        # self.add_or_get_abstract_experiment(r5)
        # self.add_or_get_abstract_experiment(r6)
        # self.add_or_get_abstract_experiment(r7)
        # self.add_or_get_abstract_experiment(r8)
        # self.add_or_get_abstract_experiment(r9)
        # self.add_or_get_abstract_experiment(r10)
        # self.add_or_get_abstract_experiment(r11)
        # self.add_or_get_abstract_experiment(r12)
        # self.add_or_get_abstract_experiment(r13)
        # self.add_or_get_abstract_experiment(r14)
        # self.add_or_get_abstract_experiment(r15)
        # self.add_or_get_abstract_experiment(r16)
        # self.add_or_get_abstract_experiment(r17)
        # self.add_or_get_abstract_experiment(r18)
        # self.add_or_get_abstract_experiment(r19)
        # self.add_or_get_abstract_experiment(r20)
        # self.add_or_get_abstract_experiment(r21)
        # self.add_or_get_abstract_experiment(r22)
        # self.add_or_get_abstract_experiment(r23)
        # self.add_or_get_abstract_experiment(r24)
        # self.add_or_get_abstract_experiment(r25)
        # self.add_or_get_abstract_experiment(r26)
        # self.add_or_get_abstract_experiment(r27)
        # self.add_or_get_abstract_experiment(r28)
        # self.add_or_get_abstract_experiment(r29)
        # self.add_or_get_abstract_experiment(r30)
        # self.add_or_get_abstract_experiment(r31)
        # self.add_or_get_abstract_experiment(r32)
        # self.add_or_get_abstract_experiment(r33)
        # self.add_or_get_abstract_experiment(r34)
        # self.add_or_get_abstract_experiment(r35)
        # self.add_or_get_abstract_experiment(r36)
        # self.add_or_get_abstract_experiment(r37)
        # self.add_or_get_abstract_experiment(r38)
        # self.add_or_get_abstract_experiment(r39)
        # self.add_or_get_abstract_experiment(r40)
        # self.add_or_get_abstract_experiment(r41)
        # self.add_or_get_abstract_experiment(r42)
        # self.add_or_get_abstract_experiment(r43)
        # self.add_or_get_abstract_experiment(r44)
        # self.add_or_get_abstract_experiment(r45)
        # self.add_or_get_abstract_experiment(r46)
        # self.add_or_get_abstract_experiment(r47)
        # self.add_or_get_abstract_experiment(r48)
        # self.add_or_get_abstract_experiment(r49)
        self.add_or_get_abstract_experiment(r50)*/
    }
}