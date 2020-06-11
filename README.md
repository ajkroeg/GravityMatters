# GravityMatters
 
This mod allows jump jet range and heat to be affected by either the biome (lunar or martian only), or by the `planet_size_xxxxx` planet/starsystem tags. Full compatible with, but does not depend on MechEngineer. Jump heat is calculated by the distance jumped. By default, this mod multiplies jump heat by `1/JumpOverride` so that the additional jump distance from the biome/low gravity does not <i>also</i> generate more heat; nor does decreased jump distance (in the case of high gravity planets) generate <i>less</i> heat.

Currently, the following settings are customizable in the mod.json;

```"Settings": {
			"UsePlanetGravityTags": true,
			"LunarJumpOverride": 1.2,
			"MartianJumpOverride": 1.2,
			"lowgravJumpOverride": 1.2,
			"highgravJumpOverride": 0.8,
			"JumpHeatOverrideFlag": false,
			"JumpHeatOverrideMultiplier": 0.83
			},
   ```
   `UsePlanetGravityTags` - bool, switches whether changes to jumpjets look for biomes or planet tags.
   
   `LunarJumpOverride` - float, multiplier for jump distance on lunar maps if `UsePlanetGravityTags: false`.
   
   `MartianJumpOverride` - float, multiplier for jump distance on martian if `UsePlanetGravityTags: false`.
   
   `lowgravJumpOverride` - float, multiplier for jump distance on planets with `planet_size_small` tag if `UsePlanetGravityTags: true`.
   
   `highgravJumpOverride` - float, multiplier for jump distance on planets with `planet_size_small` tag if `UsePlanetGravityTags: true`.
   
   `JumpHeatOverrideFlag` - bool, switch for whether jump heat generation is overriden and manually specified.
   
   `JumpHeatOverrideMultiplier` - float, multiplier for jump heat generation. Set to 1 and set `JumpHeatOverrideFlag` for vanilla jump heat calculation (more distance=more heat).
