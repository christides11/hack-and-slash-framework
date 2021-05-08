using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace TDAction.Fighter
{
    public class FighterAnimator : MonoBehaviour
    {
        public AnimationReferenceHolder Animations { get { return animations; } }

        [SerializeField] private AnimationReferenceHolder animations;
        public Animator animator;

        PlayableGraph playableGraph;
        AnimationClipPlayable playableClip;

        [SerializeField] private double currentClipTime = 0;

        public void SetAnimations(AnimationReferenceHolder animations)
        {
            this.animations = animations;
        }

        public void SetAnimation(string animation)
        {
            AnimationClip clip = animations.GetAnimation(animation);
            if(clip == null)
            {
                Debug.LogError($"{animation} does not exist.");
                return;
            }
            if (playableClip.IsValid())
            {
                playableClip.Destroy();
            }
            if (playableGraph.IsValid())
            {
                playableGraph.Destroy();
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
                currentClipTime = playableClip.GetTime();
            }
        }
    }
}