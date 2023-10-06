using BitrixRestApiClientLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BitrixRestApiClientLib.Buisness
{
    /// <summary>
    /// Web клиент с REST-API интерфейсом для взаимодействия с Bitrix24.
    /// </summary>
    public class BitrixClient
    {

        #region Fields
        string startUrl = "";
        HttpClient client = new HttpClient();
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Конструктор клиента с указанием url адреса входяхего ВебХука.
        /// </summary>
        /// <param name="webhookUrl">Url адрес входящего ВебХука.</param>
        public BitrixClient(string webhookUrl)
        {
            if (string.IsNullOrEmpty(webhookUrl)) throw new ArgumentNullException();
            startUrl = webhookUrl;
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
        #endregion Constructors

        #region Methods

        #region Public
        /// <summary>
        /// Получает список всех пользователей в Bitrix.
        /// </summary>
        /// <returns>При успешном запросе возвращает список всех пользователей, в противном случае возвращает "Null"., </returns>
        public List<UserShort>? GetUsers()
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
        /// <summary>
        /// Отправка сообщения в чат или в диалог.
        /// </summary>
        /// <param name="dialogId">ID Чата или ID диалога, узнать ID можно написав в чате "/getChatId".</param>
        /// <param name="text">Текст сообщения.</param>
        /// <param name="zeroOrMsgID">Ответ на запрос от сервера как ID успешно созданного сообщения или в случае ошибки возвращает "0".</param>
        /// <param name="IsSystemMessage">Значение</param>
        /// <returns>В случае успешной отправки возвращает True</returns>
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
        /// <summary>
        /// Получает список последних сообщений из чата.
        /// </summary>
        /// <param name="chatId">Id чата, возможно узнать отправив "/getChatId" в чат.</param>
        /// <param name="limit">Максимальное кол-во сообщений к возврату.</param>
        /// <returns>Возвращает список сообщений в случае успеха, в противном случае возвращает "Null".</returns>
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
                return null;
            }
            catch
            {

                return null;
            }
        }
        /// <summary>
        /// Отредактировать сообщение (Системное сообщение нельзя редактировать, нельзя редактировать сообщение сроком давности от 3 суток).
        /// </summary>
        /// <param name="messageId">ID существующего сообщения для редактирования.</param>
        /// <param name="text">Новый текст сообщения.</param>
        /// <returns>В случае успеха возвращает "True".</returns>
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
        /// <summary>
        /// Удаляет существующее сообщение.
        /// </summary>
        /// <param name="messageId">ID существующего сообщения к удалению.</param>
        /// <returns>В случае успеха возращает "True".</returns>
        public bool DeleteMessage(int messageId) 
        {
            try
            {
                var values = new Dictionary<string, string>
                {
                    { "MESSAGE_ID", $"{messageId}" },
                };
                var content = new FormUrlEncodedContent(values);
                var response = client.PostAsync($"{startUrl}im.message.delete.json", content);
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
