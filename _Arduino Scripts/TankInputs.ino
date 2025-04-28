
//Pin Variables
int slidePin = A0;
//Rotary Encoder A
int SW = 2;
int DT = 3;
int CLK = 4;
//5,6,7 Reserved for Rotary Encoder B
int rotPotA = A0;
int rotPotB = A1;
int sliPotA = A2;
int sliPotB = A3;

//Potentiometer Variables
int rotPotValueA;
int rotPotValueB;
int sliPotValueA;
int sliPotValueB;

//Rotary Encoder Variables
int counter = 0;
int currentStateCLK;
int lastStateCLK;
String currentDir = "";
unsigned long lastButtonPress = 0;

//Method Definitions
void ReadRotaryEncoder();

void setup() {
  Serial.begin(9600);

  //Setting Pins for Rotary Encoder
  pinMode(SW, INPUT_PULLUP);
  pinMode(DT, INPUT);
  pinMode(CLK, INPUT);

  lastStateCLK = digitalRead(CLK);


}

void loop() 
{
  ReadRotaryEncoder();
  rotPotValueA = analogRead(rotPotA);
  sliPotValueA = analogRead(sliPotA);
  Serial.println("/n");
  Serial.print("Rotary Potentiometer: ");
  Serial.println(rotPotValueA);
  Serial.print("Sliding Potentiometer: ");
  Serial.println(sliPotValueA);
  // Put in a slight delay to help debounce the reading
  delay(100);
}


void ReadRotaryEncoder()
{
  // Read the current state of CLK
  currentStateCLK = digitalRead(CLK);

  // If last and current state of CLK are different, then pulse occurred
  // React to only 1 state change to avoid double count
  if (currentStateCLK != lastStateCLK && currentStateCLK == 1) {

    // If the DT state is different than the CLK state then
    // the encoder is rotating CCW so decrement
    if (digitalRead(DT) != currentStateCLK) {
      counter++;
      currentDir = "CW";
    } else {
      // Encoder is rotating CW so increment
      counter--;
      currentDir = "CCW";
    }

    Serial.print("Direction: ");
    Serial.print(currentDir);
    Serial.print(" | Counter: ");
    Serial.println(counter);
  }

  // Remember last CLK state
  lastStateCLK = currentStateCLK;

  // Read the button state
  int btnState = digitalRead(SW);

  //If we detect LOW signal, button is pressed
  if (btnState == LOW) {
    //if 50ms have passed since last LOW pulse, it means that the
    //button has been pressed, released and pressed again
    if (millis() - lastButtonPress > 50) {
      Serial.println("Button pressed!");
    }

    // Remember last button press event
    lastButtonPress = millis();
  }
}