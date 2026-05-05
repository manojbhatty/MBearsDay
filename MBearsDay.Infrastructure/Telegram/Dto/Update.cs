using System;
using System.Collections.Generic;
using System.Text;

namespace MBearsDay.Infrastructure.Telegram.Dto
{
    public class Update
    {
        public long UpdateId { get; set; }
        public CallbackQuery? CallbackQuery { get; set; }
    }
}
