﻿using System.ComponentModel.DataAnnotations;

namespace IBSAPI.Models
{
    public class TodayInspectionModel
    {
        public string Case_No { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? Call_Recv_Dt { get; set; }
        public string Date { get { return this.Call_Recv_Dt != null ? Common.ConvertDateFormat(this.Call_Recv_Dt.Value) : ""; } }
        public int Call_Sno { get; set; }
        public string Name { get; set; }
        public string Vend_Name { get; set; }
        public decimal? Qty { get; set; }        
        public string Status { get; set; }
    }
}
