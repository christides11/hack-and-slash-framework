# [29.0.0](https://github.com/christides11/hack-and-slash-framework/compare/v28.3.1...v29.0.0) (2021-10-22)


### Features

* Changed StateHurtboxDefinition to BoxCollectionDefinition ([2fc6494](https://github.com/christides11/hack-and-slash-framework/commit/2fc64944d8cf218dd14b6ffabb3ae0bf46ea95f5))


### BREAKING CHANGES

* Changed StateHurtboxDefinition to BoxCollectionDefinition.

## [28.3.1](https://github.com/christides11/hack-and-slash-framework/compare/v28.3.0...v28.3.1) (2021-10-10)


### Bug Fixes

* Attack useState doesn't block out all other properties ([baf0696](https://github.com/christides11/hack-and-slash-framework/commit/baf0696d13171a101f6988ee0e2ab268d27fb30e))

# [28.3.0](https://github.com/christides11/hack-and-slash-framework/compare/v28.2.1...v28.3.0) (2021-10-09)


### Features

* Added pushPullCenterOffset for HitInfo ([89c6209](https://github.com/christides11/hack-and-slash-framework/commit/89c62092e83a67ddae0a9dbcd5081d8fcabea106))

## [28.2.1](https://github.com/christides11/hack-and-slash-framework/compare/v28.2.0...v28.2.1) (2021-10-09)


### Bug Fixes

* Use PropertyField for autoLinkPercentage instead of Slider. ([85afe3c](https://github.com/christides11/hack-and-slash-framework/commit/85afe3ce47958a508a51a4fcbd8c8fd92b112463))

# [28.2.0](https://github.com/christides11/hack-and-slash-framework/compare/v28.1.0...v28.2.0) (2021-10-09)


### Features

* Added autolinkPercentage to HitInfoBase ([f2f7035](https://github.com/christides11/hack-and-slash-framework/commit/f2f70355d8a4e9ecdaf738ff1f63546e372cfeac))

# [28.1.0](https://github.com/christides11/hack-and-slash-framework/compare/v28.0.0...v28.1.0) (2021-09-03)


### Features

* Added inverse option to AttackCondition ([9dc81f3](https://github.com/christides11/hack-and-slash-framework/commit/9dc81f3ac2ae4623611ce6a7cfc1803c70d94d18))

# [28.0.0](https://github.com/christides11/hack-and-slash-framework/compare/v27.0.0...v28.0.0) (2021-09-02)


### Features

* Added attack conditions to cancel list definition. ([7e027b7](https://github.com/christides11/hack-and-slash-framework/commit/7e027b7e2c864bad184769c78ccd6ce5785b7a58))
* Added GetBounds to ITargetable ([08dd20c](https://github.com/christides11/hack-and-slash-framework/commit/08dd20cf41667b8b3e1a69a509c83fc0aa97867e))
* Added IHealable ([824c0e0](https://github.com/christides11/hack-and-slash-framework/commit/824c0e055e44eb67c5a3f091554b8a2f2bf73b10))
* FighterStateBase's OnUpdate doesn't call CheckInterrupt anymore. ([51e16f4](https://github.com/christides11/hack-and-slash-framework/commit/51e16f44d48d845f181442a51989e6c69b05b843))
* Removed commandAttackCancelWindows ([28d5530](https://github.com/christides11/hack-and-slash-framework/commit/28d553056eee28f5b9688fde2398ecf4afa5d1f2))
* Removed inputCheckProccessed ([b7ebb42](https://github.com/christides11/hack-and-slash-framework/commit/b7ebb42a8e4cf495201d765c183d22246d0acf04))
* Removed LookHandler ([4af5e77](https://github.com/christides11/hack-and-slash-framework/commit/4af5e776b92b9f10e5a1733ded86e74542fd98c8))
* Removed unnecessary IFighterBase methods ([739fca7](https://github.com/christides11/hack-and-slash-framework/commit/739fca7550c11f06dfbc5ede13b5915ebb85f63f))
* Removed unnecessary IFighterStateManager methods and added RemoveState ([ac6dfa1](https://github.com/christides11/hack-and-slash-framework/commit/ac6dfa1b8ef442a9ee87b79d8cb3d9d6356543ef))


### BREAKING CHANGES

* Removed methods in IFighterBase that are not necessary.
* Removed Tick and LateTick from IFighterStateManager, and added RemoveState.
* Removed heal method from IHurtable and make an IHealable interface for it.
* Removed inputCheckProccessed from AttackEventDefinition. This should be tracked in an actual script.
* Removed commandAttackCancelWindows. Use the cancel list feature instead.

# [27.0.0](https://github.com/christides11/hack-and-slash-framework/compare/v26.0.0...v27.0.0) (2021-09-02)


### Features

* removed attack event variables ([6049c15](https://github.com/christides11/hack-and-slash-framework/commit/6049c1512d232037fbaf801230201661223da549))


### BREAKING CHANGES

* Removed attack event variables since they are now useless.

# [26.0.0](https://github.com/christides11/hack-and-slash-framework/compare/v25.0.0...v26.0.0) (2021-08-15)


### Code Refactoring

* Moved fighter managers to interfaces ([609cb84](https://github.com/christides11/hack-and-slash-framework/commit/609cb84593517b4960661e9b42652c150912d763))


### BREAKING CHANGES

* Changed to using interfaces so that the actual manager scripts can be modified more.

# [25.0.0](https://github.com/christides11/hack-and-slash-framework/compare/v24.0.0...v25.0.0) (2021-08-07)


### Code Refactoring

* Removed input handling. ([90200c6](https://github.com/christides11/hack-and-slash-framework/commit/90200c675b1e8f9aecde0afc813c032ff61f1395))


### BREAKING CHANGES

* Removed input handling. How input is handled is dependent on the game, so there is no good general way of doing it.

# [24.0.0](https://github.com/christides11/hack-and-slash-framework/compare/v23.1.0...v24.0.0) (2021-06-22)


### Features

* Hitbox attacker can be defined, along with an ignore list. ([6a2942a](https://github.com/christides11/hack-and-slash-framework/commit/6a2942a1b5a9d04c7dddf56339ba25aa418917b0))


### BREAKING CHANGES

* HitboxManager's CheckForCollision now has the attacker and an ignorelist as parameters. Useful when the attacker is not the one with the HitboxManager script attached.

# [23.1.0](https://github.com/christides11/hack-and-slash-framework/compare/v23.0.0...v23.1.0) (2021-06-14)


### Features

* Added method to check attack node conditions ([f8f8710](https://github.com/christides11/hack-and-slash-framework/commit/f8f8710cc6812bbf3abf9c8162ff975fae10fb5a))

# [23.0.0](https://github.com/christides11/hack-and-slash-framework/compare/v22.1.0...v23.0.0) (2021-06-10)


### Bug Fixes

* HitboxManager's CheckForCollision now properly returns true/false. ([a3baebb](https://github.com/christides11/hack-and-slash-framework/commit/a3baebbd31a8ccc49d935fb2a86810e568787762))


### Features

* HitboxManager event now includes the hit reaction ([06973b4](https://github.com/christides11/hack-and-slash-framework/commit/06973b4ecb576f43aceaf9d2f0c744dbfa8ce17f))


### BREAKING CHANGES

* HitReaction changed to HitReactionBase, and is not a class instead of a struct. Move the responsibility of possible needed hitreaction variables from me to the user.

# [22.1.0](https://github.com/christides11/hack-and-slash-framework/compare/v22.0.0...v22.1.0) (2021-06-10)


### Features

* Added CheckConditions method to AttackEventDefinition ([7b56098](https://github.com/christides11/hack-and-slash-framework/commit/7b56098d4f7eadddedd420a5b95fcababdcb2778))

# [22.0.0](https://github.com/christides11/hack-and-slash-framework/compare/v21.0.0...v22.0.0) (2021-06-10)


### Code Refactoring

* Input buffer clearing, changed InputControlType to FighterControlType ([b3bcf69](https://github.com/christides11/hack-and-slash-framework/commit/b3bcf69b4c6569fd8ae25e4be390eadec3e0065a))
* Removed old editor methods ([5673935](https://github.com/christides11/hack-and-slash-framework/commit/567393574bcf3cbc9985e4b6faf709279915fe1c))


### Features

* Conditions for Attack Events. ([6c2b5ac](https://github.com/christides11/hack-and-slash-framework/commit/6c2b5ac4403c3deca024490d470d3681f216bff8))


### BREAKING CHANGES

* Replaced methods with new ones to work with the type selection menu.
* Input buffer clearing is now for all buttons, instead of being done on a button by button basis. Also changed InputControlType to FighterControlType to better represent what it's for.

# [21.0.0](https://github.com/christides11/hack-and-slash-framework/compare/v20.0.0...v21.0.0) (2021-06-04)


### Bug Fixes

* Fixed HitboxGroupPropertyDrawer overlap ([3aa186a](https://github.com/christides11/hack-and-slash-framework/commit/3aa186ac3341be54fcf42b5e6787fbe030296d2f))


### Features

* Hurtboxes now have an ID priority. ([59c9fe8](https://github.com/christides11/hack-and-slash-framework/commit/59c9fe810996e43575fde030c74ea82645bbff8a))
* Improved extendibility of editors scripts ([faca53e](https://github.com/christides11/hack-and-slash-framework/commit/faca53e86837826ac8320e50d208c28d858db340))


### BREAKING CHANGES

* Hitboxes now go through hurtboxes with the lowest IDs first.
* Adjusted editor scripts to make it easier to extend and replace parts.

# [20.0.0](https://github.com/christides11/hack-and-slash-framework/compare/v19.2.0...v20.0.0) (2021-06-02)


### Code Refactoring

* Hurtbox manager changes ([62e910d](https://github.com/christides11/hack-and-slash-framework/commit/62e910d349e48e1a97b18551571adae787fe02f7))


### BREAKING CHANGES

* Fixed issues with hurtbox manager, and allow a window to be infinite with a -1 end frame.

# [19.2.0](https://github.com/christides11/hack-and-slash-framework/compare/v19.1.1...v19.2.0) (2021-05-27)


### Features

* Added LateUpdate for states. ([7643802](https://github.com/christides11/hack-and-slash-framework/commit/76438021281ed71556d438f7eb2682973c07480d))

## [19.1.1](https://github.com/christides11/hack-and-slash-framework/compare/v19.1.0...v19.1.1) (2021-05-27)


### Bug Fixes

* Input buffer check now works correctly. ([fd52b15](https://github.com/christides11/hack-and-slash-framework/commit/fd52b1525d5c3983a6922dafa94cf76bf1a72f9e))

# [19.1.0](https://github.com/christides11/hack-and-slash-framework/compare/v19.0.2...v19.1.0) (2021-05-26)


### Features

* Added getter for attack identifier ([af3c1ef](https://github.com/christides11/hack-and-slash-framework/commit/af3c1ef87f9541af6d1a2932da0517d4cb7db63c))

## [19.0.2](https://github.com/christides11/hack-and-slash-framework/compare/v19.0.1...v19.0.2) (2021-05-26)


### Bug Fixes

* Changed hitboxmanager variables to public ([951083e](https://github.com/christides11/hack-and-slash-framework/commit/951083e5ff3510657d614e9e34dc62fadd4e52db))

## [19.0.1](https://github.com/christides11/hack-and-slash-framework/compare/v19.0.0...v19.0.1) (2021-05-25)


### Bug Fixes

* Attack editor movement changes ([cea8e76](https://github.com/christides11/hack-and-slash-framework/commit/cea8e7650efd6ef6481118b6480cac88953f31e8))

# [19.0.0](https://github.com/christides11/hack-and-slash-framework/compare/v18.0.0...v19.0.0) (2021-05-13)


### Code Refactoring

* Hitbox editor improvements ([affa0dc](https://github.com/christides11/hack-and-slash-framework/commit/affa0dcae31a36c102f03d8500e178b347ff4f45))
* IsGrounded moved from FighterBase to FighterPhysicsManager. ([5d9a763](https://github.com/christides11/hack-and-slash-framework/commit/5d9a76387e2a494502ce8701f04d73ffa996787a))
* LateTickSimObjects handled directly in Tick ([d100bd5](https://github.com/christides11/hack-and-slash-framework/commit/d100bd56a9c45c66e77e37b122da57d466a6c419))
* Moved LookHandler into Fighters namespace ([2205ef6](https://github.com/christides11/hack-and-slash-framework/commit/2205ef66d3d0e02ee85faeeb685dd33255dde9d3))


### Features

* Created HitboxManager class. ([2578004](https://github.com/christides11/hack-and-slash-framework/commit/2578004c14252ae83b478674c0a92aebc6ed8200))


### BREAKING CHANGES

* Moved LookHandler since it was the only script in it's namespace and it's usage is for fighters.
* Improved editors and got rid of hitinfo variables that aren't necessary.
* Separated out the functionality in FighterHitboxManager into a HitboxManager class. Now you can use the logic for anything that may want to handle hitboxes, and just have to override the methods that have to be changed. Also changed it to a monobehaviour.
* Moved IsGrounded to FighterPhysicsManager, as it makes more sense to be there.
* Removed LateTick method. The method it calls should be called right after SimulatePhysics anyway.

# [18.0.0](https://github.com/christides11/hack-and-slash-framework/compare/v17.0.0...v18.0.0) (2021-05-08)


### Code Refactoring

* Changed name to HnSF ([f13b8be](https://github.com/christides11/hack-and-slash-framework/commit/f13b8be0daeb5e814ff818a2b3cdbe9fd849eea5))


### BREAKING CHANGES

* Changed name from character action framework to hack and slash framework, since it is a much more well known term and easier to say.

# [17.0.0](https://github.com/christides11/Character-Action-Framework/compare/v16.0.0...v17.0.0) (2021-05-08)


### Features

* Added cancel list. ([2cccac8](https://github.com/christides11/Character-Action-Framework/commit/2cccac8f5dc95b7b8a591b78d9137fa6325caf29))


### BREAKING CHANGES

* Attacks can now refer to a cancel list on what they are able to cancel into and when, instead of a standardized "CommandNormals" list. This gives much more flexibility.

# [16.0.0](https://github.com/christides11/Character-Action-Framework/compare/v15.0.0...v16.0.0) (2021-05-07)


### Code Refactoring

*  CurrentMoveset now referred to by it's index. ([15d20f6](https://github.com/christides11/Character-Action-Framework/commit/15d20f6b3237e69b47c576376cfbf0291b98be04))
* Changed to using PropertyDrawers and SerializedObjects for editors. ([12c9483](https://github.com/christides11/Character-Action-Framework/commit/12c9483f81c4c50a0c007fbd87fa32b0955556d9))
* Hitstun/Hitstop handling changes ([faa02a6](https://github.com/christides11/Character-Action-Framework/commit/faa02a6bd3781ff790ab148647a24b45a330883e))
* MovesetAttackNodes now refered to by their ID. ([8a595df](https://github.com/christides11/Character-Action-Framework/commit/8a595dfd7e960e85fa6387aff515bf09fb048e11))


### Features

* Attack Editor now allows you to move the visual horizontally. ([9b81cf3](https://github.com/christides11/Character-Action-Framework/commit/9b81cf33aaec1079ec3073bbb07542a2a930d7f5))


### BREAKING CHANGES

* Now using propertydrawers and serializedobjects for editor. Makes undoing/redoing changes possible, and makes code for attack event editors able to be separate from the class itself.
* Current moveset tracked by it's index now for networking purposes.
* The current attack is now tracked by it's identifier instead of a direct reference to it. This was done to make it easier to send and receive the current attack through the network.
* Hitstun/hitstop can not be set directly in FighterCombatManager, use the methods instead.

# [15.0.0](https://github.com/christides11/Character-Action-Framework/compare/v14.2.1...v15.0.0) (2021-05-03)


### Code Refactoring

* Better Attack Editor and hitbox handling ([bef7b2c](https://github.com/christides11/Character-Action-Framework/commit/bef7b2cf6128fd8fcfcf73b3e1f13d8a7e1e5004))


### BREAKING CHANGES

* Major refactor of the attack editor and how hitboxes are handled.

## [14.2.1](https://github.com/christides11/Character-Action-Framework/compare/v14.2.0...v14.2.1) (2021-04-30)


### Bug Fixes

* Attack general parameters are now shown if not using a state override. ([795fee6](https://github.com/christides11/Character-Action-Framework/commit/795fee64b35dc5d21ed85ee1ec02690b774d1f08))

# [14.2.0](https://github.com/christides11/Character-Action-Framework/compare/v14.1.0...v14.2.0) (2021-04-11)


### Features

* Added SetPosition to LookHandler. ([e3e0533](https://github.com/christides11/Character-Action-Framework/commit/e3e0533e378922c77ce76c90a49474532ed3e440))

# [14.1.0](https://github.com/christides11/Character-Action-Framework/compare/v14.0.0...v14.1.0) (2021-04-08)


### Features

* Added methods to set LookHandler rotation. ([5000570](https://github.com/christides11/Character-Action-Framework/commit/50005705b5bcf295de78fd3a4deb80af9bebff0a))

# [14.0.0](https://github.com/christides11/Character-Action-Framework/compare/v13.0.0...v14.0.0) (2021-04-03)


### Code Refactoring

* Input handling changes. ([282cf1c](https://github.com/christides11/Character-Action-Framework/commit/282cf1c2e03ca7fa5e367f4bf02483ec3e3b96b0))


### BREAKING CHANGES

* InputRecord changed to circular buffer.

# [13.0.0](https://github.com/christides11/Character-Action-Framework/compare/v12.0.0...v13.0.0) (2021-03-23)


### Code Refactoring

* Reference current state using ushort instead of state itself. ([3d08151](https://github.com/christides11/Character-Action-Framework/commit/3d08151eec2a444f86978b6ec77c960d685fe364))


### BREAKING CHANGES

* Changed the state identifier from a int to a ushort.
CurrentState now refered to by it's number instead of saving a reference to it.
Removed changing state by providing a state directly.

# [12.0.0](https://github.com/christides11/Character-Action-Framework/compare/v11.0.0...v12.0.0) (2021-03-21)


### Code Refactoring

* SimulationManager changes. ([9d5dc63](https://github.com/christides11/Character-Action-Framework/commit/9d5dc63a55d6677ef269c701951686e991c6085c))


### BREAKING CHANGES

* SimObject changed to an interface.

# [11.0.0](https://github.com/christides11/Character-Action-Framework/compare/v10.0.0...v11.0.0) (2021-03-16)


### Code Refactoring

* Renaming of fighter fields. ([ee893e4](https://github.com/christides11/Character-Action-Framework/commit/ee893e45625b234bc7d77e7400b055cb459dd95e))


### Features

* Added ISimObjectManager. ([901f0a0](https://github.com/christides11/Character-Action-Framework/commit/901f0a0dbea7c83059dfdb18ef84101f70ce0797))


### BREAKING CHANGES

* Renamed FighterBase fields.

# [10.0.0](https://github.com/christides11/Character-Action-Framework/compare/v9.0.1...v10.0.0) (2021-03-08)


### Code Refactoring

* Renamed Entities to Fighters. ([b41e069](https://github.com/christides11/Character-Action-Framework/commit/b41e069c2809f9131e8632be66371319cf9e45cc))


### BREAKING CHANGES

* Renamed Entities to Fighters to better reflect what they are.

## [9.0.1](https://github.com/christides11/Character-Action-Framework/compare/v9.0.0...v9.0.1) (2021-02-01)


### Bug Fixes

* Fixed ReActivate hitbox error. ([8a2e3ef](https://github.com/christides11/Character-Action-Framework/commit/8a2e3ef76b99ac6a6da97759b5aadaac2a6ac88c))

# [9.0.0](https://github.com/christides11/Character-Action-Framework/compare/v8.1.0...v9.0.0) (2021-02-01)


### Code Refactoring

* Changed CheckHits to Tick, cleanup. ([1105768](https://github.com/christides11/Character-Action-Framework/commit/1105768c5af17496ab468f0cf7a31ddeb1f77661))


### BREAKING CHANGES

* CheckHits in HitboxBase is now called Tick, since it should be called every tick.

# [8.1.0](https://github.com/christides11/Character-Action-Framework/compare/v8.0.0...v8.1.0) (2021-01-07)


### Features

* Attack Event charge level check support. ([d45ddcc](https://github.com/christides11/Character-Action-Framework/commit/d45ddcc8fe4606e935b12fb5d46b9656189600a0))

# [8.0.0](https://github.com/christides11/Character-Action-Framework/compare/v7.0.0...v8.0.0) (2021-01-06)


### Features

* CheckForInputSequence offset support ([69243c2](https://github.com/christides11/Character-Action-Framework/commit/69243c2519b9cfe077edbd3b803cd2098b8cd5f8))


### BREAKING CHANGES

* CheckForInputSequence now has a baseOffset variable, allowing you to check for a sequences that weren't inputed recently.

# [7.0.0](https://github.com/christides11/Character-Action-Framework/compare/v6.4.0...v7.0.0) (2021-01-06)


### Features

* Lookhandler methods for lockon and lookat targets. ([fcd8000](https://github.com/christides11/Character-Action-Framework/commit/fcd80000b91aa52edc1c4e866ab8a9afad696bbb))


### BREAKING CHANGES

* Renamed SetTarget to SetLockOnTarget to better reflect it's usage. Added SetLookAtTarget to set the target the camera will follow.

# [6.4.0](https://github.com/christides11/Character-Action-Framework/compare/v6.3.1...v6.4.0) (2021-01-05)


### Features

* Hitboxes now properly have support for team detection. ([ee19f27](https://github.com/christides11/Character-Action-Framework/commit/ee19f2726ed4c4f69f98693d7ca45a2a0da1db1d))

## [6.3.1](https://github.com/christides11/Character-Action-Framework/compare/v6.3.0...v6.3.1) (2020-12-29)


### Bug Fixes

* Created values for hitstun/hitstop. ([62b7445](https://github.com/christides11/Character-Action-Framework/commit/62b74454808019a44c8d1d261cc2ca3c896daaa0))

# [6.3.0](https://github.com/christides11/Character-Action-Framework/compare/v6.2.0...v6.3.0) (2020-12-26)


### Features

* Initialize hitbox without creating hitbox. ([d4712e7](https://github.com/christides11/Character-Action-Framework/commit/d4712e7982a4b4e00f9f1d0ca56f1aa122bd1664))

# [6.2.0](https://github.com/christides11/Character-Action-Framework/compare/v6.1.0...v6.2.0) (2020-12-13)


### Features

* Upgraded to 2019.4.15f1, Fixed build error. ([399d070](https://github.com/christides11/Character-Action-Framework/commit/399d070c714fef07ca0856cb4969353af9c57359))

# [6.1.0](https://github.com/christides11/Character-Action-Framework/compare/v6.0.0...v6.1.0) (2020-11-14)


### Features

* Added event for max attack charge reached per level. ([5f0e8ee](https://github.com/christides11/Character-Action-Framework/commit/5f0e8ee5a1f06155d4131d0724afae50936b0ae8))
* Inpector code for InputDefinition & InputSequence. ([56cdec2](https://github.com/christides11/Character-Action-Framework/commit/56cdec20b895c3da1682af6b15bb13495cb74968))

# [6.0.0](https://github.com/christides11/Character-Action-Framework/compare/v5.0.0...v6.0.0) (2020-10-31)


### Code Refactoring

* Made BoxDefinitionBase the default assumed class. ([1bf3769](https://github.com/christides11/Character-Action-Framework/commit/1bf3769678ce379fbe4b6b46d1008d09ff686779))
* Removed deltatime from Simulation method variables. ([389c187](https://github.com/christides11/Character-Action-Framework/commit/389c187af64c3d00e923c2beafbefda4cec948d3))
* Removed status effects. ([1b6076d](https://github.com/christides11/Character-Action-Framework/commit/1b6076dca2c82b699ee69951eafef3d544d39f2b))
* Removed styleGain variable from BoxGroup. ([5e02bf1](https://github.com/christides11/Character-Action-Framework/commit/5e02bf1c45d45859413d713410a610e4e4487093))
* Removed variables from AttackDefinition. ([f3ed16f](https://github.com/christides11/Character-Action-Framework/commit/f3ed16f127cfa872fe96661857d46d34917adeea))
* Replaced GetCenter for GetGameObject in ITargetable. ([30de643](https://github.com/christides11/Character-Action-Framework/commit/30de643e44c821861390fcfac5fd5dc19c3d511e))
* Take InputDefinition directly for CheckStickDirection. ([48d7c65](https://github.com/christides11/Character-Action-Framework/commit/48d7c653ec9c851158aa9cb8cd9314c07e0c183e))


### Features

* Added EntityPhysicsManagerBase. ([1f6d586](https://github.com/christides11/Character-Action-Framework/commit/1f6d586344b43512cb7115501ea99da31bf036c1))
* Added HurtInfoBase to store hurt information. ([b28032b](https://github.com/christides11/Character-Action-Framework/commit/b28032ba8b2084a6109e6b5381e38720d1f3a1e2))
* Added StateHurtboxDefinition. ([36e3dff](https://github.com/christides11/Character-Action-Framework/commit/36e3dffc02db9ba9d2da17e5c71b1c4ba0c76d8f))
* Editor for StateHurtboxDefinition. ([9d26e39](https://github.com/christides11/Character-Action-Framework/commit/9d26e391eebf39463275503720f27ad0b9488bde))
* Functionally of EntityHurtboxManager. ([8d7e273](https://github.com/christides11/Character-Action-Framework/commit/8d7e27355742a0507a3c166887ad98423b124d20))


### BREAKING CHANGES

* Status effects should be handled user-side, outside of what CAF needs to do.
* PhysicsManager now allows for your own implementation, with two given implementations for 3D and 2D.
* Instead of passing the huirt variables directly in OnHurt, instead allow the user to define what should be passed in their own HurtInfoBase. Avoids assuming how the user will implement this method.
* Take InputDefinition directly now for CheckStickDirection. Avoids forcing floats in the user's system.
* With this, the center of the object and any other needed data can be grabbed, while also not assuming the numeric type to be a Vector3.
* Removed deltatime variables from method relating to the simulation. Part of changes to make using fixed point variables easier.
* BoxDefinition was replaced with BoxDefinitionBase in any case that it was reference. This change was made mainly to allow changing the type of the variables used (important if you want fixed point variables).
* Removed both heightRestriction and gravityScaleAdded from AttackDefinition. Should be implemented by the user.
* Removed styleGain variable from BoxGroup. This should be defined by the user as they might either not use a style meter at all or not want to use floats.

# [5.0.0](https://github.com/christides11/Character-Action-Framework/compare/v4.1.0...v5.0.0) (2020-09-11)


### Code Refactoring

* Changed Hitbox to HitboxBase. ([89c135d](https://github.com/christides11/Character-Action-Framework/commit/89c135dfe2ddb80eb0894f92119d58d5a26956e3))


### Features

* Added reset method to LookHandler. ([317235a](https://github.com/christides11/Character-Action-Framework/commit/317235a1e38b56b24e83c6c114e38881c87f6df2))
* Attack events can now check for input. ([9409de5](https://github.com/christides11/Character-Action-Framework/commit/9409de5c6ee7b93d0b43e6754e96821a6d66af37))


### BREAKING CHANGES

* inputs sequences put into their own class, separate from attacks. Makes it easier to define and read input for things besides attacks.
* Changed Hitbox to HitboxBase to better reflect it's usage.

# [4.1.0](https://github.com/christides11/Character-Action-Framework/compare/v4.0.0...v4.1.0) (2020-08-25)


### Features

* Added methods for inertia redirection. ([2672ff2](https://github.com/christides11/Character-Action-Framework/commit/2672ff27eb0d750817113917339d112bd8d5f9de))

# [4.0.0](https://github.com/christides11/Character-Action-Framework/compare/v3.1.0...v4.0.0) (2020-08-21)


### Code Refactoring

* Changed Team property to a method. ([a22502b](https://github.com/christides11/Character-Action-Framework/commit/a22502be88e631e29a39f2962e0e834e15622d93))
* Changed to using HitInfoBase. ([d1717dd](https://github.com/christides11/Character-Action-Framework/commit/d1717dd26db209cf1aec630376b6e6ab171adfb9))
* Removed HealthManager. ([d3c0749](https://github.com/christides11/Character-Action-Framework/commit/d3c0749377ed077ab72fff19cccb54ae2cc49de6))
* Renamed Controller to Manager. ([903ef44](https://github.com/christides11/Character-Action-Framework/commit/903ef44ed7d0620c55d6d2f082a394245012f625))
* Split Hitbox into 2D and 3D. ([23705e6](https://github.com/christides11/Character-Action-Framework/commit/23705e695d590777e0fb022139e5c44f517b6326))


### Features

* Added charge level vars to EntityCombatManager. ([066b55c](https://github.com/christides11/Character-Action-Framework/commit/066b55cbcfef88a0658705b97898f0f6ff35ff36))
* Added healing support, hit info rework. ([013265d](https://github.com/christides11/Character-Action-Framework/commit/013265d234f1fd7eb5c421dbe468bb3d5becdaa2))
* Added hitstop/stun enter and added events. ([4f5cd7d](https://github.com/christides11/Character-Action-Framework/commit/4f5cd7dbdefc18e76fdf32cf2ad82c45c557f86b))
* Added method to get hit list. ([77d6a90](https://github.com/christides11/Character-Action-Framework/commit/77d6a90f29611aed84811f4ddf564b48023c02ce))
* Added team parameter to Hitbox Initialize. ([56a2699](https://github.com/christides11/Character-Action-Framework/commit/56a2699eefa9cb5c321e5542b0b085f22fcf7ea3))
* Charge Levels ([9b1247a](https://github.com/christides11/Character-Action-Framework/commit/9b1247aba6e3e379fbcb0b27c13ba05816348e73))
* Moveset changing. ([60c2440](https://github.com/christides11/Character-Action-Framework/commit/60c24408ee3906607847b1b18aab36b95fd8d63a))
* Multiple attack editor windows can be opened. ([b8dba47](https://github.com/christides11/Character-Action-Framework/commit/b8dba47fc01a1bc3082dec2c2f85cf03a1e73201))
* StateManager related events. ([5f2d5d9](https://github.com/christides11/Character-Action-Framework/commit/5f2d5d9ab589970fbcf5984a4f95e7efb8da384c))
* Support for charge level related hitboxes. ([28418b2](https://github.com/christides11/Character-Action-Framework/commit/28418b2a0921b96b3725c62d61dedb240495091c))
* Upgraded to 2019.4.7f1. ([b5b8940](https://github.com/christides11/Character-Action-Framework/commit/b5b8940db1467282ec3b36ae07a77a3368dc97ad))
* Upgraded to 2019.4.8f1 ([3d81308](https://github.com/christides11/Character-Action-Framework/commit/3d81308917a258cc509d183e089b2981ddeb9ad6))
* Various events for CombatManager related task. ([4fecc2f](https://github.com/christides11/Character-Action-Framework/commit/4fecc2f4b455b4789ad7d1066578296be272242d))


### BREAKING CHANGES

* Initialize now takes a team parameter. Useful in case you want to assign entities to teams and define who they can attack.
* Renamed anywhere using controller to manager for EntityManager.
* Hitbox is now separated into Hitbox3D and Hitbox2D, with Hitbox being the base class of the two.
* Users may want the entity's team to be exposed in the inspector, and they may also want it to be defined by a enum. This makes sure that case is covered.
* Renamed delegates.
* Removed HealthManager since a user might want to store their health values in many different ways, such as ints or fixed point values.
* Now using HitInfoBase in places where HitInfo was being used. Makes system more extendable.
* HitStop and HitStun are now properties instead of fields.
* Upgraded to 2019.4.7f1.

# [3.1.0](https://github.com/christides11/Character-Action-Framework/compare/v3.0.0...v3.1.0) (2020-08-07)


### Features

* Added data for charged attacks. ([e1ca903](https://github.com/christides11/Character-Action-Framework/commit/e1ca9037decaf7a35681e646d02c6e5e33661ada))

# [3.0.0](https://github.com/christides11/Character-Action-Framework/compare/v2.0.0...v3.0.0) (2020-08-06)


### Code Refactoring

* Removed detect box support. ([3c64b5b](https://github.com/christides11/Character-Action-Framework/commit/3c64b5b1b5a92e79766722c46ccf828a5f2007eb))


### BREAKING CHANGES

* Removed detectbox support. Hitboxes can achieve the same effect with no damage/hitstun.

# [2.0.0](https://github.com/christides11/Character-Action-Framework/compare/v1.0.0...v2.0.0) (2020-08-06)


### Styles

* Formatting. ([e60f3cf](https://github.com/christides11/Character-Action-Framework/commit/e60f3cffed75b5a9350f23d69ea7b5e82392fe82))


### BREAKING CHANGES

* Renamed SetController.

# 1.0.0 (2020-07-30)


### Features

* semantic versioning ([171259b](https://github.com/christides11/Character-Action-Framework/commit/171259b88b777269ab7f233127d771fcb7856548))
