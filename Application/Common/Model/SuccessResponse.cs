namespace Application.Common.Model;

public class SuccessResponse
{
    public bool IsSucces { get; init; }

    public SuccessResponse(bool isSucces) => IsSucces = isSucces;
    public SuccessResponse() {}
}