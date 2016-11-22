#include <Adafruit_NeoPixel.h>

#include <HID-Project.h>
#include <HID-Settings.h>

#include <ClickEncoder.h>
#include <TimerOne.h>

const double MaxVol = 100.0;
const int NumberOfLeds = 12;
ClickEncoder *encoder;
int last, value;

int soundSource = -1;

int colors[3][3] = {
  {0, 0, 0},
  {0, 25, 0},
  {0, 0, 25}
};

Adafruit_NeoPixel neoPixel = Adafruit_NeoPixel(12, 4, NEO_GRB+NEO_KHZ800);

void timerIsr() {
  encoder->service();
}

void setup() {
  Serial.begin(9600);

  encoder = new ClickEncoder(8, 9, 7);

  Timer1.initialize(1000);
  Timer1.attachInterrupt(timerIsr); 
  
  last = -1;
  Consumer.begin();
  System.begin();

  neoPixel.begin();
  setNeoPixels();
}

void loop() {
  // handle serial events
  int bytesAvailable = Serial.available();
  if (bytesAvailable > 0) {
      String response = "";
      for(int i = 0; i < bytesAvailable; i++) {
          response += (char)Serial.read();
      }

      // parse response
      if(response.startsWith("vol")) {
        String volValue = response.substring(3);
        last = volValue.toInt();
        setNeoPixels();
      }
      else if(response.startsWith("source")) {
        String sourceValue = response.substring(6);
        soundSource = sourceValue.toInt();
        setNeoPixels();
      }
      else if(response.startsWith("color:")) {
        String colorString = response.substring(6);
        
        String RVal = getValue(colorString, ':', 0);
        String GVal = getValue(colorString, ':', 1);
        String BVal = getValue(colorString, ':', 2);

        colors[soundSource+1][0] = RVal.toInt();
        colors[soundSource+1][1] = GVal.toInt();
        colors[soundSource+1][2] = BVal.toInt();

        setNeoPixels();
/*
        Serial.print("r: ");
        Serial.print(RVal);
        Serial.print("g: ");
        Serial.print(GVal);
        Serial.print("b: ");
        Serial.println(BVal);*/
      }
      else if(response.equals("opening")) {
        //setNeoPixels();
      }
      else if(response.equals("closing")) {
        soundSource = -1;
        setNeoPixels();
      }
  }
  
  // handle encoder events
  int currentEncoderVal = encoder->getValue();
  if (currentEncoderVal != 0) {
    value = min(last+currentEncoderVal, MaxVol);
    value = max(value, 0);
    if(currentEncoderVal >= 0)
      Consumer.write(MEDIA_VOLUME_UP);
    else
      Consumer.write(MEDIA_VOLUME_DOWN);
    last = value;
  }

  // handle click events
  ClickEncoder::Button b = encoder->getButton();
  if (b != ClickEncoder::Open) {
    //Serial.print("Button: ");
    //#define VERBOSECASE(label) case label: Serial.println(#label); break;
    switch (b) {
      //VERBOSECASE(ClickEncoder::Pressed);
      //VERBOSECASE(ClickEncoder::Held)
      //VERBOSECASE(ClickEncoder::Released)
      case ClickEncoder::Clicked:
        if(soundSource != -1) {
          // change sound source, send serial command
          Serial.print((char)(1));
        }
        break;
        /*
      case ClickEncoder::DoubleClicked:
          Serial.println("ClickEncoder::DoubleClicked");
          encoder->setAccelerationEnabled(!encoder->getAccelerationEnabled());
          Serial.print("  Acceleration is ");
          Serial.println((encoder->getAccelerationEnabled()) ? "enabled" : "disabled");
        break;*/
    }
  }    
}

void setNeoPixels() {
  //Serial.print("setting neopixel for soundSource: ");
  //Serial.println(soundSource);
  int ledFraction = (last/MaxVol) * NumberOfLeds;
  for(int i = 0; i < NumberOfLeds; i++) {
    if(soundSource > -1)
      neoPixel.setPixelColor(
        i, 
        (i < ledFraction ? colors[soundSource+1][0] : 0),
        (i < ledFraction ? colors[soundSource+1][1] : 0),
        (i < ledFraction ? colors[soundSource+1][2] : 0)
      );
    else {
      neoPixel.setPixelColor(
        i, 
        colors[soundSource+1][0],
        colors[soundSource+1][1],
        colors[soundSource+1][2]
      );
    }
  }
  neoPixel.show();
}

String getValue(String data, char separator, int index) {
  int found = 0;
  int strIndex[] = { 0, -1  };
  int maxIndex = data.length()-1;
  for(int i=0; i<=maxIndex && found<=index; i++){
    if(data.charAt(i)==separator || i==maxIndex){
      found++;
      strIndex[0] = strIndex[1]+1;
      strIndex[1] = (i == maxIndex) ? i+1 : i;
    }
  }
  return found>index ? data.substring(strIndex[0], strIndex[1]) : "";
}
