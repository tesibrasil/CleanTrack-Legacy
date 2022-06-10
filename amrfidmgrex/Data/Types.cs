using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace amrfidmgrex
{
    [ComVisible(true)]
    [Guid("3A26332B-4B38-38D3-A9AB-ED6CBD1F9277")]
    public class Types
	{
		public enum State
		{
			Unknown = 0,
			Clean = 1,
			Dirty = 2,
			Washing = 3,
			PreWashing = 4,
			Stored = 5,
			Expired = 6,
            ConsignedForExam = 7,
            InLeakTest = 8
		};

		public enum Result
		{
			Unknown,
			Error,
			Success,
			Timeout
        }

        [ComVisible(true)]
        [Guid("A4A50047-5EE2-3C2D-84C4-3EB399C9843A")]
        public class Info
		{
			public Result Result { get; set; }
			public string Description { get; set; }
			public int IdStepType { get; set; }
		}

		public delegate void CompleteDelegate(Info info);
	}
}
