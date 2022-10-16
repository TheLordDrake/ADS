using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using AlgernonCommons;
using ColossalFramework;
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

        [HarmonyPatch(typeof(DistrictTool), "OnToolGUI")]
        public static void Postfix()
        {
            lock (LockObject)
            {
                _disableSnapping = Input.GetKey(ModSettings.Hotkey);
            }
        }

        [HarmonyPatch(typeof(DistrictTool), nameof(DistrictTool.SimulationStep))]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase original) //, bool isKeyPressed, ToolBase.RaycastOutput output)
        {
            var flagCount = 0;

            // Iterate through all instructions
            var instructionEnumerator = instructions.GetEnumerator();
            while (instructionEnumerator.MoveNext())
            {
                var instruction = instructionEnumerator.Current;

                // for for initial 'brfalse' to flag start of modifications
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
    }
}
