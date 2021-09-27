![](https://www.xboxone-hq.com/images/git/news/drl_sim_logo-600x338.jpg)
# DRLSim Test Tasks
  
Welcome to the DRL Simulator evaluation test.
This repository contains an empty boilerplate Unity3D Project for aptitude testing of new developers.
This assignment aims to validate elementary C# / Coding / Math / Unity3D knowledge in form of a small scale project.

## Test Format
 * Using the functional sample [embedded WIN|OSX executable](https://github.com/thedroneracingleague/drlsim-test-tasks/tree/main/resources/build), work to create a product as close to this as possible
 * Please start by creating a `fork` of this repository
 * Please do not use any external packages besides Unity's UI packages (sliders, buttons, layouts,...)
 * The expectation is that all work is done from scratch
 * The completed work will ideally demonstrate the following:
   * Level of completion and similarity with the provided executable sample and JPG layout
     * Fidelity to the UI layout in every pixel measurament and colors
     * The build wil be evaluated resizing the app window and the UI is expected to function properly
   * Project/Scene Hierarchy organization.
   * Scripting planning/architecture and best practices.
   * Knowledge on Unity features and Editor usage.
   * Time of completion and polishment.

> **Please provide a link to the forked repository for it to be cloned and reviewed**

## Specifications

* ### UI
  * A layout template is provided [HERE](https://github.com/thedroneracingleague/drlsim-test-tasks/tree/main/resources/ui)
  ![](https://github.com/thedroneracingleague/drlsim-test-tasks/raw/main/resources/ui/screen-dashboard.jpg)
  * The candidate must pay atention in detail about the following aspects:
    * Layout structures metrics like width,height,margins and spacing between elements.
    * FontFace used and their sizes **(fonts provided inside the `/Core` folder)**.
    * An easy way to validate is choosing `1080p` and pasting the JPG on top of your UI work.
    * Think as the end user and cover UI/UX usability cases using your best judgement and experience
    * **The FPS monitor is already provided** and part of the template scene.
    * Slider and InputField can be the ones provided by Unity3D's basic package, but need the slight changes seen in the layout comp.
    * UI scripting is free to be chosen as long the visual aspects of the screen are maintained.
    * The provided executable contains slider value ranges and overall behavior, so self-analysis is welcomed here.
  * ### Level
    * This is the 3D scene container sector.
    * The way in which the animating spheres will work should be controllable by the user
    * There is only a single mandatory component to be done which is an `Orbit Camera`
    * The origin of the orbit is at `(0,0,0)` and the interaction is done with click+drag and wheel.
    * All of the above are demonstrated in the executable provided.
  * ### Code
    * The project implementation can be done in any fashion/style.
    * Keep in mind it would be a code base used by other developers.
    * Please ensure documentation and commenting are to industry standards
    * Employ your best architecture and source code organization.
    * The only mandatory feature is that the application must stay as close as possible to `60fps` at all times and also not have any FPS spikes/oscilation during UI interaction and parameter changing.
    * The provided executable will be the base level comparison we'll be using.
  * ## Unity3D
    * Project organization structure is up to you, but expected to follow industry best practices
    * It is suggested to apply this approach to folder and asset naming as well categorization.
  * ## Math
    * In order to evaluate a bit of Math understanding we'll be doing a small `Polar Coordinate Animation/Simulation`.
    * The explanation of the mathematical concept can be seen and experimented [HERE](https://www.geogebra.org/m/upbPEhNK).
      * For the test simulation a small spinoff is used in the format:
      * `r = Sin(angle * A) + Cos(angle * B)`
      * At the provided link you can try an example like `sin(5t) + cos(t * 10)` and move the slider there.
    * Translating the equations into the 3D space and reproducing the animation is the main objective.
    * In real life, even more in the DRL **Simulator** many times we need to extrapolate and find custom Math solutions all the time.
  * ## Color Pattern
    * The last field in the UI is a `color pattern editor`
    * Upon typing `R|G|B|*` a repeatable sequence of RED GREEN BLUE or EMPTY must be applied in the group of spheres visible in the simulation.
    * The behavior is easily testable in the provided executable.
