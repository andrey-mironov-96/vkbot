using vkbot.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace vkbot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CallbackController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IVkApi _vkApi;
        private readonly ILogger<CallbackController> _logger;

        public CallbackController(IConfiguration configuration, IVkApi vkApi, ILogger<CallbackController> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _vkApi = vkApi;
        }
        [HttpPost]
        public IActionResult Callback([FromBody] VkDTO dto)
        {
            try
            {
                // Проверяем, что находится в поле "type" 
                System.Console.WriteLine("Получил запрос " + JsonConvert.SerializeObject(dto));
                switch (dto.Type)
                {
                    // Если это уведомление для подтверждения адреса
                    case "confirmation":
                        // Отправляем строку для подтверждения 
                        return Ok(Environment.GetEnvironmentVariable("VK_AUTH_RESPONSE"));
                    // Новое сообщение
                    case "message_new":
                        {
                            // Десериализация
                            var msg = Message.FromJson(new VkResponse(dto.Object));

                            _vkApi.Messages.Send(new MessagesSendParams
                            {
                                RandomId = new DateTime().Millisecond,
                                PeerId = msg.PeerId.Value,
                                Message = "Отвечает бот: " + msg.Text
                            });
                            break;
                        }
                }
                // Возвращаем "ok" серверу Callback API

            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
            return Ok("ok");
        }
    }
}