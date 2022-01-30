using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityModManagerNet;

namespace PauseTweaks
{
    public static class PausePatches
    {
        public static bool noReturn = false;
        [HarmonyPatch(typeof(Game),"Update")]
        public static class GameUpdatePatch
        {
            public static void Prefix(Game __instance)
            {
                if (Input.GetKeyDown(KeyCode.Escape) && !Main.ispause && Game.isPlaying)
                {
                    Main.ispause = true;
                }
                else if (Input.GetKeyDown(KeyCode.Escape) && Main.ispause)
                {
                    __instance.pause.GetComponent<PauseUI>().Resume();
                    noReturn = true;
                }
                else if (Input.GetKeyDown(KeyCode.Q) && Main.ispause)
                {
                    __instance.pause.GetComponent<PauseUI>().Exit();
                    Main.ispause = false;
                }
                else if (Input.GetKeyDown(KeyCode.R) && Main.ispause)
                {
                    __instance.pause.GetComponent<PauseUI>().Retry();
                    Main.ispause = false;
                }

                if (Input.GetKeyUp(KeyCode.Escape) && noReturn)
                {
                    noReturn = false;
                }
            }
        }

        [HarmonyPatch(typeof(PauseUI),"Resume")]
        public static class PauseUIResumePatch
        {
            public static void Prefix()
            {
                Main.ispause = false;
            }
        }

        [HarmonyPatch(typeof(PauseUI),"Pause")]
        public static class PauseUIPausePatch
        {
            public static bool Prefix()
            {
                if (noReturn)
                {
                    return false;
                }
                return true;
            }
        }
    }
}
