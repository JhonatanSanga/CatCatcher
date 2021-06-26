const int PIN_LED=13;
void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  pinMode(PIN_LED, OUTPUT);
}

void loop() {
  // put your main code here, to run repeatedly:
  if(Serial.available()>0)
  {
    int Mycharacter=Serial.read();
    if(Mycharacter=='E')
    {
      digitalWrite(PIN_LED, HIGH);
      
      delay(5000);
      digitalWrite(PIN_LED, LOW);
    }
  }
}
