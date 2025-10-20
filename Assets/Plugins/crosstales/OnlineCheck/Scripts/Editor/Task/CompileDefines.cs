#if UNITY_EDITOR
using UnityEditor;

namespace Crosstales.OnlineCheck.EditorTask
{
   /// <summary>Adds the given define symbols to PlayerSettings define symbols.</summary>
   [InitializeOnLoad]
   public class CompileDefines : Crosstales.Common.EditorTask.BaseCompileDefines
   {
      private const string symbol = "CT_OC";

      static CompileDefines()
      {
         if (Crosstales.OnlineCheck.EditorUtil.EditorConfig.COMPILE_DEFINES)
         {
            addSymbolsToAllTargets(symbol);
         }
         else
         {
            removeSymbolsFromAllTargets(symbol);
         }
      }
   }
}
#endif
// © 2017-2024 crosstales LLC (https://www.crosstales.com)