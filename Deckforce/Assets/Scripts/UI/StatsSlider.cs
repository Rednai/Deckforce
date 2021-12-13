using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsSlider : MonoBehaviour
{
    Entity refEntity;
    Character character = null;
    bool isSelecting;
    bool isSet = false;

    public Slider healthSlider;
    public Text healthText;
    public Slider shieldSlider;
    public Slider actionSlider;
    public Text actionText;
    public Image actionIcon;
    public Slider movementSlider;
    public Text movementText;

    public GameObject effectsParent;
    public GameObject effectTemplate;

    void Update()
    {
        if (!isSet)
            return ;

        healthText.text = $"{refEntity.currentLife}/{refEntity.maxLife}";
        healthSlider.value = refEntity.currentLife;
        shieldSlider.value = refEntity.currentShield;

        if (character != null) {
            actionSlider.gameObject.SetActive(true);
            actionText.gameObject.SetActive(true);
            actionText.text = $"{character.currentActionPoints}/{character.maxActionPoints}";
            actionSlider.value = character.currentActionPoints;
        } else {
            actionSlider.gameObject.SetActive(false);
            actionText.gameObject.SetActive(false);
            actionIcon.gameObject.SetActive(false);
        }

        movementText.text = $"{refEntity.currentMovePoints}/{refEntity.maxMovePoints}";
        movementSlider.value = refEntity.currentMovePoints;
    }

    public void SetInfos(Entity entity, bool isSelecting)
    {
        refEntity = entity;
        character = refEntity.GetComponent<Character>();
        this.isSelecting = isSelecting;

        healthSlider.maxValue = refEntity.maxLife;
        shieldSlider.maxValue = refEntity.maxShield;
        if (character != null) {
            actionSlider.gameObject.SetActive(true);
            actionSlider.maxValue = character.maxActionPoints;
        }
        movementSlider.maxValue = refEntity.maxMovePoints;

        if (isSelecting) {
            healthText.text = $"{refEntity.maxLife}/{refEntity.maxLife}";
            healthSlider.value = refEntity.maxLife;
            shieldSlider.value = 0;
            if (character != null) {
                actionText.text = $"{character.maxActionPoints}/{character.maxActionPoints}";
                actionSlider.value = character.maxActionPoints;
            }
            movementText.text = $"{refEntity.maxMovePoints}/{refEntity.maxMovePoints}";
            movementSlider.value = refEntity.maxMovePoints;
        } else {
            isSet = true;
        }
        SetEffects();
    }

    public void SetEffects()
    {
        foreach (Effect effect in refEntity.appliedEffects) {
            GameObject newEffectDisplay = Instantiate(effectTemplate);

            newEffectDisplay.transform.Find("EffectIcon").GetComponent<Image>().sprite = effect.effectIcon;
            newEffectDisplay.transform.Find("TurnText").GetComponent<Text>().text = "" + effect.remainingTurns;

            newEffectDisplay.transform.SetParent(effectsParent.transform);
            newEffectDisplay.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void ResetEffects()
    {
        while (effectsParent.transform.childCount != 0) {
            Transform child = effectsParent.transform.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }
    }
}
