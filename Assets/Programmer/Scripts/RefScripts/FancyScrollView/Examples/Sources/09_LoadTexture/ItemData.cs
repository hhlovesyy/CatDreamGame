/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

namespace FancyScrollView.Example09
{
    class ItemData
    {
        public string Title { get; }
        public string Description { get; }
        public string Url { get; }
        
        public int LevelDifficulty { get; }

        public ItemData(string title, string description, string url, int levelDifficulty)
        {
            Title = title;
            Description = description;
            Url = url;
            LevelDifficulty = levelDifficulty;
        }
    }
}
