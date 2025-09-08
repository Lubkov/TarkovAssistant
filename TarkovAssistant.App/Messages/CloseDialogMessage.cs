namespace TarkovAssistant.App.Messages
{
    public sealed class CloseDialogMessage
    {
        public CloseDialogMessage(bool? result)
        {
            Result = result;
        }

        public bool? Result { get; }
    }
}
