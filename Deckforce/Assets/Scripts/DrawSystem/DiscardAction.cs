using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardAction : MonoBehaviour
{
    [SerializeField]protected GameObject discardPile;
    private CanvasGroup discardGroup;

    private void Start()
    {
        discardGroup = discardPile.GetComponent<CanvasGroup>();
    }

    public void OpenCloseDiscardPile()
    {
        if (discardGroup.alpha == 1)
            discardGroup.alpha = 0;
        else
            discardGroup.alpha = 1;
    }
}
