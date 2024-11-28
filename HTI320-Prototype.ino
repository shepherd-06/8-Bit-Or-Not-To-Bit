#include <LiquidCrystal.h>

// Ultrasonic Sensor Pins
const int trigPin = 7;
const int echoPin = 8;

// Sound Sensor Pins
const int soundDigitalPin = 11; // D0
const int soundAnalogPin = A0;  // A0 (optional)

// LCD Pins
LiquidCrystal lcd(9, 10, 4, 5, 6, 3); // RS, EN, D4, D5, D6, D7

void setup() {
  // Initialize Serial Monitor
  Serial.begin(9600);

  // Initialize Ultrasonic Sensor Pins
  pinMode(trigPin, OUTPUT);
  pinMode(echoPin, INPUT);

  // Initialize Sound Sensor Pins
  pinMode(soundDigitalPin, INPUT);

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
  cm = duration / 29 / 2;

  // Print distance to Serial Monitor
  Serial.print("Distance: ");
  Serial.print(cm);
  Serial.println(" cm");

  // Display distance on LCD

  // Read sound sensor digital output
  int soundDetected = digitalRead(soundDigitalPin);

  // Read sound sensor analog output (optional)
  int soundIntensity = analogRead(soundAnalogPin);

  // Print sound status to Serial Monitor
  if (soundDetected == HIGH) {
    Serial.println("Sound Detected!");
  } else {
    Serial.println("No Sound Detected");
  }

  String labels[] = {"US:", "Sound:"};
  long values[] = {duration, soundIntensity};

  displayOnLCD(labels, values, 2, 2);

  // Print analog sound intensity (if needed)
  Serial.print("Sound Intensity: ");
  Serial.println(soundIntensity);

  delay(500); // Wait before the next reading
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
