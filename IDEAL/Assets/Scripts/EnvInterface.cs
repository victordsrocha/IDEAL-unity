using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvInterface : MonoBehaviour
{
    //public SimpleTarget env { get; set; }
    private Memory memory { get; set; }
    private PlayerMovement playerMovement;

    private void Start()
    {
        memory = GetComponent<Memory>();
        playerMovement = GetComponent<PlayerMovement>();
        
        /*
         * ao invés de pensar em uma classe enviroment eu deveria comunicar o envInterface somente com as classes de "motores do meu robô"
         * ex: classe que move meu robo, classe que controla a visao, etc
         * não será preciso uma classes para ambiente de fato, isso ficara implicito na engine
         */
        var env = playerMovement; // mudar nomenclarura depois
        //env = new SimpleTarget();
    }

    public Interaction Enact(Interaction intendedInteraction)
    {
        /*
         * recebe somente interações primitivas esta função deve "tentar" executar a interação intencionada no ambiente em seguida deve retornar
         * a interação realmente executada (enacted) esta pode ou não ser igual à intencionada
         */

        string result = null;
        char act = intendedInteraction.Label[0];

        // ↖ ↑ ↗
        // ← · →
        // ↙ ↓ ↘
        
        /*
        if (act == '↑')
        {
            env.MoveUp();
        }
        else if (act == '↗')
        {
            env.MoveUpRight();
        }
        else if (act == '→')
        {
            env.MoveRight();
        }
        else if (act == '↘')
        {
            env.MoveDownUp();
        }
        else if (act == '↓')
        {
            env.MoveDown();
        }
        else if (act == '↙')
        {
            env.MoveDownLeft();
        }
        else if (act == '←')
        {
            env.MoveLeft();
        }
        else if (act == '↖')
        {
            env.MoveUpLeft();
        }
        else if (act == '*')
        {
            env.Attack();
        }
        */

        var enactedInteraction = memory.GetPrimitiveInteraction(result);

        Debug.Log("primitive enacted interaction: " + enactedInteraction);
        return enactedInteraction;
    }
}