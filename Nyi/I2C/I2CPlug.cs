
#region Usings

using System;

using Microsoft.SPOT.Hardware;

#endregion

namespace eu.Vanaheimr.Nyi
{

    public abstract class I2CPlug
    {

        #region Data

        private const  Int32                    DefaultClockRate    = 400;
        private const  Int32                    TransactionTimeout  = 3000;

        private        I2CDevice.Configuration  I2C_Config;
        private        I2CDevice                I2C_Device;

        #endregion

        #region Properties

        #region Address

        private Byte _Address;

        public Byte Address
        {
            get
            {
                return _Address;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region I2CPlug(Address, ClockRateKhz = DefaultClockRate)

        /// <summary>
        /// Create a new abstract I2C device.
        /// </summary>
        /// <param name="Address">I2C address.</param>
        /// <param name="ClockRateKhz">I2C clockrate.</param>
        public I2CPlug(Byte    Address,
                       UInt32  ClockRateKhz = DefaultClockRate)
        {
            this._Address    = Address;
            this.I2C_Config  = new I2CDevice.Configuration(this.Address, (Int32) ClockRateKhz);
            this.I2C_Device  = new I2CDevice(this.I2C_Config);
        }

        #endregion

        #endregion


        #region (private) Write(WriteBuffer)

        private void Write(Byte[] WriteBuffer)
        {
            // create a write transaction containing the bytes to be written to the device
            I2CDevice.I2CTransaction[] writeTransaction = new I2CDevice.I2CTransaction[]
        {
            I2CDevice.CreateWriteTransaction(WriteBuffer)
        };

            // write the data to the device
            int written = this.I2C_Device.Execute(writeTransaction, TransactionTimeout);

            while (written < WriteBuffer.Length)
            {
                byte[] newBuffer = new byte[WriteBuffer.Length - written];
                Array.Copy(WriteBuffer, written, newBuffer, 0, newBuffer.Length);

                writeTransaction = new I2CDevice.I2CTransaction[]
            {
                I2CDevice.CreateWriteTransaction(newBuffer)
            };

                written += this.I2C_Device.Execute(writeTransaction, TransactionTimeout);
            }

            // make sure the data was sent
            if (written != WriteBuffer.Length)
            {
                throw new Exception("Could not write to device.");
            }

        }

        #endregion

        #region (private) Read(ReadBuffer)

        private void Read(Byte[] ReadBuffer)
        {
            // create a read transaction
            I2CDevice.I2CTransaction[] readTransaction = new I2CDevice.I2CTransaction[]
        {
            I2CDevice.CreateReadTransaction(ReadBuffer)
        };

            // read data from the device
            int read = this.I2C_Device.Execute(readTransaction, TransactionTimeout);

            // make sure the data was read
            if (read != ReadBuffer.Length)
            {
                throw new Exception("Could not read from device.");
            }
        }

        #endregion

        #region (protected) WriteToRegister(Register, Value)

        protected void WriteToRegister(Byte Register, Byte Value)
        {
            this.Write(new Byte[] { Register, Value });
        }

        #endregion

        #region (protected) WriteToRegister(Register, Values)

        protected void WriteToRegister(Byte Register, Byte[] Values)
        {

            // create a single buffer, so register and values can be send in a single transaction
            var writeBuffer = new Byte[Values.Length + 1];
            writeBuffer[0] = Register;
            Array.Copy(Values, 0, writeBuffer, 1, Values.Length);

            this.Write(writeBuffer);

        }

        #endregion

        #region (protected) ReadFromRegister(Register, ReadBuffer)

        protected void ReadFromRegister(Byte Register, Byte[] ReadBuffer)
        {
            this.Write(new Byte[] { Register });
            this.Read(ReadBuffer);
        }

        #endregion


    }

}
