using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CWA150SA_Onsemi300
{
    public class EquipmentExecutePartPropertyGrid : ExpandableObjectConverter
    {
        public string WaitExecute { get; set; } 

        public string withExecute { get; set; } 

        public int ExecuteOrder { get; set; } 
        public EquipmentExecutePartPropertyGrid()
        {
            SetDefaultValues();

        }
        public void SetDefaultValues()
        {
            this.WaitExecute = "10";

            this.withExecute = "10";

            this.ExecuteOrder = 10;
        }
           
    }
   
    public class EquipmentExecutePartPropertyGridCollection : Collection<EquipmentExecutePartPropertyGrid>
    {

    }
}
