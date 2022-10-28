using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Library.Interface;
using Library.UtiAndSim;
using Library.Control;

namespace Library.Control
{
    public class ChargingControl: IChargingControl
    {
        //Constants in mA
        private const double MaxCurrent = 500.0;
        private const double FullyChargedCurrent = 5; 
        private const double NoChargeCurrent = 0.0;

        private enum ChargeState
        {
            NotConnected,
            NormalCharge,
            FullCharge,
            OverCharged
        };

        private ChargeState state;
        public bool IsConnected { get; set; }
        public double currentValue { get; private set; }

        IDisplay _display;

        IUsbCharger _usbCharger;


        public ChargingControl(IUsbCharger usbCharger, IDisplay display)
        {
            IsConnected = false;
            _display = display;
            _usbCharger = usbCharger;
            _usbCharger.CurrentValueEvent += HandleCurrentEvent;
        }

        private void HandleCurrentEvent(object? sender, CurrentEventArgs e)
        {
            currentValue = e.Current;
            CurrentState();
        }

        public void StartCharge()
        {
            IsConnected = true;
            _usbCharger.StartCharge();
        }

        public void StopCharge()
        {
            IsConnected = false;
            _usbCharger.StopCharge();
        }

        private void CurrentState()
        {
            if (currentValue <= FullyChargedCurrent && currentValue > NoChargeCurrent)
            {
                if (state == ChargeState.FullCharge)
                {
                    return;
                }

                state = ChargeState.FullCharge;
                _display.FullCharge();
            }
            else if (currentValue > FullyChargedCurrent && currentValue <= MaxCurrent)
            {
                if (state == ChargeState.NormalCharge)
                {
                    return;
                }

                state = ChargeState.NormalCharge;
                _display.NormalCharge();
            }
            else if (currentValue >= MaxCurrent)
            {
                if (state == ChargeState.OverCharged)
                {
                    return;
                }
                _usbCharger.StopCharge();
                state = ChargeState.OverCharged;
                _display.OverCharged();
            }
            else
            {
                
                if (state == ChargeState.NotConnected)
                {
                    return;
                }
                _display.NotConnected();
                state = ChargeState.NotConnected;
                
            }
        }

    }
}
