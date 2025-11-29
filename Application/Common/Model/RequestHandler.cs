namespace Application.Common.Model;

public abstract class RequestHandler<TRequest, TResult> where TRequest : class where TResult : class
{
    public Guid RequestId =>  Guid.NewGuid();

    public async Task<Result<TResult>> ProcessAuthorizedRequestAsync(TRequest request)
    {
        var result = new Result<TResult>();

        if (!await IsAuthorized())
        {
            result.SetUnauthorizedResult();
            return result;
        }
        
        //odi je reka da mozemo provjerit kesiranje, vrati se odi
        //odi prije gledamo je li iz kesa

        await HandleRequest(request,  result);
        
        //odi posli dodajemo u kes
        
        return result;
    }
    
    protected abstract Task<Result<TResult>> HandleRequest(TRequest request, Result<TResult> result);
    protected abstract Task<bool> IsAuthorized();
}