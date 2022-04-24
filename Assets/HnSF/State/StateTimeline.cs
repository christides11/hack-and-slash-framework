using UnityEngine;
using UnityEngine.Timeline;

namespace HnSF
{
    public class StateTimeline : ScriptableObject
    {
        public StateTimeline baseState;
        public int totalFrames = 10;
        public bool autoIncrement = true;
        public bool autoLoop = true;
    }
}