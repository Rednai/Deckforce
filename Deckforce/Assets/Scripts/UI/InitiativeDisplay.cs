using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitiativeDisplay : MonoBehaviour
{
    public Slider initiativeSlider;

    public GameObject entityIconTemplate;
    public GameObject iconsHolder;
    Dictionary<Entity, GameObject> icons;

    void Start()
    {
        icons = new Dictionary<Entity, GameObject>();
    }

    public void DisplayEntitiesInitiatives(List<Entity> battleEntities)
    {
        initiativeSlider.maxValue = 10;
        initiativeSlider.value = 0;

        foreach (Entity entity in battleEntities) {
            GameObject newDisplay = Instantiate(entityIconTemplate);
            newDisplay.transform.Find("IconDisplay").GetComponent<Image>().sprite = entity.entityIcon;
            //TODO: pour l'instant ca fonctionne mais plus tard faudra set une couleur dans l'entité
            newDisplay.transform.GetComponent<Image>().color = entity.GetComponent<MeshRenderer>().material.color;
            newDisplay.transform.parent = iconsHolder.transform;
            icons.Add(entity, newDisplay);            
        }
    }

    public void AdvanceInitiative(Entity playingEntity)
    {
        initiativeSlider.value = playingEntity.initiative;
    }

    public void RemoveFromTimeline(Entity playingEntity)
    {
        Destroy(icons[playingEntity]);
        icons.Remove(playingEntity);
    }
}
