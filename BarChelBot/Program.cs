using BarChelBot;
using Newtonsoft.Json.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

class Program
{
    private static Dictionary<string, string> _gems = new(){{"100к", "100"}, {"200к", "200"}, {"300к", "300"}, {"400к", "400"}, {"500к", "500"}, 
        {"600к", "600"}, {"700к", "700"}, {"800к", "800"}, {"900к", "900"}, {"1м", "1000"}, {"Больше 1м", "1001"}};
    
    private static Dictionary<string, string> _gems2 = new(){{"1.1м", "1100"}, {"1.2м", "1200"}, {"1.3м", "1300"}, {"1.4м", "1400"}, {"1.5м", "1500"}, 
        {"1.6м", "1600"}, {"1.7м", "1700"}, {"1.8м", "1800"}, {"1.9м", "1900"}, {"2м", "2000"}};

    private static Dictionary<string, string> _mights = new() {{"до 290м", "290"}, {"до 590м", "590"}, {"до 790м", "790"},
        {"до 990м", "990"}, {"до 1190м", "1190"}, {"до 1290м", "1290"}, {"до 1390м", "1390"}, {"до 1490м", "1490"}, 
        {"до 1590м", "1590"}, {"до 1690м", "1690"}, {"до 1790м", "1790"}, {"до 1890м", "1890"}, {"до 1990м", "1990"}};

    private static Dictionary<string, int> _gemsPrice = new(){{"290", 400}, {"590", 420}, {"790", 430}, {"990", 440}, {"1190", 470}, {"1290", 500},
        {"1390", 530}, {"1490", 560}, {"1590", 590}, {"1690", 610}, {"1790", 630}, {"1890", 660}, {"1990", 690}};

    private static Dictionary<string, string> _billShow = new()
    {
        {"Тинькофф", "Тинькофф"}, {"Монобанк", "Монобанк"}, 
        {"Wise", "Wise"}, {"PayPal", "PayPal"}, 
        {"Paysend", "Paysend"}, {"Binance", "Binance"},
        {"Украина", "Украина"}, {"Россия", "Россия"}, 
        {"Белорусия", "Белорусия"}, {"Армения", "Армения"},
        {"Казахстан", "Казахстан"}, {"Европа", "Европа"} 
    };
    private static Dictionary<string, string> _billPass = new()
    {
        {"Тинькофф", "`2200700708447949`\n (СБП `+79257372617`)\n Артём Владиславович Рожнов\n Тинькофф✅"}, {"Монобанк", "`5375414144964664`\n Акопян Мария А.\n Монобанк✅"}, 
        {"Wise", "Wise✅\n `dasmys159@gmail.com`"}, {"PayPal", "PayPal✅\n  `dasmys159@gmail.com`"}, 
        {"Paysend", "Paysend✅\n `dasmys159@gmail.com`"}, {"Binance", "BINANCE✅\n  `dasmys159@gmail.com`\n ❗USDT $❗"},
        {"Украина", "Украина\n  `5168752005819761`\n Акопян Артем А.\n Приватбанк✅"}, {"Россия", "Россия\n  `4279380651806087`\n (СБП `+79257372617`)\n Артём Владиславович Рожнов\n Сбербанк✅"}, 
        {"Белорусия", "Белорусия\n `4496550143326749` 03/28\n ANDREI SAVITSKI\n Сбербанк✅"}, {"Армения", "Армения\n  `4578 8900 0497 0511`\n SAHAK TOVNASYAN\n INECOBANK✅"},
        {"Казахстан", "Казахстан\n `4400 4301 2747 4093`\n Каспийбанк✅"}, {"Европа", "Европейская карта\n `Artem Akopian`\n `5167371019262503`\n Swedbank✅"}
    };
    
    

    private static Dictionary<long, UserInfo> _userInfos = new();

    public static void Main()
    {
        TelegramBotClient client = new TelegramBotClient("6304214930:AAHSHIrA7cunJQ_64EbvIIy3Zxm4LqY_X8M");
        client.StartReceiving(Update, Exeption);
        Console.ReadLine();
    }

    private static void Exeption(ITelegramBotClient botClient, Exception exeption, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    private static async void Update(ITelegramBotClient bot, Update update, CancellationToken token)
    {
        var mainId = -4080308594;




        if (update.Message != null)
        {
            
            var message = update.Message;
            var id = message.Chat.Id; 
            if(message.Chat.Id == mainId) return;
            
            var idd = update.Message.Chat.Id;
            if (!(_userInfos.ContainsKey(idd) && _userInfos.TryGetValue(idd, out var currentUser)))
            {
                currentUser = new UserInfo(); 
                _userInfos.Add(idd, currentUser); 
            }

            if (message.Text != null)
            {
                var text = message.Text; 
                if (text == "/start")
                {
                    currentUser.userName = message.From?.Username;
                    Start(id, bot);
                }
                
                if (currentUser.orderDetail == "" && currentUser.currentPrice > 0) 
                { 
                    currentUser.orderDetail = text;
                    await bot.SendTextMessageAsync(id, $"Я записал ваш заказ {currentUser.orderDetail}. \n Для получения заказа ❗1)перейдите в открытую ги (если можете)." +
                                                       " \n ❗2)Скиньте скриншот где видно ваш никнейм и гильдию");
                }
            }
            if (message.Photo != null && currentUser.currentPrice > 0 && currentUser.orderDetail == "") 
            { 
                var fileId = message.Photo[message.Photo.Length - 1].FileId; 
                currentUser.picture = InputFile.FromFileId(fileId);
                var gemsCount = GetGemsString(currentUser.currentGems);
                await bot.SendTextMessageAsync(message.Chat.Id, 
                $"Напиши что хочеш получить на {gemsCount} самоцветов 💎 и количество. \n Например: телепорты🏎, жетоны🟢, ускорения 15ч⏩, ускорения 3д⏩. \n " +
                $"(Ваше будет переслано поставщикам)");
            }
            if (message.Photo != null && currentUser.currentPrice != 0 && currentUser.orderDetail != "")
            {
                currentUser.userName = message.From?.Username;
                var fileId = message.Photo[message.Photo.Length - 1].FileId; 
                currentUser.playerPicture = InputFile.FromFileId(fileId);
                var inlineKeyboardMarkup = new InlineKeyboardMarkup(new[] {InlineKeyboardButton.WithCallbackData("Купить самоцветы 💎", "buygems")}); 
                await bot.SendTextMessageAsync(id, "Я переслал ваш заказ поставщику✅, постараемся выполнить как можно быстрее! \n При " +
                                                   "возникновении каких либо вопросов писать @CHEKYIIIKA 💬", replyMarkup: inlineKeyboardMarkup);
                    
                await bot.SendTextMessageAsync(mainId, $"Новый заказ 🔽🔽🔽 \n Заказчик: @{currentUser.userName} \n Заказ: {currentUser.orderDetail} \n Сила: {currentUser.currentMight}м💪 \n " +
                                                       $"Самов Куплено: {GetGemsString(currentUser.currentGems)}💎 " +
                                                       $"\n Цена: {currentUser.currentPrice}р💰 \n Ник, ги и чек 🔽🔽🔽");
                    
                await bot.SendPhotoAsync(mainId, currentUser.picture);
                await bot.SendPhotoAsync(mainId, currentUser.playerPicture);


                currentUser.orderDetail = ""; 
                currentUser.playerPicture = null; 
                currentUser.currentMight = 0; 
                currentUser.currentPrice = 0; 
                currentUser.currentGems = 0; 
                currentUser.picture = null; 
            }
        }


        if (update.Type == UpdateType.CallbackQuery)
        {
            var callbackQuery = update.CallbackQuery;
            var data = update.CallbackQuery?.Data;
            var id = callbackQuery.Message.Chat.Id;
            if (!(_userInfos.ContainsKey(id) &&  _userInfos.TryGetValue(id, out var currentUser)))
            {
                currentUser = new UserInfo();
                _userInfos.Add(id, currentUser);
            }
          
            if (data == "buygems")
            {
                var table = GetTable(5, 3, _mights);
                await bot.SendTextMessageAsync(id, "Какая у тебя мощ аккаунта💪", replyMarkup: table, parseMode: ParseMode.MarkdownV2);
            }
            
            if (data != null && _gemsPrice.Keys.Contains(data))
            {
                currentUser.currentMight = int.Parse(data);
                var table = GetTable(3, 4, _gems);
                await bot.SendTextMessageAsync(id, $"Сколько самоцветов 💎 хотите приобрести на силу {data}м 💪", replyMarkup: table);
            }

            if (data != null && currentUser.currentMight > 0 && (_gems.Values.Contains(data) || _gems2.Values.Contains(data)) && data != "1001")
            {
                CalculatePrice(id, data, bot, currentUser);
            }
            
            if (data == "1001" && currentUser.currentMight > 0)
            {
                var table = GetTable(3, 4, _gems2);
                await bot.SendTextMessageAsync(id, $"Сколько самоцветов 💎 хотите приобрести на силу {currentUser.currentMight}м 💪", replyMarkup: table);
            }

            if (data == "payment" && currentUser.currentPrice > 0)
            {
                var table = GetTable(_billShow.Count, 1, _billShow);
                await bot.SendTextMessageAsync(id, $"Выберите подходящий вам способ для перевода {currentUser.currentPrice}р💰 ", replyMarkup: table, parseMode: ParseMode.MarkdownV2);
            }
            
            if (data != null && _billPass.ContainsKey(data) && currentUser.currentPrice > 0)
            {
                _billPass.TryGetValue(data, out var info);
                info = info.Replace(".", "\\.").Replace("-", "\\-").Replace("(", "\\(").
                    Replace(")", "\\)").Replace("+", "\\+") + "\\.";
                var str = "\n После совершения платежа пришлите скриншот \\(фото\\)📸 перевода денег💵";
                await bot.SendTextMessageAsync(id, info + str, parseMode: ParseMode.MarkdownV2);
            }
        }

    }

    private static async void Start(long id, ITelegramBotClient bot)
    {
        var inlineKeyboardMarkup = new InlineKeyboardMarkup(new[] {InlineKeyboardButton.WithCallbackData("Купить самоцветы 💎", "buygems")});
               
        await bot.SendTextMessageAsync(id, "Привет, я BAR🍹CHEK_bot готов помоч тебе купить самоцветы быстро и удобно. Как работает наш бот: " + 
                                           "вы выбираете количество самоцветов, которое желаете приобрести, оплачиваете, далее скидываете доказательство оплаты " +
                                           "и указываете аккаунт на который должны быть переведены самоцветы и что имеенно вы хотите, после завершения вы получите самоцветы в кратчайшие сроки!",
            replyMarkup: inlineKeyboardMarkup);
    }

    private static async void CalculatePrice(long chatId, string data, ITelegramBotClient bot, UserInfo userInfo)
    {
        userInfo.currentGems = int.Parse(data);
        var gemsBought = int.Parse(data.Remove(data.Length - 2, 2));
        
        _gemsPrice.TryGetValue(userInfo.currentMight.ToString(), out int priceFor100);
        if (int.Parse(data) >= 1000) priceFor100 -= 10;
        userInfo.currentPrice = priceFor100 * gemsBought;
        
        if (int.Parse(data) >= 1000)
        {
            data = data.Remove(data.Length - 2, 2) + "м";
            data = data.Insert(1, ".");
        }
        else data += "к";

        var inlineKeyboardMarkup = new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData("Оплатить ✅", "payment"));
                
        await bot.SendTextMessageAsync(chatId, $"{data} самоцветов 💎 на силу до {userInfo.currentMight}м 💪 будет стоить {userInfo.currentPrice}р 💰", replyMarkup: inlineKeyboardMarkup);
    }
    
    private static InlineKeyboardMarkup GetTable(int x, int y, Dictionary<string, string> values)
    {
        var table = new List<List<InlineKeyboardButton>>();
        int totalI = 0;
        for (int i = 0; i < x; i++)
        {
            table.Add(new List<InlineKeyboardButton>());
            var current = table[i];
            for (int j = 0; j < y; j++)
            {
                if (totalI == values.Count)
                {
                   break;
                }
                current.Add(InlineKeyboardButton.WithCallbackData(values.Keys.ToArray()[totalI], values.Values.ToArray()[totalI]));
                totalI++;
            }
        }
        return new InlineKeyboardMarkup(table);
    }

    private static string GetGemsString(int count)
    {
        return count > 1000 ? count.ToString().Remove(2, 2).Insert(1, ".") + "м" : count + "к"; 
    }
}
    
