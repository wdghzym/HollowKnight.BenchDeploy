using HutongGames.PlayMaker.Actions;
using MagicUI.Core;
using MagicUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Mono.Security.X509.X509Stores;

namespace BenchDeploy
{
    internal class UIBenchList
    {
        internal bool Visibility { get { return visibility; } set
            {
                if (visibility != value)
                {
                    visibility = value;
                    UpdateName();
                    foreach (var item in TOBenchNames)
                        item.Visibility = value ? MagicUI.Core.Visibility.Visible : MagicUI.Core.Visibility.Hidden;
                    //BenchDeploy.LogDebug($"UIBenchList Visibility{value}");
                }
            } }
        private bool visibility = false;
        List<TextObject> TOBenchNames = new List<TextObject>();
        LayoutRoot? layout = new(true, "Persistent layout");
        internal UIBenchList() {

            for (int i = 0; i < BenchManager.BenchMaxCount; i++)
            {
                TOBenchNames.Add(
                new TextObject(layout)
                {
                    TextAlignment = HorizontalAlignment.Left,
                    Text = "test",
                    Visibility= MagicUI.Core.Visibility.Hidden,
                    //Padding = new(BenchDeploy.GS.UIBenchListX, BenchDeploy.GS.UIBenchListY - i * 20f,0f,0f),
                    Padding = new(285, 380 - i * 20f, 0f, 0f),
                    FontSize = 16
                });
            }
        }
        internal void UpdateName()
        {
            for (int i = 0; i < TOBenchNames.Count; i++)
            {
                try
                {
                    bool tgb = BenchManager.TryGetBench(i, out Bench bench);
                    if (tgb)
                    {
                        TOBenchNames[i].Text = bench.BenchScene.SN();
                        //BenchDeploy.LogDebug($"UpdateUI {i} = {bench.benchScene} {bench.benchScene.SN()}");
                    }
                    else
                        TOBenchNames[i].Text = "";

                    if (tgb && bench.Locked == true)
                    {
                        TOBenchNames[i].ContentColor = UnityEngine.Color.cyan;
                    }
                    else
                    {
                        TOBenchNames[i].ContentColor = UnityEngine.Color.white;
                    }
                }
                catch (Exception e)
                {
                    BenchDeploy.LogDebug($"error UpdateUI {e.Message}");
                    TOBenchNames[i].Text = "";
                }
            }
        }
        internal void SetSelect(int idx)
        {
            for (int i = 0; i < TOBenchNames.Count; i++)
            {
                bool tgb = BenchManager.TryGetBench(i, out Bench bench);
                if (i == idx)
                {
                    TOBenchNames[i].ContentColor = UnityEngine.Color.yellow;
                }
                else if (tgb && bench.Locked == true)
                {
                    TOBenchNames[i].ContentColor = UnityEngine.Color.cyan;
                }
                else
                {
                    TOBenchNames[i].ContentColor = UnityEngine.Color.white;
                }

            }
        }
    }
}
