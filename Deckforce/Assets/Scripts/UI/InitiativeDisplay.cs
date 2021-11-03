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
            newDisplay.transform.parent = iconsHolder.transform;
            icons.Add(entity, newDisplay);
            /*
            newDisplay.transform.position = new Vector3(
                initiativeSlider.transform.position.x - 45,
                initiativeSlider.transform.position.y + (entity.initiative * 18),
                initiativeSlider.transform.position.z
            );
            */
            //TODO: set la position

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
