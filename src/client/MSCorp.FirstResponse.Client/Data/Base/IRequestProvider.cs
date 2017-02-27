using System.Threading.Tasks;

namespace MSCorp.FirstResponse.Client.Data.Base
{
    public interface IRequestProvider
    {
        Task<TResult> GetAsync<TResult>(string uri);

        Task<TResult> PostAsync<TResult>(string uri, TResult data);

        Task<TResult> PostAsync<TRequest, TResult>(string uri, TRequest data);
    }
}
