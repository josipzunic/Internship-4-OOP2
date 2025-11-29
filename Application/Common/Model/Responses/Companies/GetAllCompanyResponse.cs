using Application.DTOs.Companies;

namespace Application.Common.Model.Responses.Companies;

public class GetAllCompanyResponse
{
    public IEnumerable<CompanyDto> Companies { get; set; }
}