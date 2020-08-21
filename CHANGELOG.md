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
