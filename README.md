![](https://www.xboxone-hq.com/images/git/news/drl_sim_logo-600x338.jpg)
# DRLSim Test Tasks
  
Welcome to the DRL Simulator evaluation test.
This repository contains an empty boilerplate Unity3D Project for aptitude testing of new developers.
This assignment aims to validate elementary C# / Coding / Math / Unity3D knowledge in form of a small scale project.

## Test Format
 * Given a functional sample [embedded executable](https://github.com/thedroneracingleague/drlsim-test-tasks/raw/main/resources/build/drlsim-test-task-1-0-0.zip) a closest as possible copy of its functionality is to be provided.
 * Starting with a `fork` of this repository, the solution must be developed inside the requested time frame.
 * No external packages besides Unity's UI ones (sliders, buttons, layouts,...) must be used.
 * Features must be done from the user's own hand.
 * The user's solution will be evaluated on different criteria but mainly:
   * Level of completion and similarity with the provided executable sample and JPG layout
     * Fidelity to the UI layout in every pixel measurament and colors
     * The build wil be evaluated resizing the app window and the UI is expected to function properly
   * Project/Scene Hierarchy organization.
   * Scripting planning/architecture and best practices.
   * Knowledge on Unity features and Editor usage.
   * Time of completion and polishment.

> **After completion the user must provide a link to the forked repository for it to be cloned and evaluated**

## Specifications

* ### UI
  * A layout template is provided [HERE](https://github.com/thedroneracingleague/drlsim-test-tasks/tree/main/resources/ui)
  ![](https://github.com/thedroneracingleague/drlsim-test-tasks/raw/main/resources/ui/screen-dashboard.jpg)
  * The candidate must pay atention in detail about the following aspects:
    * Layout structures metrics like width,height,margins and spacing between elements.
    * FontFace used and their sizes **(fonts provided inside the `/Core` folder)**.
    * An easy way to validate is choosing `1080p` and pasting the JPG on top of your UI work.
    * Think as the end user and cover UX/UI usability cases by your own measure.
    * **The FPS meter is already provided** and part of the template scene.
    * Slider and InputField can be the ones provided by Unity3D's basic package, but need the slight changes seen in the layout comp.
    * UI scripting is free to be chosen as long the visual aspects of the screen are maintained.
    * The provided executable contains slider value ranges and overall behavior, so self-analysis is welcomed here.
  * ### Level
    * This is the 3D scene container sector.
    * The way how the animating spheres will be done is free to the user.
    * There is only a single mandatory component to be done which is an `Orbit Camera`
    * The origin of the orbit is at `(0,0,0)` and the interaction is done with click+drag and wheel.
    * All can be analysed from the provided executable.
  * ### Code
    * The project implementation can be done in any fashion and style.
    * Keep in mind it would be a code base used by other developers.
    * Proper written and documented code is a must.
    * Employ your best architecture and source code organization.
    * The only mandatory feature is that the application must stay as close as possible to `60fps` at all times and also no  FPS spikes/oscilation must happen during UI interaction and parameter changing.
    * The provided executable will be the base level comparison we'll be using.
  * ## Unity3D
    * Project organization is up to you too. As long there is one.
    * Be mindful to folder and asset naming as well categorization.
    * Again think you are on a team.
  * ## Math
    * In order to evaluate a bit of Math understanding we'll be doing a small `Polar Coordinate Animation/Simulation`.
    * The explanation of the mathematical concept can be seen and experimented [HERE](https://www.geogebra.org/m/upbPEhNK).
      * For the test simulation a small spinoff is used in the format:
      * `r = Sin(angle * A) + Cos(angle * B)`
    * Translating the equations into the 3D space and reproducing the animation is the main objective.
    * In real life, even more in the DRL **Simulator** many times we need to extrapolate and find custom Math solutions all the time.
  * ## Color Pattern
    * The last field in the UI is a `color pattern editor`
    * Upon typing `R|G|B|*` a repeatable sequence of RED GREEN BLUE or EMPTY must be applied in the group of spheres visible in the simulation.
    * The behavior is easily testable in the provided executable.