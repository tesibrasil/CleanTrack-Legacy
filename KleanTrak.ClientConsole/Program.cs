using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KleanTrak.Model;

namespace KleanTrak.ClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string serverUrl = "";
            Uri uri = null;

            do
            {
                Console.Write("Server endpoint (http://127.0.0.1:8091/AcceptMessage): ");
                serverUrl = Console.ReadLine();
                if (serverUrl == "")
                    serverUrl = "http://127.0.0.1:8091/AcceptMessage";
            }
            while (!Uri.TryCreate(serverUrl, UriKind.Absolute, out uri) || !CheckServer(serverUrl));
            
            string operationBarcode = "";

            do
            {
                Console.Write("Operation barcode: ");
                operationBarcode = Console.ReadLine();
            }
            while (!CheckBarcode(serverUrl, operationBarcode, BarcodeTypes.Operation));

            string deviceBarcode = "";

            do
            {
                Console.Write("Device barcode: ");
                deviceBarcode = Console.ReadLine();
            }
            while (!CheckBarcode(serverUrl, deviceBarcode, BarcodeTypes.Device));

            string userBarcode = "";

            do
            {
                Console.Write("User barcode: ");
                userBarcode = Console.ReadLine();
            }
            while (!CheckBarcode(serverUrl, userBarcode, BarcodeTypes.Operator));

            HttpClient.Send(serverUrl, new CmdSetDeviceStatus
            { 
                OperationBarcode = operationBarcode, 
                DeviceBarcode = deviceBarcode, 
                UserBarcode = userBarcode, 
                WorklistItemID = 0 
            });

            Console.Write("Press key to exit....");
            Console.Read();
        }

        static bool CheckServer(string serverUrl)
        {
            Response response = (Response)HttpClient.Send(serverUrl, new CmdPing());
            return response.Successed;
        }

        static bool CheckBarcode(string serverUrl, string barcode, BarcodeTypes barcodeType)
        {
            Response response = (Response)HttpClient.Send(serverUrl, new CmdGetInfoFromBarcode() { Barcode = barcode });
            return (response is CmdGetInfoFromBarcodeResponse && response.Successed && ((CmdGetInfoFromBarcodeResponse)response).BarcodeType == barcodeType);
        }
    }
}
