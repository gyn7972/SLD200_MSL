using QMC.Common.NationalInstruments;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QMC.Common.Keithley.SourceMeter
{
    [Serializable]
    #region KeithleySMU
    public class KeithleySMU : Part
    {
        #region Define
        public delegate void MeasureFinished();
        #endregion

        #region Field
        public event MeasureFinished measureFinished;

        private VISALAN m_Communicator;
        private KeithleySMURecipe m_Recipe;

        public Module m_Owner;
        #endregion

        #region Constructor
        public KeithleySMU(string strName) : base(strName)
        {
            this.Communicator = new VISALAN();
        }
        #endregion

        #region Property
        public VISALAN Communicator
        {
            get { return this.m_Communicator; }
            set { this.m_Communicator = value; }
        }

        public KeithleySMURecipe Recipe
        {
            get { return this.m_Recipe; }
            set { this.m_Recipe = value; }
        }

        public SmuResultCollection Results { get; set; }

        public bool IsOpen { get; protected set; }
        #endregion

        #region Method
        public int Open()
        {
            int ret = 0;

            if (this.Communicator == null) return ret;

            if ((ret = this.Communicator.Init()) != 0)
            {
                Log.Write(this, "Communicator Open Error");
                return ret;
            }

            if (this.Communicator.IsOpen == true)
            {
                this.IsOpen = true;
                Log.Write(this, "Communicator Open Success");
            }

            return ret;
        }

        public int ConnectClose()
        {
            int ret = 0;

            if (this.Communicator == null) return ret;

            if ((ret = this.Communicator.Close()) != 0)
            {
                Log.Write(this, "Communicator Close Error");
                return ret;
            }

            if (this.Communicator.IsOpen == false)
            {
                this.IsOpen = false;
                Log.Write(this, "Communicator Close Success");
            }

            return ret;
        }

        public int LoadScript(string path)
        {
            int ret = 0;
            byte[] packet;
            string arrangeText = "";
            string arrangeScriptText = "";
            int lineCommentPosition = 0;
            bool comment = false;

            if (path == string.Empty) return ret;

            #region File Upload   
            if (path != null)
            {
                string[] textValue = System.IO.File.ReadAllLines(path);
                if (textValue.Length > 0)
                {
                    for (int i = 0; i < textValue.Length; i++)
                    {
                        //텍스트의 앞 뒤 빈공간 제거
                        arrangeText = textValue[i].Trim();

                        //빈 텍스트 경우 패스
                        if (arrangeText == "") continue;

                        //범위 주석처리 경우 패스
                        if (arrangeText.Contains("--[[") == true)
                        {
                            comment = true;
                            continue;
                        }
                        if (arrangeText.Contains("]]--") == true)
                        {
                            comment = false;
                            continue;
                        }
                        if (comment == true) continue;

                        //주석처리 경우 주석처리 앞에 텍스트가 있다면 가져온다.
                        lineCommentPosition = arrangeText.IndexOf("--");
                        if (lineCommentPosition > 0)
                            arrangeText = arrangeText.Substring(0, lineCommentPosition).Trim();

                        if (arrangeText == "") continue;

                        try
                        {
                            arrangeScriptText = arrangeText + "\n";
                            this.Communicator.Send(arrangeScriptText);
                        }
                        catch (Exception ex)
                        {
                            Log.Write(this, ex.Message);
                            MessageBox.Show(ex.Message);
                        }
                    }
                }

                string scriptName = this.Recipe.ScriptName;

                //ScriptName으로 대체 필요.
                if ((ret = this.Communicator.Send($"{scriptName}()")) != 0)
                {
                    Log.Write(this, "Script Name is Wrong");
                    //MessageBox.Show("Script Name Error");
                    return ret;
                }

                Log.Write(this, "Script Load Success");
            }
            else
            {
                Log.Write(this, "Script Path is not Found");
                MessageBox.Show("Script Path is not Found");
                return ret;
            }
            #endregion

            return ret;
        }

        public int ResetScript()
        {
            int ret = 0;

            //if (m_Owner == null) return ret;

            if (this.Communicator == null) return ret;

            string scriptName = this.Recipe.ScriptName;

            if ((ret = this.Communicator.Send($"if \"scriptName\" ~= nil then script.delete(\"scriptName\") end")) != 0)
            {
                Log.Write(this, "Script Clear Failed");
                return ret;
            }

            Log.Write(this, "Script Clear Success");

            return ret;
        }

        public int Measure()
        {
            int ret = 0;
            int count = 0;

            //if (this.Recipe.SubstrateCount == 0 || this.Recipe.SubstrateCount < 0)
            //    count = 1;
            //else
            //    count = this.Recipe.SubstrateCount;

            if (!Communicator.CheckRemainDatas())
            {
                Log.Write(this, "CheckRemainDatas Failed");
                MessageBox.Show("CheckRemainDatas Failed");
                return ret;
            }

            if ((ret = Communicator.Send("defbuffer1.clear()")) != 0)
            {
                Log.Write(this, "defbuffer1 Clear Command Send Error");
                MessageBox.Show("defbuffer1 Clear Command Send Error");
                return ret;
            }

            //TEST - smtUnit받아서 바꾸기
            StringBuilder temp = new StringBuilder();
            temp.Append("{");
            temp.Append("\"1001\",");
            temp.Append("\"1002\",");
            temp.Append("\"1003\",");
            temp.Append("\"1004\",");
            temp.Append("\"1005\",");
            temp.Append("\"1006\",");
            temp.Append("\"1007\",");
            temp.Append("\"1008\",");
            temp.Append("\"1009\",");
            temp.Append("\"1010\"");
            temp.Append("}");
            if ((ret = this.Communicator.Send($"scanning_sample({temp})")) != 0)
            {
                Log.Write(this, "scanning_sample Send Error");
                MessageBox.Show("scanning_sample Send Error");
                return ret;
            }

            Log.Write(this, "Communicator All Measure Success");

            return ret;
        }

        public int MeasureUnit(StringBuilder units)
        {
            int ret = 0;
            int count = 0;

            //if (this.Recipe.SubstrateCount == 0 || this.Recipe.SubstrateCount < 0)
            //    count = 1;
            //else
            //    count = this.Recipe.SubstrateCount;

            if (!Communicator.CheckRemainDatas())
            {
                Log.Write(this, "CheckRemainDatas Failed");
                MessageBox.Show("CheckRemainDatas Failed");
                return ret;
            }

            if ((ret = Communicator.Send("defbuffer1.clear()")) != 0)
            {
                Log.Write(this, "defbuffer1 Clear Command Send Error");
                MessageBox.Show("defbuffer1 Clear Command Send Error");
                return ret;
            }

            if ((ret = this.Communicator.Send($"scanning_sample({units})")) != 0)
            {
                Log.Write(this, "scanning_sample Send Error");
                MessageBox.Show("scanning_sample Send Error");
                return ret;
            }

            Log.Write(this, "Communicator All Measure Success");

            return ret;
        }

        public int MeasureUnit(StringBuilder vfUnits, StringBuilder ntcUnits = null, double vf1Delay = 0.05, double vf2Delay = 0.05, double ntcDelay = 0.05)
        {
            int ret = 0;

            if (!Communicator.CheckRemainDatas())
            {
                Log.Write(this, "CheckRemainDatas Failed");
                return ret;
            }

            if ((ret = Communicator.Send("defbuffer1.clear()")) != 0)
            {
                Log.Write(this, "defbuffer1 Clear Command Send Error");
                return ret;
            }

            //만약에 엔티씨유닛이 안되면 닐로 보내기.
            if (ntcUnits == null)
            {
                ntcUnits = new StringBuilder();
                ntcUnits.Append("nil");
            }
            if ((ret = this.Communicator.Send($"scanning_sample({vfUnits},{ntcUnits},{vf1Delay},{vf2Delay},{ntcDelay})")) != 0)
            {
                Log.Write(this, "scanning_sample Send Error");
                MessageBox.Show("scanning_sample Send Error");
                return ret;
            }

            Log.Write(this, "Communicator All Measure Success");

            return ret;
        }

        public int MeasureVf1()
        {
            int ret = 0;

            return ret;
        }

        public int MeasureVf2()
        {
            int ret = 0;

            return ret;
        }

        public int MeasureNTC()
        {
            int ret = 0;

            return ret;
        }

        public int ReceiveData(out string data)
        {
            int ret = 0;
            data = string.Empty;

            if ((ret = this.Communicator.Receive(out data)) != 0)
            {
                Log.Write(this, "Data Receive Error");
                MessageBox.Show("Data Receive Error");
                return ret;
            }

            if ((ret = this.SortData(data)) != 0)
            {
                Log.Write(this, "SortData Error");
                MessageBox.Show("SortData Error");
                return ret;
            }
            if (measureFinished != null)
                measureFinished();

            Log.Write(this, "Communicator Recieve Success");

            return ret;
        }

        public int ReceiveUnitData(Carrier carrier, out string data)
        {
            int ret = 0;
            data = string.Empty;

            if ((ret = this.Communicator.Receive(out data)) != 0)
            {
                Log.Write(this, "Data Receive Error");
                return ret;
            }

            if ((ret = this.SortUnitData(data, carrier)) != 0)
            {
                Log.Write(this, "SortData Error");
                return ret;
            }
            if (measureFinished != null)
            {
                measureFinished();
                Log.Write(this, "Occur MeasureFinished Event");
            }

            Log.Write(this, "Communicator Recieve Success");

            return ret;
        }

        /// <summary>
        /// 미사용
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int SortData(string data)
        {
            int ret = 0;
            int cnt = 0;
            SmuResult result = null;

            if (data == string.Empty) return ret;
            if (data.Count() == 0) return ret;


            this.Results = new SmuResultCollection();

            //Parsing
            string[] temp = data.Trim().Split(',');

            for (int i = 0; i < temp.Count() / 6; i++)
            {
                string resultState = "Ok";
                result = new SmuResult();

                //TODO : Unit Index로 넣어줘야함
                result.Index = i + 1;

                //Carrier정보 얻어와서 넣기 필요.
                result.Barcode = "Barcode" + i.ToString();

                result.Vf1 = double.Parse(temp[cnt * 6 + 1]);
                result.Vf2 = double.Parse(temp[cnt * 6 + 4]);

                //Range설정으로 판단 필요.
                if ((result.Vf1 < Recipe.Vf1 || result.Vf1 >= 18.0) && Recipe.UseInspectVf1)
                {
                    resultState = "NG";
                }
                if ((result.Vf2 < Recipe.Vf2 || result.Vf2 >= 18.0) && Recipe.UseInspectVf2)
                {
                    resultState = "NG";
                }
                if ((!Recipe.NTC.Contains(result.NTC)) && Recipe.UseInspectNTC)
                {
                    resultState = "NG";
                }

                cnt++;
                result.Result = resultState;

                //TODO : NTC추가 필요.

                this.Results.Add(result);

                //TODO : SmtUnit 정보 가져오게되면 캐리어정보 넣어주기.
                Log.Write(this, String.Format("[Result][{0} → State : {1}, Barcode : {2}, Vf1 : {3}, Vf2 : {4}, NTC : {5}", result.Index, result.Result, result.Barcode, result.Vf1, result.Vf2, result.NTC));
            }

            return ret;
        }

        public int SortUnitData(string data, Carrier carrier)
        {
            int ret = 0;
            int cnt = 0;
            SmuResult result = null;

            if (data == string.Empty) return ret;
            if (data.Count() == 0) return ret;

            this.Results = new SmuResultCollection();

            string[] temp = data.Trim().Split(',');

            for (int i = 0; i < 10; i++)
            {
                result = new SmuResult();
                
                result.Index = i + 1;

                if (carrier.ArrSmtUnit[i].UseWork == true)
                {
                    result.Barcode = carrier.ArrSmtUnit[i].ObjID;

                    if (Recipe.UseInspectVf1 && Recipe.UseInspectVf2 && Recipe.UseInspectNTC)
                    {
                        //result.Vf1 = double.Parse(temp[cnt * 9 + 1]);
                        //result.Vf2 = double.Parse(temp[cnt * 9 + 4]);
                        //result.NTC = double.Parse(temp[cnt * 9 + 7]) / double.Parse(temp[cnt * 9 + 8]);
                        result.Vf1 = (double)decimal.Parse(temp[cnt * 9 + 1], System.Globalization.NumberStyles.Float);
                        result.Vf2 = (double)decimal.Parse(temp[cnt * 9 + 4], System.Globalization.NumberStyles.Float);
                        result.NTC = (double)decimal.Parse(temp[cnt * 9 + 7], System.Globalization.NumberStyles.Float);
                    }
                    else if (Recipe.UseInspectVf1 && Recipe.UseInspectVf2)
                    {
                        //result.Vf1 = double.Parse(temp[cnt * 6 + 1]);
                        //result.Vf2 = double.Parse(temp[cnt * 6 + 4]);
                        result.Vf1 = (double)decimal.Parse(temp[cnt * 6 + 1], System.Globalization.NumberStyles.Float);
                        result.Vf2 = (double)decimal.Parse(temp[cnt * 6 + 4], System.Globalization.NumberStyles.Float);
                        Log.Write("Data Vallue", string.Format("Vf1 : {0}, Vf2 : {1}",result.Vf1, result.Vf2));
                        Log.Write("Data Vallue", string.Format("Vf1 decimal : {0}", decimal.Parse(temp[cnt * 6 + 1], System.Globalization.NumberStyles.Float)));
                        Log.Write("Data Vallue", string.Format("Vf2 decimal : {0}", decimal.Parse(temp[cnt * 6 + 4], System.Globalization.NumberStyles.Float)));
                    }

                    //Range설정으로 판단 필요.
                    if ((result.Vf1 < Recipe.Vf1|| result.Vf1 >= 9.3) && Recipe.UseInspectVf1)
                    {
                        if(result.Vf1>=9.3)
                        {
                            result.Vf1 = 0.0;
                        }
                        carrier.ArrSmtUnit[i].Result = SmtUnit.ResultKey.Ng;

                    }
                    if ((result.Vf2 < Recipe.Vf2 || result.Vf2 >= 11.0) && Recipe.UseInspectVf2)
                    {
                        if (result.Vf2 >= 11.0)
                        {
                            result.Vf2 = 0.0;
                        }
                        carrier.ArrSmtUnit[i].Result = SmtUnit.ResultKey.Ng;
                    }
                    if ((!Recipe.NTC.Contains(result.NTC)) && Recipe.UseInspectNTC)
                    {
                        carrier.ArrSmtUnit[i].Result = SmtUnit.ResultKey.Ng;
                    }

                    carrier.ArrSmtUnit[i].WorkStatus = SmtUnit.UnitWorkStateKey.Complete;

                    cnt++;
                }
                result.Result = carrier.ArrSmtUnit[i].Result.ToString();

                carrier.ArrSmtUnit[i].MeasureData = result;

                this.Results.Add(result);

                Log.Write(this, String.Format("[Result][{0} → State : {1}, Barcode : {2}, Vf1 : {3}, Vf2 : {4}, NTC : {5}", result.Index, result.Result, result.Barcode, result.Vf1, result.Vf2, result.NTC));
            }

            return ret;
        }
        #endregion

        #region Part Members
        public override void UpdateRecipeData()
        {
            base.UpdateRecipeData();

            this.Communicator.ReceiveTimeout = Recipe.ReceiveTimeout;

        }

        protected override void InitAlarm()
        {
            base.InitAlarm();



        }
        #endregion
    }
    #endregion

    #region SmuResult
    public class SmuResult
    {

        #region Constructor
        public SmuResult()
        {
            this.Index = 1;
            this.Barcode = "Barcode";
            this.Result = "NG";
            this.Vf1 = 0.0;
            this.Vf2 = 0.0;
            this.NTC = 0.0;
        }
        #endregion

        public int Index { get; set; }
        public string Barcode { get; set; }
        public string Result { get; set; }
        public double Vf1 { get; set; }
        public double Vf2 { get; set; }
        public double NTC { get; set; }

        public override string ToString()
        {
            string value = string.Empty;

            value = string.Format($"Vf1:{Vf1}@Vf2:{Vf2}@NTC:{NTC}");

            return value;
        }
    }
    #endregion

    public class SmuResultCollection : Collection<SmuResult>
    {
        public SmuResultCollection()
        {

        }
    }
}