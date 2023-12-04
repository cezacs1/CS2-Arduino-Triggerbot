#include <Servo.h>

Servo servoMotor;

void setup()
{
  servoMotor.attach(9);
  Serial.begin(9600);
  servoMotor.write(90);
}

char oldvalue = ' ';
bool alreadyRotated = false;

void loop()
{
  if (Serial.available() > 0)
  { 
    char receivedChar = Serial.read();

    if (oldvalue == receivedChar)
    {
      return;
    }
    else
    {
      oldvalue = receivedChar;

      if (receivedChar == '1' && !alreadyRotated)
      {
        servoMotor.write(83);
        delay(1000);
        servoMotor.write(99);
        delay(1000);

        servoMotor.write(90);
        //alreadyRotated = true; // Servo motor döndürüldüğünde true yap
      }
    }
  }
}
