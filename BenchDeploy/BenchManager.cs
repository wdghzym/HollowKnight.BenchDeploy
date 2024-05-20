using Benchwarp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchDeploy
{
    public static class BenchManager
    {
        public static int BenchCount = 5;
        public static void AddBench(Bench bench)
        {
            int idx = FindBench(bench.benchScene);
            if (idx >= 0)
            {
                BenchDeploy.LS.Benches.RemoveAt(idx);
            }
            else if (BenchDeploy.LS.Benches.Count >= BenchCount)
            {
                BenchDeploy.LS.Benches.RemoveAt(0);
            }
            
            BenchDeploy.LS.Benches.Add(bench);
            BenchDeploy.LogDebug("Benches:");
            foreach (var item in BenchDeploy.LS.Benches)
            {
                BenchDeploy.LogDebug($"-{item.benchScene}={item.benchX}/{item.benchY}*");
            }
        }
        public static void SetBench(int idx)
        {
            BenchDeploy.LogDebug($"SetBench {idx}");
            if (BenchDeploy.LS.Benches.Count > idx && idx >= 0)
                SetBench(BenchDeploy.LS.Benches[BenchDeploy.LS.Benches.Count - idx - 1]);
        }
        public static void SetBench(Bench bench) {
            Benchwarp.Benchwarp.LS.benchScene = bench.benchScene;
                Benchwarp.Benchwarp.LS.benchX = bench.benchX;
            Benchwarp.Benchwarp.LS.benchY = bench.benchY;
        }
        public static int FindBench(string scene)
        {
            for (int i = 0; i < BenchDeploy.LS.Benches.Count; i++)
                if (BenchDeploy.LS.Benches[i].benchScene == scene)
                {
                    return i;
                }
            return -1;
        }
        public static void ClearBench() 
        {
            BenchDeploy.LS.Benches.Clear();
        }
    }
}
