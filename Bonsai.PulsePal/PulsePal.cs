using System;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Threading;

namespace Bonsai.PulsePal
{
    public sealed class PulsePal : IDisposable
    {
        public const int BaudRate = 115200;
        const int CycleFrequency = 20000;
        const int MaxCyclePeriod = 36000000;
        const int MaxPulseLength = 1000;
        const int MaxDataBytes = 35;

        const byte OpMenu                = 213;
        const byte Handshake             = 72;
        const byte Acknowledge           = 75;

        const byte ProgramParam          = 74;
        const byte ProgramPulseTrain1    = 75;
        const byte ProgramPulseTrain2    = 76;
        const byte TriggerCommand        = 77;
        const byte UpdateDisplayCommand  = 78;
        const byte SetVoltageCommand     = 79;
        const byte AbortCommand          = 80;
        const byte DisconnectCommand     = 81;
        const byte LoopCommand           = 82;
        const byte ClientIdCommand       = 89;
        const byte LineBreak             = 254;

        bool disposed;
        bool initialized;
        int firmwareVersion;
        int dacMaxValue;
        readonly SerialPort serialPort;
        readonly byte[] commandBuffer;
        readonly byte[] readBuffer;

        public PulsePal(string portName)
        {
            serialPort = new SerialPort(portName);
            serialPort.BaudRate = BaudRate;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;
            serialPort.Parity = Parity.None;
            serialPort.DtrEnable = false;
            serialPort.RtsEnable = true;

            commandBuffer = new byte[MaxDataBytes];
            readBuffer = new byte[serialPort.ReadBufferSize];
        }

        public int FirmwareVersion
        {
            get { return firmwareVersion; }
        }

        public bool IsOpen
        {
            get { return serialPort.IsOpen; }
        }

        Task RunAsync(CancellationToken cancellationToken)
        {
            serialPort.Open();
            serialPort.ReadExisting();
            Connect();

            return Task.Factory.StartNew(() =>
            {
                var offset = 0;
                using var cancellation = cancellationToken.Register(serialPort.Dispose);
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var bytesToRead = serialPort.BytesToRead;
                        if (bytesToRead == 0)
                        {
                            var nextByte = (byte)serialPort.ReadByte();
                            if (nextByte < 0) break;
                            readBuffer[offset++] = nextByte;
                            offset -= ProcessResponse(offset);
                        }
                        else
                        {
                            while (bytesToRead > 0)
                            {
                                var bytesRead = serialPort.Read(readBuffer, offset, Math.Min(bytesToRead, readBuffer.Length - offset));
                                bytesToRead -= bytesRead;
                                offset += bytesRead;
                                offset -= ProcessResponse(offset);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        if (!cancellationToken.IsCancellationRequested)
                        {
                            throw;
                        }
                        break;
                    }
                }
            },
            cancellationToken,
            TaskCreationOptions.LongRunning,
            TaskScheduler.Default);
        }

        /// <summary>
        /// Opens a new serial port connection to the Pulse Pal device.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        public void Open(CancellationToken cancellationToken = default)
        {
            RunAsync(cancellationToken);
        }

        void Connect()
        {
            using var writer = new CommandWriter(this);
            writer.Write(OpMenu);
            writer.Write(Handshake);
        }

        int ProcessResponse(int count)
        {
            if (!initialized)
            {
                const int ResponseLength = 5;
                if (count < ResponseLength) return 0;
                if (readBuffer[0] != Acknowledge)
                {
                    throw new InvalidOperationException("Unexpected return value from Pulse Pal.");
                }

                firmwareVersion = BitConverter.ToInt32(readBuffer, 1);
                dacMaxValue = firmwareVersion switch
                {
                    < 20 => byte.MaxValue,
                    < 40 => ushort.MaxValue,
                    _ => throw new InvalidOperationException($"Unknown Pulse Pal firmware version {firmwareVersion}.")
                };
                initialized = true;
                return ResponseLength;
            }
            else return count;
        }

        void Disconnect()
        {
            using var writer = new CommandWriter(this);
            writer.Write(OpMenu);
            writer.Write(DisconnectCommand);
        }

        public void SetBiphasic(PulsePalChannel channel, bool isBiphasic)
        {
            ProgramParameter(channel, ParameterCode.Biphasic, isBiphasic);
        }

        public void SetPhase1Voltage(PulsePalChannel channel, double volts)
        {
            ProgramParameterVoltage(channel, ParameterCode.Phase1Voltage, volts);
        }

        public void SetPhase2Voltage(PulsePalChannel channel, double volts)
        {
            ProgramParameterVoltage(channel, ParameterCode.Phase2Voltage, volts);
        }

        public void SetPhase1Duration(PulsePalChannel channel, double seconds)
        {
            ProgramParameterTime(channel, ParameterCode.Phase1Duration, seconds);
        }

        public void SetInterPhaseInterval(PulsePalChannel channel, double seconds)
        {
            ProgramParameterTime(channel, ParameterCode.InterPhaseInterval, seconds);
        }

        public void SetPhase2Duration(PulsePalChannel channel, double seconds)
        {
            ProgramParameterTime(channel, ParameterCode.Phase2Duration, seconds);
        }

        public void SetInterPulseInterval(PulsePalChannel channel, double seconds)
        {
            ProgramParameterTime(channel, ParameterCode.InterPulseInterval, seconds);
        }

        public void SetBurstDuration(PulsePalChannel channel, double seconds)
        {
            ProgramParameterTime(channel, ParameterCode.BurstDuration, seconds);
        }

        public void SetInterBurstInterval(PulsePalChannel channel, double seconds)
        {
            ProgramParameterTime(channel, ParameterCode.InterBurstInterval, seconds);
        }

        public void SetPulseTrainDuration(PulsePalChannel channel, double seconds)
        {
            ProgramParameterTime(channel, ParameterCode.PulseTrainDuration, seconds);
        }

        public void SetPulseTrainDelay(PulsePalChannel channel, double seconds)
        {
            ProgramParameterTime(channel, ParameterCode.PulseTrainDelay, seconds);
        }

        public void SetTriggerOnChannel1(PulsePalChannel channel, bool enabled)
        {
            ProgramParameter(channel, ParameterCode.TriggerOnChannel1, enabled);
        }

        public void SetTriggerOnChannel2(PulsePalChannel channel, bool enabled)
        {
            ProgramParameter(channel, ParameterCode.TriggerOnChannel2, enabled);
        }

        public void SetCustomTrainIdentity(PulsePalChannel channel, CustomTrainId identity)
        {
            ProgramParameter(channel, ParameterCode.CustomTrainIdentity, (byte)identity);
        }

        public void SetCustomTrainTarget(PulsePalChannel channel, CustomTrainTarget target)
        {
            ProgramParameter(channel, ParameterCode.CustomTrainTarget, (byte)target);
        }

        public void SetCustomTrainLoop(PulsePalChannel channel, bool loop)
        {
            ProgramParameter(channel, ParameterCode.CustomTrainLoop, loop);
        }

        public void SetRestingVoltage(PulsePalChannel channel, double volts)
        {
            ProgramParameterVoltage(channel, ParameterCode.RestingVoltage, volts);
        }

        public void SetTriggerMode(PulsePalChannel channel, TriggerMode triggerMode)
        {
            ProgramParameter(channel, ParameterCode.TriggerMode, (byte)triggerMode);
        }

        void ProgramParameter(PulsePalChannel channel, ParameterCode parameter, bool value)
        {
            using var writer = new CommandWriter(this);
            writer.WriteProgramHeader(channel, parameter);
            writer.Write(value);
        }

        void ProgramParameter(PulsePalChannel channel, ParameterCode parameter, byte value)
        {
            using var writer = new CommandWriter(this);
            writer.WriteProgramHeader(channel, parameter);
            writer.Write(value);
        }

        void ProgramParameterVoltage(PulsePalChannel channel, ParameterCode parameter, double volts)
        {
            using var writer = new CommandWriter(this);
            writer.WriteProgramHeader(channel, parameter);
            writer.WriteVoltage(volts);
        }

        void ProgramParameterTime(PulsePalChannel channel, ParameterCode parameter, double seconds)
        {
            using var writer = new CommandWriter(this);
            writer.WriteProgramHeader(channel, parameter);
            writer.WriteTime(seconds);
        }

        public void SendCustomPulseTrain(CustomTrainId id, double[] pulseTimes, double[] pulseVoltages)
        {
            var command = id switch
            {
                CustomTrainId.CustomTrain1 => ProgramPulseTrain1,
                CustomTrainId.CustomTrain2 => ProgramPulseTrain2,
                _ => throw new ArgumentException("Invalid pulse train id.", nameof(id))
            };

            if (pulseTimes == null)
            {
                throw new ArgumentNullException(nameof(pulseTimes));
            }

            if (pulseVoltages == null)
            {
                throw new ArgumentNullException(nameof(pulseVoltages));
            }

            var nPulses = (uint)pulseTimes.Length;
            if (nPulses > MaxPulseLength)
            {
                throw new ArgumentOutOfRangeException("Exceeded the maximum allowed pulse length.", nameof(pulseTimes));
            }

            if (pulseTimes.Length != pulseVoltages.Length)
            {
                throw new ArgumentException("Array of pulse voltages must be of same length as array of pulse times.", nameof(pulseVoltages));
            }

            using var writer = new CommandWriter(this);
            writer.Write(OpMenu);
            writer.Write(command);
            if (firmwareVersion < 20)
            {
                // USB packet correction byte
                writer.Write(0);
            }

            writer.Write(nPulses);
            for (int i = 0; i < pulseTimes.Length; i++)
            {
                writer.WriteTime(pulseTimes[i]);
            }

            for (int i = 0; i < pulseVoltages.Length; i++)
            {
                writer.WriteVoltage(pulseVoltages[i]);
            }
        }

        public void TriggerChannels(TriggerChannels channels)
        {
            using var writer = new CommandWriter(this);
            writer.Write(OpMenu);
            writer.Write(TriggerCommand);
            writer.Write((byte)channels);
        }

        public void UpdateDisplay(string text)
        {
            UpdateDisplay(text, string.Empty);
        }

        public void UpdateDisplay(string row1, string row2)
        {
            const int MaxDisplayCharacters = 16;
            var textWriter = new CommandWriter(this);
            textWriter.WriteText(row1, MaxDisplayCharacters);
            if (!string.IsNullOrEmpty(row2))
            {
                textWriter.Write(LineBreak);
                textWriter.WriteText(row2, MaxDisplayCharacters);
            }

            var message = new byte[textWriter.Length];
            Array.Copy(commandBuffer, message, message.Length);

            using var writer = new CommandWriter(this);
            writer.Write(OpMenu);
            writer.Write(UpdateDisplayCommand);
            writer.Write((byte)message.Length);
            writer.Write(message);
        }

        public void SetFixedVoltage(PulsePalChannel channel, double volts)
        {
            using var writer = new CommandWriter(this);
            writer.Write(OpMenu);
            writer.Write(SetVoltageCommand);
            writer.Write((byte)channel);
            writer.WriteVoltage(volts);
        }

        public void AbortPulseTrains()
        {
            using var writer = new CommandWriter(this);
            writer.Write(OpMenu);
            writer.Write(AbortCommand);
        }

        public void SetContinuousLoop(PulsePalChannel channel, bool loop)
        {
            using var writer = new CommandWriter(this);
            writer.Write(OpMenu);
            writer.Write(LoopCommand);
            writer.Write((byte)channel);
            writer.Write(loop);
        }

        public void SetClientId(string id)
        {
            using var writer = new CommandWriter(this);
            writer.Write(OpMenu);
            writer.Write(ClientIdCommand);
            for (int i = 0; i < 6; i++)
            {
                writer.Write(i < id.Length ? (byte)id[i] : (byte)' ');
            }
        }

        public void Close()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~PulsePal()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Disconnect();
                    serialPort.Close();
                    disposed = true;
                }
            }
        }

        void IDisposable.Dispose()
        {
            Close();
        }

        struct CommandWriter : IDisposable
        {
            int offset;
            readonly PulsePal device;

            public CommandWriter(PulsePal pulsePal)
            {
                offset = 0;
                device = pulsePal;
            }

            public readonly int Length => offset;

            public void Write(byte value)
            {
                device.commandBuffer[offset++] = value;
            }

            public void Write(bool value)
            {
                device.commandBuffer[offset++] = (byte)(value ? 1 : 0);
            }

            public void Write(ushort value)
            {
                device.commandBuffer[offset++] = (byte)value;
                device.commandBuffer[offset++] = (byte)(value >> 8);
            }

            public void Write(uint value)
            {
                device.commandBuffer[offset++] = (byte)value;
                device.commandBuffer[offset++] = (byte)(value >> 8);
                device.commandBuffer[offset++] = (byte)(value >> 16);
                device.commandBuffer[offset++] = (byte)(value >> 24);
            }

            public void Write(byte[] bytes)
            {
                Array.Copy(bytes, 0, device.commandBuffer, offset, bytes.Length);
                offset += bytes.Length;
            }

            public void WriteTime(double seconds)
            {
                var cycles = GetTimeCycles((decimal)seconds);
                Write(cycles);
            }

            public void WriteVoltage(double volts)
            {
                var steps = GetVoltageSteps((decimal)volts);
                if (device.dacMaxValue > byte.MaxValue)
                {
                    Write((ushort)steps);
                }
                else Write((byte)steps);
            }

            public void WriteText(string text, int maxChars)
            {
                for (int i = 0; i < text.Length && i < maxChars; i++)
                {
                    Write((byte)text[i]);
                }
            }

            public void WriteProgramHeader(PulsePalChannel channel, ParameterCode parameter)
            {
                Write(OpMenu);
                Write(ProgramParam);
                Write((byte)parameter);
                Write((byte)channel);
            }

            readonly int GetVoltageSteps(decimal volts)
            {
                return (int)(decimal.Ceiling((volts + 10) / 20) * device.dacMaxValue);
            }

            readonly uint GetTimeCycles(decimal seconds)
            {
                var cycles = (uint)(seconds * CycleFrequency);
                ThrowIfCyclesOutOfRange(cycles, nameof(seconds));
                return cycles;
            }

            static void ThrowIfCyclesOutOfRange(uint cycles, string paramName)
            {
                if (cycles > MaxCyclePeriod)
                {
                    throw new ArgumentOutOfRangeException(
                        "The specified value exceeds the maximum allowed Pulse Pal time interval.",
                        paramName);
                }
            }

            public void Dispose()
            {
                device.serialPort.Write(device.commandBuffer, 0, offset);
                offset = 0;
            }
        }
    }
}
