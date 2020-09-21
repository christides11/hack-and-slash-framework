# Entity Setup

## Overview
In CAF, an Entity refers to objects that take in input (weither it be from the player or from AI) to decide what state it should be in/transition into. It has helpers for physics force handling, combat related task such as hitbox creation and attack processing, and other various methods. 
All of these combine to create an Entity, which will usually be either a player controlled character or an enemy. Here we'll walk through the components required for a entity along with what they do.

## GameObject Setup
First let's set up the GameObject that will contain all our scripts. 
Create a GameObject and attach a collider and a Rigidbody (or whatever you use to control the character's forces, such as a [Character Controller](https://docs.unity3d.com/ScriptReference/CharacterController.html)). Turn off the Rigidbody's gravity and set it's Angular Drag to 0. It should look like this.


For the model or sprite of the Entity, you should assign it as a child of this GameObject. You should end up with a hierarchy like so.

