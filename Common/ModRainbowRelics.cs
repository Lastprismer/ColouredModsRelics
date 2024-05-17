using Microsoft.Xna.Framework.Graphics;
using MonoMod.RuntimeDetour;
using rail;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Terraria.GameContent;
using Terraria.ModLoader;
using static MonoMod.Cil.ILContext;

namespace ColouredModsRelics.Common
{
    public abstract class ModRainbowRelics
    {
        // 彩色圣物物品
        public Dictionary<int, Asset<Texture2D>> ColoredRelicItemAssets;

        // 彩色圣物物块
        public Dictionary<int, Asset<Texture2D>> ColoredRelicTileAssets;

        // 原版圣物物品
        public Dictionary<int, Asset<Texture2D>> OriginalRelicItemAssets;

        // Mod的圣物物品的ID
        public List<int> RelicItemTypes;

        // Mod的圣物物块的ID
        public List<int> RelicTileTypes;

        // 是否应该生效
        public bool Active;

        // 用于处理SpecialDraw的ILHook
        protected List<ILHook> Hooks;

        public virtual MethodBase HookOrigin { get; }

        public virtual Manipulator Manip { get; }

        public abstract string ModName { get; }

        public virtual string BaseRelicItemType { get; }

        public virtual string BaseRelicTileType { get; }

        public string GetItemPath() => $"ColouredModsRelics/Assets/{ModName}/Items/";

        public string GetTilePath() => $"ColouredModsRelics/Assets/{ModName}/Tiles/";

        protected Mod Mod => ModLoader.GetMod(ModName);

        public ModRainbowRelics()
        {
            ColoredRelicItemAssets = new();
            ColoredRelicTileAssets = new();
            OriginalRelicItemAssets = new();
            RelicItemTypes = new();
            RelicTileTypes = new();
        }

        public virtual void Reset()
        {
            ColoredRelicItemAssets.Clear();
            ColoredRelicTileAssets.Clear();
            OriginalRelicItemAssets.Clear();
            RelicItemTypes.Clear();
        }

        public virtual void InitHooks()
        {
            Hooks = [new ILHook(HookOrigin, Manip)];
        }

        public virtual void LoadHook()
        {
            InitHooks();
            foreach (var hook in Hooks)
            {
                hook.Apply();
            }
        }

        public virtual void UnloadHook()
        {
            if (Hooks is not null)
            {
                foreach (var hook in Hooks)
                {
                    hook?.Dispose();
                }
                Hooks = null;
            }
        }

        public virtual IEnumerable<ModItem> GetRelicItems()
        {
            if (string.IsNullOrEmpty(BaseRelicItemType))
            {
                ColouredModsRelics.Instance.Logger.Warn($"{ModName}物品加载失败：BaseRelicItemType为空，需要重写GetRelicTileTypes");
                return Enumerable.Empty<ModItem>();
            }
            Type RelicItemType = Mod.Code.GetType(BaseRelicItemType);
            IEnumerable<ModItem> relicItems = Mod.GetContent().Where(c =>
            {
                return c is ModItem && c.GetType().IsSubclassOf(RelicItemType);
            }).Select(c => (ModItem)c);
            return relicItems;
        }

        public virtual IEnumerable<ModTile> GetRelicTiles()
        {
            if (string.IsNullOrEmpty(BaseRelicTileType))
            {
                ColouredModsRelics.Instance.Logger.Warn($"{ModName}物块加载失败：BaseRelicTileType为空，需要重写GetRelicTileTypes");
                return Enumerable.Empty<ModTile>();
            }
            Type RelicTileType = Mod.Code.GetType(BaseRelicTileType);
            IEnumerable<ModTile> relicTiles = Mod.GetContent().Where(c =>
            {
                return c is ModTile t && c.GetType().IsSubclassOf(RelicTileType);
            }).Select(c => (ModTile)c);
            return relicTiles;
        }

        public virtual void LoadItemTextures()
        {
            var relicItems = GetRelicItems();
            foreach (var relic in relicItems)
            {
                int type = relic.Type;
                RelicItemTypes.Add(type);
                ColoredRelicItemAssets[type] = RainbowUtils.Request(GetItemPath() + relic.Name);
                OriginalRelicItemAssets[type] = TextureAssets.Item[type];
            }
        }

        public virtual void LoadTileTextures()
        {
            var relicTiles = GetRelicTiles();
            foreach (var relic in relicTiles)
            {
                int type = relic.Type;
                RelicTileTypes.Add(type);
                ColoredRelicTileAssets[type] = RainbowUtils.Request(GetTilePath() + relic.Name);
            }
        }

        public void OverriveItemTextures()
        {
            foreach (int type in RelicItemTypes)
            {
                TextureAssets.Item[type] = ColoredRelicItemAssets[type];
            }
        }

        public void ResetItemTexture()
        {
            foreach (int type in RelicItemTypes)
            {
                TextureAssets.Item[type] = OriginalRelicItemAssets[type];
            }
        }

        public void ToggleTextures(bool value)
        {
            if (value == Active)
                return;

            if (value)
                OverriveItemTextures();
            else
                ResetItemTexture();
            Active = value;
        }
    }
}
