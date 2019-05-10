# OutOfFrustum
Tracking objects and display markers on screen edges when they are aout of view

![Screenshot](Demo/screenshot.png?raw=true "Screenshot")

![Screenshot Debug](Demo/debug.png?raw=true "Screenshot Debug")

Basic and ther simplest method to test objects' visibiliy is using OnBacameVisible and OnBecameUnvisible callbacks, but it has a bunch of limitations. It is related to a single renderer component. Also on Hololens, the actual viewport is narrower than the camera rendering frustum. To resolve these issues there is the second method, it is used in Demo scene. You can track complex objects, all renederes in their hierarchy is included, also visibility checking happens inside custom narrower frustum.
