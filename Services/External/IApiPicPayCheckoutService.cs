using IAutor.Api.Data.Dtos.Picpay;
using Refit;

namespace IAutor.Api.Services.External
{
    [Headers("Authorization: Bearer")]
    public interface IApiPicPayCheckOutService
    {
        [Post("/api/v1/checkout")]
        Task<ApiResponse<PicPayCheckoutResponse>> CheckOut(PicPayCheckOut checkout);
    }
}
