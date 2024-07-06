using ColouredModsRelics.Common.Mods;
using Humanizer;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using rail;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using static MonoMod.Cil.ILContext;

namespace ColouredModsRelics.Common
{
    public abstract class ModRainbowRelics
    {
        public class MethodBaseInfo
        {
            public string AssemblyName;
            public string TypeName;
            public string MethodName;
            public BindingFlags Flags;

            public MethodBaseInfo(string assemblyName, string typeName, string methodName, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
            {
                AssemblyName = assemblyName;
                TypeName = typeName;
                MethodName = methodName;
                Flags = flags;
            }

            public ILHook MakeILHook(Manipulator manipulator)
            {
                try
                {
                    return new ILHook(GetMethodBase(ModLoader.GetMod(AssemblyName).Code), manipulator);
                }
                catch (KeyNotFoundException e)
                {
                    ColouredModsRelics.Instance.Logger.Error("KeyNotFoundException: 未找到对应 Mod，请将信息报告给圣物上色 Mod 开发者：{0}".FormatWith(e.Message));
                }
                catch (Exception e)
                {
                    ColouredModsRelics.Instance.Logger.Error("Exception: ILHook加载失败，请尝试检查游戏完整性，或将信息报告给圣物上色 Mod 开发者：{0}".FormatWith(e.Message));
                }
                return null;
            }

            public Hook MakeHook(Delegate target)
            {
                try
                {
                    return new Hook(GetMethodBase(ModLoader.GetMod(AssemblyName).Code), target);
                }
                catch (Exception e)
                {
                    ColouredModsRelics.Instance.Logger.Error("Exception: Hook加载失败，请尝试检查游戏完整性，或将信息报告给圣物上色 Mod 开发者：{0}".FormatWith(e.Message));
                }
                return null;
            }

            public MethodBase GetMethodBase(Assembly assembly)
            {
                try
                {
                    var type = assembly.GetType(TypeName, true);
                    return type.GetMethod(MethodName, Flags);
                }
                catch (TypeLoadException e)
                {
                    // TODO：Localize this
                    // ColouredModsRelics.Instance.Logger.Error(Language.GetTextValue(ErrorTextsKey + ".TypeLoadException", e.Message));
                    ColouredModsRelics.Instance.Logger.Error("TypeLoadException: Mod DLL加载失败，请尝试检查游戏完整性，或将信息报告给圣物上色Mod开发者：{0}".FormatWith(e.Message));
                    return null;
                }
            }

        }

        public static readonly MethodBase ModTile_Type;

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

        public MethodBaseInfo HookInfo;

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

        static ModRainbowRelics()
        {
            ModTile_Type = typeof(ModTile).GetProperty("Type", BindingFlags.Instance | BindingFlags.Public).GetGetMethod();
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
            var hook = HookInfo.MakeILHook(Manip);
            if (hook is null)
            {
                Hooks = [];
                return;
            }
            Hooks = [hook];
            return;
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
                return [];
            }
            Type RelicItemType = Mod.Code.GetType(BaseRelicItemType);
            IEnumerable<ModItem> relicItems = Mod.GetContent().Where(c => c is ModItem && c.GetType().IsSubclassOf(RelicItemType)).Select(c => (ModItem)c);
            return relicItems;
        }

        public virtual IEnumerable<ModTile> GetRelicTiles()
        {
            if (string.IsNullOrEmpty(BaseRelicTileType))
            {
                ColouredModsRelics.Instance.Logger.Warn($"{ModName}物块加载失败：BaseRelicTileType为空，需要重写GetRelicTileTypes");
                return [];
            }
            Type RelicTileType = Mod.Code.GetType(BaseRelicTileType);
            IEnumerable<ModTile> relicTiles = Mod.GetContent().Where(c => c is ModTile t && c.GetType().IsSubclassOf(RelicTileType)).Select(c => (ModTile)c);
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
