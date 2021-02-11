using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Requests;

namespace TelegramTestBot
{
    class Program
    {
        static ITelegramBotClient botClient;
        public static InlineKeyboardMarkup keyboard;

        static void Main()
        {
            botClient = new TelegramBotClient("1680338133:AAGMYEWsEeq04ry4WQhG6CmzYBfEYAk1ezs");

            var me = botClient.GetMeAsync().Result;
            Console.WriteLine(
              $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );

            botClient.OnMessage += Bot_OnMessage;
            List<InlineKeyboardButton> keyboardButtonsRow1 = new List<InlineKeyboardButton>();
            List<InlineKeyboardButton> keyboardButtonsRow2 = new List<InlineKeyboardButton>();
            keyboardButtonsRow1.Add(InlineKeyboardButton.WithCallbackData("А какие есть?", "callback1"));
            keyboardButtonsRow1.Add(InlineKeyboardButton.WithCallbackData("Другой тип ответа", "callback2"));
            keyboardButtonsRow2.Add(InlineKeyboardButton.WithCallbackData("снизу", "callback23"));
            List<List<InlineKeyboardButton>> rowList = new List<List<InlineKeyboardButton>>();
            rowList.Add(keyboardButtonsRow1);
            rowList.Add(keyboardButtonsRow2);
            keyboard = new InlineKeyboardMarkup(rowList);

            botClient.OnCallbackQuery += async (object sc, CallbackQueryEventArgs ev) =>
            {
                var message = ev.CallbackQuery.Message;
                if (ev.CallbackQuery.Data == "callback1")
                {
                    message = await botClient.SendTextMessageAsync(
                chatId: ev.CallbackQuery.Message.Chat,
                text: "hello - отправит вам сообщение Hello, world!\n" +
                "sendpic - отправит вам любую картинку\n" +
                "sendpoll - отправит вам любое голосование\n" +
                "sendfile - отправит вам любой файл\n" +
                "sendsticker - отправит стикер\n" +
                "sendlocation - отправит вам локацию главного офиса компании\n" +
                "sendcontact - отравит вам рабочий телефон директора компании",
                parseMode: ParseMode.Default,
                disableNotification: false);
                }
                else
                if (ev.CallbackQuery.Data == "callback2")
                {
                    await botClient.AnswerCallbackQueryAsync(ev.CallbackQuery.Id, "ответ типа callBackQueryAnswer");
                }
            };
            botClient.StartReceiving();
            
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            botClient.StopReceiving();
            
        }

        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
           
            if (e.Message.Text == "/hello")
            {
                Message textMessage = await botClient.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Hello, world!",
                parseMode: ParseMode.Default,
                disableNotification: false);
            }
            if(e.Message.Text =="/sendpic")
            {
                 Message photoMessage = await botClient.SendPhotoAsync(
                 chatId: e.Message.Chat,
                 photo: "https://cdn5.zp.ru/job/attaches/2019/08/35/b8/35b8649f6773c52f3535793d96d84317.jpg",
                 caption: "<b>Логотип ИнПАД</b>. <i>Источник</i>: <a href=\"https://www.inpad.ru\">ИнПАД</a>",
                 parseMode: ParseMode.Html);
            }
            if(e.Message.Text == "/sendpoll")
            {
                Message pollMessage = await botClient.SendPollAsync(
                chatId: e.Message.Chat,
                isAnonymous: false,
                question: "Ты работаешь в ИнПАДе?",
                options: new[]
                {
                    "Да",
                    "Нет, в другом месте"
                });
            }
            if(e.Message.Text == "/sendfile")
            {
                    Message fileMessage = await botClient.SendDocumentAsync(
                    chatId: e.Message.Chat,
                    document: "https://media.tenor.com/images/e9cc959f643d3aac2322295eb95d19ce/tenor.gif"
                    );
            }
            if(e.Message.Text == "/sendsticker")
            {
                Message stickerMessage = await botClient.SendStickerAsync(
                chatId: e.Message.Chat,
                sticker: "https://raw.githubusercontent.com/zubanidze/telegramBot/master/TelegramTestBot/1146271698.webp");
            }
            if(e.Message.Text =="/sendlocation")
            {
                Message locationMessage = await botClient.SendVenueAsync(
                chatId: e.Message.Chat.Id,
                latitude: 56.8393f,
                longitude: 60.5836f,
                title: "ИНСТИТУТ ПРОЕКТИРОВАНИЯ, архитектуры и дизайна",
                address: "ул. Шейнкмана, 10, Екатеринбург, Свердловская обл., 620014",
                replyMarkup: new InlineKeyboardMarkup(InlineKeyboardButton.WithUrl(
                "Перейти на сайт организации",
                "https://www.inpad.ru/")));
            }
            if (e.Message.Text == "/sendcontact")
            {
                Message contactMessage = await botClient.SendContactAsync(
                chatId: e.Message.Chat.Id,
                phoneNumber: "+73432875694",
                firstName: "Виктор",
                lastName: "Сальников");
            }
            if((!e.Message.Text.Contains("/") && (!e.Message.Text.All(char.IsDigit)))) 
            {               
                Message textMessage = await botClient.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Такой команды пока нет :(",
                parseMode: ParseMode.Default,
                replyMarkup : keyboard);              
            }
            if(e.Message.Text == "custom")
            {
                var rkm = new ReplyKeyboardMarkup();
                var rows = new List<KeyboardButton[]>();
                var cols = new List<KeyboardButton>();
                for (var Index = 1; Index < 17; Index++)
                {
                    cols.Add(new KeyboardButton("" + Index));
                    if (Index % 4 != 0) continue;
                    rows.Add(cols.ToArray());
                    cols = new List<KeyboardButton>();
                }
                rkm.Keyboard = rows.ToArray();


                await botClient.SendTextMessageAsync(
                    e.Message.Chat,
                    "Choose",
                    replyMarkup: rkm);
            }
            if (e.Message.Text == "1")
            {
                Message textMessage = await botClient.SendTextMessageAsync(
                chatId: e.Message.Chat,
                text: "Hello, world!",
                parseMode: ParseMode.Default,
                disableNotification: false);
            }
            if(e.Message.Text == "2")
            {
                Message contactMessage = await botClient.SendContactAsync(
                chatId: e.Message.Chat.Id,
                phoneNumber: "+73432875694",
                firstName: "Виктор",
                lastName: "Сальников");
            }
            if (e.Message.Text == "3")
            {
                Message pollMessage = await botClient.SendPollAsync(
                chatId: e.Message.Chat,
                isAnonymous: false,
                question: "Ты работаешь в ИнПАДе?",
                options: new[]
                {
                    "Да",
                    "Нет, в другом месте"
                });
            }
        }
    }
}