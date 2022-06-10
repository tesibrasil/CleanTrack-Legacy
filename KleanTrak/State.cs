using OdbcExtensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace KleanTrak
{
    public class State
    {
        public int ID { set; get; }

        public string Description { set; get; }

        public string ActionDescription { set; get; }

        public int Hue { set; get; }

        public int MenuOrder { set; get; }

        public int PrincipalListOrder { set; get; }

        public int DetailListOrder { set; get; }

        public bool StartCycle { set; get; }

        public string ShortcutKeys { set; get; }

        public int PrincipalInterfaceOrder { set; get; }

        public Bitmap Image
        {
            get
            {
                Bitmap bmpSource = kleanTrak.Properties.Resources.state;
                Bitmap bmpDest = new Bitmap(bmpSource.Width, bmpSource.Height, PixelFormat.Format24bppRgb);

                BitmapData bmpDataSource = bmpSource.LockBits(new System.Drawing.Rectangle(0, 0, bmpSource.Width, bmpSource.Height),
                                                              System.Drawing.Imaging.ImageLockMode.ReadOnly,
                                                              bmpSource.PixelFormat);

                BitmapData bmpDataDest = bmpDest.LockBits(new System.Drawing.Rectangle(0, 0, bmpDest.Width, bmpDest.Height),
                                                          System.Drawing.Imaging.ImageLockMode.ReadWrite,
                                                          bmpDest.PixelFormat);

                // Declare an array to hold the bytes of the bitmap.
                byte[] pixelValuesSource = new byte[Math.Abs(bmpDataSource.Stride) * bmpDataSource.Height];
                System.Runtime.InteropServices.Marshal.Copy(bmpDataSource.Scan0, pixelValuesSource, 0, pixelValuesSource.Length);

                byte[] pixelValuesDest = new byte[Math.Abs(bmpDataDest.Stride) * bmpDataDest.Height];

                for (int y = 0; y < bmpDataDest.Height; y++)
                {
                    for (int x = 0; x < bmpDataDest.Width; x++)
                    {
                        byte valueB = pixelValuesSource[y * bmpDataSource.Stride + x * 3 + 0];
                        byte valueG = pixelValuesSource[y * bmpDataSource.Stride + x * 3 + 1];
                        byte valueR = pixelValuesSource[y * bmpDataSource.Stride + x * 3 + 2];

                        Color color = Color.FromArgb(valueR, valueG, valueB);

                        float h = color.GetHue();
                        float s = color.GetSaturation();
                        float br = color.GetBrightness();

                        Color newColor = ColorFromAhsb(255, Hue >= 0 && Hue <= 360 ? Hue : 0, s, br);

                        pixelValuesDest[y * bmpDataDest.Stride + x * 3 + 0] = newColor.B;
                        pixelValuesDest[y * bmpDataDest.Stride + x * 3 + 1] = newColor.G;
                        pixelValuesDest[y * bmpDataDest.Stride + x * 3 + 2] = newColor.R;
                    }
                }

                System.Runtime.InteropServices.Marshal.Copy(pixelValuesDest, 0, bmpDataDest.Scan0, pixelValuesDest.Length);

                bmpDest.UnlockBits(bmpDataDest);
                bmpSource.UnlockBits(bmpDataSource);

                bmpSource.Dispose();
                return bmpDest;
            }
        }

        private Color ColorFromAhsb(int a, float h, float s, float b)
        {

            if (0 > a || 255 < a)
                return Color.FromArgb(0, 0, 0, 0);
            if (0f > h || 360f < h)
                return Color.FromArgb(0, 0, 0, 0);
            if (0f > s || 1f < s)
                return Color.FromArgb(0, 0, 0, 0);
            if (0f > b || 1f < b)
                return Color.FromArgb(0, 0, 0, 0);

            if (0 == s)
                return Color.FromArgb(a, Convert.ToInt32(b * 255), Convert.ToInt32(b * 255), Convert.ToInt32(b * 255));

            float fMax, fMid, fMin;
            int iSextant, iMax, iMid, iMin;

            if (0.5 < b)
            {
                fMax = b - (b * s) + s;
                fMin = b + (b * s) - s;
            }
            else
            {
                fMax = b + (b * s);
                fMin = b - (b * s);
            }

            iSextant = (int)Math.Floor(h / 60f);
            if (300f <= h)
            {
                h -= 360f;
            }

            h /= 60f;
            h -= 2f * (float)Math.Floor(((iSextant + 1f) % 6f) / 2f);
            if (0 == iSextant % 2)
            {
                fMid = h * (fMax - fMin) + fMin;
            }
            else {
                fMid = fMin - h * (fMax - fMin);
            }

            iMax = Convert.ToInt32(fMax * 255);
            iMid = Convert.ToInt32(fMid * 255);
            iMin = Convert.ToInt32(fMin * 255);

            switch (iSextant)
            {
                case 1:
                    return Color.FromArgb(a, iMid, iMax, iMin);
                case 2:
                    return Color.FromArgb(a, iMin, iMax, iMid);
                case 3:
                    return Color.FromArgb(a, iMin, iMid, iMax);
                case 4:
                    return Color.FromArgb(a, iMid, iMin, iMax);
                case 5:
                    return Color.FromArgb(a, iMax, iMin, iMid);
                default:
                    return Color.FromArgb(a, iMax, iMid, iMin);
            }
        }
    }

    public class StateList
    {
        private static StateList _instance = new StateList();

        private List<State> _list = new List<State>();

        protected StateList()
        {
        }

        public void Init(string odbcConnectionString)
        {
            OdbcConnection conn = new OdbcConnection(odbcConnectionString);
            OdbcCommand cmd = new OdbcCommand("SELECT * FROM STATO WHERE ELIMINATO = 0 ORDER BY ID", conn);
            OdbcDataReader reader = null;

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    _list.Add(new State() {
                        ID = reader.GetIntEx("ID"),
                        Description = reader.GetStringEx("DESCRIZIONE"),
                        ActionDescription = reader.GetStringEx("DESCRIZIONEAZIONE"),
                        Hue = reader.GetIntEx("COLORE"),
                        StartCycle = reader.GetBoolEx("INIZIOCICLO"),
                        MenuOrder = reader.GetIntEx("VISIBILEMENU"),
                        PrincipalListOrder = reader.GetIntEx("VISIBILELISTA"),
                        DetailListOrder = reader.GetIntEx("VISIBILELISTADETTAGLIO"),
                        ShortcutKeys = reader.GetStringEx("SCELTARAPIDA"),
                        PrincipalInterfaceOrder = reader.GetIntEx("VISIBILEPRINCIPALE")
                    });
                }
            }
            catch (Exception e)
            {
                LibLog.Logger.Get().Write("local", "State.Init", e.ToString(), null, LibLog.Logger.LogLevel.Error);
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                        reader.Close();
                    reader.Dispose();
                }

                if (conn.State != ConnectionState.Closed)
                    conn.Close();
                conn.Dispose();
            }
        }

        public List<State> GetList()
        {
            return _list;
        }

        public List<State> GetMenuList()
        {
            return (from s in _list
                    where s.MenuOrder > 0
                    orderby s.MenuOrder ascending
                    select s).ToList();
        }

        public List<State> GetPrincipalList()
        {
            return (from s in _list
                    where s.PrincipalListOrder > 0
                    orderby s.PrincipalListOrder ascending
                    select s).ToList();
        }

        public List<State> GetDetailList()
        {
            return (from s in _list
                    where s.DetailListOrder > 0
                    orderby s.DetailListOrder ascending
                    select s).ToList();
        }

        public List<State> GetPrincipalInterfaceList()
        {
            return (from s in _list
                    where s.PrincipalInterfaceOrder > 0
                    orderby s.PrincipalInterfaceOrder ascending
                    select s).ToList();
        }

        public static StateList Instance 
        {
            get { return _instance; }
        }
    }
}
