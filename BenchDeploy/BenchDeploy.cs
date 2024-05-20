using Benchwarp;
using Modding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UObject = UnityEngine.Object;

namespace BenchDeploy
{
    public class BenchDeploy : Mod, IGlobalSettings<GlobalSettings>, ILocalSettings<SaveSettings>,IMenuMod
    {
        public static BenchDeploy Instance;
        public static GlobalSettings GS = new();
        public static SaveSettings LS = new();
        public static bool inGame = false;

        public bool ToggleButtonInsideMenu => true;

        //public override List<ValueTuple<string, string>> GetPreloadNames()
        //{
        //    return new List<ValueTuple<string, string>>
        //    {
        //        new ValueTuple<string, string>("White_Palace_18", "White Palace Fly")
        //    };
        //}

        //public BenchDeploy() : base("BenchDeploy")
        //{
        //    Instance = this;
        //}

        public override string GetVersion()
        {
            var version = GetType().Assembly.GetName().Version.ToString();
#if DEBUG
            version += "-debug";
#endif
            return version;
        }
        public override int LoadPriority() => 5;

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Instance = this;
            Localization.HookLocalization();
            GUIController.Setup();

            On.GameManager.StartNewGame += StartNewGame;
            On.GameManager.ContinueGame += ContinueGame;
            On.QuitToMenu.Start += QuitToMenu;
            On.HeroController.Update += HeroController_Update;
        }

        private void HeroController_Update(On.HeroController.orig_Update orig, HeroController self)
        {
            orig.Invoke(self);
            /*
            if (inGame && BenchDeploy.GS.WarpToStartBench)
            {
                BenchDeploy.GS.WarpToStartBench = false;
                Events.SetToStart();
                ChangeScene.WarpToRespawn();
            }
             */
        }

        private static void StartNewGame(On.GameManager.orig_StartNewGame orig, GameManager self, bool permadeathMode, bool bossRushMode)
        {
            orig(self, permadeathMode, bossRushMode);
            inGame = true;
        }

        private static void ContinueGame(On.GameManager.orig_ContinueGame orig, GameManager self)
        {
            orig(self);
            inGame = true;
        }
        private IEnumerator QuitToMenu(On.QuitToMenu.orig_Start orig, QuitToMenu self)
        {
            inGame = false;
            BenchManager.ClearBench();
            return orig(self);
        }
        internal static new void LogDebug(string msg)
        {
#if DEBUG
            Instance.Log(msg);
#endif
        }

        public void OnLoadGlobal(GlobalSettings s)
        {
            GS = s;
        }

        public GlobalSettings OnSaveGlobal()
        {
            return GS;
        }
        public void OnLoadLocal(SaveSettings s)
        {
            LS = s;
        }

        public SaveSettings OnSaveLocal()
        {
            return LS;
        }

        public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? toggleButtonEntry)
        {
            return [
                /*
                new IMenuMod.MenuEntry
                {
                    Name = "Benchs Count".L(),
                    Values = new string[] { "1", "2", "3", "4", "5" },
                    Saver = bs => GS.BenchCount = bs + 1,
                    Loader = () =>
                    {
                        if (GS.BenchCount > 5 || GS.BenchCount < 1)
                            GS.BenchCount = 5;
                        return GS.BenchCount - 1;
                    }
                },
                */
                new IMenuMod.MenuEntry
                {
                    Name = "WWD Support".L(),
                    Description = "Disabled it if your direction binded KEY.D B W D".L(),
                    Values = new string[] { "Enabled".L(), "Disabled".L() },
                    Saver = bs => GS.BenchwarpHotkey = bs == 0,
                    Loader = () => GS.BenchwarpHotkey ? 0 : 1
                },

                new IMenuMod.MenuEntry
                {
                    Name = "Try Warp To Start".L(),
                    Description = "if your bench saved in unsafe room".L(),
                    Values = new string[] { "Enabled".L(), "Disabled".L() },
                    Saver = bs => GS.TryWarpToStart = bs == 0,
                    Loader = () => GS.TryWarpToStart ? 0 : 1
                },
                new IMenuMod.MenuEntry
                {
                    Name = "or Enter \"warptostart\" in game".L(),
                    Description = "if your bench saved in unsafe room".L(),
                    Values = new string[] { "" },
                    Saver = bs => bs = 0,
                    Loader = () => 0
                }
                
            ];
        }

    }
}