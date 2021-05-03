using CAF.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TDAction.Combat
{
    public class AttackDefinitionEditorWindow : CAF.Combat.AttackDefinitionEditorWindow
    {
        public struct DrawnBoxDefinition
        {
            public int group;
            public bool attached;
            public Vector2 position;
            public Vector2 size;
        }

        public List<DrawnBoxDefinition> hitboxes = new List<DrawnBoxDefinition>();
        public List<DrawnBoxDefinition> hurtboxes = new List<DrawnBoxDefinition>();

        public static void Init(AttackDefinition attack)
        {
            AttackDefinitionEditorWindow window = ScriptableObject.CreateInstance<AttackDefinitionEditorWindow>();
            window.attack = attack;
            window.Show();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            hitboxes.Clear();
            hurtboxes.Clear();
        }

        protected override void CreateFighter()
        {
            base.CreateFighter();
            visualFighterSceneReference.GetComponent<Entities.EntityAnimator>().animations.OnEnable();
            visualFighterSceneReference.GetComponent<Entities.EntityAnimator>().SetAnimation((attack as AttackDefinition).animationName);
            visualFighterSceneReference.GetComponent<Prime31.CharacterController2D>().Awake();
        }

        protected override void IncrementForward()
        {
            base.IncrementForward();
            visualFighterSceneReference.GetComponent<Entities.EntityAnimator>().SetFrame(timelineFrame);
        }

        protected override void MoveEntity()
        {
            //base.MoveEntity();
            TDAction.Entities.CharacterPhysicsManager pm = visualFighterSceneReference.GetComponent<TDAction.Entities.CharacterPhysicsManager>();
            pm.Tick();
        }

        protected override void ResetFighterVariables()
        {
            base.ResetFighterVariables();
            hitboxes.Clear();
            hurtboxes.Clear();
            if (string.IsNullOrEmpty((attack as AttackDefinition).animationName) == false)
            {
                visualFighterSceneReference.GetComponent<Entities.EntityAnimator>().SetAnimation((attack as AttackDefinition).animationName);
                visualFighterSceneReference.GetComponent<Entities.EntityAnimator>().SetFrame(0);
            }
        }

        protected override void DrawHitboxes()
        {
            if (visualFighterSceneReference == null)
            {
                return;
            }
            Handles.color = Color.green;
            foreach (var hitbox in hitboxes){
                if (hitbox.attached)
                {
                    Handles.DrawWireCube(visualFighterSceneReference.transform.position + (Vector3)hitbox.position, hitbox.size);
                }
                else
                {
                    Handles.DrawWireCube(hitbox.position, hitbox.size);
                }
            }
        }

        protected override void DrawHurtboxes()
        {
            if (visualFighterSceneReference == null)
            {
                return;
            }
            Handles.color = Color.blue;
            foreach (var hurtbox in hurtboxes)
            {
                if (hurtbox.attached)
                {
                    Handles.DrawWireCube(visualFighterSceneReference.transform.position + (Vector3)hurtbox.position, hurtbox.size);
                }
                else
                {
                    Handles.DrawWireCube(hurtbox.position, hurtbox.size);
                }
            }
        }

        protected override void HandleHurtboxGroup(int index, HurtboxGroup hurtboxGroup)
        {
            if (visualFighterSceneReference == null)
            {
                return;
            }

            if(timelineFrame == hurtboxGroup.activeFramesStart)
            {
                for(int i = 0; i < hurtboxGroup.boxes.Count; i++)
                {
                    Vector3 position = hurtboxGroup.attachToEntity ? ((CAF.Combat.BoxDefinition)hurtboxGroup.boxes[i]).offset
                        : visualFighterSceneReference.transform.position + ((CAF.Combat.BoxDefinition)hurtboxGroup.boxes[i]).offset;

                    hurtboxes.Add(new DrawnBoxDefinition()
                    {
                        attached = hurtboxGroup.attachToEntity,
                        group = index,
                        position = position,
                        size = ((CAF.Combat.BoxDefinition)hurtboxGroup.boxes[i]).size
                    });
                }
            }

            // Remove stray hitboxes
            if (timelineFrame == hurtboxGroup.activeFramesEnd + 1)
            {
                for (int i = 0; i < hurtboxes.Count; i++)
                {
                    if (hurtboxes[i].group == index)
                    {
                        hurtboxes.RemoveAt(i);
                    }
                }
            }
        }

        protected override void HandleHitboxGroup(int index, HitboxGroup hitboxGroup)
        {
            if (visualFighterSceneReference == null)
            {
                return;
            }

            // Create the hitboxes
            if(timelineFrame == hitboxGroup.activeFramesStart)
            {
                for(int i = 0; i < hitboxGroup.boxes.Count; i++)
                {
                    Vector3 position = hitboxGroup.attachToEntity ? ((CAF.Combat.BoxDefinition)hitboxGroup.boxes[i]).offset
                        : visualFighterSceneReference.transform.position + ((CAF.Combat.BoxDefinition)hitboxGroup.boxes[i]).offset;

                    hitboxes.Add(new DrawnBoxDefinition() { 
                            attached = hitboxGroup.attachToEntity, 
                            group = index, 
                            position = position,
                            size = ((CAF.Combat.BoxDefinition)hitboxGroup.boxes[i]).size
                        });
                }
            }

            // Remove stray hitboxes
            if(timelineFrame == hitboxGroup.activeFramesEnd + 1)
            {
                for(int i = 0; i < hitboxes.Count; i++)
                {
                    if(hitboxes[i].group == index)
                    {
                        hitboxes.RemoveAt(i);
                    }
                }
            }
        }
    }
}