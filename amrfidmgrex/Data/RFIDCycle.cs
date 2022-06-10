using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace amrfidmgrex
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("8B90F1E0-A4B5-3802-919F-FFB70F974C26")]
    public class RFIDCycle : IRFIDCycle
	{
        public string ID { get; set; }

        private string _ExamDate = "";

		public string ExamDate
		{
			get { return _ExamDate; }
			set { _ExamDate = value; }
		}
		private string _SterilizationStartDate = "";

		public string SterilizationStartDate
		{
			get { return _SterilizationStartDate; }
			set { _SterilizationStartDate = value; }
		}
		private string _SterilizationEndDate = "";

		public string SterilizationEndDate
		{
			get { return _SterilizationEndDate; }
			set { _SterilizationEndDate = value; }
		}
		private string _ExamOperatorName = "";

		public string ExamOperatorName
		{
			get { return _ExamOperatorName; }
			set { _ExamOperatorName = value; }
		}
		private string _ExamOperatorSurname = "";

		public string ExamOperatorSurname
		{
			get { return _ExamOperatorSurname; }
			set { _ExamOperatorSurname = value; }
		}
		private string _SterilizationStartOperatorName = "";

		public string SterilizationStartOperatorName
		{
			get { return _SterilizationStartOperatorName; }
			set { _SterilizationStartOperatorName = value; }
		}
		private string _SterilizationStartOperatorSurname = "";

		public string SterilizationStartOperatorSurname
		{
			get { return _SterilizationStartOperatorSurname; }
			set { _SterilizationStartOperatorSurname = value; }
		}
		private string _SterilizationEndOperatorName = "";

		public string SterilizationEndOperatorName
		{
			get { return _SterilizationEndOperatorName; }
			set { _SterilizationEndOperatorName = value; }
		}
		private string _SterilizationEndOperatorSurname = "";

		public string SterilizationEndOperatorSurname
		{
			get { return _SterilizationEndOperatorSurname; }
			set { _SterilizationEndOperatorSurname = value; }
		}
		private string _DeviceIdNumber = "";

		public string DeviceIdNumber
		{
			get { return _DeviceIdNumber; }
			set { _DeviceIdNumber = value; }
		}
		private string _DeviceDescription = "";

		public string DeviceDescription
		{
			get { return _DeviceDescription; }
			set { _DeviceDescription = value; }
		}
		private string _SterilizerDescription = "";

		public string SterilizerDescription
		{
			get { return _SterilizerDescription; }
			set { _SterilizerDescription = value; }
		}


		private string _CustomTrackingOperatorName1 = "";

		public string CustomTrackingOperatorName1
		{
			get { return _CustomTrackingOperatorName1; }
			set { _CustomTrackingOperatorName1 = value; }
		}
		private string _CustomTrackingOperatorSurname1 = "";

		public string CustomTrackingOperatorSurname1
		{
			get { return _CustomTrackingOperatorSurname1; }
			set { _CustomTrackingOperatorSurname1 = value; }
		}
		private string _CustomTrackingDate1 = "";

		public string CustomTrackingDate1
		{
			get { return _CustomTrackingDate1; }
			set { _CustomTrackingDate1 = value; }
		}

		private string _IdVasca = "";

		public string IdVasca
		{
			get { return _IdVasca; }
			set { _IdVasca = value; }
		}

		private string _MachineCycleId = "";

		public string MachineCycleId
		{
			get { return _MachineCycleId; }
			set { _MachineCycleId = value; }
		}

		private string _DisinfectantCycleId = "";

		public string DisinfectantCycleId
		{
			get { return _DisinfectantCycleId; }
			set { _DisinfectantCycleId = value; }
		}

		private string _AdditionalInfo = "";

		public string AdditionalInfo
		{
			get { return _AdditionalInfo; }
			set { _AdditionalInfo = value; }
		}
	}
}
