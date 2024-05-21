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
        public static int BenchMaxCount = 5;
        public static int GetBenchCount() => BenchDeploy.LS.Benches.Count;
        public static bool AddBench(Bench bench)
        {
            int idx = FindBenchInternal(bench.BenchScene);
            if (idx.ioc())
            {
                bench.Locked = BenchDeploy.LS.Benches[idx].Locked;
                BenchDeploy.LS.Benches.RemoveAt(idx);
            }
            else if (GetBenchCount() >= BenchMaxCount)
            {
                for (int i = 0; i < GetBenchCount(); i++)
                {
                    if (!BenchDeploy.LS.Benches[i].Locked)
                    {
                        BenchDeploy.LS.Benches.RemoveAt(i);
                        break;
                    }
                }
            }
            if (GetBenchCount() < BenchMaxCount)
            {
                BenchDeploy.LS.Benches.Add(bench);
                BenchDeploy.LogDebug("Benches:");
                foreach (var item in BenchDeploy.LS.Benches)
                {
                    BenchDeploy.LogDebug($"    new Bench(\"{item.BenchScene}\",{item.BenchX}f,{item.BenchY}f), lock:{item.Locked}");
                }
                BenchDeploy.uIBenchList.UpdateName();
                return true;
            }
            return false;
        }
        public static bool TryGetBench(int idx, out Bench bench)
        {
            if (idx.ioc())
            {
                bench = BenchDeploy.LS.Benches[idx.ir()];
                return true;
            }
            bench = new();
            return false;
        }
        public static void SetBench(int idx)
        {
            BenchDeploy.LogDebug($"SetBench {idx}");
            if (idx.ioc())
                SetBench(BenchDeploy.LS.Benches[idx.ir()]);
        }
        public static void SetBench(Bench bench) {
            Benchwarp.Benchwarp.LS.benchScene = bench.BenchScene;
                Benchwarp.Benchwarp.LS.benchX = bench.BenchX;
            Benchwarp.Benchwarp.LS.benchY = bench.BenchY;
        }
        private static int FindBenchInternal(string scene)
        {
            for (int i = 0; i < GetBenchCount(); i++)
                if (BenchDeploy.LS.Benches[i].BenchScene == scene)
                {
                    return i;
                }
            return -1;
        }
        public static int FindBench(string scene)
        {
            return FindBenchInternal(scene).ir();
        }

        public static void LockBench(int idx, bool? locked = null)
        {
            if (idx.ioc())
            {
                Bench bench = BenchDeploy.LS.Benches[idx.ir()];
                bench.Locked = locked is null ? !bench.Locked : locked.Value;
                BenchDeploy.LS.Benches[idx.ir()] = bench;
            }
        }
        public static void ClearBench() 
        {
            BenchDeploy.LS.Benches.Clear();
        }
        public static bool ioc(this int idx) {
            return (idx >= 0 && idx < GetBenchCount());
        }
        public static int ir(this int idx)
        {
            return GetBenchCount() - idx - 1;
        }
    }
}
