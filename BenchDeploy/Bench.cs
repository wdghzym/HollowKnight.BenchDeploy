using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchDeploy
{
    public struct Bench
    {
        public bool Locked;
        public float BenchX;
        public float BenchY;
        public string BenchScene;

        public Bench(string scene,float x, float y) { 
            BenchScene = scene;
            BenchX = x;
            BenchY = y;
        }
    }
}
