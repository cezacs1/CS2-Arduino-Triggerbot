#include <Servo.h>

Servo servoMotor;

void setup() {
  servoMotor.attach(9);
  Serial.begin(9600);
  servoMotor.write(90);
}

char oldvalue = ' '; // Tek bir karakter olarak tanımlanmış ve boş bir karakterle başlatılmış
bool alreadyRotated = false; // Servo motorun zaten döndürüldüğünü kontrol etmek için bir bayrak kullanılıyor

void loop() {
  if (Serial.available() > 0) { 
    char receivedChar = Serial.read();

    if (oldvalue == receivedChar) {
      return;
    } else {
      oldvalue = receivedChar;

      if (receivedChar == '1' && !alreadyRotated) {
        servoMotor.write(83);
        delay(500);
        servoMotor.write(99);
        delay(500);

        servoMotor.write(90);
        //alreadyRotated = true; // Servo motor döndürüldüğünde bayrağı true yap
      }
    }
  }
}
