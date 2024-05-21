using Language;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchDeploy
{
    internal static class SceneName
    {
        private static Dictionary<string, string> _map;

        internal static void Setup()
        {
            try
            {
                _map = JsonUtil.Deserialize<Dictionary<string, string>>($"{nameof(BenchDeploy)}.Resources.SceneName.json");
            }
            catch (Exception e)
            {
                BenchDeploy.LogDebug($"Error SceneName: {e}");
                _map = null;
            }
        }

        public static string GetName(string scene)
        {
            return _map is not null && _map.TryGetValue(scene, out string newText) ? newText : scene;
        }
        //public static string BL(this string text) => Benchwarp.Localization.Localize(text);
        public static string SN(this string scene) => BenchDeploy.GS.OriginalSceneName ? scene : GetName(scene.L());
    }
}
