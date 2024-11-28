/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using EasingCore;
using TMPro;
using UnityEngine.AddressableAssets;

namespace FancyScrollView.Example09
{
    class Cell : FancyCell<ItemData>
    {
        readonly EasingFunction alphaEasing = Easing.Get(Ease.OutQuint);

        [SerializeField] TMP_Text title = default;
        [SerializeField] TMP_Text description = default;
        [SerializeField] RawImage image = default;
        [SerializeField] Image background = default;
        [SerializeField] CanvasGroup canvasGroup = default;
        [SerializeField] private GameObject starGroups = default;
        ItemData data;

        public override void UpdateContent(ItemData itemData)
        {
            data = itemData;
            //image.texture = null;
            string addressableLink = itemData.Url;
            if (addressableLink != "null")
            {
                Addressables.LoadAssetAsync<Texture2D>(addressableLink).Completed += handle =>
                {
                    image.texture = handle.Result;
                };
            }
            // TextureLoader.Load(itemData.Url, result =>
            // {
            //     if (image == null || result.Url != data.Url)
            //     {
            //         return;
            //     }
            //
            //     image.texture = result.Texture;
            // });

            int difficulty = itemData.LevelDifficulty;
            for (int i = 0; i < starGroups.transform.childCount; i++)
            {
                starGroups.transform.GetChild(i).gameObject.SetActive(i < difficulty);
            }
            title.gameObject.SetActive(true);
            title.text = itemData.Title;
            description.text = itemData.Description;
            UpdateSibling();
        }

        public override void UpdateFirstIndex(int index)
        {
            base.UpdateFirstIndex(index);
            if (index == Index)
            {
                //title.gameObject.SetActive(true);
                description.gameObject.SetActive(true);
            }
            else
            {
                //title.gameObject.SetActive(false);
                description.gameObject.SetActive(false);
            }
        }

        void UpdateSibling()
        {
            var cells = transform.parent.Cast<Transform>()
                .Select(t => t.GetComponent<Cell>())
                .Where(cell => cell.IsVisible);
            
            if (Index == cells.Min(x => x.Index))
            {
                transform.SetAsLastSibling();
            }

            if (Index == cells.Max(x => x.Index))
            {
                transform.SetAsFirstSibling();
            }
        }

        public override void UpdatePosition(float t)
        {
            const float popAngle = -15;
            const float slideAngle = 25;

            const float popSpan = 0.75f;
            const float slideSpan = 0.25f;

            t = 1f - t;

            var pop = Mathf.Min(popSpan, t) / popSpan;
            var slide = Mathf.Max(0, t - popSpan) / slideSpan;

            transform.localRotation = t < popSpan
                ? Quaternion.Euler(0, 0, popAngle * (1f - pop))
                : Quaternion.Euler(0, 0, slideAngle * slide);

            transform.localPosition = Vector3.left * 500f * slide;

            canvasGroup.alpha = alphaEasing(1f - slide);

            background.color = Color.Lerp(Color.gray, Color.white, pop);
        }
        
        
    }
}
