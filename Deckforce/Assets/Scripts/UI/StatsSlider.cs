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

    public void SetInfos(Entity entity, bool isSelecting)
    {
        Character character = entity.GetComponent<Character>();
        healthSlider.maxValue = entity.maxLife;
        shieldSlider.maxValue = entity.maxShield;
        if (character != null) {
            actionSlider.maxValue = character.maxActionPoints;
        }
        movementSlider.maxValue = entity.maxMovePoints;

        if (isSelecting) {
            healthText.text = $"{entity.maxLife}/{entity.maxLife}";
            healthSlider.value = entity.maxLife;
            shieldSlider.value = 0;
            if (character != null) {
                actionText.text = $"{character.maxActionPoints}/{character.maxActionPoints}";
                actionSlider.value = character.maxActionPoints;
            }
            movementText.text = $"{entity.maxMovePoints}/{entity.maxMovePoints}";
            movementSlider.value = entity.maxMovePoints;
        } else {
            healthText.text = $"{entity.currentLife}/{entity.maxLife}";
            healthSlider.value = entity.currentLife;
            shieldSlider.value = entity.currentShield;

            if (character != null) {
                actionText.text = $"{character.currentActionPoints}/{character.maxActionPoints}";
                actionSlider.value = character.currentActionPoints;
            }

            movementText.text = $"{entity.currentMovePoints}/{entity.maxMovePoints}";
            movementSlider.value = entity.currentMovePoints;
        }
    }
}
