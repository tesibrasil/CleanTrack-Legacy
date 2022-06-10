using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace amrfidmgrex
{
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("4DB0B607-41FB-3930-904E-24E68F84B57B")]
    public interface IRFIDCycle
	{
        string ID { get; set; }
        string ExamDate { get; set; }
		string SterilizationStartDate { get; set; }
		string SterilizationEndDate { get; set; }
		string ExamOperatorName { get; set; }
		string ExamOperatorSurname { get; set; }
		string SterilizationStartOperatorName { get; set; }
		string SterilizationStartOperatorSurname { get; set; }
		string SterilizationEndOperatorName { get; set; }
		string SterilizationEndOperatorSurname { get; set; }
		string DeviceIdNumber { get; set; }
		string DeviceDescription { get; set; }
		string SterilizerDescription { get; set; }
		string CustomTrackingDate1 { get; set; }
		string CustomTrackingOperatorName1 { get; set; }
		string CustomTrackingOperatorSurname1 { get; set; }
		string IdVasca { get; set; }
		string MachineCycleId { get; set; }
		string DisinfectantCycleId { get; set; }
		string AdditionalInfo { get; set; }
	}
}
