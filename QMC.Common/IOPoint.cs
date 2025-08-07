using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace QMC.Common
{
    [Serializable]
    public class IOPoint : IActor
    {
        public enum FunctionID
        {
            Read,
            Write,
            Wait,
            Count
        }
        private bool m_bStop;
        private IOModule m_Module;
        public virtual IOModule Module => m_Module;

        public IOBoard Board { set; get; }
        private IOPointConfiguration m_Configuration;

        public uint UID
        {
            set
            {
                Configuration.UID = value;
            }
            get
            {
                return Configuration.UID;
            }
        }

        public IOPointConfiguration Configuration
        {
            get
            {
                return m_Configuration;
            }
            set
            {
                m_Configuration = value;
            }
        }
        public string PartUid
        {
            get
            {
                return Configuration.PartUid;
            }
            set
            {
                Configuration.PartUid = value;
            }
        }

        public string ModuleUid
        {
            get
            {
                return Configuration.ModuleUid;
            }
            set
            {
                Configuration.ModuleUid = value;
            }
        }

        public string Tag
        {
            get
            {
                return Configuration.Tag;
            }
            set
            {
                Configuration.Tag = value;
            }
        }

        public bool Simulated
        {
            get; set;
        }

        public string Name
        {
            set
            {
                Configuration.Name = value;
            }
            get
            {
                return Configuration.Name;
            }

        }

        public string Description => Configuration.Description;

        public int Address => Configuration.Address;
        public string Locator
        {
            get { return Configuration.Locator; }
            set { Configuration.Locator = value; }
        }
        public uint ModuleNo => Configuration.ModuleNo;
        public IoType IoType => Configuration.IoType;

        public void SetModule(IOModule module)
        {
            m_Module = module;
        }
        public virtual int Load(FileStream fs)
        {
            int ret = 0;

            //IOPointConfiguration configuration = new IOPointConfiguration();
            ret = SaveManager.BinaryDeserialize<IOPointConfiguration>(fs, out m_Configuration);
            //this.Configuration = configuration;
            return ret;
        }

        public int Save(FileStream fs)
        {
            int ret = 0;

            ret = SaveManager.BinarySerialize(fs, this.Configuration);

            return ret;
        }

        public int Initialize()
        {
            return 0;
        }

        public int Execute(uint nID, SettingParameterCollection parameter)
        {
            int ret = 0;
            DioModule module = Module as DioModule;
            DioValue value = DioValue.On;
            if (module != null)
            {
                switch ((FunctionID)nID)
                {
                    case FunctionID.Read:
                        value = module.GetValue(false, this.Address, this.IoType);
                        parameter[0].BoolValue = (value == DioValue.On ? true : false);
                        break;
                    case FunctionID.Write:
                        ret = module.SetValue(false, this.Address, parameter[0].BoolValue);
                        break;
                    case FunctionID.Wait:
                        {
                            value = parameter[0].BoolValue ? DioValue.On : DioValue.Off;
                            while (true)
                            {
                                if (m_bStop)
                                    break;
                                if (module.GetValue(false, this.Address, this.IoType) == value)
                                {
                                    break;
                                }
                                Thread.Sleep(1);
                            }
                        }
                        break;
                    default:
                        ret = -1;
                        break;
                }
                ret = 0;
                m_bStop = false;
                Thread.Sleep(5000);
                Console.WriteLine("{0} : Execute", Name);
            }
            else
            {
                ret = -1;
            }
            return ret;

        }

        public SettingParameterCollection GetParameters(uint nID)
        {
            SettingParameterCollection actionParameters = new SettingParameterCollection();
            switch ((FunctionID)nID)
            {
                case FunctionID.Read:
                    {
                        actionParameters.Add(new SettingParameter("ReadIOValue", DataType.Bool, ParameterType.IO, this));
                    }
                    break;
                case FunctionID.Write:
                    {
                        actionParameters.Add(new SettingParameter("WriteIOValue", DataType.Bool, ParameterType.IO, this));
                    }
                    break;
                case FunctionID.Wait:
                    {
                        actionParameters.Add(new SettingParameter("WaitIOValue", DataType.Bool, ParameterType.IO, this));
                    }
                    break;
                default:
                    break;
            }

            return actionParameters;
        }

        //public Function GetFunction(uint nID)
        //{
        //    Function func = new Function();
        //    func.ID = nID;
        //    func.Name = ((FunctionID)nID).ToString();

        //    return func;
        //}

        public int GetFunctionCount()
        {
            return (int)FunctionID.Count;
        }

        public int StopExecute()
        {
            m_bStop = true;
            return 0;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    [Serializable]
    public class IOPointConfiguration
    {
        public string Name { set; get; }
        public string Description { set; get; }
        public int Address { set; get; }

        public bool Enabled { set; get; }

        public IoType IoType { set; get; }

        public string ModuleUid { set; get; }

        public string Locator { set; get; }

        public string PartUid { set; get; }

        public bool Simulated { set; get; }

        public string Label { set; get; }

        public uint ModuleNo { set; get; }

        public uint UID { set; get; }

        public string Tag { set; get; }

        public IOPointConfiguration()
        {
            SetDefaultValues();
        }

        protected virtual void SetDefaultValues()
        {
            UID = 0;
            Name = "";
            Enabled = true;
            IoType = IoType.Input;
            ModuleUid = "";
            Locator = "";
            PartUid = "";
            Simulated = false;
            Label = "";
            ModuleNo = 0;
        }

    }

    [Serializable]
    public class DioPoint : IOPoint
    {

        public DioPoint() : base()
        {
            Configuration = new DioPointConfiguration();
        }

        public DioValue GetValue()
        {
            DioModule module = Module as DioModule;
            if (module != null)
            {
                //20250722 IO TEST 여기 제거.
                //module.Read();
                return module.GetValue(this.Configuration.Simulated, this.Configuration.Address, this.Configuration.IoType);
            }
            else
            {
                return DioValue.Off;
            }
        }

        public int Read()
        {
            int result = 0;
            if (Configuration.IoType == IoType.Input)
            {
                result = Module.Read();
            }
            return result;
        }

        public int Write(DioValue value)
        {
            DioModule module = Module as DioModule;
            int result = 0;
            if (!Configuration.Enabled)
            {
                return result;
            }
            if (base.Simulated)
            {
                if (Configuration.IoType == IoType.Output)
                {
                    DioBuffer dioBuffer = module.SimulationOutBuffer as DioBuffer;
                    dioBuffer.SetValue(this.Configuration.Address, value);
                }
                else
                {
                    DioBuffer dioBuffer = module.SimulationInBuffer as DioBuffer;
                    dioBuffer.SetValue(this.Configuration.Address, value);
                }
            }
            else
            {
                if (Configuration.IoType != IoType.Output)
                {
                    return result;
                }
                lock (module.OutputBuffer.SyncRoot)
                {
                    DioBuffer dioBuffer = module.OutputBuffer as DioBuffer;
                    dioBuffer.SetValue(this.Configuration.Address, value);
                    if ((result = Module.Write()) != 0)
                    {
                        return result;
                    }
                }
            }
            return result;
        }

        public override int Load(FileStream fs)
        {
            int ret = 0;
            DioPointConfiguration configuration;
            ret = SaveManager.BinaryDeserialize<DioPointConfiguration>(fs, out configuration);
            Configuration = configuration;
            return ret;
        }

    }

    public class DioPointCollection : Collection<DioPoint>
    {
        public void Add(DioPointCollection dioPoints)
        {
            foreach (DioPoint point in dioPoints)
            {
                if(point != null)
                    this.Add(point);
            }
        }

        public void Add(List<DioPoint> dioPoints)
        {
            foreach (DioPoint point in dioPoints)
            {
                if (point != null)
                    this.Add(point);
            }
        }
    }
    [Serializable]
    public class DioPointConfiguration : IOPointConfiguration
    {
        public ActiveLevel Level { set; get; }
    }



    public sealed class AioPoint : IOPoint
    {
        public double AnalogValue
        {
            get
            {
                int digitalValue = DigitalValue;
                return Digital2Analog(digitalValue);
            }
        }

        public int DigitalValue
        {
            get
            {
                AioBuffer aioBuffer = null;
                int num = 0;
                string text = "";
                if (base.Simulated)
                {
                    AioBuffer aioBuffer2;
                    if (Configuration.IoType != 0)
                    {
                        aioBuffer2 = Module.SimulationOutBuffer;
                    }
                    else
                    {
                        aioBuffer2 = Module.SimulationInBuffer;
                    }
                    aioBuffer = aioBuffer2;
                }
                else
                {
                    aioBuffer = ((Configuration.IoType == IoType.Input) ? Module.InputBuffer : Module.OutputBuffer);
                }
                num = aioBuffer.GetValue(this.Configuration.Address);
                AioItemStyles itemStyle = Configuration.ItemStyle;
                if (itemStyle != 0)
                {
                    text = BytesConverter.ToBcdString(BytesConverter.ToBytes(num));
                    num = int.Parse(text);
                }
                return num;
            }
        }

        public double Value
        {
            get
            {
                int digitalValue = DigitalValue;
                double analog = Digital2Analog(digitalValue);
                return Analog2Value(analog);
            }
        }

        public double MaxValue
        {
            get
            {
                int digitalMaximum = Configuration.DigitalMaximum;
                double analog = Digital2Analog(digitalMaximum);
                return Analog2Value(analog);
            }
        }

        public double MinValue
        {
            get
            {
                int digitalMinimum = Configuration.DigitalMinimum;
                double analog = Digital2Analog(digitalMinimum);
                return Analog2Value(analog);
            }
        }

        public new AioPointConfiguration Configuration
        {
            get
            {
                return base.Configuration as AioPointConfiguration;
            }
            set
            {
                base.Configuration = value;
            }
        }

        public new AioModule Module => base.Module as AioModule;

        public AioPoint()
        {
            Configuration = new AioPointConfiguration();
        }

        public double Digital2Analog(int digital)
        {
            double num = 0.0;
            num = Configuration.AnalogMinimum + (Configuration.AnalogMaximum - Configuration.AnalogMinimum) * ((double)(digital - Configuration.DigitalMinimum) / (double)(Configuration.DigitalMaximum - Configuration.DigitalMinimum));
            if (num < Configuration.AnalogMinimum)
            {
                num = Configuration.AnalogMinimum;
            }
            else if (num > Configuration.AnalogMaximum)
            {
                num = Configuration.AnalogMaximum;
            }
            return num + Configuration.Offset;
        }

        public double Analog2Value(double analog)
        {
            double result = analog;
            AnalogValue analogValue = default(AnalogValue);
            AnalogValue analogValue2 = default(AnalogValue);
            AnalogValueCollection analogValues = Configuration.AnalogValues;
            if (analogValues == null || analogValues.Count < 2)
            {
                return result;
            }
            bool flag;
            bool flag2 = (flag = false);
            for (int i = 0; i < analogValues.Count; i++)
            {
                if (analogValues[i].Analog == analog)
                {
                    return analogValues[i].Value;
                }
                int num;
                if (flag2)
                {
                    if (analogValue2.Analog == analog)
                    {
                        if (analog < analogValues[i].Analog)
                        {
                            num = 1;
                            goto IL_012a;
                        }
                    }
                    num = ((analogValue2.Analog > analogValues[i].Analog && analog < analogValues[i].Analog) ? 1 : 0);
                    goto IL_012a;
                }
                if (analog <= analogValues[i].Analog)
                {

                    analogValue2 = analogValues[i];
                    flag2 = true;
                }
                goto IL_0183;
            IL_0183:
                int num2;
                if (flag)
                {
                    if (analogValue.Analog == analog)
                    {
                        if (analog > analogValues[i].Analog)
                        {
                            num2 = 1;
                            goto IL_0213;
                        }
                    }
                    if (analogValue.Analog < analogValues[i].Analog)
                    {
                        num2 = ((analog > analogValues[i].Analog) ? 1 : 0);
                    }
                    else
                    {
                        num2 = 0;
                    }
                    goto IL_0213;
                }
                if (analog >= analogValues[i].Analog)
                {
                    analogValue = analogValues[i];
                    flag = true;
                }
                continue;
            IL_012a:
                if (num != 0)
                {
                    analogValue2 = analogValues[i];
                }
                goto IL_0183;
            IL_0213:
                if (num2 != 0)
                {
                    analogValue = analogValues[i];
                }
            }
            if (flag2 && flag)
            {
                result = analogValue.Value + (analogValue2.Value - analogValue.Value) / (analogValue2.Analog - analogValue.Analog) * (analog - analogValue.Analog);
            }
            else if (flag2 && !flag)
            {
                result = analogValue2.Value;
            }
            else if (!flag2 && flag)
            {
                result = analogValue.Value;
            }
            return result;
        }



        public int Analog2Digital(double analog)
        {
            if (analog < Configuration.AnalogMinimum)
            {
                analog = Configuration.AnalogMinimum;
            }
            else if (analog > Configuration.AnalogMaximum)
            {
                analog = Configuration.AnalogMaximum;
            }
            analog += Configuration.Offset;
            double value = (double)Configuration.DigitalMinimum + (double)(Configuration.DigitalMaximum - Configuration.DigitalMinimum) * ((analog - Configuration.AnalogMinimum) / (Configuration.AnalogMaximum - Configuration.AnalogMinimum));
            return (int)Math.Round(value, 0);
        }

        public double Value2Analog(double value)
        {
            double result = value;
            AnalogValue analogValue = default(AnalogValue);
            AnalogValue analogValue2 = default(AnalogValue);
            AnalogValueCollection analogValues = Configuration.AnalogValues;
            if (analogValues == null || analogValues.Count < 2)
            {
                return value;
            }
            bool flag;
            bool flag2 = (flag = false);
            for (int i = 0; i < analogValues.Count; i++)
            {
                if (analogValues[i].Value == value)
                {
                    return analogValues[i].Analog;
                }
                int num;
                if (flag2)
                {
                    if (analogValue2.Value == value)
                    {
                        if (value < analogValues[i].Value)
                        {
                            num = 1;
                            goto IL_011e;
                        }
                    }
                    if (analogValue2.Value > analogValues[i].Value)
                    {
                        num = ((value < analogValues[i].Value) ? 1 : 0);
                    }
                    else
                    {
                        num = 0;
                    }
                    goto IL_011e;
                }
                if (value <= analogValues[i].Value)
                {
                    analogValue2 = analogValues[i];
                    flag2 = true;
                }
                goto IL_0163;
            IL_011e:
                if (num != 0)
                {
                    analogValue2 = analogValues[i];
                }
                goto IL_0163;
            IL_01f1:
                int num2;
                if (num2 == 0)
                {
                    continue;
                }

                analogValue = analogValues[i];
                continue;
            IL_0163:
                if (flag)
                {
                    if (analogValue.Value == value)
                    {
                        if (value > analogValues[i].Value)
                        {
                            num2 = 1;
                            goto IL_01f1;
                        }
                    }
                    if (analogValue.Value < analogValues[i].Value)
                    {
                        num2 = ((value > analogValues[i].Value) ? 1 : 0);
                    }
                    else
                    {
                        num2 = 0;
                    }
                    goto IL_01f1;
                }
                if (!(value >= analogValues[i].Value))
                {
                    continue;
                }
                analogValue = analogValues[i];
                flag = true;
            }

            if (flag2 && flag)
            {
                result = analogValue.Analog + (analogValue2.Analog - analogValue.Analog) / (analogValue2.Value - analogValue.Value) * (value - analogValue.Value);
            }
            else
            {
                int num3;
                if (flag2)
                {
                    num3 = ((!flag) ? 1 : 0);
                }
                else
                {
                    num3 = 0;
                }
                if (num3 != 0)
                {
                    result = analogValue2.Analog;
                }
                else if (!flag2 && flag)
                {
                    result = analogValue.Analog;
                }
            }
            return result;
        }

        public int Read()
        {
            int result = 0;
            if (Configuration.IoType == IoType.Input)
            {
                result = Module.Read();
            }
            return result;
        }

        public int Write(double value)
        {
            int result = 0;
            double num = 0.0;
            int num2 = 0;
            if (!Configuration.Enabled)
            {
                return result;
            }
            num = Value2Analog(value);
            num2 = Analog2Digital(num);
            AioItemStyles itemStyle = Configuration.ItemStyle;
            if (itemStyle != 0)
            {
                byte[] bytes = BytesConverter.ToBytesFromBcd(num2.ToString());
                num2 = BytesConverter.ToInt32(bytes);
            }
            if (base.Simulated)
            {
                if (Configuration.IoType == IoType.Output)
                {
                    Module.SimulationOutBuffer.SetValue(this.Configuration.Address, num2);
                }
                else
                {
                    Module.SimulationInBuffer.SetValue(this.Configuration.Address, num2);
                }
            }
            else
            {
                if (Configuration.IoType != IoType.Output)
                {
                    throw new InvalidOperationException();
                }
                Module.OutputBuffer.SetValue(this.Configuration.Address, num2);
                if ((result = Module.Write()) != 0)
                {
                    return result;
                }
            }
            return result;
        }
    }
    public class AioPointConfiguration : IOPointConfiguration
    {
        public string Unit { set; get; }

        public double AnalogMaximum { set; get; }

        public double AnalogMinimum { set; get; }

        public double Offset { set; get; }

        public int DigitalMaximum { set; get; }

        public int DigitalMinimum { set; get; }

        public AioItemStyles ItemStyle { set; get; }

        public AnalogValueCollection AnalogValues { set; get; }
        public AioPointConfiguration()
        {
            SetDefaultValues();
        }

        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();
            Unit = "";
            AnalogMaximum = 5.0;
            AnalogMinimum = 0.0;
            Offset = 0.0;
            DigitalMaximum = 4096;
            DigitalMinimum = 0;
            ItemStyle = AioItemStyles.Number;
            AnalogValues = new AnalogValueCollection();
        }
    }
}
