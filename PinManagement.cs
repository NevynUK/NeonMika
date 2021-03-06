using System;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using SecretLabs.NETMF.Hardware;
using Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.WebServer.NeonMika
{
    /// <summary>
    /// Interface between NeonMika.Webserver and the executing programm
    /// Use this class to work with your pins
    /// </summary>
    static public class PinManagement
    {
        //public
        public const int DIGITAL_PIN_COUNT = 14;
        public const int ANALOG_PIN_COUNT = 6;
        public static int[] PWM_IDs = new int[] { 5, 6, 9, 10 };

        //Standard output ports
        static private OutputPort Digital0 = new OutputPort(Pins.GPIO_PIN_D0, false);
        static private OutputPort Digital1 = new OutputPort(Pins.GPIO_PIN_D1, false);
        static private OutputPort Digital2 = new OutputPort(Pins.GPIO_PIN_D2, false);
        static private OutputPort Digital3 = new OutputPort(Pins.GPIO_PIN_D3, false);
        static private OutputPort Digital4 = new OutputPort(Pins.GPIO_PIN_D4, false);
        static private OutputPort Digital7 = new OutputPort(Pins.GPIO_PIN_D7, false);
        static private OutputPort Digital8 = new OutputPort(Pins.GPIO_PIN_D8, false);
        static private OutputPort Digital11 = new OutputPort(Pins.GPIO_PIN_D11, false);
        static private OutputPort Digital12 = new OutputPort(Pins.GPIO_PIN_D12, false);
        static private OutputPort Digital13 = new OutputPort(Pins.GPIO_PIN_D13, false);

        //Onboard led
        static public OutputPort OnboardLED = new OutputPort(Pins.ONBOARD_LED, false);

        //Output ports with PWM functionallity
        static private PWM PWM3 = new PWM(Cpu.PWMChannel.PWM_4, 0, 0, false);
        static private PWM PWM5 = new PWM(Cpu.PWMChannel.PWM_0, 0, 0, false);
        static private PWM PWM6 = new PWM(Cpu.PWMChannel.PWM_1, 0, 0, false);
        static private PWM PWM9 = new PWM(Cpu.PWMChannel.PWM_2, 0, 0, false);
        static private PWM PWM10 = new PWM(Cpu.PWMChannel.PWM_3, 0, 0, false);
        static private PWM PWM11 = new PWM(Cpu.PWMChannel.PWM_5, 0, 0, false);

        //Analog inputs
        static private AnalogInput Analog0 = new AnalogInput(AnalogChannels.ANALOG_PIN_A0);
        static private AnalogInput Analog1 = new AnalogInput(AnalogChannels.ANALOG_PIN_A1);
        static private AnalogInput Analog2 = new AnalogInput(AnalogChannels.ANALOG_PIN_A2);
        static private AnalogInput Analog3 = new AnalogInput(AnalogChannels.ANALOG_PIN_A3);
        static private AnalogInput Analog4 = new AnalogInput(AnalogChannels.ANALOG_PIN_A4);
        static private AnalogInput Analog5 = new AnalogInput(AnalogChannels.ANALOG_PIN_A5);

        //Arrays
        static private OutputPort[] Digitals = new OutputPort[] { Digital0, Digital1, Digital2, Digital3, Digital4, null, null, Digital7, Digital8, null, null, Digital11, Digital12, Digital13 };
        static private AnalogInput[] Analogs = new AnalogInput[] { Analog0, Analog1, Analog2, Analog3, Analog4, Analog5 };
        
        //PWM
        static private PWM[] PWMs = new PWM[] { PWM3, PWM5, PWM6, PWM9, PWM10, PWM11 };
        static private uint[] PWM_Durations = new uint[] { 0, 0, 0, 0, 0, 0 };
        static private uint[] PWM_Periods = new uint[] { 0, 0, 0, 0, 0, 0 };
        static private bool[] PWM_On = new bool[] { false, false, false, false };


        /// <summary>
        /// Switches a pin from true to false or vis-a-vis
        /// </summary>
        /// <param name="PinNumber"></param>
        static public void SwitchDigitalPinState(int PinNumber)
        {
            bool actState = GetDigitalPinState(PinNumber);

            SetDigitalPinState(PinNumber, !actState);
        }

        /// <summary>
        /// Sets a pin ON or OFF
        /// </summary>
        /// <param name="PinNumber"></param>
        /// <param name="active"></param>
        static public void SetDigitalPinState(int PinNumber, bool active)
        {
            if (Digitals[PinNumber] == null)
            {
                SetPWM(PinNumber, 20000, active ? (uint)20000 : (uint)0);
            }
            else
                Digitals[PinNumber].Write(active);
            
        }

        /// <summary>
        /// Sets the PWM value for a PWM-enabled pin
        /// </summary>
        /// <param name="PinNumber">5,6,9 or 10</param>
        /// <param name="Period"></param>
        /// <param name="Duration"></param>
        /// <returns>True fora valid pin, false otherwise.</returns>
        static public bool SetPWM(int PinNumber, uint Period, uint Duration)
        {
            if (PinNumber == 3 || PinNumber == 5 || PinNumber == 6 || PinNumber == 9 || PinNumber == 10 || PinNumber == 11)
            {
                PWM tempPWM = null;
                int i = 0;

                switch (PinNumber)
                {
                    case 3:
                        i = 0;
                        break;
                    case 5:
                        i = 1;
                        break;
                    case 6:
                        i = 2;
                        break;
                    case 9:
                        i = 3;
                        break;
                    case 10:
                        i = 4;
                        break;
                    case 11:
                        i = 5;
                        break;
                }

                tempPWM = PWMs[i];
                tempPWM.Duration = Duration;
                tempPWM.Period = Period;
                PWM_Durations[i] = Duration;
                PWM_Periods[i] = Period;

                if (Duration > 0)
                    PWM_On[i] = true;
                else
                    PWM_On[i] = false;

                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PinNumber"></param>
        /// <returns>True if duration of PWM > 0</returns>
        static public bool GetPWM(int PinNumber)
        {
            if (PinNumber == 3 || PinNumber == 5 || PinNumber == 6 || PinNumber == 9 || PinNumber == 10 || PinNumber == 11)
            {
                bool state = false;

                switch (PinNumber)
                {
                    case 3:
                        state = PWM_On[0];
                        break;
                    case 5:
                        state = PWM_On[1];
                        break;
                    case 6:
                        state = PWM_On[2];
                        break;
                    case 9:
                        state = PWM_On[3];
                        break;
                    case 10:
                        state = PWM_On[4];
                        break;
                    case 11:
                        state = PWM_On[5];
                        break;
                }
                return state;
            }
            else
                return false;
        }

        static public int GetPWMDuration(int PinNumber)
        {
            switch (PinNumber)
            {
                case 3:
                    return (int) PWM_Durations[0];
                case 5:
                    return (int)PWM_Durations[1];
                case 6:
                    return (int)PWM_Durations[2];
                case 9:
                    return (int)PWM_Durations[3];
                case 10:
                    return (int) PWM_Durations[4];
                case 11:
                    return (int) PWM_Durations[5];
                default:
                    return -1;
            }
        }

        static public int GetPWMPeriod(int PinNumber)
        {
            switch (PinNumber)
            {
                case 3:
                    return (int)PWM_Periods[0];
                case 5:
                    return (int)PWM_Periods[1];
                case 6:
                    return (int)PWM_Periods[2];
                case 9:
                    return (int)PWM_Periods[3];
                case 10:
                    return (int) PWM_Periods[4];
                case 11:
                    return (int) PWM_Periods[5];
                default:
                    return -1;
            }
        }

        /// <summary>
        /// Check if pin is ON or OFF
        /// </summary>
        /// <param name="PinNumber"></param>
        /// <returns></returns>
        static public bool GetDigitalPinState(int PinNumber)
        {
            if (Digitals[PinNumber] == null)
                return GetPWM(PinNumber);
            else       
                return Digitals[PinNumber].Read();
        }

        /// <summary>
        /// Reads the value from an analog pin
        /// </summary>
        /// <param name="PinNumber"></param>
        /// <returns></returns>
        static public int GetAnalogPinValue(int PinNumber)
        {
            //TODO: Fix this - return value in GetAnalogPinValue
            //return Analogs[PinNumber].Read();
            return (1);
        }
    }
}
