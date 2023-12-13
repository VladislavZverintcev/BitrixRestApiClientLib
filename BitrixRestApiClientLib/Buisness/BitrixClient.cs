using System.Net.Http.Headers;
using System.Text;
using BitrixRestApiClientLib.Models;
using Newtonsoft.Json;
using IO = System.IO;

namespace BitrixRestApiClientLib.Buisness
{
    /// <summary>
    /// Предоставляет класс для взаимодействия с Bitrix24 при помощи REST API
    /// </summary>
    public class BitrixClient : IDisposable
    {
        #region Fields

        #region Private
        private bool disposed = false;
        private readonly string webHookUrl = string.Empty;
        private readonly HttpClient client = new();
        #endregion Private

        #endregion Fields

        #region Constructors

        #region Public
        /// <summary>
        /// Инициализирует новый объект класса BitrixClient с указанием URL-адреса входящего вебхука
        /// </summary>
        /// <param name="webHookUrl">URL-адрес входящего вебхука</param>
        public BitrixClient(string webHookUrl)
        {
            this.webHookUrl = webHookUrl ?? throw new ArgumentNullException(nameof(webHookUrl));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        #endregion Public

        #endregion Constructors

        #region Methods

        #region Private

        #region Method to convet string to integer
        /// <summary>
        /// Преобразовывает строковое представление в эквивалентное 32-разрядное целое число, исключая символы, не имеющих соотвествующий эквивалент
        /// </summary>
        /// <param name="str">Строковое представление, содержащее число для преобразования</param>
        /// <returns>32-разрядное целое число эквивалентное числу в параметре str, или 0 (ноль), если параметр str был NULL</returns>
        private static int ConvertStringToInt32(string str)
        {
            List<char> allowedSymbols = new() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            return Convert.ToInt32(new string((from symbol in str.ToCharArray().ToList() where allowedSymbols.Contains(symbol) select symbol).ToArray()));
        }
        #endregion Method to convet string to integer

        #endregion Private

        #region Protected
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {

            }

            client.CancelPendingRequests();
            client.Dispose();

            disposed = true;
        }
        #endregion Protected

        #region Public

        #region Methods for getting users
        /// <summary>
        /// Получает список пользователей Bitrix24
        /// </summary>
        /// <returns>Если успешно список пользователей, в противном случае NULL</returns>
        public List<User>? GetUsers()
        {
            try
            {
                Task<string> response = client.GetStringAsync($"{webHookUrl}user.get.json");

                return JsonConvert.DeserializeObject<GetUsersResponse>(response.Result)?.Result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Получает список пользователей Bitrix24
        /// </summary>
        /// <returns>Объект задачи, представляющий асинхронную операцию</returns>
        public async Task<List<User>?> GetUsersAsync()
        {
            List<User>? users = null;

            await Task.Run(() =>
            {
                Task<string> taskResponse = client.GetStringAsync($"{webHookUrl}user.get.json");
                taskResponse.Wait();

                GetUsersResponse? resultResponse = JsonConvert.DeserializeObject<GetUsersResponse>(taskResponse.Result);
                users = resultResponse?.Result;
            });

            return users;
        }
        #endregion Methods for getting users

        #region Methods for getting messages
        /// <summary>
        /// Получает список последних сообщений из чата или диалога
        /// </summary>
        /// <param name="dialogId">ID чата или диалога</param>
        /// <param name="limit">Количество сообщений к возврату</param>
        /// <returns>Если успешно список последних сообщений, в противном случае NULL</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public List<Message>? GetMessages(string dialogId, int limit)
        {
            if (dialogId == null)
            {
                throw new ArgumentNullException(nameof(dialogId));
            }

            FormUrlEncodedContent requestContent = new(new Dictionary<string, string>
            {
                { "DIALOG_ID", $"{dialogId}" },
                { "LIMIT", $"{limit}" }
            });

            try
            {
                Task<HttpResponseMessage> response = client.PostAsync($"{webHookUrl}im.dialog.messages.get.json", requestContent);
                List<BaseMessage>? baseMessages = JsonConvert.DeserializeObject<GetMessagesResponse>(response.Result.Content.ReadAsStringAsync().Result)?.Result.Messages;
                List<Message>? messages = baseMessages == null ? null : (from baseMessage in baseMessages select new Message(baseMessage, dialogId)).ToList();
                messages?.Reverse();

                return messages;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Получает список последних сообщений из чата или диалога
        /// </summary>
        /// <param name="dialogId">ID чата или диалога</param>
        /// <returns>Объект задачи, представляющий асинхронную операцию</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<List<Message>?> GetMessagesAsync(string dialogId)
        {
            if (dialogId == null)
            {
                throw new ArgumentNullException(nameof(dialogId));
            }

            List<Message>? messages = null;
            FormUrlEncodedContent requestContent = new(new Dictionary<string, string> { { "DIALOG_ID", $"{dialogId}" } });

            await Task.Run(() =>
            {
                Task<HttpResponseMessage> mainTaskResponse = client.PostAsync($"{webHookUrl}im.dialog.messages.get.json", requestContent);
                mainTaskResponse.Wait();

                Task<string> subTaskResponse = mainTaskResponse.Result.Content.ReadAsStringAsync();
                subTaskResponse.Wait();

                GetMessagesResponse? resultResponse = JsonConvert.DeserializeObject<GetMessagesResponse>(subTaskResponse.Result);
                messages = (from baseMessage in resultResponse?.Result.Messages select new Message(baseMessage, dialogId)).ToList();
                messages.Reverse();
            });

            return messages;
        }
        #endregion Methods for getting messages

        #region Methods for sending message
        /// <summary>
        /// Отправляет сообщение в чат или диалог
        /// </summary>
        /// <param name="dialogId">ID чата или диалога</param>
        /// <param name="messageText">Текст сообщения</param>
        /// <param name="messageId">ID отрправленного сообщения. Если не успешно, то значение 0</param>
        /// <param name="sysMessage">Параметр для указания сообщения, как системного при отправке</param>
        /// <returns>Если успешно true, в противном случае false</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool SendMessage(string dialogId, string messageText, out int messageId, bool sysMessage = true)
        {
            if (dialogId == null)
            {
                throw new ArgumentNullException(nameof(dialogId));
            }

            if (messageText == null)
            {
                throw new ArgumentNullException(nameof(messageText));
            }

            string sysAttribute = sysMessage ? "Y" : "N";
            FormUrlEncodedContent requestContent = new(new Dictionary<string, string>
            {
                { "DIALOG_ID", $"{dialogId}" },
                { "MESSAGE", $"{messageText}" },
                { "SYSTEM", sysAttribute },
                { "ATTACH", "" },
                { "URL_PREVIEW", "Y" },
                { "KEYBOARD", "" },
                { "MENU", "" }
            });

            try
            {
                Task<HttpResponseMessage> response = client.PostAsync($"{webHookUrl}im.message.add.json", requestContent);
                messageId = Convert.ToInt32(JsonConvert.DeserializeObject<SendMessageResponse>(response.Result.Content.ReadAsStringAsync().Result)?.Result);

                return response.IsCompletedSuccessfully;
            }
            catch
            {
                messageId = 0;

                return false;
            }
        }

        /// <summary>
        /// Отправляет сообщение в чат или диалог
        /// </summary>
        /// <param name="dialogId">ID чата или диалога</param>
        /// <param name="messageText">Текст сообщения</param>
        /// <param name="files">Список файлов для прикрепления к сообщению</param>
        /// <param name="sysMessage">Параметр для указания сообщения, как системного при отправке</param>
        /// <returns>Объект задачи, представляющий асинхронную операцию</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<int> SendMessageAsync(string dialogId, string messageText, List<string>? files = null, bool sysMessage = false)
        {
            if (dialogId == null)
            {
                throw new ArgumentNullException(nameof(dialogId));
            }

            if (messageText == null)
            {
                throw new ArgumentNullException(nameof(messageText));
            }

            int messageId = 0;
            List<BaseFile>? loadedFiles = null;

            await Task.Run(() =>
            {
                if (files != null && files.Count > 0)
                {
                    Task<int> getInfoLocationChatFilesResponse = GetInfoLocationChatFilesAsync(dialogId);
                    getInfoLocationChatFilesResponse.Wait();

                    loadedFiles = new List<BaseFile>();
                    files?.ForEach((file) =>
                    {
                        Task<Models.File?> uploadFileInFolderResponse = UploadFileInFolderAsync(Convert.ToInt32(getInfoLocationChatFilesResponse?.Result), file);
                        uploadFileInFolderResponse.Wait();
                        if (uploadFileInFolderResponse.Result != null)
                        {
                            loadedFiles.Add(new BaseFile(uploadFileInFolderResponse.Result, Convert.ToInt32(new FileInfo(file).Length)));
                        }
                    });
                }

                MessageContent messageContent = new()
                {
                    DialogId = dialogId,
                    Message = messageText,
                    System = sysMessage ? "Y" : "N",
                    UrlPreview = "Y"
                };
                if (loadedFiles != null && loadedFiles.Count > 0)
                {
                    messageContent.Attachment = new Attachment
                    {
                        Id = 1,
                        Blocks = new List<object>()
                    };
                    loadedFiles.ForEach((file) =>
                    {
                        messageContent.Attachment.Blocks.Add(new AttachFile
                        {
                            File = new BaseFile
                            {
                                Name = file.Name,
                                DownloadUrl = file.DownloadUrl,
                                Size = file.Size
                            }
                        });
                    });
                }
                HttpContent requestContent = new StringContent(JsonConvert.SerializeObject(messageContent), Encoding.UTF8, "application/json");

                Task<HttpResponseMessage>? sendMessageResponse = client.PostAsync($"{webHookUrl}im.message.add.json", requestContent);
                sendMessageResponse.Wait();

                Task<string> readAsStringResponse = sendMessageResponse.Result.Content.ReadAsStringAsync();
                readAsStringResponse.Wait();

                SendMessageResponse? resultResponse = JsonConvert.DeserializeObject<SendMessageResponse>(readAsStringResponse.Result);
                messageId = Convert.ToInt32(resultResponse?.Result);
            });

            return messageId;
        }
        #endregion Methods for sending message

        #region Methods for updating message
        /// <summary>
        /// Редактирует сообщение (системное сообщение нельзя редактировать, как и сообщение сроком давности от 3 суток)
        /// </summary>
        /// <param name="messageId">ID сообщения для редактирования</param>
        /// <param name="messageText">Новый текст сообщения</param>
        /// <returns>Если успешно true, в противном случае false</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool UpdateMessage(int messageId, string messageText)
        {
            if (messageText == null)
            {
                throw new ArgumentNullException(nameof(messageText));
            }

            FormUrlEncodedContent requestContent = new(new Dictionary<string, string>
            {
                { "MESSAGE_ID", $"{messageId}" },
                { "MESSAGE", $"{messageText}" },
                { "ATTACH", "" },
                { "URL_PREVIEW", "Y" },
                { "KEYBOARD", "" },
                { "MENU", "" },
            });

            try
            {
                Task<HttpResponseMessage> response = client.PostAsync($"{webHookUrl}im.message.update.json", requestContent);

                return response.IsCompletedSuccessfully;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Редактирует сообщение (системное сообщение нельзя редактировать, как и сообщение сроком давности от 3 суток)
        /// </summary>
        /// <param name="messageId">ID сообщения для редактирования</param>
        /// <param name="messageText">Новый текст сообщения</param>
        /// <returns>Объект задачи, представляющий асинхронную операцию</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> UpdateMessageAsync(int messageId, string messageText)
        {
            if (messageText == null)
            {
                throw new ArgumentNullException(nameof(messageText));
            }

            bool isSuccessfully = false;
            FormUrlEncodedContent requestContent = new(new Dictionary<string, string>
            {
                { "MESSAGE_ID", $"{messageId}" },
                { "MESSAGE", $"{messageText}" },
                { "ATTACH", "" },
                { "URL_PREVIEW", "Y" },
                { "KEYBOARD", "" },
                { "MENU", "" }
            });

            await Task.Run(() =>
            {
                Task<HttpResponseMessage> taskResponse = client.PostAsync($"{webHookUrl}im.message.update.json", requestContent);
                taskResponse.Wait();
                isSuccessfully = taskResponse.IsCompletedSuccessfully;
            });

            return isSuccessfully;
        }
        #endregion Methods for updating message

        #region Method for deleting message
        /// <summary>
        /// Удаляет существующее сообщение
        /// </summary>
        /// <param name="messageId">ID существующего сообщения</param>
        /// <returns>Если успешно true, в противном случае false</returns>
        public bool DeleteMessage(int messageId)
        {
            FormUrlEncodedContent requestContent = new(new Dictionary<string, string> { { "MESSAGE_ID", $"{messageId}" } });

            try
            {
                Task<HttpResponseMessage> response = client.PostAsync($"{webHookUrl}im.message.delete.json", requestContent);

                return response.IsCompletedSuccessfully;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Удаляет существующее сообщение
        /// </summary>
        /// <param name="messageId">ID существующего сообщения</param>
        /// <returns>Объект задачи, представляющий асинхронную операцию</returns>
        public async Task<bool> DeleteMessageAsync(int messageId)
        {
            bool isSuccessfully = false;
            FormUrlEncodedContent requestContent = new(new Dictionary<string, string> { { "MESSAGE_ID", $"{messageId}" } });

            await Task.Run(() =>
            {
                Task<HttpResponseMessage> taskResponse = client.PostAsync($"{webHookUrl}im.message.delete.json", requestContent);
                taskResponse.Wait();
                isSuccessfully = taskResponse.IsCompletedSuccessfully;
            });

            return isSuccessfully;
        }
        #endregion Method for deleting message

        #region Method to send file
        /// <summary>
        /// Отправляет файл в чат
        /// </summary>
        /// <param name="dialogId">ID чата</param>
        /// <param name="pathToFile">Локальный путь к отправляемому файлу</param>
        /// <returns>Объект задачи, представляющий асинхронную операцию</returns>
        public async Task<bool> SendFileAsync(string dialogId, string pathToFile)
        {
            bool isSuccessfully = false;

            await Task.Run(() =>
            {
                Task<int> getInfoLocationChatFilesResponse = GetInfoLocationChatFilesAsync(dialogId);
                getInfoLocationChatFilesResponse.Wait();

                Task<Models.File?> uploadFileInFolderResponse = UploadFileInFolderAsync(Convert.ToInt32(getInfoLocationChatFilesResponse?.Result), pathToFile);
                uploadFileInFolderResponse.Wait();

                FormUrlEncodedContent requestContent = new(new Dictionary<string, string>
                {
                    { "CHAT_ID", $"{ConvertStringToInt32(dialogId)}" },
                    { "UPLOAD_ID", $"{Convert.ToInt32(uploadFileInFolderResponse?.Result?.Id)}" }
                });

                Task<HttpResponseMessage> sendFileResponse = client.PostAsync($"{webHookUrl}im.disk.file.commit.json", requestContent);
                sendFileResponse.Wait();
                isSuccessfully = sendFileResponse.IsCompletedSuccessfully;
            });

            return isSuccessfully;
        }
        #endregion Method to send file 

        #region Method to get info location chat files
        /// <summary>
        /// Получает ID папки, в которой располагаются файлы чата
        /// </summary>
        /// <param name="dialogId">ID чата</param>
        /// <returns>Объект задачи, представляющий асинхронную операцию</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<int> GetInfoLocationChatFilesAsync(string dialogId)
        {
            if (dialogId == null)
            {
                throw new ArgumentNullException(nameof(dialogId));
            }

            int locationChatFilesId = 0;
            FormUrlEncodedContent requestContent = new(new Dictionary<string, string> { { "CHAT_ID", $"{ConvertStringToInt32(dialogId)}" } });

            await Task.Run(() =>
            {
                Task<HttpResponseMessage> mainTaskResponse = client.PostAsync($"{webHookUrl}im.disk.folder.get.json", requestContent);
                mainTaskResponse.Wait();

                Task<string> subTaskResponse = mainTaskResponse.Result.Content.ReadAsStringAsync();
                subTaskResponse.Wait();

                GetInfoLocationChatFilesResponse? resultResponse = JsonConvert.DeserializeObject<GetInfoLocationChatFilesResponse>(subTaskResponse.Result);
                locationChatFilesId = Convert.ToInt32(resultResponse?.Result.Id);
            });

            return locationChatFilesId;
        }
        #endregion Method to get info location chat files

        #region Method to upload file in folder
        /// <summary>
        /// Загружает файл в папку
        /// </summary>
        /// <param name="folderId">ID папки</param>
        /// <param name="pathToFile">Локальный путь к отправляемому файлу</param>
        /// <returns>Объект задачи, представляющий асинхронную операцию</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        public async Task<Models.File?> UploadFileInFolderAsync(int folderId, string pathToFile)
        {
            if (pathToFile == null)
            {
                throw new ArgumentNullException(nameof(pathToFile));
            }

            if (!IO.File.Exists(pathToFile))
            {
                throw new FileNotFoundException();
            }

            Models.File? file = null;
            HttpContent requestContent = new StringContent(JsonConvert.SerializeObject(new UploadFileInFolderRequest
            {
                Id = folderId,
                Data = new FileContent() { Name = Path.GetFileName(pathToFile) },
                FileContent = Convert.ToBase64String(IO.File.ReadAllBytes(pathToFile)),
                GenerateUniqueName = true,
                Rights = null
            }),
            Encoding.UTF8,
            "application/json");

            await Task.Run(() =>
            {
                Task<HttpResponseMessage> mainTaskResponse = client.PostAsync($"{webHookUrl}disk.folder.uploadfile.json", requestContent);
                mainTaskResponse.Wait();

                Task<string> subTaskResponse = mainTaskResponse.Result.Content.ReadAsStringAsync();
                subTaskResponse.Wait();

                UploadFileInFolderResponse? resultResponse = JsonConvert.DeserializeObject<UploadFileInFolderResponse>(subTaskResponse.Result);
                file = resultResponse?.Result;
            });

            return file;
        }
        #endregion Method to upload file in folder

        #region Method for disposing resources
        /// <summary>
        /// Освобождает неуправляемые и управляемые ресурсы
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion Method for disposing resources

        #endregion Public

        #endregion Methods
    }
}
