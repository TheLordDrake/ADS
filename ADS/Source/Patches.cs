using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using AlgernonCommons;
using ColossalFramework;
using ColossalFramework.UI;
using HarmonyLib;
using UnityEngine;

namespace ADS.Source
{
    [HarmonyDebug]
    [HarmonyPatch]
    public static class Patches
    {
        private static bool _disableSnapping;
        private static bool _patched;
        private static readonly object LockObject = new object();
        private static int _brushIndex = 2;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(DistrictTool), "OnToolGUI")]
        public static void LockPatch()
        {
            lock (LockObject)
            {
                _disableSnapping = Input.GetKey(ModSettings.Hotkey);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(DistrictOptionPanel), "Awake")]
        // ReSharper disable once InconsistentNaming
        public static bool BrushPatch(DistrictOptionPanel __instance)
        {
            try
            {
                var districtTool = ToolsModifierControl.GetTool<DistrictTool>();
                if (!(districtTool != null))
                    return false;
                var strip = __instance.component as UITabstrip;
                if (!(strip != null))
                    return false;

                // TODO: Add tooltips

                if (!(strip.tabs[0] is UIButton minusBtn))
                {
                    throw new NullReferenceException("Could not find Minus Button");
                }

                minusBtn.atlas = AlgernonCommons.UI.UITextures.LoadSpriteAtlas("minus_atlas",
                    new[]
                    {
                        "OptionBaseDisabled",
                        "OptionBaseFocused",
                        "OptionBasePressed",
                        "OptionBaseHovered"
                    });
                Logging.KeyMessage($">))°> ---- {minusBtn.atlas}");
                minusBtn.eventClicked += (UIComponent component, UIMouseEventParameter eventParam) =>
                {
                    if (_brushIndex <= 0)
                    {
                        return;
                    }

                    strip.selectedIndex = --_brushIndex;
                    SetBrushSize(__instance, districtTool, _brushIndex);
                    Logging.KeyMessage($"Brush index: {_brushIndex}");
                };

                if (!(strip.tabs[2] is UIButton plusBtn))
                {
                    throw new NullReferenceException("Could not find Plus Button");
                }

                plusBtn.eventClicked += (UIComponent component, UIMouseEventParameter eventParam) =>
                {
                    if (_brushIndex >= 4)
                    {
                        return;
                    }

                    strip.selectedIndex = ++_brushIndex;
                    SetBrushSize(__instance, districtTool, _brushIndex);
                    Logging.KeyMessage($"Brush index: {_brushIndex}");
                };
            }
            catch (Exception ex)
            {
                Logging.LogException(ex);
            }

            // Return before original method runs
            return false;
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(DistrictTool), nameof(DistrictTool.SimulationStep))]
        public static IEnumerable<CodeInstruction> SnapPatch(IEnumerable<CodeInstruction> instructions, MethodBase original) //, bool isKeyPressed, ToolBase.RaycastOutput output)
        {
            var flagCount = 0;

            // Iterate through all instructions
            var instructionEnumerator = instructions.GetEnumerator();
            while (instructionEnumerator.MoveNext())
            {
                var instruction = instructionEnumerator.Current;

                // look for initial 'brfalse' to flag start of modifications
                if (!_patched && instruction != null && instruction.opcode == OpCodes.Brfalse)
                {
                    flagCount++;
                }

                if (!_patched && flagCount == 2)
                {
                    _patched = true;

                    // Keep that brfalse
                    yield return instruction;

                    // Add custom method call
                    // No operand needed for ldloc.1; the operand is implicit in the opcode
                    yield return new CodeInstruction(OpCodes.Ldloc_1);

                    // Call needs an operand which is the MethodInfo of the target method.
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patches), nameof(HandleSnapping)));

                    // Set value of output / ldloc.1
                    yield return new CodeInstruction(OpCodes.Stloc_1);

                    // Drop code
                    do
                    {
                        if (!instructionEnumerator.MoveNext())
                        {
                            Logging.Error("ADS Transpiler STOP error: ldarg.0 not found");
                            break;
                        }

                        instruction = instructionEnumerator.Current;
                    } while (instruction != null && instruction.opcode != OpCodes.Ldarg_0);
                }

                yield return instruction;
            }

            instructionEnumerator.Dispose();
        }

        private static ToolBase.RaycastOutput HandleSnapping(ToolBase.RaycastOutput output)
        {
            lock (LockObject)
            {
                if (_disableSnapping)
                {
                    return output;
                }

                if (output.m_netNode != 0)
                {
                    output.m_hitPos = Singleton<NetManager>.instance.m_nodes.m_buffer[output.m_netNode].m_position;
                }
                else if (output.m_netSegment != 0)
                {
                    output.m_hitPos = Singleton<NetManager>.instance.m_segments.m_buffer[output.m_netSegment]
                        .GetClosestPosition(output.m_hitPos);
                }

                return output;
            }
        }

        private static void SetBrushSize(DistrictOptionPanel panel, DistrictTool districtTool, int index)
        {
            const int xSmallBrushSize = 25;
            const int xLargeBrushSize = 600;

            switch (index)
            {
                case 0:
                    districtTool.m_brushSize = xSmallBrushSize;
                    break;
                case 1:
                    districtTool.m_brushSize = panel.m_SmallBrushSize;
                    break;
                case 2:
                    districtTool.m_brushSize = panel.m_MediumBrushSize;
                    break;
                case 3:
                    districtTool.m_brushSize = panel.m_LargeBrushSize;
                    break;
                case 4:
                    districtTool.m_brushSize = xLargeBrushSize;
                    break;
            }
        }
    }
}
