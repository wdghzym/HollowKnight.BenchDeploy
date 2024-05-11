using Modding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace BenchDeploy
{
    public class BenchDeploy : Mod,ILocalSettings<SaveSettings>
    {
        public static BenchDeploy Instance;
        public static GlobalSettings GS = new();
        public static SaveSettings LS = new();
        public static bool inGame = false;

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
            GUIController.Setup();

            On.GameManager.StartNewGame += StartNewGame;
            On.GameManager.ContinueGame += ContinueGame;
            On.QuitToMenu.Start += QuitToMenu;
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

        public void OnLoadLocal(SaveSettings s)
        {
            LS = s;
        }

        public SaveSettings OnSaveLocal()
        {
            return LS;
        }
    }
}