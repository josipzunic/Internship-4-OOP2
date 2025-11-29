using Application.DTOs.Companies;

namespace Application.Common.Model.Responses.Companies;

public class GetCompanyByIdResponse
{
    public CompanyDto Company { get; set; }
}