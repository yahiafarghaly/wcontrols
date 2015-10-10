## Types ##
Each animation is started with a `from_value`, a `to_value`, a `animation_length`, and one of the listed ease functions. At each step in the animation, a value is chosen by the following process:
  1. Compute the percent completeness of the animation
```
progress_percent = (cur_time - start_time) / animation_length
```
  1. Pass `progress_percent` into the ease function to obtain `value_percent`
```
value_percent = ease_function(progress_percent)
```
  1. Compute the value for this step of the animation by moving `value_percent` percent from `from_value` to `to_value`
```
difference = to_value - from_value
value = from_value + (difference * value_percent)
```

For each of the graphs below, the x-axis represents `progress_percent` and the y-axis represents `value_percent`.

| **Type** | **EaseInOut** | **EaseIn** | **EaseOut** |
|:---------|:--------------|:-----------|:------------|
| **Linear** |<img src='http://wcontrols.googlecode.com/svn/wiki/images/linear.png' width='125px' height='125px' />|<img src='http://wcontrols.googlecode.com/svn/wiki/images/linear.png' width='125px' height='125px' />|<img src='http://wcontrols.googlecode.com/svn/wiki/images/linear.png' width='125px' height='125px' />|
| **Quadratic** |<img src='http://wcontrols.googlecode.com/svn/wiki/images/quadratic.png' width='125px' height='125px' />|<img src='http://wcontrols.googlecode.com/svn/wiki/images/quadraticIn.png' width='125px' height='125px' />|<img src='http://wcontrols.googlecode.com/svn/wiki/images/quadraticOut.png' width='125px' height='125px' />|
| **Cubic** |<img src='http://wcontrols.googlecode.com/svn/wiki/images/cubic.png' width='125px' height='125px' />|<img src='http://wcontrols.googlecode.com/svn/wiki/images/cubicIn.png' width='125px' height='125px' />|<img src='http://wcontrols.googlecode.com/svn/wiki/images/cubicOut.png' width='125px' height='125px' />|
| **Sine** |<img src='http://wcontrols.googlecode.com/svn/wiki/images/sine.png' width='125px' height='125px' />|<img src='http://wcontrols.googlecode.com/svn/wiki/images/sineIn.png' width='125px' height='125px' />|<img src='http://wcontrols.googlecode.com/svn/wiki/images/sineOut.png' width='125px' height='125px' />|