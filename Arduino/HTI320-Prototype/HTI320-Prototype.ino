#include <LiquidCrystal.h>

// Digital Pins
// Ultrasonic Sensor Pins
const int trigPin = 7;
const int echoPin = 8;
const int switchPin = 2; // Switch connected to D2


//Analog Pins
const int ldrPin = A0; // LDR connected to analog pin A0
const int potPin = A2; // Potentiometer connected to A2
const int ldrPin2 = A3; // second LDR, connect to A3; because 1 is not enough!



// LCD Pins
LiquidCrystal lcd(9, 10, 4, 5, 6, 3); // RS, EN, D4, D5, D6, D7

void setup() {
  // Initialize Serial Monitor
  Serial.begin(9600);

  // Initialize Ultrasonic Sensor Pins
  pinMode(trigPin, OUTPUT);
  pinMode(echoPin, INPUT);
  pinMode(switchPin, INPUT_PULLUP); // Enable internal pull-up resistor for the switch


  // Initialize LCD
  lcd.begin(16, 2);
  lcd.setCursor(0, 0); 
  lcd.print("8 bit or not to");
  lcd.setCursor(6, 1); 
  lcd.print("bit!");
  delay(5000); // Show welcome message
  lcd.clear();
}

void loop() {

  int switchState = digitalRead(switchPin);
  // Turn the LED on or off based on the switch state
  // if (switchState == LOW) {
  //   // digitalWrite(ledPin, HIGH); // Turn on the LED
  //   Serial.println("Switch ON");
  // } else {
  //   // digitalWrite(ledPin, LOW);  // Turn off the LED
  //   Serial.println("Switch OFF");
  // }


  long duration, cm;

  // Trigger the ultrasonic sensor
  digitalWrite(trigPin, LOW);
  delayMicroseconds(2);
  digitalWrite(trigPin, HIGH);
  delayMicroseconds(10);
  digitalWrite(trigPin, LOW);

  // Read the echo pin
  duration = pulseIn(echoPin, HIGH);

  // Calculate distance in centimeters
  //cm = duration / 29 / 2;

  // Print distance to Serial Monitor
  // Serial.print("Distance: ");
  // Serial.print(cm);
  // Serial.println(" cm");

  
  int ldrValue = analogRead(ldrPin); // Read the LDR value
  int secondLdrVal = analogRead(ldrPin2); // second LDR value.

  // Serial.print("LDR Value: ");
  // Serial.println(ldrValue); // Print the value (0 to 1023)

  // Serial.print("2nd LDR Val: ");
  // Serial.println(secondLdrVal);

  int manSoundIntensity = analogRead(potPin);

  String labels[] = {"US:", "LRD:", "POT: "};
  long values[] = {duration, ldrValue + secondLdrVal, manSoundIntensity};

  displayOnLCD(labels, values, 3, 3);
  sendHexColorWithRandomMapping(duration, ldrValue + secondLdrVal, manSoundIntensity);
  delay(150); // Wait before the next reading' 150ms - SUPER SONIC
}

// Function to display up to three strings and three values on the LCD
void displayOnLCD(String labels[], long values[], int numLabels, int numValues) {
  /**
  * The displayOnLCD function dynamically displays up to three labels and three values on a 16x2 LCD. 
  * It formats and adjusts the content to fit the LCD screen's two rows, 
  * ensuring it accommodates varying numbers of labels and values. 
  * Excess text is truncated if it exceeds the 16-character limit.
  **/
  lcd.clear(); // Clear the LCD for new content

  // Display labels on the first row
  lcd.setCursor(0, 0); // Row 0
  int charCount = 0;   // Track characters printed on the row
  for (int i = 0; i < numLabels && charCount < 16; i++) {
    lcd.print(labels[i]); // Print the label
    charCount += labels[i].length();
    if (i < numLabels - 1 && charCount < 15) {
      lcd.print(" "); // Add space between labels
      charCount++;
    }
  }

  // Display values on the second row
  lcd.setCursor(0, 1); // Row 1
  charCount = 0;       // Reset character count for the second row
  for (int i = 0; i < numValues && charCount < 16; i++) {
    lcd.print(values[i]); // Print the value
    charCount += String(values[i]).length();
    if (i < numValues - 1 && charCount < 15) {
      lcd.print(" "); // Add space between values
      charCount++;
    }
  }
}


void sendHexColorWithRandomMapping(long distance, int ldrValue, int soundIntensity) {
  static unsigned long lastSendTime = 0; // Static variable to retain value between calls
  const int sendInterval = 500;         // 500 ms interval for sending data
  
  unsigned long currentMillis = millis(); // Get the current time
  
  // Check if it's time to send data
  if (currentMillis - lastSendTime >= sendInterval) {
    lastSendTime = currentMillis; // Update the last send time

    // Randomize sensor-to-color mapping
    int sensorValues[] = {distance, ldrValue, soundIntensity};
    int randomMapping[3];

    // Shuffle the sensorValues array
    for (int i = 0; i < 3; i++) {
      int randIndex = random(0, 3); // Random index between 0 and 2
      randomMapping[i] = sensorValues[randIndex];
      sensorValues[randIndex] = sensorValues[i]; // Swap to avoid duplication
    }

    // Assign shuffled values to RGB
    int red = map(randomMapping[0], 0, 1023, 0, 255);
    red = constrain(red, 0, 255);

    int green = map(randomMapping[1], 0, 1023, 0, 255);
    green = constrain(green, 0, 255);

    int blue = map(randomMapping[2], 0, 1023, 0, 255);
    blue = constrain(blue, 0, 255);

    // Format as a hexadecimal color
    char hexColor[8]; // Enough space for "#RRGGBB\0"
    sprintf(hexColor, "#%02X%02X%02X", red, green, blue);

    // Send the hexadecimal color via Serial
    // Serial.print("Hex Color: ");
    Serial.println(hexColor);

    // Debugging: Print which sensor controls which channel
    // Serial.print("Red: ");
    // Serial.println(randomMapping[0]);
    // Serial.print("Green: ");
    // Serial.println(randomMapping[1]);
    // Serial.print("Blue: ");
    // Serial.println(randomMapping[2]);
  }
}



