using HnSF.Fighters;
using System;
using TDAction.Combat;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace TDAction.Fighter
{
    public class FighterAnimator : MonoBehaviour
    {
        public delegate void AnimationEmptyAction();

        public class AnimationState
        {
            public double Time { get { return playableClip.GetTime(); } }

            public PlayableGraph playableGraph;
            public AnimationClipPlayable playableClip;
            public AnimationEmptyAction onEnd;
            public AnimationClip clip;

            public void Cleanup()
            {
                playableClip.Destroy();
                playableGraph.Destroy();
            }

            public void SetTime(double value)
            {
                playableClip.SetTime(value);
                if (playableClip.GetTime() >= clip.length)
                {
                    switch (clip.wrapMode)
                    {
                        case WrapMode.ClampForever:
                            playableClip.SetTime(clip.length);
                            break;
                    }
                    onEnd?.Invoke();
                }
            }
        }

        [SerializeField] private FighterManager manager;
        public Animator animator;
        public AnimationState currentAnimationState;

        public AnimationState PlayAnimation(AnimationClip animationClip)
        {
            if(animationClip == null)
            {
                return null;
            }
            if (currentAnimationState != null)
            {
                currentAnimationState.Cleanup();
            }
            currentAnimationState = new AnimationState();
            currentAnimationState.playableGraph = PlayableGraph.Create();
            var playableOutput = AnimationPlayableOutput.Create(currentAnimationState.playableGraph, "Animation", animator);
            currentAnimationState.playableClip = AnimationClipPlayable.Create(currentAnimationState.playableGraph, animationClip);
            playableOutput.SetSourcePlayable(currentAnimationState.playableClip);
            currentAnimationState.playableGraph.Play();
            currentAnimationState.playableClip.Pause();
            currentAnimationState.clip = animationClip;
            return currentAnimationState;
        }

        public void SetFrame(int frame)
        {
            if(currentAnimationState != null)
            {
                currentAnimationState.SetTime(frame * Time.fixedDeltaTime);
            }
        }

        public void SetTime(float time)
        {
            if (currentAnimationState != null)
            {
                currentAnimationState.SetTime(time);
            }
        }
    }
}