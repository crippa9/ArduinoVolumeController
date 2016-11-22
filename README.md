# ArduinoVolumeController
Arduino based volume controller and sound source switcher

## What is it?
A custom made volume controller and sound source switcher, which is built using a Sparkfun Pro Micro, a clickable rotary encoder as well as an Adafruit Neopixel ring. 

Turning the knob changes volume on the connected computer (which emulates a keyboard media key press, and therefore works without the extra software), and clicking it will toggle between the sound sources. In my case, I toggle between my headphones which are connected through 3.5mm speaker jack, and my receiver, which is connected through HDMI.

### Driven by Sparkfun Pro Micro
![alt text](https://cdn.sparkfun.com//assets/parts/9/3/2/6/12640-01a.jpg "Sparkfun Pro Micro") 

The volume controller is powered by a Sparkfun Pro Micro, which was chosen because of its convenient size and price, but any Arduino-compatible device is supported. The volume control however is used by emulating an HID-device, which is not supported by all Arduino devices.

### Interactable through rotary encoder
![alt text](https://www.modmypi.com/image/cache/data/electronics/sensors/rotary-encoder/DSC_0700-800x609.jpg "Rotary encoder")

The volume controller is controlled by a Keyes clickable rotary encoder, which allows both turning and clicking actions.

### Displays through Adafruit Neopixel ring
![alt text](https://cdn-shop.adafruit.com/970x728/1643-01.jpg "Neopixel ring")

The volume and sound source is displayed through an Adafruit Neopixel ring with 12 LEDs. The current sound source is displayed through a specific color on the Neopixel ring, and the volume is shown by lighting up a certain amount of LEDs on it.

## How does it work?
The volume control itself works "out of the box", and to change sound source as well as display volume and current sound source requires the WPF program in the repository. The communication between the PC and Arduino is handled through serial communication.

The desktop program monitors changes in volume and sound source, and sends new states to the Arduino when changes happen.
The Arduino program sends volume changes as presses of a media volume button on a keyboard, while toggling through sound sources is done through the serial port.
