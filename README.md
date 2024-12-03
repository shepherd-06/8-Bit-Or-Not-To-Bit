# 8 Bit or Not to Bit

This project is a prototype for HTI320, combining Arduino-based sensors and actuators with a future Unity visualization component. The current portion of the project focuses on the Arduino system.

## File Structure

```markdown
Arduino/
│
└── HTI320-Prototype/
    └── HTI320-Prototype.ino
```

* **HTI320-Prototype.ino**: The main Arduino sketch for managing sensor inputs and actuating outputs.

## Current Features

1. Sensor Integration:

* Ultrasonic Sensor: Measures distance and maps the values to control visual elements.
* Photoresistor (LDR): Reads ambient light intensity and integrates the data into the visualization.
* Potentiometer: Manually simulates sound intensity or other analog inputs for prototyping.

2. Switch Control: [Planned]
A simple switch setup is used to detect ON/OFF states, which can be logged or used for triggering actions.

3. RGB Color Mapping:
The sensor data dynamically controls an RGB color model.
The mapping can randomize sensor-to-color relationships for experimentation.

## Planned Features
* Unity Integration:

The Arduino will send real-time sensor data to Unity via serial communication.
Unity will visualize the data dynamically, such as creating gradients or animations.

## How to Use
### Setup:
Connect the Arduino and wire the sensors and components as described in the comments within HTI320-Prototype.ino.
### Upload the Code:
Open HTI320-Prototype.ino in the Arduino IDE.
Select the appropriate board and COM port.
Upload the code to the Arduino.
### Serial Monitor:
Use the Serial Monitor to view sensor readings and debug outputs.
## Hardware Requirements
* Arduino board (e.g., UNO or similar)
* Ultrasonic sensor
* 2x Photoresistor (LDR)
* 2x Potentiometer
* (as required) RGB LED
* 1x Switch
* Breadboard, wires, and resistors (as needed)

