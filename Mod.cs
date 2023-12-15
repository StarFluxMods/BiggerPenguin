using KitchenLib;
using KitchenLib.Logging;
using KitchenLib.Logging.Exceptions;
using KitchenMods;
using System.Linq;
using System.Reflection;
using Kitchen;
using KitchenData;
using KitchenLib.Event;
using KitchenLib.Utils;
using UnityEngine;

namespace KitchenBiggerPenguin
{
    public class Mod : BaseMod, IModSystem
    {
        public const string MOD_GUID = "com.starfluxgames.biggerpenguin";
        public const string MOD_NAME = "Bigger Penguin";
        public const string MOD_VERSION = "0.1.0";
        public const string MOD_AUTHOR = "StarFluxGames";
        public const string MOD_GAMEVERSION = ">=1.1.8";

        public static AssetBundle Bundle;
        public static KitchenLogger Logger;

        public Mod() : base(MOD_GUID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_GAMEVERSION, Assembly.GetExecutingAssembly()) { }

        protected override void OnInitialise()
        {
            Logger.LogWarning($"{MOD_GUID} v{MOD_VERSION} in use!");
        }

        protected override void OnUpdate()
        {
        }

        protected override void OnPostActivate(KitchenMods.Mod mod)
        {
            Bundle = mod.GetPacks<AssetBundleModPack>().SelectMany(e => e.AssetBundles).FirstOrDefault() ?? throw new MissingAssetBundleException(MOD_GUID);
            Logger = InitLogger();
            
            Events.BuildGameDataEvent += (s, args) =>
            {
                PlayerCosmetic penguin = args.gamedata.Get<PlayerCosmetic>(765190223);
                penguin.Visual = MaterialUtils.AssignMaterialsByNames(Bundle.LoadAsset<GameObject>("PenguinCosmetic"));
                penguin.HeadSize = 1;
            
                PlayerOutfitComponent component = penguin.Visual.TryAddComponent<PlayerOutfitComponent>();
                component.Renderers.Add(GameObjectUtils.GetChild(penguin.Visual, "Eyes_Black").GetComponent<SkinnedMeshRenderer>());
                component.Renderers.Add(GameObjectUtils.GetChild(penguin.Visual, "Hat_001").GetComponent<SkinnedMeshRenderer>());
                component.Renderers.Add(GameObjectUtils.GetChild(penguin.Visual, "Sphere").GetComponent<SkinnedMeshRenderer>());
            };
        }
    }
}

