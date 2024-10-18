namespace IAutor.Api.Services;


public interface IQuestionService
{
    Task<Chapter?> GetByIdAsync(long id);
    Task<List<Chapter>> GetAllAsync(QuestionFilters filters);
    Task<Chapter?> CreateAsync(Question model);
    Task<Chapter?> UpdateAsync(Question model, long loggedUserId, string loggedUserName);
    Task<Chapter?> PatchAsync(Question model, long loggedUserId, string loggedUserName);
    Task<bool?> DeleteAsync(long id, long loggedUserId, string loggedUserName);
}
public sealed class QuestionService(
    IAutorDb db,
    INotificationService notification) : IQuestionService
{
    public async Task<Chapter?> CreateAsync(Question model)
    {
        throw new NotImplementedException();
    }

    public async Task<bool?> DeleteAsync(long id, long loggedUserId, string loggedUserName)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Chapter>> GetAllAsync(QuestionFilters filters)
    {
        throw new NotImplementedException();
    }

    public async Task<Chapter?> GetByIdAsync(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<Chapter?> PatchAsync(Question model, long loggedUserId, string loggedUserName)
    {
        throw new NotImplementedException();
    }

    public async Task<Chapter?> UpdateAsync(Question model, long loggedUserId, string loggedUserName)
    {
        throw new NotImplementedException();
    }
}
