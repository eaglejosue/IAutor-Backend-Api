using IAutor.Api.Data.Dtos.Picpay;
using Refit;

namespace IAutor.Api.Services.External
{
    public interface IApiPicPayLoginService
    {

        [Post("/oauth2/token")]
        Task<ApiResponse<PicPayToken>> Login(PicPayLogin login);
    }
}
