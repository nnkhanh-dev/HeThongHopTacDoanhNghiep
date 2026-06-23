using HopTacDoanhNghiep.ViewModels;

namespace HopTacDoanhNghiep.Services
{
    public interface IMailService
    {
        bool SendMail(MailData mailData);
    }
}
