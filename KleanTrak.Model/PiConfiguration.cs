namespace KleanTrak.Model
{
    public class PiConfiguration : ObjectSerializeHelper
    {
        public string ServerHttpEndpoint { set; get; }

        public string DefaultOperationBarcode { set; get; }

        public string DefaultOperationDescription { set; get; }

        public string LabelOperationText { set; get; }

        public string LabelDeviceText { set; get; }

        public string LabelUserText { set; get; }

        public int? TimeoutCompletingSteps { set; get; }

        public int? DelayBeforeApplyChanges { set; get; }

        public bool? WorklistSelection { set; get; }

        public int? DelayBeforeClosingPopup { set; get; }
    }
}
