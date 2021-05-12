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
        public AnimationReferenceHolder[] AnimationReferences { get { return animations; } }

        [SerializeField] private FighterManager manager;
        [SerializeField] private AnimationReferenceHolder[] animations = new AnimationReferenceHolder[2];
        public Animator animator;

        PlayableGraph playableGraph;
        AnimationClipPlayable playableClip;

        [SerializeField] private double currentClipTime = 0;
        [SerializeField] private string currentClipIdentifier;

        public void SetMovesetAnimations(AnimationReferenceHolder animations)
        {
            this.animations[0] = animations;
        }

        public void SetSharedAnimations(AnimationReferenceHolder animations)
        {
            this.animations[1] = animations;
        }

        public void Refresh()
        {
            SetAnimation(currentClipIdentifier);
            SetTime(currentClipTime);
        }

        public void SetAnimation(string animationName)
        {
            AnimationClip clip = FindAnimation(animationName);
            if(clip == null)
            {
                //Debug.LogError($"FighterAnimator: {animation} does not exist.");
                return;
            }
            currentClipIdentifier = animationName;
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

        private AnimationClip FindAnimation(string animationName)
        {
            if (animations[0].TryGetAnimation(animationName, out AnimationClip movesetAnimation))
            {
                return movesetAnimation;
            }
            if(animations[1].TryGetAnimation(animationName, out AnimationClip sharedAnimation))
            {
                return sharedAnimation;
            }
            return null;
        }

        public void SetFrame(int frame)
        {
            if (playableClip.IsValid())
            {
                playableClip.SetTime(frame * Time.fixedDeltaTime);
                currentClipTime = playableClip.GetTime();
            }
        }

        public void SetTime(double time)
        {
            if (playableClip.IsValid())
            {
                playableClip.SetTime(time);
                currentClipTime = playableClip.GetTime();
            }
        }
    }
}