using BitrixRestApiClientLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BitrixRestApiClientLib.Buisness
{
    public class BitrixClient
    {

        #region Fields
        string startUrl = "";
        HttpClient client = new HttpClient();
        #endregion Fields

        #region Constructors
        public BitrixClient(string webhookUrl)
        {
            if (string.IsNullOrEmpty(webhookUrl)) throw new ArgumentNullException();
            startUrl = webhookUrl;
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
        #endregion Constructors

        #region Methods

        #region Public
        public List<UserShort> GetUsers()
        {
            try
            {
                BitrixResponseUserGet response = client.GetFromJsonAsync<BitrixResponseUserGet>($"{startUrl}user.get.json").Result;
                return response.Result;
            }
            catch
            {
                return null;
            }
        }
        public bool SendMessageToDialog(string dialogId, string text, out int zeroOrMsgID, bool IsSystemMessage = true)
        {
            string GetSystemAttribute()
            {
                if (IsSystemMessage)
                {
                    return "Y";
                }
                else { return "N"; }
            }
            //Получить id чата можно отправив в чат сообщение /getChatId
            try
            {
                var values = new Dictionary<string, string>
                {
                    { "DIALOG_ID", $"{dialogId}" },
                    { "MESSAGE", $"{text}" },
                    { "SYSTEM", GetSystemAttribute() },
                    { "ATTACH", "" },
                    { "URL_PREVIEW", "Y" },
                    { "KEYBOARD", "" },
                    { "MENU", "" },
                };
                var content = new FormUrlEncodedContent(values);
                var response = client.PostAsync($"{startUrl}im.message.add.json", content);
                zeroOrMsgID = response.Result.Content.ReadFromJsonAsync<MsgResult>().Result.result;
                return response.IsCompletedSuccessfully;
            }
            catch
            {
                zeroOrMsgID = 0;
                return false;
            }
        }
        public List<Message> GetMessagesFromChat(string chatId, int limit)
        {
            try
            {
                var values = new Dictionary<string, string>
                {
                    { "DIALOG_ID", $"{chatId}" },
                    { "LIMIT", $"{limit}" },
                };
                var content = new FormUrlEncodedContent(values);

                var response = client.PostAsync($"{startUrl}im.dialog.messages.get.json", content);

                if (response.IsCompletedSuccessfully)
                {
                    var result = response.Result.Content.ReadFromJsonAsync<BitrixResponseImDialogMessagesGet>().Result;
                    return result.Result.Messages;
                }
                return new List<Message>();
            }
            catch
            {

                return new List<Message>();
            }
        }
        public bool UpdateMessage(int messageId, string text)
        {
            try
            {
                var values = new Dictionary<string, string>
                {
                    { "MESSAGE_ID", $"{messageId}" },
                    { "MESSAGE", $"{text}" },
                    { "ATTACH", "" },
                    { "URL_PREVIEW", "Y" },
                    { "KEYBOARD", "" },
                    { "MENU", "" },
                };
                var content = new FormUrlEncodedContent(values);
                var response = client.PostAsync($"{startUrl}im.message.update.json", content);
                return response.IsCompletedSuccessfully;
            }
            catch
            {
                return false;
            }
        }

        #endregion Public

        #endregion Methods
    }
}
