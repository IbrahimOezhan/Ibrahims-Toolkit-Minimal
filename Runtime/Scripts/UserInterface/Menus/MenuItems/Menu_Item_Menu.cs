﻿using Sirenix.OdinInspector;
using UnityEngine;

namespace IbrahKit
{
    [System.Serializable]
    public class Menu_Item_Menu : Menu_Item_Button
    {
        [SerializeField] private Menu_Change_Type changeType;

        [ShowIf(nameof(changeType), Menu_Change_Type.REFERENCE), SerializeField] private UI_Menu_Basic menuReference;
        [ShowIf(nameof(changeType), Menu_Change_Type.TRANSITION), SerializeField] private int transitionReference = -1;

        public override void Spawn(RectTransform parent, UI_Menu_Extended menu)
        {
            base.Spawn(parent, menu);

            switch (changeType)
            {
                case Menu_Change_Type.REFERENCE:
                    spawnedButton.Initialize(value).AddListener(() =>
                    {
                        menu.MenuTransition(menuReference);
                    });
                    break;
                case Menu_Change_Type.TRANSITION:
                    spawnedButton.Initialize(value).AddListener(() =>
                    {
                        menu.MenuTransition(transitionReference);
                    });
                    break;
            }
        }

        public enum Menu_Change_Type
        {
            REFERENCE,
            TRANSITION,
        }
    }
}