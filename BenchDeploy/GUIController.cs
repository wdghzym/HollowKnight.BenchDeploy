using Benchwarp;
using InControl;
using Modding.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static tk2dSpriteCollectionDefinition;

namespace BenchDeploy
{
    public class GUIController : MonoBehaviour
    {
        public static void Setup()
        {
            GameObject GUIObj = new("BenchDeploy GUI");
            _instance = GUIObj.AddComponent<GUIController>();
            DontDestroyOnLoad(GUIObj);
        }
        public static void Unload()
        {
        }
        private static GUIController _instance;

        public void Update()
        {
            if (!BenchDeploy.inGame) return;
            DetectHotkeys();
        }
        private KeyCode dbd = KeyCode.D, dbb = KeyCode.B, wdw = KeyCode.W, wdd = KeyCode.D;
        private KeyCode lastkey = KeyCode.None;
        private int keylength = 0, keylength2 = 0;
        private string WTS = "";
        private void DetectHotkeys()
        {
            if (BenchDeploy.GS.TryWarpToStart)
            {
                BenchDeploy.GS.TryWarpToStart = false;
                Events.SetToStart();
                ChangeScene.WarpToRespawn();
            }
            for (KeyCode letter = KeyCode.A; letter <= KeyCode.Z; letter++)
            {
                if (Input.GetKeyDown(letter))
                {
                    WTS += letter.ToString();
                    if (WTS.Length > 11)
                        WTS = WTS.Substring(1);
                    if (Input.GetKeyDown(KeyCode.T))
                        BenchDeploy.LogDebug($"WTS = {WTS}");
                    if (WTS == "WARPTOSTART")
                    {
                        WTS = "";
                        Events.SetToStart();
                        ChangeScene.WarpToRespawn();
                    }
                }
            }

            if (!(GameManager.UnsafeInstance != null && GameManager.instance.IsGamePaused()))
            {
                lastkey = KeyCode.None;
                if (keylength != -1 || keylength2 != -1)
                {
                    keylength = keylength2 = -1;
                    BenchDeploy.uIBenchList.SetSelect(keylength2);
                }
                BenchDeploy.uIBenchList.Visibility = false;
                return;
            }
            else
                BenchDeploy.uIBenchList.Visibility = true;
            if (InputHandler.Instance.inputActions.superDash.WasPressed)//按下
            {
                keylength2 = -1;
                BenchDeploy.uIBenchList.SetSelect(keylength2);
                //BenchManager.SetBench(0);
                DeployClickedbefore();
            }
            if (InputHandler.Instance.inputActions.left.WasPressed && InputHandler.Instance.inputActions.superDash.IsPressed) //按住+左
            {
                keylength2++;
                if (keylength2 >= BenchManager.GetBenchCount())
                    keylength2 = -1;
                BenchDeploy.uIBenchList.SetSelect(keylength2);
            }
            if (InputHandler.Instance.inputActions.right.WasPressed && InputHandler.Instance.inputActions.superDash.IsPressed) 
            {
                if (keylength2 != -1)
                {
                    BenchManager.LockBench(keylength2);
                    keylength2 = -1;
                    BenchDeploy.uIBenchList.SetSelect(keylength2);
                }
                else
                {
                    TopMenu.DeployClicked(null);
                    if (AddBench())
                    {
                        if(BenchManager.TryGetBench(0,out Bench bench))
                        {
                            BenchManager.LockBench(0, true);
                        }
                    }
                    BenchDeploy.uIBenchList.SetSelect(keylength2);
                }
            }
            //if (InputHandler.Instance.inputActions.left.WasPressed && InputHandler.Instance.inputActions.superDash.IsPressed)
            if (InputHandler.Instance.inputActions.superDash.WasReleased && keylength2 >= 0)//松开
            {
                if (keylength2 >= BenchManager.GetBenchCount())
                    keylength2 = BenchManager.GetBenchCount() - 1;
                BenchManager.SetBench(keylength2);
                keylength2 = -1;
                BenchDeploy.uIBenchList.SetSelect(keylength2);
                TopMenu.SetClicked(null);
                ChangeScene.WarpToRespawn();
            }

            if (InputHandler.Instance.inputActions.down.WasPressed && InputHandler.Instance.inputActions.superDash.IsPressed)
            {
                TopMenu.DeployClicked(null);
                AddBench();
                keylength2 = -1;
                BenchDeploy.uIBenchList.SetSelect(keylength2);
            }
            if (InputHandler.Instance.inputActions.up.WasPressed && InputHandler.Instance.inputActions.superDash.IsPressed)
            {
                ChangeScene.WarpToRespawn();
                return;
            }

            for (KeyCode letter = KeyCode.A; letter <= KeyCode.Z; letter++)
            {
                if (Input.GetKeyDown(letter))
                {
                    keyDown(letter);
                }
            }
            for (KeyCode alpha = KeyCode.Alpha0; alpha <= KeyCode.Alpha9; alpha++)
            {
                if (Input.GetKeyDown(alpha))
                {
                    keyDown(alpha);
                }
            }
            for (KeyCode pad = KeyCode.Keypad0; pad <= KeyCode.Keypad9; pad++)
            {
                if (Input.GetKeyDown(pad))
                {
                    keyDown(pad);
                }
            }
#if DEBUG
            if (Input.GetKeyDown(KeyCode.PageDown))
            {
                bossBenchidx--;
                if (bossBenchidx >= 0 && bossBenchidx < bossBenchs.Count)
                {
                    BenchManager.SetBench(bossBenchs[bossBenchidx]);
                    TopMenu.SetClicked(null);
                    ChangeScene.WarpToRespawn();
                }
            }
            if (Input.GetKeyDown(KeyCode.PageUp))
            {
                bossBenchidx++;
                if (bossBenchidx >= 0 && bossBenchidx < bossBenchs.Count)
                {
                        BenchManager.SetBench(bossBenchs[bossBenchidx]);
                        TopMenu.SetClicked(null);
                        ChangeScene.WarpToRespawn();
                }
            }
#endif
        }
        private void keyDown(KeyCode key)
        {
            if (BenchDeploy.GS.BenchwarpHotkey)
            {
                if (lastkey == dbd && key == dbb)
                    AddBench();
                if (key == dbd)
                    DeployClickedbefore();
                if (key == wdw)
                {
                    keylength++;
                    BenchDeploy.uIBenchList.SetSelect(keylength);
                    BenchManager.SetBench(keylength);
                }
                else if(keylength != -1)
                {
                    keylength = -1;
                    BenchDeploy.uIBenchList.SetSelect(keylength);
                }
                if (key != wdw && key != wdd && lastkey == wdw)
                {
                    BenchManager.SetBench(0);
                }
            }
            //if (lastkey == key)
            //    keylength++;
            lastkey = key;
        }
        private Bench GetCurrentDeployBench()
        {
            return new Bench()
            {
                BenchScene = Benchwarp.Benchwarp.LS.benchScene,
                BenchX = Benchwarp.Benchwarp.LS.benchX,
                BenchY = Benchwarp.Benchwarp.LS.benchY,
            };
        }
        Bench lastDeployBench;
        private void DeployClickedbefore()
        {
            lastDeployBench = GetCurrentDeployBench();
        }
        private bool AddBench()
        {
            Bench bench = GetCurrentDeployBench();
            //compatible benchwarp options (cd no-air unsafe)
            BenchDeploy.LogDebug($"add Bench {bench.BenchX} {bench.BenchY}/ {lastDeployBench.BenchX} {lastDeployBench.BenchY} / {HeroController.instance.transform.position.x} {HeroController.instance.transform.position.y}");
            if ((lastDeployBench.BenchScene == bench.BenchScene
                    && lastDeployBench.BenchX == bench.BenchX
                    && lastDeployBench.BenchY == bench.BenchY)
                && (lastDeployBench.BenchScene != GameManager.instance.sceneName
                    || lastDeployBench.BenchX != HeroController.instance.transform.position.x
                    || lastDeployBench.BenchY != HeroController.instance.transform.position.y))
                return false;

            if (BenchManager.AddBench(bench))
            {
                BenchDeploy.LogDebug("added Bench!");
                return true;
            }
            BenchDeploy.LogDebug("add Bench Fail!");
            return false;
        }
#if DEBUG
        int bossBenchidx = -1;
        List<Bench> bossBenchs = new List<Bench>() {
            new Bench("Fungus1_20_v02",44.87786f,13.40812f),
            new Bench("Crossroads_04",89.50107f,15.40812f),
            new Bench("Crossroads_10",12.23085f,27.40957f),
            new Bench("Fungus1_29",56.22735f,7.408124f),
            new Bench("Fungus1_04",36.53254f,28.39217f),
            new Bench("Cliffs_02",61.43559f,33.40812f),
            new Bench("Waterways_05",88.64581f,7.408124f),
            new Bench("Ruins1_23",20.79554f,74.40811f),
            new Bench("Crossroads_09",71.88161f,4.408116f),
            new Bench("RestingGrounds_02",105.8514f,11.40812f),
            new Bench("Mines_18",34.34601f,11.40812f),
            new Bench("Ruins1_24",38.17287f,29.41362f),
            new Bench("Room_Colosseum_01",33.76233f,6.527729f),
            new Bench("Fungus2_15",38.41888f,7.408124f),
            new Bench("Fungus3_40",79.82953f,10.40812f),
            new Bench("Deepnest_32",67.00544f,4.408116f),
            new Bench("Waterways_12",24.59712f,23.45116f),
            new Bench("Abyss_19",36.9612f,28.40812f),
            new Bench("Hive_05",55.64154f,27.40812f),
            new Bench("Fungus2_32",48.58864f,3.408118f),
            new Bench("Ruins2_11",41.07265f,95.40811f),
            new Bench("Grimm_Main_Tent",69.38802f,6.408124f),
            new Bench("Deepnest_40",46.30303f,14.40812f),
            new Bench("Room_Bretta_Basement",22.648f,6.408124f),
            new Bench("Fungus3_archive_02",53.39156f,130.3407f),
            new Bench("Deepnest_East_Hornet",14.92828f,28.40812f),
            new Bench("Fungus1_35",46.92358f,3.408124f),
            new Bench("Fungus3_23",41.27085f,21.40812f),
            new Bench("Waterways_15",21.18322f,4.408124f),
            new Bench("Deepnest_East_10",14.51538f,3.408124f),
            new Bench("Ruins2_03",28.24735f,70.40811f)
        };
#endif
    }
}
