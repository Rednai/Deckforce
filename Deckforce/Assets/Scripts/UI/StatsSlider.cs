using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsSlider : MonoBehaviour
{
    public Slider healthSlider;
    public Text healthText;
    public Slider shieldSlider;
    public Slider actionSlider;
    public Text actionText;
    public Slider movementSlider;
    public Text movementText;

    public void SetInfos(Character character, bool isSelecting)
    {
        healthSlider.maxValue = character.maxLife;
        shieldSlider.maxValue = character.maxShield;
        actionSlider.maxValue = character.maxActionPoints;
        movementSlider.maxValue = character.maxMovePoints;

        if (isSelecting) {
            healthText.text = $"{character.maxLife}/{character.maxLife}";
            healthSlider.value = character.maxLife;
            shieldSlider.value = 0;
            actionText.text = $"{character.maxActionPoints}/{character.maxActionPoints}";
            actionSlider.value = character.maxActionPoints;
            movementText.text = $"{character.maxMovePoints}/{character.maxMovePoints}";
            movementSlider.value = character.maxMovePoints;
        } else {
            healthText.text = $"{character.currentLife}/{character.maxLife}";
            healthSlider.value = character.currentLife;
            shieldSlider.value = character.currentShield;

            actionText.text = $"{character.currentActionPoints}/{character.maxActionPoints}";
            actionSlider.value = character.currentActionPoints;

            movementText.text = $"{character.currentMovePoints}/{character.maxMovePoints}";
            movementSlider.value = character.currentMovePoints;
        }
    }
}
