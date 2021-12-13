using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Draw Card")]
public class DrawCard : ManipulationCard
{
    public int drawAmount;

    public override bool Activate(Player currentPlayer, List<Tile> targetsTiles, Tile centerTile)
    {
        if (currentPlayer.selectedCharacter.currentActionPoints >= cost) {
            for (int i = 0; i != drawAmount; i++)
                currentPlayer.deck.Draw();
            SoundsManager.instance.PlaySound(activateClip);
            return (true);
        }
        return (false);
    }
}
