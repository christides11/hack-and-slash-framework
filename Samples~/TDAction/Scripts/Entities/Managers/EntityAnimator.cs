using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace TDAction.Entities
{
    public class EntityAnimator : MonoBehaviour
    {
        public AnimationReferenceHolder animations;
        public Animator animator;

        PlayableGraph playableGraph;
        AnimationClipPlayable playableClip;

        public void SetAnimation(string animation)
        {
            AnimationClip clip = animations.GetAnimation(animation);
            if(clip == null)
            {
                Debug.LogError($"{animation} does not exist.");
                return;
            }
            playableGraph = PlayableGraph.Create();
            var playableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", animator);
            playableClip = AnimationClipPlayable.Create(playableGraph, clip);
            playableOutput.SetSourcePlayable(playableClip);
            playableGraph.Play();
            playableClip.Pause();
        }

        public void SetFrame(int frame)
        {
            if (playableClip.IsValid())
            {
                playableClip.SetTime(frame * Time.fixedDeltaTime);
            }
        }
    }
}