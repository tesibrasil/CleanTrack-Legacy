using System;
using System.Collections.Generic;

using System.Text;

namespace ImageSI.Configuration
{
    public class SettingList : List<Setting>
    {
    }

    public class Setting
    {
        public string Section { set; get; }
        public string Key { set; get; }
        public string Value { set; get; }
        public bool Encrypted { set; get; }
    }
}
