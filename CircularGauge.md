## Overview ##

The circular gauge control consists of 2 main parts in addition to the basic control background and gloss: the **axis** and the **needle**. The needle points to the gauge's Value, and transitions between values are animated by default. Almost all of the style and functionality attributes can be customized through the Visual Studio designer or programmatically.

<img src='http://wcontrols.googlecode.com/svn/wiki/images/gaugeWhiteRed.png' alt='a circular gauge' title='An example CircularGauge'></img>
<img src='http://wcontrols.googlecode.com/svn/wiki/images/gaugeAsClock.png' alt='a circular gauge' title='CircularGauge used as clock'></img>

## Properties ##

**Axis** ([CircularAxis](CircularAxis.md)) - the CircularAxis associated with the gauge for drawing ticks and labels.

**Needle** ([Needle](Needle.md))- the Needle associated with the gauge to draw the hub, needle, and their shadows.

**Value** (double) - the value on the axis that the needle should point to. Setting a new value will (if animation is enabled) start the animation to move the needle from the old value to the new value. Setting a value outside of `[`Axis.MinValue, Axis.MaxValue`]` will throw an ArgumentOutOfRangeException.

**Animate** (bool) - whether or not to animate the transition from one value to another.

**AnimationLength** (int) - the total length (in ms) of the animation between values.

**AnimationInterval** (int) - the amount of time (ms) between animation frames. For example: to get a 20 FPS animation, this value would be set to 50 (`1 sec = 1000 ms, 1000 ms/ 50 ms = 20 FPS`).

**EaseFunction** (EaseFunctionType) - defines how the animation will look. For more information on easing see [Easing Functions](http://msdn.microsoft.com/en-us/library/ee308751.aspx).

**EaseMode** (EaseMode) - specifies how to use the [EaseFunction](EaseFunctionType.md) that is given.