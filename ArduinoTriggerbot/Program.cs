namespace ArduinoTriggerbot
{
    using System;
    using System.Diagnostics;
    using System.IO.Ports;
    using System.Threading;

    class Program
    {
        static void Main()
        {
            Console.Title = "Arduino Triggerbot";

            Process cs2 = null;
            bool msgshowed = false;

            while (cs2 == null)
            {
                if (!msgshowed)
                {
                    msgshowed = true;
                    Console.WriteLine("searching cs2..");
                }

                cs2 = memory.InitializeCS2();
                Console.WriteLine("memory.client -> " + memory.client);

                Thread.Sleep(2000);
            }

            Arduino arduino = new Arduino(new SerialPort("COM7", 9600));

            Console.Beep(2000, 500); // oyun bulunup handle alındığı zaman bip sesi ver.
            Console.WriteLine("cs2 found.");
            Run(arduino);
        }

        private static void Run(Arduino arduino)
        {
            // pixel search ile bellek okuma olmadan colorbot da yapılabilir.

            new Thread(() =>
            {
                while (true)
                {
                    // localplayerpawn'ı okuma. Eğer 0 ise offsetler bozuk olabilir kontrol edin.
                    UInt64 local = memory.Read<UInt64>(memory.client + offsets.dwlocalplayer);

                    byte crossid = memory.Read<byte>(local + offsets.ıdentityındex);

                    if (crossid != 255 && crossid != 0)
                    {
                        // triggerbotun çalışacağını doğrulamak için konsol yazısıyla kontrol
                        // Console.WriteLine("triggered!");

                        // ateş komutu gönderme
                        arduino.SendCommand("1");

                        // ateş ettikten sonra bir süre bekle
                        Thread.Sleep(1500); 
                    }

                    // döngü hızı için gecikme
                    Thread.Sleep(10);
                }
            }).Start();
        }
    }
}
