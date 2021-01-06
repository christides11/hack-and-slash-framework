# Setup

## Implement Classes
First, you should create scripts that inherit from the ones listed below. These classes contain methods that you'll want to override depending on your game's needs, so it's important to do this now instead of potentially having to switch out the CAF component later.

 - @CAF.Simulation.SimObjectManager 
 - @CAF.Entities.EntityState
 - Entity Managers (Basically anything under @"CAF.Entities")
 - AttackDefinition
 - MovesetDefinition

## Run the Simulation
In CAF, every object that exist in the game depends on the SimObjectManager. This script handles the creation and tracking of these objects, along with calling their update methods and simulating physics. Because of this, the next thing we want to do is create an instance of this class somewhere, perferably when your game is in the state where you want to instantiate the player's character and other stage elements.

	simulationObjectManager = new SimObjectManager();
	
Once you do that, you want to call these two every fixed update. 
			
	public void FixedUpdate(){
		simulationObjectManager.Update(Time.fixedDeltaTime);
		simulationObjectManager.LateUpdate(Time.fixedDeltaTime);
	}
	
With that, the simulation should run, but right now we have nothing in the simulation.    
In the next step we'll walk through the components of a entity and show how to add objects to the simulation.

### Side Note: Physics
SimObjectManager defaults to updating 3D Physics. If you're using 2D Physics, you'll want to override the SimulatePhysics method like so.

	protected override void SimulatePhysics(float deltatime){
		Physics2D.Simulate(deltatime);
	}
	
### Side Note: Tick Rate
since the simulation is ticked every fixed update, how often the simulation updates depends on the value of [Fixed Timestep](https://docs.unity3d.com/Manual/class-TimeManager.html). By default it's set to 0.02, which translates to a tick rate of 50 times a second (0.02 = 1/50). 

Most action games target a tick rate of 60 times a second, so you may want to change the value to 0.01666667 (~1/60). You also might want to make the Maximum Allowed Timestep something like 0.02 for reasons explained [here](https://johnaustin.io/articles/2019/fix-your-unity-timestep).
