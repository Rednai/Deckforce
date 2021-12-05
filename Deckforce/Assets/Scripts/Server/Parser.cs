using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parser : MonoBehaviour
{
    public CardsManager cardsManager;

    void ParseData(object obj)
    {
        switch (obj) {
            case ActivateCard cardObj:
                //TODO: récupérer la carte depuis le CardManager avec l'id et l'activer à une position
                break;
            case PlayerMove moveObj:
                //TODO: faire bouger le joueur a une position
                break;
            case PlayerSpawn spawnObj:
                //TODO: faire spawn le joueur a une position
                break;
            case SkipTurn turnObj:
                //TODO: passer au tour suivant
                break;
        }
    }
}
