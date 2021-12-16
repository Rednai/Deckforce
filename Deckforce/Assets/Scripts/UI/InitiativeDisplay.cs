using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitiativeDisplay : MonoBehaviour
{
    public GameObject entityIconTemplate;
    public GameObject iconsHolder;
    [Serializable]
    public struct EntityIconDisplay {
        public int battleId;
        public Entity entity;
        public GameObject iconDisplay;
    };
    [SerializeField]
    public List<EntityIconDisplay> iconsDisplays;

    public void DisplayEntitiesInitiatives(List<Entity> battleEntities)
    {
        iconsDisplays = new List<EntityIconDisplay>();

        foreach (Entity entity in battleEntities) {
            GameObject newDisplay = Instantiate(entityIconTemplate);
            newDisplay.transform.Find("StatsSliders").GetComponent<StatsSlider>().SetInfos(entity, false);
            newDisplay.transform.Find("IconDisplay").GetComponent<Image>().sprite = entity.entityIcon;
            //TODO: pour l'instant ca fonctionne mais plus tard faudra set une couleur dans l'entité
            newDisplay.transform.GetComponent<Image>().color = entity.GetComponent<MeshRenderer>().material.color;
            newDisplay.transform.SetParent(iconsHolder.transform);
            EntityIconDisplay iconDisplay = new EntityIconDisplay();
            iconDisplay.battleId = entity.battleId;
            iconDisplay.entity = entity;
            iconDisplay.iconDisplay = newDisplay;
            iconsDisplays.Add(iconDisplay);
        }
    }

    public void ResetDisplay()
    {
        while (iconsHolder.transform.childCount > 0) {
            Transform child = iconsHolder.transform.GetChild(0);

            child.SetParent(null);
            Destroy(child.gameObject);
        }
    }

    public void SetBackInTimeline(Entity playingEntity)
    {
        EntityIconDisplay iconDisplay = iconsDisplays.Find(x => x.battleId == playingEntity.battleId);
        if (iconDisplay.iconDisplay != null) {
            iconDisplay.iconDisplay.transform.SetParent(null);
            iconDisplay.iconDisplay.transform.SetParent(iconsHolder.transform);
            iconDisplay.iconDisplay.transform.Find("EndTurnDisplay").gameObject.SetActive(true);
        }
    }

    public void RemoveFromTimeline(Entity playingEntity)
    {
        EntityIconDisplay iconDisplay = iconsDisplays.Find(x => x.battleId == playingEntity.battleId);

        if (iconDisplay.iconDisplay != null) {
            iconsDisplays.Remove(iconDisplay);
            Destroy(iconDisplay.iconDisplay);
        }
        iconsDisplays.Clear();
    }
}
