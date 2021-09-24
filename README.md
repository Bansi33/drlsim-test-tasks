![](https://www.xboxone-hq.com/images/git/news/drl_sim_logo-600x338.jpg)
# DRLSim Test Tasks
  
Welcome to the DRL Simulator evaluation test.
This repository contains an empty boilerplate Unity3D Project for aptitude testing of new developers.
This assignment aims to validate elementary C# / Coding / Math / Unity3D knowledge in form of a small scale project.

## Test Format
 * Given a functional sample (embedded executable) a closest as possible copy of its functionality is to be provided.
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
    * **The FPS meter is already provided** and part of the template scene.
    * Slider and InputField can be the ones provided by Unity3D's basic package, but need the slight changes seen in the layout comp.
    * UI scripting is free to be chosen as long the visual aspects of the screen are maintained.
  