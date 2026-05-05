using MBearsDay.Infrastructure.Telegram.Dto;

public class TelegramGetUpdatesResponseDto
{
    public List<TelegramUpdateDto> Result { get; set; } = new();
}