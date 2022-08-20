#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace HnSF
{
    // https://gist.github.com/lazlo-bonin/a85586dd37fdf7cf4971d93fa5d2f6f7
    public static class UnityObjectUtility
    {		
        public static UnityObject GetPrefabDefinition(this UnityObject uo)
        {
            #if UnityEditor
            return PrefabUtility.GetPrefabParent(uo);
            #endif
            return null;
        }

        public static bool IsPrefabInstance(this UnityObject uo)
        {
            return GetPrefabDefinition(uo) != null;
        }

        public static bool IsPrefabDefinition(this UnityObject uo)
        {
            #if UNITY_EDITOR
            return GetPrefabDefinition(uo) == null && PrefabUtility.GetPrefabObject(uo) != null;
            #endif
            return false;
        }

        public static bool IsConnectedPrefabInstance(this UnityObject go)
        {
            #if UNITY_EDITOR
            return IsPrefabInstance(go) && PrefabUtility.GetPrefabObject(go) != null;
            #endif
            return false;
        }

        public static bool IsDisconnectedPrefabInstance(this UnityObject go)
        {
            #if UNITY_EDITOR
            return IsPrefabInstance(go) && PrefabUtility.GetPrefabObject(go) == null;
            #endif
            return false;
        }

        public static bool IsSceneBound(this UnityObject uo)
        {
            return
                (uo is GameObject && !IsPrefabDefinition((UnityObject)uo)) ||
                (uo is Component && !IsPrefabDefinition(((Component)uo).gameObject));
        }
    }
}