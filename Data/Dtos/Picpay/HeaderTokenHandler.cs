namespace IAutor.Api.Data.Dtos.Picpay
{
    public class HeaderTokenHandler(IApiPicPayLoginService apiPicPayLoginService, IConfiguration configuration) : DelegatingHandler
    {
        
     

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,CancellationToken cancellationToken)
        {
            var clientId = configuration["PicPayConfig:ClientId"];
            var clientSecret = configuration["PicPayConfig:ClientSecret"];
            var result = await apiPicPayLoginService.Login(new PicPayLogin() { ClientId = clientId, ClientSecret = clientSecret });


            //request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result?.Content?.Token);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "123456");
            return await base.SendAsync(request, cancellationToken);  
        }
    }
}
