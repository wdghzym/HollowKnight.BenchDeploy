using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchDeploy
{
    public struct Bench
    {
        public float benchX;
        public float benchY;
        public string benchScene;

        public Bench(string scene,float x, float y) { 
            benchScene = scene;
            benchX = x;
            benchY = y;
        }
    }
}
