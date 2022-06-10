using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace KleanTrak.Pi
{
    public class Buzzer
    {
        private static Buzzer obj;
        private GpioController gpio;
        private GpioPin LedControlGPIOPin;

        public static Buzzer Instance
        {
            private set
            {
            }
            get
            {
                if (obj == null)
                    obj = new Buzzer();
                return obj;
            }
        }

        public Buzzer()
        {
            gpio = GpioController.GetDefault();
            if (gpio != null)
            {
                LedControlGPIOPin = gpio.OpenPin(21);
                LedControlGPIOPin.SetDriveMode(GpioPinDriveMode.Output);
                LedControlGPIOPin.Write(GpioPinValue.Low);
            }
        }

        public void ErrorMessage()
        {
            if (LedControlGPIOPin == null)
                return;

            new Task(async () =>
            {
                for (int i = 0; i < 2; i++)
                {
                    LedControlGPIOPin.Write(GpioPinValue.High);
                    await Task.Delay(500);
                    LedControlGPIOPin.Write(GpioPinValue.Low);
                    await Task.Delay(250);
                }
            }).Start();
        }
    }
}
