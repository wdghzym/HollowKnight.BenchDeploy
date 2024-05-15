using Benchwarp;
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
        private void DetectHotkeys()
        {
            if (!(GameManager.UnsafeInstance != null && GameManager.instance.IsGamePaused()))
            {
                lastkey = KeyCode.None;
                keylength = keylength2 = -1;
                return;
            }
            if (InputHandler.Instance.inputActions.superDash.WasPressed)
            {
                keylength2 = -1;
                //BenchManager.SetBench(0);
                DeployClickedbefore();
            }
            if (InputHandler.Instance.inputActions.left.WasPressed && InputHandler.Instance.inputActions.superDash.IsPressed)
            {
                keylength2++;
            }
            //if (InputHandler.Instance.inputActions.left.WasPressed && InputHandler.Instance.inputActions.superDash.IsPressed)
            if (InputHandler.Instance.inputActions.superDash.WasReleased && keylength2 >= 0)
            {
                if (keylength2 >= BenchDeploy.GS.BenchCount)
                    keylength2 = BenchDeploy.GS.BenchCount - 1;
                BenchManager.SetBench(keylength2);
                keylength2 = -1;
                TopMenu.SetClicked(null);
                ChangeScene.WarpToRespawn();
            }

            if (InputHandler.Instance.inputActions.down.WasPressed && InputHandler.Instance.inputActions.superDash.IsPressed)
            {
                TopMenu.DeployClicked(null);
                AddBench();
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

        }
        private void keyDown(KeyCode key)
        {
            if (lastkey == dbd && key == dbb)
                AddBench();
            if (key == dbd)
                DeployClickedbefore();
            if (key == wdw)
            {
                keylength++;
                BenchManager.SetBench(keylength);
            }
            else
                keylength = -1;
            if (key != wdw && key != wdd && lastkey == wdw)
            {
                BenchManager.SetBench(0);
            }
                
            //if (lastkey == key)
            //    keylength++;
            lastkey = key;
        }
        private Bench GetCurrentDeployBench()
        {
            return new Bench()
            {
                benchScene = Benchwarp.Benchwarp.LS.benchScene,
                benchX = Benchwarp.Benchwarp.LS.benchX,
                benchY = Benchwarp.Benchwarp.LS.benchY,
            };
        }
        Bench lastDeployBench;
        private void DeployClickedbefore()
        {
            lastDeployBench = GetCurrentDeployBench();
        }
        private void AddBench()
        {
            Bench bench = GetCurrentDeployBench();
            //compatible benchwarp options (cd no-air unsafe)
            BenchDeploy.LogDebug($"add Bench {bench.benchX} {bench.benchY}/ {lastDeployBench.benchX} {lastDeployBench.benchY} / {HeroController.instance.transform.position.x} {HeroController.instance.transform.position.y}");
            if ((lastDeployBench.benchScene == bench.benchScene
                    && lastDeployBench.benchX == bench.benchX
                    && lastDeployBench.benchY == bench.benchY)
                && (lastDeployBench.benchScene != GameManager.instance.sceneName
                    || lastDeployBench.benchX != HeroController.instance.transform.position.x
                    || lastDeployBench.benchY != HeroController.instance.transform.position.y))
                return;

            BenchManager.AddBench(bench);
            BenchDeploy.LogDebug("added Bench!");
        }
        
    }
}
