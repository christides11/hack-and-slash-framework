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
The first thing you want to do is start the simulation. You can do this whenever, but you probably want to do this as soon as the player starts the game and you want to spawn the player's character. When that does happen, you want to do this once.

	simulationObjectManager = new SimObjectManager();
	timeStepManager = new TimeStepManager(Tickrate, Timescale, Tickrate*2, Tickrate/2);
	timeStepManager.OnUpdate += Tick;
	
Along with that, you want to call the TimeStepManager's update method every Update, like so.
			
	public void Update(){
		timeStepManager.Update(Time.deltaTime);
	}
	
With that, the simulation is now running. The next step is to create objects and add them to the simulation. 

### Side Note
SimObjectManager defaults to updating 3D Physics. If you're using 2D Physics, you'll want to override the SimulatePhysics method like so.

	protected override void SimulatePhysics(float deltatime){
		Physics2D.Simulate(deltatime);
	}
