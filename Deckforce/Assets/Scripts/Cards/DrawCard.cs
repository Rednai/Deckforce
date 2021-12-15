using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Draw Card")]
public class DrawCard : ManipulationCard
{
    public int drawAmount;

    public override bool Activate(Player currentPlayer, List<Tile> targetsTiles, Tile centerTile)
    {
        if (CheckIfPossible(currentPlayer) & currentPlayer.isClient) {
            currentPlayer.selectedCharacter.currentActionPoints -= cost;
            for (int i = 0; i != drawAmount; i++)
                currentPlayer.deck.Draw();
            SoundsManager.instance.PlaySound(activateClip);
            return (true);
        }
        SoundsManager.instance.PlaySound(cannotClip);
        return (false);
    }
}
