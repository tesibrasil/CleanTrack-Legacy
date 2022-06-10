using System.Runtime.InteropServices;

namespace amrfidmgrex
{
  [ClassInterface(ClassInterfaceType.None)]
  [ComVisible(true)]
  public class RFIDCiclo : IRFIDCiclo
  {
    private string _ExamDate = "";
    private string _SterilizationStartDate = "";
    private string _SterilizationEndDate = "";
    private string _ExamOperatorName = "";
    private string _ExamOperatorSurname = "";
    private string _SterilizationStartOperatorName = "";
    private string _SterilizationStartOperatorSurname = "";
    private string _SterilizationEndOperatorName = "";
    private string _SterilizationEndOperatorSurname = "";
    private string _DeviceIdNumber = "";
    private string _DeviceDescription = "";
    private string _SterilizerDescription = "";
    private string _CustomTrackingOperatorName1 = "";
    private string _CustomTrackingOperatorSurname1 = "";
    private string _CustomTrackingDate1 = "";
    private string _CustomTrackingOperatorName2 = "";
    private string _CustomTrackingOperatorSurname2 = "";
    private string _CustomTrackingDate2 = "";
    private string _IdVasca = "";
    private string _MachineCycleId = "";
    private string _DisinfectantCycleId = "";
    private string _AdditionalInfo = "";

    public string ExamDate
    {
      get
      {
        return this._ExamDate;
      }
      set
      {
        this._ExamDate = value;
      }
    }

    public string SterilizationStartDate
    {
      get
      {
        return this._SterilizationStartDate;
      }
      set
      {
        this._SterilizationStartDate = value;
      }
    }

    public string SterilizationEndDate
    {
      get
      {
        return this._SterilizationEndDate;
      }
      set
      {
        this._SterilizationEndDate = value;
      }
    }

    public string ExamOperatorName
    {
      get
      {
        return this._ExamOperatorName;
      }
      set
      {
        this._ExamOperatorName = value;
      }
    }

    public string ExamOperatorSurname
    {
      get
      {
        return this._ExamOperatorSurname;
      }
      set
      {
        this._ExamOperatorSurname = value;
      }
    }

    public string SterilizationStartOperatorName
    {
      get
      {
        return this._SterilizationStartOperatorName;
      }
      set
      {
        this._SterilizationStartOperatorName = value;
      }
    }

    public string SterilizationStartOperatorSurname
    {
      get
      {
        return this._SterilizationStartOperatorSurname;
      }
      set
      {
        this._SterilizationStartOperatorSurname = value;
      }
    }

    public string SterilizationEndOperatorName
    {
      get
      {
        return this._SterilizationEndOperatorName;
      }
      set
      {
        this._SterilizationEndOperatorName = value;
      }
    }

    public string SterilizationEndOperatorSurname
    {
      get
      {
        return this._SterilizationEndOperatorSurname;
      }
      set
      {
        this._SterilizationEndOperatorSurname = value;
      }
    }

    public string DeviceIdNumber
    {
      get
      {
        return this._DeviceIdNumber;
      }
      set
      {
        this._DeviceIdNumber = value;
      }
    }

    public string DeviceDescription
    {
      get
      {
        return this._DeviceDescription;
      }
      set
      {
        this._DeviceDescription = value;
      }
    }

    public string SterilizerDescription
    {
      get
      {
        return this._SterilizerDescription;
      }
      set
      {
        this._SterilizerDescription = value;
      }
    }

    public string CustomTrackingOperatorName1
    {
      get
      {
        return this._CustomTrackingOperatorName1;
      }
      set
      {
        this._CustomTrackingOperatorName1 = value;
      }
    }

    public string CustomTrackingOperatorSurname1
    {
      get
      {
        return this._CustomTrackingOperatorSurname1;
      }
      set
      {
        this._CustomTrackingOperatorSurname1 = value;
      }
    }

    public string CustomTrackingDate1
    {
      get
      {
        return this._CustomTrackingDate1;
      }
      set
      {
        this._CustomTrackingDate1 = value;
      }
    }

    public string CustomTrackingOperatorName2
    {
      get
      {
        return this._CustomTrackingOperatorName2;
      }
      set
      {
        this._CustomTrackingOperatorName2 = value;
      }
    }

    public string CustomTrackingOperatorSurname2
    {
      get
      {
        return this._CustomTrackingOperatorSurname2;
      }
      set
      {
        this._CustomTrackingOperatorSurname2 = value;
      }
    }

    public string CustomTrackingDate2
    {
      get
      {
        return this._CustomTrackingDate2;
      }
      set
      {
        this._CustomTrackingDate2 = value;
      }
    }

    public string IdVasca
    {
      get
      {
        return this._IdVasca;
      }
      set
      {
        this._IdVasca = value;
      }
    }

    public string MachineCycleId
    {
      get
      {
        return this._MachineCycleId;
      }
      set
      {
        this._MachineCycleId = value;
      }
    }

    public string DisinfectantCycleId
    {
      get
      {
        return this._DisinfectantCycleId;
      }
      set
      {
        this._DisinfectantCycleId = value;
      }
    }

    public string AdditionalInfo
    {
      get
      {
        return this._AdditionalInfo;
      }
      set
      {
        this._AdditionalInfo = value;
      }
    }
  }
}
