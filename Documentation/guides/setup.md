# Setup
## Implement Classes
First, you should implement these classes so that you have your own implementations to modify. These classes have many methods whose implementation will depend on your game.

 - SimObjectManager
 - EntityState
 - EntityController
 - EntityCombatManager
 - EntityHitboxManager
 - EntityInputManager
 - EntityPhysicsManager
 - EntityStateManager
 - AttackDefinition
 - MovesetDefinition

## Simulation
The first thing you want to do is start the simulation. You can do this whenever, but you probably want to do this as soon as the player starts the game and you want to spawn the player's character. When that does happen, you want to create an instance of the SimObjectManager, like so.

	simulationObjectManager = new SimObjectManager();
	
Once you do that, you want to call these two every fixed update. 
			
	public void FixedUpdate(){
		simulationObjectManager.Update(Time.fixedDeltaTime);
		simulationObjectManager.LateUpdate(Time.fixedDeltaTime);
	}
	
With that, the simulation is now running. The next step is to create objects and add them to the simulation. 

### Side Note: Physics
SimObjectManager defaults to updating 3D Physics. If you're using 2D Physics, you'll want to override the SimulatePhysics method like so.

	protected override void SimulatePhysics(float deltatime){
		Physics2D.Simulate(deltatime);
	}
	
### Side Note: Tick Rate
since the simulation is ticked every fixed update, how often the simulation updates depends on the value of Fixed Timestep. By default it's set to 0.02, which translates to a tick rate of 50 times a second (0.02 = 1/50). 
Most action games target a tick rate of 60 times a second, so you may want to change the value to 0.01666667 (~1/60). You also might want to make the Maximum Allowed Timestep something like 0.02 for reasons explained [here](https://johnaustin.io/articles/2019/fix-your-unity-timestep).