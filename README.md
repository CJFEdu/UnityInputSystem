# Unity Input System

This is meant to be a wrapper for they Unity input system.  The current Unity system has a number of failure points that this attempts to address.  The most critical failure point is the systems inability to recognize a wide range of controllers as controllers. This system addresses this by getting a full list of devices and allows rebinding of unrecognized devices.

There are other less critical failure points that are also addressed.  Input Actions are identified with enums rather than strings.  This allows identification errors to identified at compile time.  Identification of multiple players more explicitly so that new players aren’t randomly added.  Centralized control of inputs with access via event handlers.

This project is a work in progress and currently in the middle of it’s third refactor to add more flexibility to its use.
