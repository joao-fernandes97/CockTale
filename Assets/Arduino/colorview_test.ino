#include <Wire.h>
#include "Adafruit_TCS34725.h"
#include <FastLED.h>
#include <light_CD74HC4067.h>

#define LED_DATA_PIN 5
#define NUM_LEDS 38
#define BRIGHTNESS 50
#define TILT_DATA_PIN A0
#define SHAKER_DATA_PIN 2

// set to false if using a common cathode LED
#define commonAnode true

// our RGB -> eye-recognized gamma color
byte gammatable[256];
CRGB leds[NUM_LEDS];
CD74HC4067 mux(8, 9, 10, 11);


Adafruit_TCS34725 tcs = Adafruit_TCS34725(TCS34725_INTEGRATIONTIME_50MS, TCS34725_GAIN_4X);

void setup() {
  Serial.begin(9600);
  //Serial.println("Color View Test!");

  //tilt pin mode
  pinMode(TILT_DATA_PIN, INPUT);
  pinMode(SHAKER_DATA_PIN, INPUT_PULLUP);

  if (tcs.begin()) {
    //Serial.println("Found sensor");
  } else {
    Serial.println("No TCS34725 found ... check your connections");
    while (1); // halt!
  }

  FastLED.addLeds<WS2812B, LED_DATA_PIN, GRB>(leds, NUM_LEDS);
  FastLED.setBrightness(BRIGHTNESS);

  // thanks PhilB for this gamma table!
  // it helps convert RGB colors to what humans see
  for (int i=0; i<256; i++) {
    float x = i;
    x /= 255;
    x = pow(x, 2.5);
    x *= 255;

    if (commonAnode) {
      gammatable[i] = 255 - x;
    } else {
      gammatable[i] = x;
    }
    //Serial.println(gammatable[i]);
  }
}

// The commented out code in loop is example of getRawData with clear value.
// Processing example colorview.pde can work with this kind of data too, but It requires manual conversion to 
// [0-255] RGB value. You can still uncomments parts of colorview.pde and play with clear value.
void loop() {
  float red, green, blue;
  
  tcs.setInterrupt(false);  // turn on LED

  delay(60);  // takes 50ms to read

  tcs.getRGB(&red, &green, &blue);
  
  tcs.setInterrupt(true);  // turn off LED

  Serial.print("C:"); Serial.print(int(red)); 
  Serial.print(","); Serial.print(int(green)); 
  Serial.print(","); Serial.println(int(blue));
  

  //Serial.print("\t");
  //Serial.print((int)red, HEX); Serial.print((int)green, HEX); Serial.println((int)blue, HEX);

//  uint16_t red, green, blue, clear;
//  
//  tcs.setInterrupt(false);  // turn on LED
//
//  delay(60);  // takes 50ms to read
//
//  tcs.getRawData(&red, &green, &blue, &clear);
//  
//  tcs.setInterrupt(true);  // turn off LED
//
//  Serial.print("C:\t"); Serial.print(int(clear)); 
//  Serial.print("R:\t"); Serial.print(int(red)); 
//  Serial.print("\tG:\t"); Serial.print(int(green)); 
//  Serial.print("\tB:\t"); Serial.print(int(blue));
//  Serial.println();

  //fill_solid(leds, NUM_LEDS, CRGB((int)red,(int)green,(int)blue));
  //FastLED.show();

  //loop through tilt channels 0 -15
  for (byte i = 0; i<16; i++)
  {
    mux.channel(i);
    int val = digitalRead(TILT_DATA_PIN);
    int val2 = digitalRead(SHAKER_DATA_PIN);
    Serial.println("T"+String(i)+":"+String(val));
    Serial.println("S:"+String(val2));
    delay(50);
  }

  delay(10);
}
