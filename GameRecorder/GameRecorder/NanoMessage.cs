namespace GameRecorder
{
    public class NanoMessage
    {
        public List<NanoMessage> AllMessages;

        public NanoMessage(NanoMessageType nanoMessageType , string text)
        {
            MessageType = nanoMessageType;
            Message = text;
        }

        public NanoMessageType MessageType { get; set; }
        public string Message {  get; set; }
        public NanoMessage Set(NanoMessageType type , string text)
        {
            AllMessages.Add(this);
            return new NanoMessage(type , text);
        }
        public string ToString(bool hasTime = true)
        {
            if(hasTime) return $"[{DateTime.Now.ToString("G")}] {Message}";
            else return Message;
        }

    }
    public enum NanoMessageType
    {
        Common = 0,
        Kill = 1,
        Meeting = 2,
        GameState = 3,
        PlayerData = 4
    }
}
