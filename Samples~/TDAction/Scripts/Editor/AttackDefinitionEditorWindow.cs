﻿using HnSF.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TDAction.Combat
{
    public class AttackDefinitionEditorWindow : HnSF.Combat.AttackDefinitionEditorWindow
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
            if (visualFighterSceneReference)
            {
                visualFighterSceneReference.GetComponent<Fighter.FighterManager>().entityDefinition.sharedAnimations.OnEnable();
                foreach(var moveset in visualFighterSceneReference.GetComponent<Fighter.FighterManager>().entityDefinition.movesets)
                {
                    moveset.animations.OnEnable();
                }
                //visualFighterSceneReference.GetComponent<Fighter.FighterAnimator>().PlayAnimation((attack as AttackDefinition).animationName);
                visualFighterSceneReference.GetComponent<Prime31.CharacterController2D>().Awake();
            }
        }

        protected override void IncrementForward()
        {
            base.IncrementForward();
            if (visualFighterSceneReference)
            {
                visualFighterSceneReference.GetComponent<Fighter.FighterAnimator>().SetFrame(timelineFrame);
            }
        }

        protected override void MoveEntity()
        {
            Vector2 mov = visualFighterSceneReference.GetComponent<TDAction.Fighter.FighterPhysicsManager>().GetOverallForce();
            Vector3 finalMove = new Vector3(mov.x, mov.y, 0);
            finalMove *= Time.fixedDeltaTime;

            visualFighterSceneReference.transform.position += finalMove;

            TDAction.Fighter.FighterPhysicsManager pm = visualFighterSceneReference.GetComponent<TDAction.Fighter.FighterPhysicsManager>();
            pm.Tick();
        }

        protected override void ResetFighterVariables()
        {
            base.ResetFighterVariables();
            visualFighterSceneReference.GetComponent<TDAction.Fighter.FighterPhysicsManager>().forceMovement = Vector2.zero;
            hitboxes.Clear();
            hurtboxes.Clear();
            if (string.IsNullOrEmpty((attack as AttackDefinition).animationName) == false)
            {
                //visualFighterSceneReference.GetComponent<Fighter.FighterAnimator>().PlayAnimation((attack as AttackDefinition).animationName);
                visualFighterSceneReference.GetComponent<Fighter.FighterAnimator>().SetFrame(0);
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

        protected override void DrawHurtboxDefinition(StateHurtboxDefinition hurtboxDefinition)
        {
            if (visualFighterSceneReference == null)
            {
                return;
            }

            if (hurtboxDefinition == null)
            {
                return;
            }

            for(int i = 0; i < hurtboxDefinition.hurtboxGroups.Count; i++)
            {
                if(timelineFrame == hurtboxDefinition.hurtboxGroups[i].activeFramesStart
                    || hurtboxDefinition.hurtboxGroups[i].activeFramesEnd == -1)
                {
                    for (int w = 0; w < hurtboxDefinition.hurtboxGroups[i].boxes.Count; w++)
                    {
                        
                        Vector3 position = hurtboxDefinition.hurtboxGroups[i].attachToEntity 
                            ? ((HnSF.Combat.BoxDefinition)hurtboxDefinition.hurtboxGroups[i].boxes[w]).offset
                            : visualFighterSceneReference.transform.position + ((HnSF.Combat.BoxDefinition)hurtboxDefinition.hurtboxGroups[i].boxes[w]).offset;

                        hurtboxes.Add(new DrawnBoxDefinition()
                        {
                            attached = hurtboxDefinition.hurtboxGroups[i].attachToEntity,
                            group = i,
                            position = position,
                            size = ((HnSF.Combat.BoxDefinition)hurtboxDefinition.hurtboxGroups[i].boxes[w]).size
                        });
                    }
                }

                // Remove stray hitboxes
                if (hurtboxDefinition.hurtboxGroups[i].activeFramesEnd != -1 && timelineFrame == hurtboxDefinition.hurtboxGroups[i].activeFramesEnd + 1)
                {
                    for (int w = 0; w < hurtboxes.Count; w++)
                    {
                        if (hurtboxes[w].group == i)
                        {
                            hurtboxes.RemoveAt(i);
                        }
                    }
                }
            }
        }

        protected override void DrawHitboxGroup(int index, HitboxGroup hitboxGroup)
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
                    Vector3 position = hitboxGroup.attachToEntity ? ((HnSF.Combat.BoxDefinition)hitboxGroup.boxes[i]).offset
                        : visualFighterSceneReference.transform.position + ((HnSF.Combat.BoxDefinition)hitboxGroup.boxes[i]).offset;

                    hitboxes.Add(new DrawnBoxDefinition() { 
                            attached = hitboxGroup.attachToEntity, 
                            group = index, 
                            position = position,
                            size = ((HnSF.Combat.BoxDefinition)hitboxGroup.boxes[i]).size
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