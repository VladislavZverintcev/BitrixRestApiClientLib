namespace BitrixRestApiClientLib.Models
{
    public class Message : BaseMessage
    {
        #region Properties

        #region Public
        public new string ChatId { get; set; }
        #endregion Public

        #endregion Properties

        #region Constructors

        #region Public
        public Message() : base()
        {
            ChatId = string.Empty;
        }

        public Message(BaseMessage baseMessage, string chatId)
        {
            if (baseMessage == null)
            {
                throw new ArgumentNullException(nameof(baseMessage));
            }

            Id = baseMessage.Id;
            ChatId = chatId ?? string.Empty;
            AuthorId = baseMessage.AuthorId;
            Date = baseMessage.Date;
            Text = baseMessage.Text;
        }
        #endregion Public

        #endregion Constructors
    }
}