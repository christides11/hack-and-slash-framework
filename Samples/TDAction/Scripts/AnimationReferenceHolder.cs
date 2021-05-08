using System.Collections.Generic;
using UnityEngine;

namespace TDAction
{
    [CreateAssetMenu(fileName = "AnimationReferenceHolder", menuName = "TDA/AnimationReferenceHolder")]
    public class AnimationReferenceHolder : ScriptableObject
    {
        [System.Serializable]
        public class AnimationEntry
        {
            public string animationName;
            public AnimationClip animation;
        }

        private Dictionary<string, AnimationClip> animationDictionary = new Dictionary<string, AnimationClip>();
        [SerializeField] private AnimationEntry[] animations = new AnimationEntry[0];

        public void OnEnable()
        {
            if(animationDictionary == null)
            {
                animationDictionary = new Dictionary<string, AnimationClip>();
            }
            animationDictionary.Clear();
            for(int i = 0; i < animations.Length; i++)
            {
                if (animationDictionary.ContainsKey(animations[i].animationName.ToLower()))
                {
                    Debug.LogError($"{name} AnimationReferenceHolder has a duplicate animation for {animations[i].animationName.ToLower()}.");
                    continue;
                }
                animationDictionary.Add(animations[i].animationName.ToLower(), animations[i].animation);
            }
        }

        public AnimationClip GetAnimation(string animationName)
        {
            animationName = animationName.ToLower();
            if (animationDictionary.TryGetValue(animationName, out AnimationClip clip))
            {
                return clip;
            }
            return null;
        }
    }
}