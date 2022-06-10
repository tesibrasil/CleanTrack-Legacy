using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KleanTrak.Model
{
    public class Washer
    {
        public int ID { set; get; }

        public int IDUO { get; set; }

        public int IDSede { get; set; }

        public string Code { set; get; }

        public string Description { set; get; }

        public string SerialNumber { set; get; }

        public int? TimeToClean { set; get; }

        public WasherStorageTypes Type { set; get; }

        public int PollingTime { set; get; }

        public string FolderOrFileName { set; get; }

        public string User { set; get; }

        public string Password { set; get; }

        public override string ToString() =>
            $"ID: {ID} - {Code} {Environment.NewLine}" +
            $"SerialNumber: {SerialNumber} {Environment.NewLine}" +
            $"Description: {Description} {Environment.NewLine}" +
            $"Type: {Type} {Environment.NewLine}" +
            $"FolderOrFileName: {FolderOrFileName} {Environment.NewLine}" +
            $"User: {User} {Environment.NewLine}" +
            $"Password: {Password} {Environment.NewLine}" +
            $"TimeToClean: {TimeToClean} {Environment.NewLine}" +
            $"PollingTime: {PollingTime} {Environment.NewLine}" +
            $"IdSede: {IDSede} {Environment.NewLine}";
    }
}
