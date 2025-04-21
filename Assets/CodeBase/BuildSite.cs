using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace TowerDefence
{
    public class BuildSite : MonoBehaviour, IPointerDownHandler
    {
        public TowerAsset[] BuildableTowers;
        public void SetBuildableTowers(TowerAsset[] towers)
        {
            if(towers == null || towers.Length == 0)
            {
                Destroy(transform.parent.gameObject); 
            }
            else
            {
                BuildableTowers = towers;
            }
        }
        public static event Action<BuildSite> OnClickEvent;
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            OnClickEvent(this);
        }
        public static void HideControls()
        {
            OnClickEvent(null);
        }

    }
}
