//Sensor pins.
int PIR = 7;
int LDR = A0;
int LM35 = A1;

//LED pins.
int GREENLED = 8;
int REDLED = 12;
int WHITELED = 4;

//Relay pins.
int RELAY_01 = 13;

//LM35 Vars.
int LM35_READING = 0;
float TEMP_VOLTAGE = 0;
float TEMP = 0;

//Alarm's State
bool isAlarmOn = true;

//Serial port stuff
String inputString = "";         // a String to hold incoming data
bool stringComplete = false;  // whether the string is complete

void setup() {
  pinMode(GREENLED, OUTPUT);
  pinMode(PIR, INPUT);
  pinMode(REDLED, OUTPUT);
  pinMode(LDR, INPUT);
  pinMode(WHITELED, OUTPUT);
  pinMode(LM35, INPUT);
  pinMode(RELAY_01, OUTPUT);
  // initialize serial:
  Serial.begin(9600);
  // reserve 200 bytes for the inputString:
  inputString.reserve(200);
}

void loop() {
  delay(200);
  LM35_READING = analogRead(LM35);
  TEMP_VOLTAGE = (LM35_READING / 1023.0)*5000;
  TEMP = TEMP_VOLTAGE / 10;
  //sending datas of sensors to serial port.
  Serial.print("MOTION: ");
  Serial.println(digitalRead(PIR));
  Serial.print("LDR'S VALUE: ");
  Serial.println(analogRead(LDR));
  Serial.print("LM35 READING: ");
  Serial.println(LM35_READING);
  Serial.print("LM35 VOLTAGE: ");
  Serial.println(TEMP_VOLTAGE);
  Serial.print("TEMP: ");
  Serial.println(TEMP);
  Serial.print("ALARM: ");
  if (isAlarmOn){
    Serial.println("ON");
  } else {
    Serial.println("OFF");
  }

  //Checks if any command recieves.
  readComingCommands();
  
  //LDR
  if(analogRead(LDR) > 500)
  {
    digitalWrite(GREENLED, HIGH);
    digitalWrite(RELAY_01, HIGH);
    
  }
  else
  {
    digitalWrite(GREENLED, LOW);
    digitalWrite(RELAY_01, LOW);
  }
  //PIR
  if(digitalRead(PIR) == 1 && isAlarmOn)
  {
    digitalWrite(REDLED, HIGH);
  }
  else
  {
    digitalWrite(REDLED, LOW);
  }
  //LM35
  if(TEMP < 30)
  {
    digitalWrite(WHITELED, HIGH);
  }
  else
  {
    digitalWrite(WHITELED, LOW);
  }
}

/*
  SerialEvent occurs whenever a new data comes in the hardware serial RX. This
  routine is run between each time loop() runs, so using delay inside loop can
  delay response. Multiple bytes of data may be available.
*/
void serialEvent() {
  while (Serial.available()) {
    // get the new byte:
    char inChar = (char)Serial.read();
    // add it to the inputString:
    inputString += inChar;
    // if the incoming character is a newline, set a flag so the main loop can
    // do something about it:
    if (inChar == '\n') {
      stringComplete = true;
    }
  }
}

void readComingCommands() {
  inputString.trim();
  if (stringComplete) {
    if(inputString.equals("SETALARMON")){
      isAlarmOn = true;
    }
    else if(inputString.equals("SETALARMOFF")){
      isAlarmOn = false;
    }
    // clear the string:
    inputString = "";
    stringComplete = false;
  }
}
